using Company.G02.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.G02.BLL.Interfaces
{
    public interface IGenaricRepository<T> where T : BaseEntity
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetAsync(int id);
        Task AddAsync(T model);
        void Update(T model);
        void Delete(T model);
       

    }
}
