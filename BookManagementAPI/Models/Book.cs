﻿using System;

namespace BookManagementAPI.Models
{
    public class Book : CommonProperties
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public DateOnly Publication { get; set; }
        public Genre Genre { get; set; }
    }
}
