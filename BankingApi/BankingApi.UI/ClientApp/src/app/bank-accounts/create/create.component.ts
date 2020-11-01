import { Component, OnInit } from '@angular/core';
import { BankAccountsService, BankAccount } from '../bank-accounts.service';
import { CustomersService, Customer } from '../../customers/customers.service';

@Component({
  selector: 'app-banka-ccounts-create',
  templateUrl: './create.component.html',
  styleUrls: ['./create.component.css']
})
export class CreateBankAccountComponent implements OnInit {
  private bankAccount: BankAccount;
  private customers: Customer[];
  submitted = false;

  constructor(private bankAccountsService: BankAccountsService, private customersService: CustomersService) {
    this.clearBankAccount();
  }

  ngOnInit() {
    this.customersService.getAll()
      .subscribe(response => {
        console.log(response);
        this.customers = response;
      },
      error => {
        console.log(error);
      });
  }

  createBankAccount(): void {
    const newBankAccount = {
      customerId: this.bankAccount.customerId,
      displayName: this.bankAccount.displayName,
      type: this.bankAccount.type
    };
    this.bankAccountsService.create(newBankAccount)
      .subscribe(response => {
        console.log(response);
        this.bankAccount.id = response.id;
        this.bankAccount.createdAt = response.createdAt;
        this.submitted = true;
      },
      error => {
        console.log(error);
      });
  }

  clearBankAccount(): void {
    this.submitted = false;
    this.bankAccount = {
      id: '',
      customerId: '',
      displayName: '',
      postedBalance: 0,
      type: null,
      createdAt: '',
      updatedAt: ''
    };
  }
}
