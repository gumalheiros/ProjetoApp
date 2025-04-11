using System.ComponentModel.DataAnnotations;

public class CreateTaskCommentDto
{
    [Required]
    public string Content { get; set; }
}