using System;
using Microsoft.EntityFrameworkCore;
using noesis_api.Models;

namespace noesis_api.Contexts
{
    public class NoesisApiContext : DbContext
    {
        public NoesisApiContext(DbContextOptions<NoesisApiContext> options)
            : base(options)
        {
        }

        public DbSet<Book> Book { get; set; }
        public DbSet<Comment> Comment { get; set; }
        public DbSet<Author> Author { get; set; }
        public DbSet<UserComment> UserComments { get; set; }
        public DbSet<UserNote> UserNotes { get; set; }
        public DbSet<UserRating> UserRatings { get; set; }
        public DbSet<BookAuthor> BookAuthors { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<BookCategory> BookCategory { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Note> Note { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Book>().HasIndex(b => b.Title).IsUnique();
            builder.Entity<Author>().HasIndex(a => a.Name).IsUnique();
            builder.Entity<User>().HasIndex(u => new { u.Username, u.Email }).IsUnique();
        }
    }
}
