using System;
namespace noesis_api.Models
{
    public class BookAuthor
    {
        public long Id { get; set; }

        public Book Book { get; set; }
        public long BookId { get; set; }

        public Author Author { get; set; }
        public long AuthorId { get; set; }
    }
}
