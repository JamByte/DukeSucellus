using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace OSRSXPTracker.DataBase
{
    internal class AppDbContext:DbContext
    {
        public DbSet<Player> Players { get; set; }
        public DbSet<PlayerStats> PlayerStats { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //I wanted to learn how to use entity framework in this project
            //Probably a bad idea lol
            optionsBuilder.UseSqlite("Data Source=playerStats.db");



        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //No idea what i am doing right now 
            modelBuilder.Entity<PlayerStats>().HasOne<Player>().WithMany().HasForeignKey(ps => ps.PlayerId);
            modelBuilder.Entity<PlayerStats>()
                .HasIndex(ps => ps.Timestamp);
        }



    }
}
