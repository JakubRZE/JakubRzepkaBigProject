using MailService.Entities;
using MailService.Enumerations;
using MailService.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MailService.Interfaces
{
    public interface IMailSrv
    {
        Task SendMailAsync(int userId);
        Task<int> SaveMailAsync(MailDto model);
        Task<IEnumerable<MailDto>> GatAllMailsByUserIdAsync(int id);
        Task<MailDto> GatMailByIdAsync(int id);
        Task<Status> GatMailStatusByIdAsync(int id);
    }
}
