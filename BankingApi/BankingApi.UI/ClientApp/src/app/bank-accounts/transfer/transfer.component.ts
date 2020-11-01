import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';
import { BankAccountsService, BankAccount, BankAccountDetails } from '../bank-accounts.service';

@Component({
  selector: 'app-bank-accounts-transfer',
  templateUrl: './transfer.component.html',
  styleUrls: ['./transfer.component.css']
})
export class BankAccountTransferComponent implements OnInit {
  private sourceBankAccountId: string;
  private destinationBankAccountId: string;
  private transferAmount: number;
  private bankAccount: BankAccount;
  private bankAccounts: any[];
  private transferSummary: any;
  submitted = false;

  constructor(private bankAccountsService: BankAccountsService, private route: ActivatedRoute) {
    this.clearTransfer();
  }

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      this.sourceBankAccountId = params.get('id');

      console.log('sourceBankAccountId: ' + this.sourceBankAccountId);

      this.bankAccountsService.get(this.sourceBankAccountId)
        .subscribe(response => {
          console.log(response);
          this.bankAccount = response;

          this.bankAccountsService.getList(this.bankAccount.customerId)
            .subscribe(response => {
              console.log('Unfiltered accounts', response);
              // Remove source bank account from list to choose from
              this.bankAccounts = response.filter(ba => ba.id !== this.sourceBankAccountId);
              console.log('Filtered accounts: ', this.bankAccounts);
            });
        },
        error => {
          console.log(error);
        });    
    });    
  }

  performTransfer(): void {
    this.bankAccountsService.transfer(this.sourceBankAccountId, this.destinationBankAccountId, this.transferAmount)
      .subscribe(response => {
        console.log(response);
        this.transferSummary = {
          id: response.data.transferId,
          amount: response.data.amount,
          postedBalance: response.data.postedBalance,
          createdAt: response.data.transferDate
        };
        console.log(this.transferSummary);
        this.submitted = true;
      },
      error => {
        console.log(error);
      });
  }

  clearTransfer(): void {
    this.submitted = false;
    this.transferAmount = null;
    this.destinationBankAccountId = null;
  }
}
