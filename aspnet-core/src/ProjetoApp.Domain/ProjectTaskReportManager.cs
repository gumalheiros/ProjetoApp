using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;
using Volo.Abp.Domain.Repositories;

namespace ProjetoApp.Domain;

public class ProjectTaskReportManager : DomainService
{
    private readonly IRepository<ProjectTask, Guid> _taskRepository;
    private readonly IRepository<TaskHistory, Guid> _historyRepository;

    public ProjectTaskReportManager(
        IRepository<ProjectTask, Guid> taskRepository,
        IRepository<TaskHistory, Guid> historyRepository)
    {
        _taskRepository = taskRepository;
        _historyRepository = historyRepository;
    }

    public async Task<List<TaskPerformanceReport>> GenerateUserPerformanceReportAsync(
        DateTime startDate,
        DateTime endDate)
    {
        var tasksQuery = await _taskRepository.WithDetailsAsync(); // Await the Task<IQueryable<ProjectTask>>
        var tasks = tasksQuery
            .Where(t => t.CreationTime >= startDate && t.CreationTime <= endDate)
            .ToList();

        var historiesQuery = await _historyRepository.WithDetailsAsync(); // Await the Task<IQueryable<TaskHistory>>
        var histories = historiesQuery
            .Where(h => h.ChangedAt >= startDate &&
                       h.ChangedAt <= endDate &&
                       h.FieldName == "Status" &&
                       h.NewValue == ProjectTaskStatus.Completed.ToString())
            .ToList();

        var report = tasks
            .GroupBy(t => t.CreatorId)
            .Select(g => new TaskPerformanceReport
            {
                UserId = g.Key ?? Guid.Empty,
                TotalTasks = g.Count(),
                CompletedTasks = g.Count(t => t.Status == ProjectTaskStatus.Completed),
                CompletionRate = g.Count() > 0
                    ? (double)g.Count(t => t.Status == ProjectTaskStatus.Completed) / g.Count()
                    : 0,
                AverageCompletionTimeInDays = CalculateAverageCompletionTime(g.ToList(), histories),
                TasksByPriority = g.Count(t => t.Priority == TaskPriority.High),
                ReportPeriodStart = startDate,
                ReportPeriodEnd = endDate
            })
            .ToList();

        return report;
    }

    private double CalculateAverageCompletionTime(
        List<ProjectTask> tasks,
        List<TaskHistory> histories)
    {
        var completedTasks = tasks
            .Where(t => t.Status == ProjectTaskStatus.Completed)
            .ToList();

        if (!completedTasks.Any())
            return 0;

        var totalDays = completedTasks.Sum(task =>
        {
            var completionRecord = histories
                .FirstOrDefault(h => h.TaskId == task.Id);

            if (completionRecord == null)
                return 0;

            return (completionRecord.ChangedAt - task.CreationTime).TotalDays;
        });

        return totalDays / completedTasks.Count;
    }
}
