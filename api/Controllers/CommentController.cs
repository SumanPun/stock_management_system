using api.Dtos.Comment;
using api.Dtos.Stock;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/comment")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepository;
        public CommentController(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
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
            var comment = request.FromCreateRequestToComment(request, stockId);
            var result = await _commentRepository.CreateAsync(stockId, comment);
            return Ok(result.toCommentDto());
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