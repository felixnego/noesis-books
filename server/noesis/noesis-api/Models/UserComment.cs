using System;
namespace noesis_api.Models
{
    public class UserComment
    {
        public long Id { get; set; }

        public User User { get; set; }
        public long UserId { get; set; }

        public Book Book { get; set; }
        public long BookId { get; set; }

        public Comment Comment { get; set; }
        public long CommentId { get; set; }
    }
}
