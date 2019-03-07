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
        public async Task<IActionResult> Create([FromBody] Book book)
        {
            book.id = Guid.NewGuid();
            var repo = this.uow.GetRepositoryAsync<Book>();    
            Console.WriteLine("Created:" + book.name);
            book.updated_at = DateTime.Now;
            await repo.AddAsync(book);    
            this.uow.SaveChanges();   

            await this.messageBus.Publish<BookCreateEvent>(new 
            {   book.id, 
                book.name, 
                book.desc, 
                book.price,
                book.updated_at  }
            );

            return Ok(book);
        }

        [HttpPost("update")]
        public async Task<IActionResult> Update([FromBody] Book book)
        {
            var repo = this.uow.GetRepositoryAsync<Book>();    
            book.updated_at = DateTime.Now;
            repo.UpdateAsync(book);           
            this.uow.SaveChanges();
            await this.messageBus.Publish<BookUpdateEvent>(new 
            { 
                book.id, 
                book.name, 
                book.desc, 
                book.price,
                book.updated_at
            }
            );
            return Ok(book); 
        }  

        [HttpPost("delete")]
        public async Task<IActionResult> Delete([FromBody] Guid id)
        {            
            var repo = this.uow.GetRepository<Book>();
            Console.WriteLine(id);
            repo.Delete(id);   
            this.uow.SaveChanges();
            await this.messageBus.Publish<BookDeleteEvent>(new { id } );
            return Ok("delete");
        }
    }
}
