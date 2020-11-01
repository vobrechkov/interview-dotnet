import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { CustomersRoutingModule } from './customers-routing.module';
import { ListCustomersComponent } from './list/list.component';
import { CreateCustomerComponent } from './create/create.component';

@NgModule({
  declarations: [ListCustomersComponent, CreateCustomerComponent],
  imports: [
    CommonModule,
    CustomersRoutingModule,
    FormsModule
  ]
})
export class CustomersModule { }
