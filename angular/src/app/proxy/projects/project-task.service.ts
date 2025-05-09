import type { CreateUpdateProjectTaskDto, ProjectTaskDto } from './dtos/models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedAndSortedResultRequestDto, PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { ProjectTaskStatus } from '../domain/project-task-status.enum';
import type { TaskPerformanceReportDto } from '../dtos/models';

@Injectable({
  providedIn: 'root',
})
export class ProjectTaskService {
  apiName = 'Default';
  

  addComment = (taskId: string, input: CreateTaskCommentDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, TaskCommentDto>({
      method: 'POST',
      url: `/api/app/project-task/comment/${taskId}`,
      body: input,
    },
    { apiName: this.apiName,...config });
  

  create = (input: CreateUpdateProjectTaskDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ProjectTaskDto>({
      method: 'POST',
      url: '/api/app/project-task',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/app/project-task/${id}`,
    },
    { apiName: this.apiName,...config });
  

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ProjectTaskDto>({
      method: 'GET',
      url: `/api/app/project-task/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getComments = (taskId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, TaskCommentDto[]>({
      method: 'GET',
      url: `/api/app/project-task/comments/${taskId}`,
    },
    { apiName: this.apiName,...config });
  

  getHistory = (taskId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, TaskHistoryDto[]>({
      method: 'GET',
      url: `/api/app/project-task/history/${taskId}`,
    },
    { apiName: this.apiName,...config });
  

  getList = (input: PagedAndSortedResultRequestDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<ProjectTaskDto>>({
      method: 'GET',
      url: '/api/app/project-task',
      params: { sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  getPerformanceReport = (startDate?: string, endDate?: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, TaskPerformanceReportDto[]>({
      method: 'GET',
      url: '/api/app/project-task/performance-report',
      params: { startDate, endDate },
    },
    { apiName: this.apiName,...config });
  

  getProjectTasks = (projectId: string, input: PagedAndSortedResultRequestDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<ProjectTaskDto>>({
      method: 'GET',
      url: `/api/app/project-task/project-tasks/${projectId}`,
      params: { sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  update = (id: string, input: CreateUpdateProjectTaskDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ProjectTaskDto>({
      method: 'PUT',
      url: `/api/app/project-task/${id}`,
      body: input,
    },
    { apiName: this.apiName,...config });
  

  updateStatus = (id: string, status: ProjectTaskStatus, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ProjectTaskDto>({
      method: 'PUT',
      url: `/api/app/project-task/${id}/status`,
      params: { status },
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
