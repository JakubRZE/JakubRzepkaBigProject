using MailService.Entities;
using MailService.Enumerations;
using MailService.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MailService.Repositories
{
    public class MailRepository : IMailRepository
    {
        private MailDbContext _context;

        public MailRepository(MailDbContext context)
        {
            _context = context;
        }

        public async Task<int> SaveMailAsync(Mail entity)
        {
            var newEntity = await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
            return newEntity.Entity.Id;
        }

        public async Task<IEnumerable<Mail>> GetAllMailsByUserIdAsync(int id)
        {
            return await _context.Mails.Include(x => x.Recipients).Where(x => x.UserId == id).ToListAsync();
        }

        public async Task<Mail> GetMailByIdAsync(int id)
        {
            return await _context.Mails.Include(x => x.Recipients).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Status> GetMailStatusByIdAsync(int id)
        {
            return await _context.Mails.Where(y => y.Id == id).Select(x => x.Status).FirstAsync();
        }

        public async Task<IEnumerable<Mail>> GetPendingMailsByUserIdAsync(int id)
        {
            return await _context.Mails.Include(x => x.Recipients).Where(x => x.UserId == id && x.Status == Status.Pending).OrderByDescending(x => x.Priority).ToListAsync();
        }

        public Task SaveAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}