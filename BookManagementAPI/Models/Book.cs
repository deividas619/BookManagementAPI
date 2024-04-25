using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookManagementAPI.Models
{
    public class Book : CommonProperties
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public DateOnly Publication { get; set; }
        public Genre Genre { get; set; }
        //[ForeignKey("CreatedByUser")]
        public Guid CreatedByUserId { get; set; }
        //public virtual User CreatedByUser { get; set; }
    }
}
