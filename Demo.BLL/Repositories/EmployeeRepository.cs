using Demo.BLL.Interfaces;
using Demo.DAL.Contexts;
using Demo.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repositories
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        private readonly MvcDbContext _dbContext;// we difined a new dbcontext to use it in our new method
        // because the dbcontext in the base class is private we can't access it here
        // and because we need to use it in our new method we will define it here again

        public EmployeeRepository(MvcDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Employee>> GetEMployeeByName(string searchValue)
        {
            return await _dbContext.Employees
                .Where(e => e.Name.ToLower().Contains(searchValue.ToLower()))
                .ToListAsync();
        }


        public async Task<IEnumerable<Employee>> GetEmployeesByAddress(string address)
        {
            return await _dbContext.Employees
                .Where(e => e.Address == address)
                .ToListAsync();
        }

    }
}
