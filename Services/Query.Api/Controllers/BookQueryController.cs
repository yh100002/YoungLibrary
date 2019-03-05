using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Data;
using Events;
using MassTransit;
using Models;

namespace Query.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookQueryController : ControllerBase
    {
        private readonly IUnitOfWork uow;

        public BookQueryController(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        [HttpGet("booklist")]
        public async Task<IActionResult> BookList(int page, int size)
        {
            var repo =this.uow.GetRepositoryAsync<Book>();
            var result = await repo.GetListAsync(index:page, size:size);    
            return Ok(result);
        }
        
        [HttpGet("getBook/{id}")]
        public async Task<IActionResult> GetBook(int id)
        {
            var repo =this.uow.GetRepositoryAsync<Book>();
            var result = await repo.SingleAsync(s => s.id == id);
            return Ok(result);
        }
    }
}
