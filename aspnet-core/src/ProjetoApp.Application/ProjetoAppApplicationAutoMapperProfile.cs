using AutoMapper;
using ProjetoApp.Projects.Dtos;
using ProjetoApp.Domain;
using ProjetoApp.Domain.Dtos;
using ProjetoApp.Dtos;

namespace ProjetoApp;

public class ProjetoAppApplicationAutoMapperProfile : Profile
{
    public ProjetoAppApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */

        CreateMap<Project, ProjectDto>();
        CreateMap<ProjectTask, ProjectTaskDto>();
        CreateMap<CreateUpdateProjectDto, Project>();
        CreateMap<CreateUpdateProjectTaskDto, ProjectTask>();
        CreateMap<TaskHistory, TaskHistoryDto>();
        CreateMap<TaskComment, TaskCommentDto>();
        CreateMap<TaskPerformanceReport, TaskPerformanceReportDto>();
        CreateMap<Customer, CustomerDto>();
        CreateMap<CreateUpdateCustomerDto, Customer>(MemberList.Source);
    }
}
