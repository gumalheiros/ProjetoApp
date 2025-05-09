import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CustomerComponent } from './customer.component';
import { CustomerDetailComponent } from './customer-detail.component';
import { AuthGuard, PermissionGuard } from '@abp/ng.core';

const routes: Routes = [
  {
    path: '',
    component: CustomerComponent,
    canActivate: [AuthGuard, PermissionGuard],
    data: {
      requiredPolicy: 'ProjetoApp.Customer'
    }
  },
  {
    path: 'new',
    component: CustomerDetailComponent,
    canActivate: [AuthGuard, PermissionGuard],
    data: {
      requiredPolicy: 'ProjetoApp.Customer.Create'
    }
  },
  {
    path: 'edit/:id',
    component: CustomerDetailComponent,
    canActivate: [AuthGuard, PermissionGuard],
    data: {
      requiredPolicy: 'ProjetoApp.Customer.Update'
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CustomerRoutingModule { } 