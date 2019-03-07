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
            var repo = this.uow.GetRepositoryAsync<Book>();

            var book = new Book()
            {                
                id = context.Message.id,
                name = context.Message.name,
                desc = context.Message.desc,
                price = context.Message.price,
                updated_at = context.Message.updated_at   
            };

            repo.UpdateAsync(book);

            this.uow.SaveChanges();
        }       
    }
}