using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repositories.EFCore.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;



namespace Repositories.EFCore
{
    public class RepositoryContext : DbContext //veritabanı gibi düşün
    {
        public RepositoryContext(DbContextOptions options) // Gelen parametreyi options ile db ye gödericem.
            : base(options)
        {
        }
        public  DbSet<Book> Books { get; set; } //veritabanı tablosu gibi düşün bunu dbSet bu işe yarıyor.




        //Config ayarı yapıldı.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new BookConfig());
        }
    }
}

