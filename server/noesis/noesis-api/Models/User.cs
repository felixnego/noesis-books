using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace noesis_api.Models
{
    public class User
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Username { get; set; }

        [JsonIgnore]
        public string Password { get; set; }

        public string Email { get; set; }

        public DateTime CreationDate { get; set; }

        public string Token { get; set; }

        public List<UserRating> UserRatings { get; set; }

        public List<UserComment> UserComments { get; set; }

        public List<UserNote> UserNotes { get; set; }

        public string UserRole { get; set; } = "User";
    }
}
