using System;
using System.ComponentModel.DataAnnotations;

namespace noesis_api.Models
{
    public class Note
    {
        public long Id { get; set; }

        [Required]
        public string Text { get; set; }
    }
}
