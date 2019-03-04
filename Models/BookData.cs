using System;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class BookData
    {
        [Key]
        public int BookId { get; set; }
        public string Name { get; set; }      
        public string Description { get; set; }           
    }
}
