import { PagedAndSortedResultRequestDto } from '@abp/ng.core';

export interface CustomerDto {
  id: string;
  name: string;
  birthDate: string;
}

export interface CustomerGetListInput extends PagedAndSortedResultRequestDto {
  name?: string;
  birthDate?: string;
}

export interface CreateUpdateCustomerDto {
  name: string;
  birthDate?: string;
} 