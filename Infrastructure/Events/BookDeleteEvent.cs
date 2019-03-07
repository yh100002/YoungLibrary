

using System;

namespace Events
{
    public class BookDeleteEvent : IntegrationEvent
    {
        public Guid id { get; set; }

        public BookDeleteEvent(Guid id)
        {
            this.id = id;         
        }
    }
}
