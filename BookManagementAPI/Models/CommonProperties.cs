using System;
using System.ComponentModel;

namespace BookManagementAPI.Models
{
    public class CommonProperties
    {
        [DefaultValue("00000000-0000-0000-0000-000000000000")]
        public Guid Id { get; set; }
    }
}
