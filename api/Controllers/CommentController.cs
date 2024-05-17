using api.Dtos.Comment;
using api.Dtos.Stock;
using api.Extensions;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/comment")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IStockRepository _stockRepo;
        private readonly UserManager<AppUser> _userManager;
        public CommentController(ICommentRepository commentRepository, IStockRepository stockRepo, UserManager<AppUser> userManager)
        {
            _commentRepository = commentRepository;
            _stockRepo = stockRepo;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _commentRepository.GetAllAsync();
            var commentDto = result.Select(comment => comment.toCommentDto());
            return Ok(commentDto);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var commentDto = await _commentRepository.GetByIdAsync(id);
            if (commentDto == null)
            {
                return NotFound();
            }
            return Ok(commentDto.toCommentDto());
        }

        [HttpPost("{stockId:int}")]
        public async Task<IActionResult> CreateComment([FromRoute] int stockId, [FromBody] CreateCommentRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if(!await _stockRepo.IsStockExit(stockId)) return BadRequest("Stock didnot found");

            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);

            var comment = request.FromCreateRequestToComment(stockId);
            comment.AppUserId = appUser.Id;
            await _commentRepository.CreateAsync(comment);
            return CreatedAtAction(nameof(GetById), new {id = comment.Id}, comment.toCommentDto());
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> updateComment([FromRoute] int id, [FromBody] UpdateCommentRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var comment = request.FromUpdateRequestToComment(request);
            var result = await _commentRepository.updateAsync(id, comment);
            return Ok(result.toCommentDto());
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> deleteComment([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var comment = await _commentRepository.deleteAsync(id);
            if (comment == null)
            {
                return NotFound();
            }
            return Ok("comment deleted successfully!!");
        }
    }
}