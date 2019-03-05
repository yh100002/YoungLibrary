using System;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Book
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }      
        public string desc { get; set; }       
        public decimal price { get; set; }
        public DateTime updated_at { get; set; } 
    }
}
