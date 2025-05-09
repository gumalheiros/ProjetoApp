import type { TaskPriority } from '../../domain/task-priority.enum';
import type { AuditedEntityDto } from '@abp/ng.core';
import type { ProjectTaskStatus } from '../../domain/project-task-status.enum';

export interface CreateUpdateProjectDto {
  name: string;
  description?: string;
}

export interface CreateUpdateProjectTaskDto {
  title: string;
  description?: string;
  dueDate: string;
  projectId: string;
  priority: TaskPriority;
}

export interface ProjectDto extends AuditedEntityDto<string> {
  name?: string;
  description?: string;
  tasks: ProjectTaskDto[];
}

export interface ProjectTaskDto extends AuditedEntityDto<string> {
  title?: string;
  description?: string;
  dueDate?: string;
  status?: ProjectTaskStatus;
  projectId?: string;
  priority?: TaskPriority;
}
