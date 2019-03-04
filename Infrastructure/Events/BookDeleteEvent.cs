 

namespace Events
{
    public class BookDeleteEvent : IntegrationEvent
    {
        public int BookId { get; set; }

        public BookDeleteEvent(int BookId)
        {
            this.BookId = BookId;         
        }
    }
}
