using Invoices_Manager_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Invoices_Manager_API
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options)
        {
        }

        public DbSet<BackUpInfoModel> BackUpInfo { get; set; } = default!;
        public DbSet<BackUpModel> BackUp { get; set; } = default!;
        public DbSet<InvoiceBackUpModel> InvoiceBackUp { get; set; } = default!;
        public DbSet<InvoiceModel> Invoice { get; set; } = default!;
        public DbSet<NotebookModel> Notebook { get; set; } = default!;
        public DbSet<NoteModel> Note { get; set; } = default!;
        public DbSet<UserModel> User { get; set; } = default!;
    }
}