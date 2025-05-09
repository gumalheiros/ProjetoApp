import { mapEnumToOptions } from '@abp/ng.core';

export enum TaskPriority {
  Low = 0,
  Medium = 1,
  High = 2,
}

export const taskPriorityOptions = mapEnumToOptions(TaskPriority);
