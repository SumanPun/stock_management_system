using api.Models;

namespace api.Interfaces
{
    public interface IPortfoliRepository
    {
        
        Task<List<Stock>> GetUserPortfolio(AppUser user);
        Task<Portfolio> CreateAsync(Portfolio portfolio);
        Task<Portfolio> DeletePortfolio(AppUser appUser, string symbol);
    }
}