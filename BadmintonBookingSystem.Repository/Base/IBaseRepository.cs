using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadmintonBookingSystem.Repository.Base
{
    public interface IBaseRepository<T> where T : class
    {
        Task<T> GetById(string id);
        IQueryable<T> GetAll();
        Task Add(T entity);
        Task Update(T entity);
        Task Delete(T entity);

    }
}
