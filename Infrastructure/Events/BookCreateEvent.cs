 

namespace Events
{
    public class BookCreateEvent : IntegrationEvent
    {
        public int BookId { get; set; }
        public string Name { get; set; }      
        public string Description { get; set; }         
        public BookCreateEvent(int BookId, string Name, string Description)
        {
            this.BookId = BookId;
            this.Name = Name;
            this.Description = Description;           
        }
    }
}
