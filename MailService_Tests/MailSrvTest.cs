using MailService.Entities;
using MailService.Enumerations;
using MailService.Interfaces;
using MailService.Services;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace MailService_Tests
{
    public class MailSrvTest
    {
        private readonly Mock<IMailRepository> _mailRepositoryMock;
        private readonly Mock<IConfiguration> _configMock;
        private readonly Mock<ISMTPService> _sMTPServiceMock;
        private readonly IMailSrv _mailSrv;

        public MailSrvTest()
        {
            _mailRepositoryMock = new Mock<IMailRepository>();
            _configMock = new Mock<IConfiguration>();
            _sMTPServiceMock = new Mock<ISMTPService>();

            _mailSrv = new MailSrv(_mailRepositoryMock.Object, _configMock.Object, _sMTPServiceMock.Object);
        }

        [Fact]
        public async void ShouldChangeMailStatusAfterSending()
        {
            List<Mail> initialMails = new List<Mail>
            {
                new Mail
                {
                      Sender= "test@test.com",
                      Content= "test",
                      Priority= Priority.Important,
                      Status= Status.Pending,
                      Recipients = new List<Recipient>
                      {
                            new Recipient
                            {
                                RecipientMail = "test@test.com"
                            }
                      }
                },
                new Mail
                {
                       Sender= "test@test.com",
                       Content= "test",
                       Priority= Priority.Important,
                       Status= Status.Pending,
                       Recipients = new List<Recipient>
                       {
                             new Recipient
                             {
                                RecipientMail = "test@test.com"
                             }
                       }
                }
            };
            int userId = 1;
            _mailRepositoryMock.Setup(x => x.GetPendingMailsByUserIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => initialMails);
            _mailRepositoryMock.Setup(x => x.GetAllMailsByUserIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => initialMails);

            await _mailSrv.SendMailAsync(userId);
            var mails = await _mailSrv.GatAllMailsByUserIdAsync(userId);

            Assert.True(mails.All((x) => x.Status == Status.Sent));
        }

        [Fact]
        public async void ShouldSaveInformationAboutFailedSendOperation()
        {
            var initialMails = new List<Mail>
            {
                new Mail
                {
                      Sender= "test@test.com",
                      Content= "test",
                      Priority= Priority.Important,
                      Status= Status.Pending,
                      Recipients = new List<Recipient>
                      {
                            new Recipient
                            {
                                RecipientMail = "test@test.com"
                            }
                      }
                }
            };
            int userId = 1;
            _mailRepositoryMock.Setup(x => x.GetPendingMailsByUserIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => initialMails);
            _sMTPServiceMock.Setup(x => x.SendMail(It.IsAny<Mail>()))
                .Throws<Exception>();

            await _mailSrv.SendMailAsync(userId);

            Assert.Equal(1, initialMails.First().NumberOfTries);
        }
    }
}