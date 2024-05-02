using System;
using System.Collections.Generic;

namespace BookManagementAPI.Models
{
    public class Book : CommonProperties
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public DateOnly Publication { get; set; }
        public Genre Genre { get; set; }
        public Guid CreatedByUserId { get; set; }
        public virtual ICollection<Review>? Reviews { get; set; }
        public decimal AverageRating { get; set; } = 0;
        public int SearchedTimes { get; set; } = 0;
    }
}