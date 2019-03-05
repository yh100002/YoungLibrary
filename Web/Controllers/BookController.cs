using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Web.Configuration;
using Microsoft.AspNetCore.Mvc.Versioning;
using Models;
using Http;
using Web.Dto;
using AutoMapper;
using Newtonsoft.Json;
using Data;
using Data.Paging;
using Microsoft.Extensions.Logging;

namespace Web.Controllers
{   

    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IOptions<LibrarySettings> librarySettings;          
        private readonly IHttpClient apiClient;
        private readonly IMapper mapper;
        private readonly ILogger<BookController> logger;

        public BookController(IOptions<LibrarySettings> librarySettings, IHttpClient apiClient, IMapper mapper, ILogger<BookController> logger)
        {
            this.librarySettings = librarySettings;
            this.apiClient = apiClient;    
            this.mapper = mapper; 
            this.logger = logger;
        }
        
        
        [HttpGet("getBooks")]
        public async Task<IActionResult> GetBooks(int page, int size)
        {
            var response = await this.apiClient.GetStringAsync(this.librarySettings.Value.BookQueryApiUrl + $"/api/bookquery/booklist?page={page}&size={size}");
            return Ok(response);
        }

        [HttpGet("getBook/{id}")]
        public async Task<IActionResult> GetBook(int id)
        {
            var response = await this.apiClient.GetStringAsync(this.librarySettings.Value.BookQueryApiUrl + $"/api/bookquery/getbook/{id}");
            return Ok(response);
        }
        
        [HttpDelete("{id}")]
        public async Task Delete(string id)
        {           
            await this.apiClient.PostAsync(this.librarySettings.Value.BookCommandApiUrl + "/api/bookcommand/delete", id);
        }

        [HttpPut("update")]
        public async Task Put([FromBody] Book book)
        {            
            //Create or Update
            await this.apiClient.PostAsync(this.librarySettings.Value.BookCommandApiUrl + "/api/bookcommand/create", book);
        }

        [HttpPost("addBook")]
        public async Task AddBook([FromBody] Book book)
        {            
            //Create or Update
            await this.apiClient.PostAsync(this.librarySettings.Value.BookCommandApiUrl + "/api/bookcommand/create", book);
        }
    }
}
