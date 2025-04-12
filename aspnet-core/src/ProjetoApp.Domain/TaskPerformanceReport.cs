using System;
using Volo.Abp.Domain.Entities;

namespace ProjetoApp.Domain;

public class TaskPerformanceReport : Entity
{
    public Guid UserId { get; set; }
    public int TotalTasks { get; set; }
    public int CompletedTasks { get; set; }
    public double CompletionRate { get; set; }
    public double AverageCompletionTimeInDays { get; set; }
    public int TasksByPriority { get; set; }
    public DateTime ReportPeriodStart { get; set; }
    public DateTime ReportPeriodEnd { get; set; }

    public TaskPerformanceReport() { }

    public TaskPerformanceReport(
        Guid userId,
        int totalTasks,
        int completedTasks,
        double completionRate,
        double averageCompletionTimeInDays,
        int tasksByPriority,
        DateTime reportPeriodStart,
        DateTime reportPeriodEnd)
    {
        UserId = userId;
        TotalTasks = totalTasks;
        CompletedTasks = completedTasks;
        CompletionRate = completionRate;
        AverageCompletionTimeInDays = averageCompletionTimeInDays;
        TasksByPriority = tasksByPriority;
        ReportPeriodStart = reportPeriodStart;
        ReportPeriodEnd = reportPeriodEnd;
    }

    public override object[] GetKeys()
    {
        return new object[] { UserId, ReportPeriodStart, ReportPeriodEnd };
    }
}
