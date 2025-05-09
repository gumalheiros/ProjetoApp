import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { CustomerService } from '../proxy/domain/customer.service';
import { CreateUpdateCustomerDto } from '../proxy/domain/dtos';

@Component({
  selector: 'app-customer-detail',
  standalone: false,
  templateUrl: './customer-detail.component.html'
})
export class CustomerDetailComponent implements OnInit {
  form: FormGroup;
  customerId: string;
  isEditMode = false;

  constructor(
    private fb: FormBuilder,
    private customerService: CustomerService,
    private route: ActivatedRoute,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.buildForm();
    this.customerId = this.route.snapshot.paramMap.get('id');
    
    if (this.customerId) {
      this.isEditMode = true;
      this.customerService.get(this.customerId).subscribe(customer => {
        this.form.patchValue({
          name: customer.name,
          birthDate: this.formatDate(customer.birthDate)
        });
      });
    }
  }

  buildForm(): void {
    this.form = this.fb.group({
      name: [null, [Validators.required, Validators.maxLength(100)]],
      birthDate: [null]
    });
  }

  formatDate(date: string): string {
    if (!date) return null;
    const d = new Date(date);
    const year = d.getFullYear();
    const month = (`0${d.getMonth() + 1}`).slice(-2);
    const day = (`0${d.getDate()}`).slice(-2);
    return `${year}-${month}-${day}`;
  }

  save(): void {
    if (this.form.invalid) {
      return;
    }

    const input = this.form.value as CreateUpdateCustomerDto;

    if (this.isEditMode) {
      this.customerService.update(this.customerId, input).subscribe(() => {
        this.navigateToList();
      });
    } else {
      this.customerService.create(input).subscribe(() => {
        this.navigateToList();
      });
    }
  }

  navigateToList(): void {
    this.router.navigate(['/customer']);
  }
}