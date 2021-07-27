using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using StoryForce.Shared.Interfaces;
using StoryForce.Shared.Models;

namespace StoryForce.Server.Data
{
    public class PgDbContext : IdentityDbContext<Person, IdentityRole<int>, int>
    {
        public DbSet<Tag> Tags { get; set; }

        public DbSet<Submission> Submissions { get; set; }

        public DbSet<StoryFile> StoryFiles { get; set; }

        public DbSet<Story> Stories { get; set; }

        public DbSet<Person> Persons { get; set; }

        public DbSet<Note> Notes { get; set; }

        public DbSet<Event> Events { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<AuditDetail> AuditDetails { get; set; }
        public DbSet<StoryFileAssignment> StoryFileAssignments { get; set; }       

        public DbSet<Category> Category { get; set; }

        public PgDbContext(DbContextOptions options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Submission>()
                .HasMany(submission => submission.SubmittedFiles)
                .WithOne(sf => sf.Submission)
                .HasForeignKey(sf => sf.SubmissionId);

            modelBuilder.Entity<Submission>()
                .HasMany(submission => submission.ApprovedFiles)
                .WithOne(sf => sf.ApprovedSubmission);

            modelBuilder.Entity<Submission>()
                .HasMany(submission => submission.RejectedFiles)
                .WithOne(sf => sf.RejectedSubmission);

            modelBuilder.Entity<Submission>()
                .HasMany(submission => submission.FeaturedPeople)
                .WithMany(person => person.FeaturedSubmissions);

            modelBuilder.Entity<Submission>()
                .HasOne(submission => submission.SubmittedBy)
                .WithMany(person => person.SubmittedSubmissions);

            modelBuilder.Entity<Submission>()
                .HasOne(submission => submission.ReviewedBy)
                .WithMany(person => person.ReviewedBySubmissions);

            modelBuilder.Entity<Submission>()
                .HasOne(submission => submission.ApprovedBy)
                .WithMany(person => person.ApprovedSubmissions);
                
            modelBuilder.Entity<Person>()
                .HasMany(person=> person.FeaturedStoryFile)
                .WithMany(sf => sf.FeaturedPeople);

            modelBuilder.Entity<StoryFile>()
                .HasOne(sf => sf.SubmittedBy)
                .WithMany(person => person.SubmittedStoryFiles)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<StoryFile>()
                .HasOne(sf => sf.Event)
                .WithMany(ev => ev.StoryFiles);

            modelBuilder.Entity<StoryFile>()
                .HasOne(sf => sf.RequestedBy)
                .WithMany(person => person.RequestedStoryFiles);

            modelBuilder.Entity<StoryFile>()
                .HasOne(sf => sf.UpdatedBy)
                .WithMany(person => person.UpdatedStoryFiles);
         
            modelBuilder.Entity<StoryFile>()
                .HasMany(sf => sf.Categories)
                .WithMany(cat => cat.StoryFiles);

            modelBuilder.Entity<StoryFile>()
                .HasMany(sf => sf.Tags)
                .WithMany(tag => tag.StoryFiles);

            modelBuilder.Entity<Person>()
                .HasMany(person => person.FeaturedStoryFile)
                .WithMany(sf => sf.FeaturedPeople);

            modelBuilder.Entity<Person>()
                .HasMany(person => person.FeaturedStoryFile)
                .WithMany(sf => sf.FeaturedPeople);

            modelBuilder.Entity<Person>()
                .HasMany(person => person.FeaturedStoryFile)
                .WithMany(sf => sf.FeaturedPeople);

            modelBuilder.Entity<Person>()
                .HasMany(person => person.FavouritesStoryFile)
                .WithMany(sf => sf.FavouritesPeople)
                .UsingEntity(join => join.ToTable("Favorites"));

            modelBuilder.Entity<StoryFile>()
                .HasMany(sf => sf.StoryFileAssignment)
                .WithOne(sf => sf.StoryFile)
                .HasForeignKey(sf => sf.StoryFileId);


            modelBuilder.Entity<Person>()
                .HasMany(sf => sf.StoryFileAssignments)
                .WithOne(sf => sf.AssignedTo)
                .HasForeignKey(sf => sf.AssignedToId);

            modelBuilder.Entity<StoryFileAssignment>()
                .Property(c => c.Description).HasMaxLength(1000);

            modelBuilder.Entity<StoryFileAssignment>()
                .Property(c => c.Title).HasMaxLength(1000);
        }


        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            IEnumerable<EntityEntry> modified = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Modified || e.State == EntityState.Added);
            foreach (EntityEntry item in modified)
            {
                if (item.Entity is IDateTracking changedOrAddedItem)
                {
                    if (item.State == EntityState.Added)
                    {
                        changedOrAddedItem.CreatedDate = DateTime.Now;
                    }
                    else
                    {
                        changedOrAddedItem.LastModifiedDate = DateTime.Now;
                    }
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
