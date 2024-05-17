using api.Extensions;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("/api/portfolio")]
    [ApiController]
    public class PortfolioController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IStockRepository _stockRepo;
        private readonly IPortfoliRepository _portfolioRepo;
        public PortfolioController(UserManager<AppUser> userManager, IStockRepository stockRepo, IPortfoliRepository portfoliRepo)
        {
            _userManager = userManager;
            _stockRepo = stockRepo;
            _portfolioRepo = portfoliRepo;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserPortfolio()
        {
            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);
            var userPortfolio = await _portfolioRepo.GetUserPortfolio(appUser);
            return Ok(userPortfolio);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreatPortfolio(string symbol)
        {
            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);
            var stock = await _stockRepo.GetBySymbolAsync(symbol);
            if (stock == null) return BadRequest("Stock not found");
            var userPortfolio = await _portfolioRepo.GetUserPortfolio(appUser);

            if (userPortfolio.Any(x => x.Symbol.ToLower() == symbol.ToLower())) return BadRequest("Same stock cannot added");

            var portfolioModel = new Portfolio
            {
                AppUserId = appUser.Id,
                StockId = stock.Id
            };

            await _portfolioRepo.CreateAsync(portfolioModel);
            if (portfolioModel == null)
                return StatusCode(500, "Cannot create");
            else
                return Created();
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeletePortfolio(string symbol)
        {
            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);
            
            var userPortfolio = await _portfolioRepo.GetUserPortfolio(appUser);

            var filterStock = userPortfolio.Where(x=>x.Symbol.ToLower() == symbol.ToLower()).ToList();
            
            if(filterStock.Count() == 1)
            {
                await _portfolioRepo.DeletePortfolio(appUser,symbol);
            }else{
                return BadRequest("Stock not in your Portfolio");
            }

            return Ok("Deleted successfully!!");

        }


    }
}