using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudXam.Interfaces
{
    public interface ISqlService
    {
        Task<IEnumerable<T>> GetAllDataAsync<T>() where T : new();

        Task AddData<T>(T data);

        Task UpdateData<T>(T data);

        Task AddAllData<T>(List<T> listData);
    }
}