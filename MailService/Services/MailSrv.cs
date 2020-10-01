using MailService.Entities;
using MailService.Enumerations;
using MailService.Interfaces;
using MailService.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MailService.Services
{
    public class MailSrv : IMailSrv
    {
        private readonly IMailRepository _mailRepository;
        private readonly IConfiguration _configuration;
        private readonly ISMTPService _sMTPService;

        public MailSrv(IMailRepository mailRepository, IConfiguration configuration, ISMTPService sMTPService)
        {
            _mailRepository = mailRepository;
            _configuration = configuration;
            _sMTPService = sMTPService;
        }

        public async Task<IEnumerable<MailDto>> GatAllMailsByUserIdAsync(int id)
        {
            var entity = await _mailRepository.GetAllMailsByUserIdAsync(id);
            return entity.Select(x => new MailDto(x));
        }

        public async Task<MailDto> GatMailByIdAsync(int id)
        {
            var entity = await _mailRepository.GetMailByIdAsync(id);
            return entity != null ? new MailDto(entity) : null;
        }

        public async Task<Status> GatMailStatusByIdAsync(int id)
        {
            try
            {
                var status = await _mailRepository.GetMailStatusByIdAsync(id);
                return status;
            }
            catch (InvalidOperationException)
            {
                throw;
            }
        }

        public async Task<int> SaveMailAsync(MailDto model)
        {
            var entity = new Mail
            {
                UserId = model.UserId,
                Sender = model.Sender,
                Recipients = model.Recipients.Select(x => new Recipient
                {
                    RecipientMail = x.RecipientMail
                }).ToList(),
                Content = model.Content,
                Status = Status.Pending
            };

            if (string.IsNullOrWhiteSpace(entity.Sender))
                entity.Sender = _configuration["DefaultSenderEmail"];

            return await _mailRepository.SaveMailAsync(entity);
        }

        public async Task SendMailAsync(int id)
        {
            var mails = await _mailRepository.GetPendingMailsByUserIdAsync(id);

            foreach (var mail in mails)
            {
                try
                {
                    _sMTPService.SendMail(mail);
                    mail.Status = Status.Sent;
                }
                catch (Exception)
                {
                    mail.NumberOfTries++;
                    /// SMTP seding failed
                }
            }
            await _mailRepository.SaveAsync();
        }
    }
}