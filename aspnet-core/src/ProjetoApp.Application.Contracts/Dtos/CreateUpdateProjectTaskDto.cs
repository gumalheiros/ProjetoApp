using ProjetoApp.Domain.Shared;
using System;
using System.ComponentModel.DataAnnotations;

namespace ProjetoApp.Projects.Dtos;

public class CreateUpdateProjectTaskDto
{
    [Required]
    [StringLength(ProjectTaskConsts.MaxTitleLength)]
    public string Title { get; set; }

    [StringLength(ProjectTaskConsts.MaxDescriptionLength)]
    public string Description { get; set; }

    [Required]
    public DateTime DueDate { get; set; }

    [Required]
    public Guid ProjectId { get; set; }
}
