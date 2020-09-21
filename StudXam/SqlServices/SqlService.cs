using SQLite;
using StudXam.Interfaces;
using StudXam.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudXam.SqlServices
{
    public class SqlService : ISqlService
    {
        public SqlService(ISQLiteDb db)
        {
            _connection = db.GetConnection();
            _connection.CreateTableAsync<Student>();
            _connection.CreateTableAsync<FileModel>();
        }

        private SQLiteAsyncConnection _connection;

        async Task<IEnumerable<T>> ISqlService.GetAllDataAsync<T>()
        {
            return await _connection.Table<T>().ToListAsync();
        }

        async public Task AddData<T>(T data)
        {
            await _connection.InsertAsync(data);
        }

        public async Task AddAllData<T>(List<T> listData)
        {
            await _connection.InsertAllAsync(listData);
        }

        public async Task UpdateData<T>(T data)
        {
            await _connection.UpdateAsync(data);
        }
    }
}