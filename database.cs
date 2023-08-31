using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.ComponentModel.DataAnnotations;


namespace Noter
{
    public class note_data
    {
        [Key]
        public int ID { get; set; }
        public string? title;
        public string? text;
    }


    public class ApplicationContext : DbContext
    {

        public DbSet<note_data?> noteList { get; set; }

        public ApplicationContext()
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=notesdb;Username=postgres;Password=PSQLQa5267806Q");
        }
    }
}

