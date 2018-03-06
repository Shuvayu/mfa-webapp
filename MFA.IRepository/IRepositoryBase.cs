using System.Collections.Generic;
using System.Threading.Tasks;

namespace MFA.IRepository
{
    public interface IRepositoryBase<T> where T : class
    {
        /// <summary>
        /// Adds a record
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        Task InsertAsync(T item);

        /// <summary>
        /// Adds a list of records
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        Task InsertAllAsync(IEnumerable<T> items);

        /// <summary>
        /// Deletes a record related to the id
        /// </summary>
        /// <param name="id">Identity Key</param>
        /// <returns></returns>
        Task DeleteAsync(int id);

        /// <summary>
        /// Update a record
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        Task UpdateAsync(T item);

        /// <summary>
        /// Gets a record related to the id
        /// </summary>
        /// <param name="id">Identity Key</param>
        /// <returns></returns>
        Task<T> GetAsync(int id);

        /// <summary>
        /// Gets all data present in that table.
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="fetch"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        Task<IEnumerable<T>> GetAllAsync(int offset, int fetch, string sort);

        /// <summary>
        /// Gets the total record count of the table.
        /// </summary>
        /// <returns></returns>
        Task<int> TotalRecordCountAsync();
    }
}
