import { Component, OnInit } from '@angular/core';
import { ListService, PagedResultDto } from '@abp/ng.core';
import { CustomerService } from '../proxy/domain/customer.service';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { CustomerDto, CustomerGetListInput, CreateUpdateCustomerDto } from '../proxy/domain/dtos';
import { ConfirmationService, Confirmation } from '@abp/ng.theme.shared';

@Component({
  selector: 'app-customer',
  standalone: false,
  templateUrl: './customer.component.html',
  providers: [ListService],
})
export class CustomerComponent implements OnInit {
  customer = { items: [], totalCount: 0 } as PagedResultDto<CustomerDto>;
  selectedCustomer = {} as CustomerDto;
  public filterForm: FormGroup;
  public form: FormGroup;
  public isModalOpen = false;
  
  filters = {} as CustomerGetListInput;

  constructor(
    public readonly list: ListService,
    private customerService: CustomerService,
    private fb: FormBuilder,
    private confirmation: ConfirmationService
  ) {}

  ngOnInit(): void {
    this.initFilterForm();
    
    const customerStreamCreator = (query) => this.customerService.getList({
      ...query,
      ...this.filters,
    });

    this.list.hookToQuery(customerStreamCreator).subscribe((response) => {
      this.customer = response;
    });
  }

  private initFilterForm(): void {
    this.filterForm = this.fb.group({
      name: [null],
      birthDate: [null],
    });
  }

  create() {
    this.selectedCustomer = {} as CustomerDto;
    this.buildForm();
    this.isModalOpen = true;
  }

  edit(id: string) {
    this.customerService.get(id).subscribe((customer) => {
      this.selectedCustomer = customer;
      this.buildForm();
      this.isModalOpen = true;
    });
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

  clearFilters(): void {
    this.filters = {} as CustomerGetListInput;
    this.filterForm.reset();
    this.list.get();
  }

  filter(): void {
    this.filters = this.filterForm.value;
    this.list.get();
  }

  private buildForm(): void {
    this.form = this.fb.group({
      name: [this.selectedCustomer.name || '', [Validators.required, Validators.maxLength(100)]],
      birthDate: [this.selectedCustomer.birthDate ? new Date(this.selectedCustomer.birthDate) : null]
    });
  }

  save(): void {
    if (this.form.invalid) {
      return;
    }
    const input = this.form.value as CreateUpdateCustomerDto;
    const request = this.selectedCustomer.id
      ? this.customerService.update(this.selectedCustomer.id, input)
      : this.customerService.create(input);
    request.subscribe(() => {
      this.isModalOpen = false;
      this.form.reset();
      this.list.get();
    });
  }
}