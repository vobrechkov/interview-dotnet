import { Component, OnInit } from '@angular/core';
import { CustomersService, Customer } from '../customers.service';
import { InstitutionsService, Institution } from '../../institutions/institutions.service';

@Component({
  selector: 'app-create',
  templateUrl: './create.component.html',
  styleUrls: ['./create.component.css']
})
export class CreateCustomerComponent implements OnInit {
  private customer: Customer;
  private institutions: Institution[];
  submitted = false;

  constructor(private customersService: CustomersService, private institutionsService: InstitutionsService) {
    this.clearCustomer();
  }

  ngOnInit() {
    this.institutionsService.getAll()
      .subscribe(response => {
        console.log(response);
        this.institutions = response;
      },
      error => {
        console.log(error);
      });
  }

  createCustomer(): void {
    const newCustomer = {
      institutionId: this.customer.institutionId,
      firstName: this.customer.firstName,
      lastName: this.customer.lastName,
      phone: this.customer.phone,
      email: this.customer.email
    };
    this.customersService.create(newCustomer)
      .subscribe(response => {
        console.log(response);
        this.customer.id = response.id;
        this.customer.createdAt = response.createdAt;
        this.submitted = true;
      },
      error => {
        console.log(error);
      });
  }

  clearCustomer(): void {
    this.submitted = false;
    this.customer = {
      id: '',
      institutionId: '',
      firstName: '',
      lastName: '',
      phone: '',
      email: '',
      createdAt: '',
      updatedAt: ''
    };
  }
}
