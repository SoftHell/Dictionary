using System;
using System.Linq;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class AppDbContext : IdentityDbContext
    {
        public DbSet<Language> Languages { get; set; } = default!;
        
        public DbSet<PartOfSpeech> PartsOfSpeech { get; set; } = default!;
        
        public DbSet<Topic> Topics { get; set; } = default!;
        
        public DbSet<Word> Words { get; set; } = default!;
        
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
           
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            // disable cascade delete initially for everything
            foreach (var relationship in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
            

        }
        
        
    }
    
}