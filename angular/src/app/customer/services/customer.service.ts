import { Injectable } from '@angular/core';
import { RestService } from '@abp/ng.core';
import { CustomerDto, CustomerGetListInput, CreateUpdateCustomerDto } from '../models/customer';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CustomerService {
  apiName = 'Default';

  constructor(private restService: RestService) {}

  getList(input: CustomerGetListInput): Observable<any> {
    return this.restService.request<any, any>({
      method: 'GET',
      url: '/api/app/customer',
      params: {
        skipCount: input.skipCount,
        maxResultCount: input.maxResultCount,
        sorting: input.sorting,
        name: input.name,
        birthDate: input.birthDate
      }
    });
  }

  get(id: string): Observable<CustomerDto> {
    return this.restService.request<any, CustomerDto>({
      method: 'GET',
      url: `/api/app/customer/${id}`
    });
  }

  create(input: CreateUpdateCustomerDto): Observable<CustomerDto> {
    return this.restService.request<CreateUpdateCustomerDto, CustomerDto>({
      method: 'POST',
      url: '/api/app/customer',
      body: input
    });
  }

  update(id: string, input: CreateUpdateCustomerDto): Observable<CustomerDto> {
    return this.restService.request<CreateUpdateCustomerDto, CustomerDto>({
      method: 'PUT',
      url: `/api/app/customer/${id}`,
      body: input
    });
  }

  delete(id: string): Observable<void> {
    return this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/app/customer/${id}`
    });
  }
} 