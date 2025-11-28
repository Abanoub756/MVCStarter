using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        // signature for properties for each repository interface
        public IEmployeeRepository EmployeeRepository { get; set; }
        public IDepartmentRepository DepartmentRepository { get; set; }

        public Task<int> CompleteAsync(); // to save changes
        public void Dispose(); // to release resources

    }
}
