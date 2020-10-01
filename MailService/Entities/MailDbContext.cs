using Microsoft.EntityFrameworkCore;

namespace MailService.Entities
{
    public class MailDbContext : DbContext
    {
        public MailDbContext(DbContextOptions<MailDbContext> options)
            : base(options) { }

        public DbSet<Mail> Mails { get; set; }
    }
}