using System;
using noesis_api.Models;
using System.Collections.Generic;

namespace noesis_api.DTOs
{
    public class BookListDTO
    {
        public long Id { get; set; }

        public string Title { get; set; }

        public List<BookCategory> BookCategories { get; set; }

        public double AverageRating { get; set; }

        public List<AuthorListDTO> BookAuthors { get; set; } = new List<AuthorListDTO>();
    }
}
