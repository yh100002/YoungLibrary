using System.Threading.Tasks;
using Data;
using Events;
using MassTransit;
using Models;

namespace Query.Api.Messaging.Consumers
{
    public class BookCreateEventConsumer : IConsumer<BookCreateEvent>
    {
        private readonly IUnitOfWork uow;

        public BookCreateEventConsumer(IUnitOfWork uow)
        {
            this.uow = uow;
        }
        
        public async Task Consume(ConsumeContext<BookCreateEvent> context)
        {
            var repo = this.uow.GetRepositoryAsync<Book>();

            var book = new Book()
            {                
                name = context.Message.name,
                desc = context.Message.desc,
                price = context.Message.price,
                updated_at = context.Message.updated_at   
            };

            await repo.AddAsync(book);    

            this.uow.SaveChanges();

        }
    }
}