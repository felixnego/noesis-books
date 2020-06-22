using System;
namespace noesis_api.Models
{
    public class UserRating
    {
        public long Id { get; set; }

        public User User { get; set; }
        public long UserId { get; set; }

        public Book Book { get; set; }
        public long BookId { get; set; }

        public int RatingValue { get; set; }
    }
}
