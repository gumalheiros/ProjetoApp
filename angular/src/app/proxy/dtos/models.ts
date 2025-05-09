
export interface TaskPerformanceReportDto {
  userId?: string;
  userName?: string;
  totalTasks: number;
  completedTasks: number;
  completionRate: number;
  averageCompletionTimeInDays: number;
  tasksByPriority: number;
  reportPeriodStart?: string;
  reportPeriodEnd?: string;
}
