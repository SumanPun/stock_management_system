using System.ComponentModel.DataAnnotations;

namespace api.Dtos.Comment
{
    public class CreateCommentRequest
    {
        [Required]
        [MinLength(5, ErrorMessage = "Title must be more than 5 characters")]
        [MaxLength(280, ErrorMessage = "Title must be less than 280 characters")]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MinLength(5, ErrorMessage = "Content must be more than 5 characters")]
        [MaxLength(280, ErrorMessage = "Title must be less than 280 characters")]
        public string Content { get; set; } = string.Empty;
    }
}