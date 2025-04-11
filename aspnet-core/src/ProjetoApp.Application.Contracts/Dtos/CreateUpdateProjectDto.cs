using ProjetoApp.Domain.Shared;
using System.ComponentModel.DataAnnotations;
namespace ProjetoApp.Projects.Dtos;

public class CreateUpdateProjectDto
{
    [Required]
    [StringLength(ProjectConsts.MaxNameLength)]
    public string Name { get; set; }

    [StringLength(ProjectConsts.MaxDescriptionLength)]
    public string Description { get; set; }
}
