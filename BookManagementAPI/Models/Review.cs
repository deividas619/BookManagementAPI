using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookManagementAPI.Models
{
    public class Review : CommonProperties
    {
        public string Text { get; set; }
        public int Rating { get; set; }
        public Guid CreatedByUserId { get; set; }
        public string BookTitle { get; set; } = string.Empty;


    }
}
