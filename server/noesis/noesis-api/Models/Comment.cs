using System;
using System.ComponentModel.DataAnnotations;

namespace noesis_api.Models
{
    public class Comment
    {
        public long Id { get; set; }

        [Required]
        public string Text { get; set; }

        public DateTime AddedOn { get; set; }
    }
}
