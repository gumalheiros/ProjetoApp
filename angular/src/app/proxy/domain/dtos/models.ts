import type { EntityDto, PagedAndSortedResultRequestDto } from '@abp/ng.core';

export interface CreateUpdateCustomerDto {
  name?: string;
  birthDate?: string;
}

export interface CustomerDto extends EntityDto<string> {
  name?: string;
  birthDate?: string;
}

export interface CustomerGetListInput extends PagedAndSortedResultRequestDto {
  name?: string;
  birthDate?: string;
}
