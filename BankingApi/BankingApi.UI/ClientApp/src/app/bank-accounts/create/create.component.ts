import { Component, OnInit } from '@angular/core';
import { BankAccountsService, AccountTypes, BankAccount, NewBankAccount } from '../bank-accounts.service';
import { CustomersService, Customer } from '../../customers/customers.service';

@Component({
  selector: 'app-banka-ccounts-create',
  templateUrl: './create.component.html',
  styleUrls: ['./create.component.css']
})
export class CreateBankAccountComponent implements OnInit {
  private accountTypes = AccountTypes;
  private bankAccount: BankAccount;
  private customers: Customer[];
  private openingBalance: number;
  submitted = false;

  constructor(private bankAccountsService: BankAccountsService,
              private customersService: CustomersService) {
    this.clearBankAccount();
  }

  ngOnInit() {  
    this.customersService.getAll()
      .subscribe(response => {
        console.log(response);
        this.customers = response;
      },
      error => console.log(error));
  }

  createBankAccount(): void {
    this.bankAccountsService.create(this.bankAccount as NewBankAccount)
      .subscribe(response => {
        console.log(response);
        this.bankAccount.number = response.number;
        this.bankAccount.createdAt = response.createdAt;

        this.bankAccountsService.deposit(response.number, this.openingBalance)
          .subscribe(response => {

            if (response.errors && response.errors.length) {
              console.log('Error making deposit', response.errors);
            }

            this.submitted = true;
          },
          error => console.log(error));        
      },
      error => console.log(error));
  }

  accountTypeChange(e): void {
    this.bankAccount.type = parseInt(e.target.value);
    console.log(this.bankAccount);
  }

  clearBankAccount(): void {
    this.submitted = false;
    this.bankAccount = {
      number: '',
      routingNumber: '',
      customerId: '',
      displayName: '',
      postedBalance: 0,
      type: null,
      createdAt: '',
      updatedAt: ''
    };
  }
}
