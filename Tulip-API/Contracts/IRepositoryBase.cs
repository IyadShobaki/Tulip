using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tulip_API.Contracts
{
    // Generic (Its function relative to whatever class we choose
    // from the Data folder
    public interface IRepositoryBase<T> where T : class 
    {
        Task<IList<T>> FindAll();
        Task<T> FindById(int id);
        Task<bool> Create(T entity);
        Task<bool> Update(T entity);
        Task<bool> Delete(T entity);
        Task<bool> Save();  // To commit the changes to the database
    }
}
