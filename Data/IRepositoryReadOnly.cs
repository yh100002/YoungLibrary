using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using Data.Paging;

namespace Data
{
    public interface IRepositoryReadOnly<T> : IReadRepository<T> where T : class
    {
       
    }
}