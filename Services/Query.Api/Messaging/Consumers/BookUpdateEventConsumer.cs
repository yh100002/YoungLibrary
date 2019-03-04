using System.Threading.Tasks;
using Data;
using Events;
using MassTransit;
using Models;


namespace Query.Api.Messaging.Consumers
{
    public class BookUpdateEventConsumer : IConsumer<BookUpdateEvent>
    {
        private readonly IUnitOfWork uow;

        public BookUpdateEventConsumer(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task Consume(ConsumeContext<BookUpdateEvent> context)
        {
            var repo = this.uow.GetRepositoryAsync<BookData>();

            var product = new BookData()
            {
                BookId = context.Message.BookId,
                Name = context.Message.Name,
                Description = context.Message.Description               
            };

            repo.UpdateAsync(product);

            this.uow.SaveChanges();
        }       
    }
}