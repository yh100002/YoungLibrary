using System;
using System.Threading.Tasks;
using Data;
using Events;
using MassTransit;
using Models;

namespace Query.Api.Messaging.Consumers
{
    public class BookDeleteEventConsumer : IConsumer<BookDeleteEvent>
    {
        private readonly IUnitOfWork uow;

        public BookDeleteEventConsumer(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task Consume(ConsumeContext<BookDeleteEvent> context)
        {
            var repo = this.uow.GetRepository<BookData>();          

            Console.WriteLine("Consume=======>" + context.Message.BookId);
            repo.Delete(context.Message.BookId);

            this.uow.SaveChanges();
        }           
    }
}