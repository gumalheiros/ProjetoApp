import { mapEnumToOptions } from '@abp/ng.core';

export enum ProjectTaskStatus {
  Pending = 0,
  InProgress = 1,
  Completed = 2,
}

export const projectTaskStatusOptions = mapEnumToOptions(ProjectTaskStatus);
