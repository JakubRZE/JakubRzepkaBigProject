using MailService.Entities;
using MailService.Enumerations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MailService.Interfaces
{
    public interface IMailRepository
    {
        Task SaveAsync();

        Task<int> SaveMailAsync(Mail entity);

        Task<IEnumerable<Mail>> GetAllMailsByUserIdAsync(int id);

        Task<Mail> GetMailByIdAsync(int id);

        Task<Status> GetMailStatusByIdAsync(int id);

        Task<IEnumerable<Mail>> GetPendingMailsByUserIdAsync(int id);
    }
}