using System;
namespace noesis_api.DTOs
{
    public class NoteDTO
    {
        public long Id { get; set; }

        public string Text { get; set; }

        public long UserId { get; set; }
    }
}
