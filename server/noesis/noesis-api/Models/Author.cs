using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace noesis_api.Models
{
    public class Author
    {
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string PlaceOfBirth { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string PictureURL { get; set; }

        public string About { get; set; }

        public List<BookAuthor> BookAuthors { get; set; }
    }
}
