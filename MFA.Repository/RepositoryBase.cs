using Dapper;
using Dapper.Contrib.Extensions;
using MFA.Entities.Configurations;
using MFA.IRepository;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace MFA.Repository
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        private readonly DatabaseConnectionStrings _connectionStrings;
        private readonly string _tableName;

        public RepositoryBase(IOptions<DatabaseConnectionStrings> connectionStrings)
        {
            _connectionStrings = connectionStrings.Value;
            _tableName = typeof(T).Name;
            SqlMapperExtensions.TableNameMapper = (T) =>
            {
                return T.Name;
            };
        }

        internal IDbConnection Connection
        {
            get
            {
                return new SqlConnection(_connectionStrings.MFADB);
            }
        }

        public Task InsertAsync(T item)
        {
            using (IDbConnection databaseConnection = Connection)
            {
                return databaseConnection.InsertAsync(item);
            }
        }

        public Task InsertAllAsync(IEnumerable<T> items)
        {
            using (IDbConnection databaseConnection = Connection)
            {
                return databaseConnection.InsertAsync(items);
            }
        }

        public Task<IEnumerable<T>> GetAllAsync(int offset, int fetch, string sort)
        {
            var query = string.Format("SELECT * FROM {1} ORDER BY Id {0} OFFSET @offset ROWS FETCH NEXT @fetch ROWS ONLY", sort, _tableName);

            using (IDbConnection databaseConnection = Connection)
            {
                return databaseConnection.QueryAsync<T>(query, new { offset, fetch });
            }
        }

        public Task<T> GetAsync(int id)
        {
            var query = string.Format("SELECT * FROM {0} WHERE Id = @id", _tableName);

            using (IDbConnection databaseConnection = Connection)
            {
                return databaseConnection.QueryFirstAsync<T>(query, new { id });
            }
        }

        public Task DeleteAsync(int id)
        {
            var query = string.Format("DELETE FROM {0} WHERE Id = @id", _tableName);

            using (IDbConnection databaseConnection = Connection)
            {
                return databaseConnection.ExecuteAsync(query, new { id });
            }
        }

        public Task<int> TotalRecordCountAsync()
        {
            var query = string.Format("SELECT COUNT(Id) FROM {0}", _tableName);

            using (IDbConnection databaseConnection = Connection)
            {
                return databaseConnection.QueryFirstAsync<int>(query);
            }
        }

        public Task UpdateAsync(T item)
        {
            using (IDbConnection databaseConnection = Connection)
            {
                return databaseConnection.UpdateAsync(item);
            }
        }
    };
}
