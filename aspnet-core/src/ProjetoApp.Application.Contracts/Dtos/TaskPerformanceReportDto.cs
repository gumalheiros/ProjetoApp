using System;

namespace ProjetoApp.Dtos;

public class TaskPerformanceReportDto
{
    public Guid UserId { get; set; }
    public string UserName { get; set; }
    public int TotalTasks { get; set; }
    public int CompletedTasks { get; set; }
    public double CompletionRate { get; set; }
    public double AverageCompletionTimeInDays { get; set; }
    public int TasksByPriority { get; set; }
    public DateTime ReportPeriodStart { get; set; }
    public DateTime ReportPeriodEnd { get; set; }
}
