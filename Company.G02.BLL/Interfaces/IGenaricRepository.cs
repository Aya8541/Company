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
        IEnumerable<T> GetAll();
        T? Get(int id);
        void Add(T model);
        void Update(T model);
        void Delete(T model);
       

    }
}
