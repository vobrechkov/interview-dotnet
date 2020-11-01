import { Component, Inject } from '@angular/core';
import { CustomersService, Customer } from '../customers.service';

@Component({
  selector: 'app-list-customers',
  templateUrl: './list.component.html'
})
export class ListCustomersComponent {
  public customers: Customer[];

  constructor(customersService: CustomersService) {
    customersService.getAll().subscribe(result => {
        console.log(result);
        this.customers = result;
      }, error => console.error(error));
    }
}

