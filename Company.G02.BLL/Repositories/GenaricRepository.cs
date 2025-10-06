using Company.G02.BLL.Interfaces;
using Company.G02.DAL.Data.Contexts;
using Company.G02.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.G02.BLL.Repositories
{
    public class GenaricRepository<T> : IGenaricRepository<T> where T : BaseEntity
    {
        private readonly CompanyDbContext _context;

        //ASK CLR Create Object of CompanyDbContext

        public GenaricRepository(CompanyDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            if (typeof(T) == typeof(Employee)) {
                return (IEnumerable<T>)await _context.Employees.Include(E=>E.Department).ToListAsync();

            }
            return await _context.Set<T>().ToListAsync();

        }

        public async Task <T?> GetAsync(int id)
        {
            if (typeof(T) == typeof(Employee))
            {
                return await _context.Employees.Include(E => E.Department).FirstOrDefaultAsync(e => e.Id == id) as T;

            }
            return await _context.Set<T>().FindAsync(id);
        }
        public async Task AddAsync(T model)
        {
           await _context.Set<T>().AddAsync(model);
        }


        public void Update(T model)
        {
            _context.Set<T>().Update(model);
        }



        public void Delete(T model)
        {
           
            _context.Set<T>().Remove(model);
        }

      

    }
}
