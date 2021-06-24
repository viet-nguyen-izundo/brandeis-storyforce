using Microsoft.EntityFrameworkCore;
using StoryForce.Shared.Models;

namespace StoryForce.Server.Data
{
    public class PgDbContext : DbContext
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
                .WithOne(sf => sf.ApprovedSubmission)
                .HasForeignKey(sf => sf.ApprovedSubmissionId);

            modelBuilder.Entity<Submission>()
                .HasMany(submission => submission.RejectedFiles)
                .WithOne(sf => sf.RejectedSubmission)
                .HasForeignKey(sf => sf.RejectedSubmissionId);

            modelBuilder.Entity<Submission>()
                .HasMany(submission => submission.FeaturedPeople)
                .WithMany(sf => sf.FeaturedSubmissions);

            modelBuilder.Entity<Submission>()
                .HasOne(submission => submission.SubmittedBy)
                .WithMany(person => person.SubmittedSubmissions)
                .HasForeignKey(s => s.SubmittedById);

            modelBuilder.Entity<Submission>()
                .HasOne(submission => submission.ReviewedBy)
                .WithMany(person => person.ReviewedBySubmissions)
                .HasForeignKey(s => s.ReviewedById);

            modelBuilder.Entity<Submission>()
                .HasOne(submission => submission.ApprovedBy)
                .WithMany(person => person.ApprovedSubmissions)
                .HasForeignKey(s => s.ApprovedById);
        }
    }
}
