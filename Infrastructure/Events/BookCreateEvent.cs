

using System;

namespace Events
{
    public class BookCreateEvent : IntegrationEvent
    {
        public Guid id { get; set; }
        public string name { get; set; }      
        public string desc { get; set; }       
        public decimal price { get; set; }
        public DateTime updated_at { get; set; }      
        public BookCreateEvent(Guid id, string name, string desc, decimal price, DateTime updated_at)
        {
            this.id = id;
            this.name = name;
            this.desc = desc;
            this.price = price;
            this.updated_at = updated_at; 
        }
    }
}
