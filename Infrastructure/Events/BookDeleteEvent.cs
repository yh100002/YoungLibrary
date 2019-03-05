 

namespace Events
{
    public class BookDeleteEvent : IntegrationEvent
    {
        public int id { get; set; }

        public BookDeleteEvent(int id)
        {
            this.id = id;         
        }
    }
}
