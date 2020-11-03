import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';
import { BankAccountsService, BankAccount, BankAccountDetails } from '../bank-accounts.service';

@Component({
  selector: 'app-bank-accounts-transfer',
  templateUrl: './transfer.component.html',
  styleUrls: ['./transfer.component.css']
})
export class BankAccountTransferComponent implements OnInit {
  private sourceAccountNumber: string;
  private destinationAccountNumber: string;
  private transferAmount: number;
  private bankAccount: BankAccount;
  private bankAccounts: any[];
  private transferSummary: any;
  submitted = false;

  constructor(private bankAccountsService: BankAccountsService, private route: ActivatedRoute) {
  }

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      this.sourceAccountNumber = params.get('number');

      console.log('sourceAccountNumber: ' + this.sourceAccountNumber);

      this.bankAccountsService.get(this.sourceAccountNumber)
        .subscribe(response => {
          console.log(response);
          this.bankAccount = response;
          this.bankAccountsService.getList(this.bankAccount.customerId)
            .subscribe(response => {
              // Remove source bank account from list to choose from
              this.bankAccounts = response.filter(ba => ba.number !== this.sourceAccountNumber);
            });
        },
        error => {
          console.log(error);
        });    
    });    
  }

  performTransfer(): void {
    this.bankAccountsService.transfer(this.sourceAccountNumber, this.destinationAccountNumber, this.transferAmount)
      .subscribe(response => {
        console.log(response);
        this.transferSummary = {
          id: response.data.transferId,
          amount: response.data.amount,
          sourceAccountNumber: response.data.sourceAccountNumber,
          sourceAccountBalance: response.data.sourceAccountBalance,
          destinationAccountNumber: response.data.destinationAccountNumber,
          destinationAccountBalance: response.data.destinationAccountBalance,
          createdAt: response.data.transferDate
        };
        this.bankAccount.postedBalance = response.sourceAccountBalance;
        console.log('Transfer Summary', this.transferSummary);
        console.log('Bank Account', this.bankAccount);
        this.submitted = true;
      },
      error => {
        console.log(error);
      });
  }
}
