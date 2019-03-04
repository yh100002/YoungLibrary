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
        public async Task<IActionResult> BookList()
        {
            var repo =this.uow.GetRepositoryAsync<BookData>();
            var result = await repo.GetListAsync();    
            return Ok(result);
        }
        
    }
}
