using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QuizApp.Models;

namespace QuizApp.Data
{
    public class QuizAppContext : DbContext
    {
        public QuizAppContext(DbContextOptions<QuizAppContext> options) : base(options)
        {
        }

        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<Question> Questions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Question>()
                .ToTable("Question")
                .HasOne(q => q.Quiz)
                    .WithMany(q => q.Questions);

            modelBuilder.Entity<Quiz>()
                .ToTable("Quiz");
        }
    }
}
