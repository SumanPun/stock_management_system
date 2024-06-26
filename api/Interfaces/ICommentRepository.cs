using api.Dtos.Comment;
using api.Models;

namespace api.Interfaces
{
    public interface ICommentRepository
    {
        Task<List<Comment>> GetAllAsync();
        Task<Comment?> GetByIdAsync(int id);
        Task<Comment?> CreateAsync(Comment comment);
        Task<Comment?> updateAsync(int id, Comment comment); 
        Task<Comment?> deleteAsync(int id);
    }
}