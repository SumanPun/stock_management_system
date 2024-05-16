using api.Dtos.Stock;
using api.Models;

namespace api.Mappers
{
    public static class StockMappers
    {
        public static StockDto ToStockDto(this Stock stockModel)
        {
            return new StockDto
            {
                Id = stockModel.Id,
                Symbol = stockModel.Symbol,
                CompanyName = stockModel.CompanyName,
                Purchase = stockModel.Purchase,
                LastDividend = stockModel.LastDividend,
                Industry = stockModel.Industry,
                MarketCap = stockModel.MarketCap,
                Comments = stockModel.Comments.Select(comment => comment.toCommentDto()).ToList()
            };
        }

        public static Stock ToStockFromCreateDto(this CreateStockRequest stockRequest)
        {
            return new Stock
            {
                Symbol = stockRequest.Symbol,
                CompanyName = stockRequest.CompanyName,
                Purchase = stockRequest.Purchase,
                LastDividend = stockRequest.LastDividend,
                Industry = stockRequest.Industry,
                MarketCap = stockRequest.MarketCap
            };
        }
    }
}