using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Data;
using Models;
using Events;
using MassTransit;

namespace Command.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookCommandController : ControllerBase
    {
        private readonly IUnitOfWork uow;
        private readonly IBus messageBus;
        public BookCommandController(IUnitOfWork uow, IBus messageBus)
        {
            this.uow = uow;
            this.messageBus = messageBus;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] BookData book)
        {
            var repo = this.uow.GetRepositoryAsync<BookData>();               
            var existed = repo.SingleAsync(s => s.BookId == book.BookId);
            if(existed.Result != null)
            {
                //Console.WriteLine("Updated:" + existed.Result.Name);
                repo.UpdateAsync(existed.Result);
                this.uow.SaveChanges();
                 await this.messageBus.Publish<BookUpdateEvent>(new 
                    { 
                        book.BookId, 
                        book.Name, 
                        book.Description 
                    }
                    );
                return Ok();               
            }
            //Console.WriteLine("Created:" + product.Name);
            await repo.AddAsync(book);    
            this.uow.SaveChanges();   

            await this.messageBus.Publish<BookCreateEvent>(new 
                {   book.BookId, 
                    book.Name, 
                    book.Description  }
                );

            return Ok();
        }

        [HttpPost("delete")]
        public async Task<IActionResult> Delete([FromBody] string bookId)
        {            
            var repo = this.uow.GetRepository<BookData>();
            repo.Delete(bookId);   
            this.uow.SaveChanges();
            await this.messageBus.Publish<BookDeleteEvent>(new { bookId } );
            return Ok("delete");
        }
    }
}
