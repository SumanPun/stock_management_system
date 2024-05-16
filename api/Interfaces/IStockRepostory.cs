using api.Dtos.Stock;
using api.Helpers;
using api.Models;

namespace api.Interfaces
{

    public interface IStockRepository
    {

        Task<List<Stock>> GetAllAsync(QueryObject query);
        Task<Stock?> GetByIdAsync(int id);
        Task<Stock> CreateAsync(Stock stock);
        Task<Stock?> updateAsync(int id, UpdateStockRequest stockDto);
        Task<Stock?> DeleteAsync(int id);
        Task<bool> IsStockExit(int id);
    }
}