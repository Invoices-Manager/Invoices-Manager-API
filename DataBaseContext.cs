using Microsoft.EntityFrameworkCore;

namespace Invoices_Manager_API
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options) { }

        public DbSet<BackUpInfoModel> BackUpInfo { get; set; }
        public DbSet<BackUpModel> BackUp { get; set; } 
        public DbSet<InvoiceBackUpModel> InvoiceBackUp { get; set; } 
        public DbSet<InvoiceModel> Invoice { get; set; }
        public DbSet<NoteModel> Note { get; set; } 
        public DbSet<UserModel> User { get; set; }
        public DbSet<LoginModel> Logins { get; set; }
    }
}
    