 

namespace Events
{
    public class BookUpdateEvent : IntegrationEvent
    {
        public int BookId { get; set; }
        public string Name { get; set; }      
        public string Description { get; set; }         
        public BookUpdateEvent(int BookId, string Name, string Description)
        {
            this.BookId = BookId;
            this.Name = Name;
            this.Description = Description;           
        }
    }
}
