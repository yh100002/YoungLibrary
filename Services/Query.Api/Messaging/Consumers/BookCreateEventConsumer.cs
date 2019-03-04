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
            var repo = this.uow.GetRepositoryAsync<BookData>();

            var product = new BookData()
            {
                BookId = context.Message.BookId,
                Name = context.Message.Name,
                Description = context.Message.Description               
            };

            await repo.AddAsync(product);    

            this.uow.SaveChanges();

        }
    }
}