using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace noesis_api.Models
{
    public class Book
    {
        public long Id { get; set; }

        [Required]
        public string Title { get; set; }

        public double Pages { get; set; }

        public string ISBN { get; set; }

        public int Year { get; set; }

        public List<BookCategory> BookCategories { get; set; }

        public long GoodReadsId { get; set; }

        public string Description { get; set; }

        public string Publisher { get; set; }

        public List<UserRating> UserRatings { get; set; }

        public List<UserComment> UserComments { get; set; }

        public List<UserNote> UserNotes { get; set; }

        public List<BookAuthor> BookAuthors { get; set; }

        public string ThumbnailURL { get; set; }

        public string CoverBigURL { get; set; }

    }
}
