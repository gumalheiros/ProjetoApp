import { Component, OnInit } from '@angular/core';
import { ListService, PagedResultDto } from '@abp/ng.core';
import { CustomerService } from './services/customer.service';
import { FormGroup, FormBuilder } from '@angular/forms';
import { CustomerDto, CustomerGetListInput } from './models/customer';
import { ConfirmationService, Confirmation } from '@abp/ng.theme.shared';
import { Router } from '@angular/router';

@Component({
  selector: 'app-customer',
  templateUrl: './customer.component.html',
  providers: [ListService],
})
export class CustomerComponent implements OnInit {
  customer = { items: [], totalCount: 0 } as PagedResultDto<CustomerDto>;
  selectedCustomer = {} as CustomerDto;
  form: FormGroup;
  isModalOpen = false;
  
  filters = {} as CustomerGetListInput;

  constructor(
    public readonly list: ListService,
    private customerService: CustomerService,
    private fb: FormBuilder,
    private confirmation: ConfirmationService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.initForm();
    
    const customerStreamCreator = (query) => this.customerService.getList({
      ...query,
      ...this.filters,
    });

    this.list.hookToQuery(customerStreamCreator).subscribe((response) => {
      this.customer = response;
    });
  }

  initForm() {
    this.form = this.fb.group({
      name: [null],
      birthDate: [null],
    });
  }

  create() {
    this.router.navigate(['/customer/new']);
  }

  edit(id: string) {
    this.router.navigate(['/customer/edit', id]);
  }

  delete(id: string) {
    this.confirmation.warn(
      '::AreYouSureToDelete',
      '::AreYouSure',
      { messageLocalizationParams: [] }
    ).subscribe((status) => {
      if (status === Confirmation.Status.confirm) {
        this.customerService.delete(id).subscribe(() => {
          this.list.get();
        });
      }
    });
  }

  clearFilters() {
    this.filters = {} as CustomerGetListInput;
    this.form.reset();
    this.list.get();
  }

  filter() {
    this.filters = this.form.value;
    this.list.get();
  }
} 