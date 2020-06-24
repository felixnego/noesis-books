using System;
namespace noesis_api.DTOs
{
    public class CommentDTO
    {
        public long Id { get; set; }

        public string Username { get; set; }

        public string Text { get; set; }

        public DateTime AddedOn { get; set; }
    }
}
