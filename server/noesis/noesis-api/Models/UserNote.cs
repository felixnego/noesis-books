using System;
namespace noesis_api.Models
{
    public class UserNote
    {
        public long Id { get; set; }

        public User User { get; set; }
        public long UserId { get; set; }

        public Book Book { get; set; }
        public long BookId { get; set; }

        public Note Note { get; set; }
        public long NoteId { get; set; }
    }
}
