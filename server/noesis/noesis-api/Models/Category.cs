using System;
using System.Collections.Generic;

namespace noesis_api.Models
{
    public class Category
    {
        public long Id { get; set; }

        public string CategoryDescription { get; set; }

        public List<BookCategory> BookCategories { get; set; }
    }
}
