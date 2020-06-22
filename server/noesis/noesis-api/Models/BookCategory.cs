using System;
namespace noesis_api.Models
{
    public class BookCategory
    {
        public long Id { get; set; }

        public Book Book { get; set; }
        public long BookId { get; set; }

        public Category Category { get; set; }
        public long CategoryId { get; set; }
    }
}
