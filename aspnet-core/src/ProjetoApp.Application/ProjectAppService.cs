using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ProjetoApp.Domain;
using ProjetoApp.Projects.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Users;

namespace ProjetoApp.Projects;

public class ProjectAppService : 
    CrudAppService<
        Project,
        ProjectDto,
        Guid,
        PagedAndSortedResultRequestDto,
        CreateUpdateProjectDto,
        CreateUpdateProjectDto>,
    IProjectAppService
{
    private readonly ICurrentUser _currentUser;

    public ProjectAppService(
        IRepository<Project, Guid> repository,
        ICurrentUser currentUser) : base(repository)
    {
        _currentUser = currentUser;
    }

    public async Task<PagedResultDto<ProjectDto>> GetMyProjectsAsync(PagedAndSortedResultRequestDto input)
    {
        await CheckGetListPolicyAsync();

        var query = await Repository.GetQueryableAsync();
        query = query
            .Where(x => x.UserId == _currentUser.Id.Value);

        if (!string.IsNullOrWhiteSpace(input.Sorting))
        {
            query = query.OrderBy(x => EF.Property<object>(x, input.Sorting)); // Use dynamic sorting with EF.Property
        }
        else
        {
            query = query.OrderBy(x => x.Name); // Default sorting
        }

        var totalCount = await AsyncExecuter.CountAsync(query);
        var items = await AsyncExecuter.ToListAsync(
            query.PageBy(input.SkipCount, input.MaxResultCount)
        );

        return new PagedResultDto<ProjectDto>(
            totalCount,
            ObjectMapper.Map<List<Project>, List<ProjectDto>>(items)
        );
    }
}
