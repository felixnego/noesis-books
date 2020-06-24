using System;
using System.Collections.Generic;

namespace noesis_api.DTOs
{
    public class BookDetailDTO
    {
        public long Id { get; set; }

        public string Title { get; set; }

        public double Pages { get; set; }

        public string ISBN { get; set; }

        public int Year { get; set; }

        public long GoodReadsId { get; set; }

        public string Description { get; set; }

        public string Publisher { get; set; }

        public string ThumbnailURL { get; set; }

        public string CoverBigURL { get; set; }

        public double AverageRating { get; set; }

        public List<CategoryDTO> BookCategories { get; set; }

        public List<AuthorListDTO> BookAuthors { get; set; } = new List<AuthorListDTO>();

        public List<CommentDTO> UserComments { get; set; } = new List<CommentDTO>();

        public List<NoteDTO> UserNotes { get; set; } = new List<NoteDTO>();
    }
}
