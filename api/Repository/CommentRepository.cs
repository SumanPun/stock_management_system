using api.Data;
using api.Dtos.Comment;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class CommentRepository : ICommentRepository
    {

        private readonly ApplicationDbContext _context;
        private readonly IStockRepository _stockRepo; 
        public CommentRepository(ApplicationDbContext context, IStockRepository stockRepo)
        {
            _context = context;
            _stockRepo = stockRepo;
        }

        public async Task<Comment?> CreateAsync(Comment comment)
        {
            comment.CreatedOn = DateTime.Now;
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
            return comment;
        }

        public async Task<Comment?> deleteAsync(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
            {
                return null;
            }
            _context.Remove(comment);
            await _context.SaveChangesAsync();
            return comment;
        }

        public async Task<List<Comment>> GetAllAsync()
        {
            return await _context.Comments.Include(x=>x.AppUser).ToListAsync();
        }

        public async Task<Comment?> GetByIdAsync(int id)
        {
            var comment = await _context.Comments.Include(x=>x.AppUser).FirstOrDefaultAsync(x => x.Id == id);
            if (comment == null)
            {
                return null;
            }
            return comment;
        }

        public async Task<Comment?> updateAsync(int id, Comment updateComment)
        {
            var comment = await _context.Comments.FirstOrDefaultAsync(x => x.Id == id);
            if (comment == null)
            {
                return null;
            }
            comment.Title = updateComment.Title;
            comment.Content = updateComment.Content;
            await _context.SaveChangesAsync();
            return comment;
        }
    }
}