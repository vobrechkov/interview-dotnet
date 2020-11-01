import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CreateCustomerComponent } from './create/create.component';
import { ListCustomersComponent } from './list/list.component';

const routes: Routes = [
  { path: 'customers', component: ListCustomersComponent },
  { path: 'customers/create', component: CreateCustomerComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CustomersRoutingModule { }
