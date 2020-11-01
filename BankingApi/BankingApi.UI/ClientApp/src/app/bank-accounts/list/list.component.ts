import { Component, Inject } from '@angular/core';
import { BankAccountsService, BankAccount } from '../bank-accounts.service';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';

@Component({
  selector: 'app-list-bank-accounts',
  templateUrl: './list.component.html'
})
export class ListBankAccountsComponent {
  public bankAccounts: BankAccount[];

  constructor(private service: BankAccountsService, private router: Router) {
    service.getAll().subscribe(result => {
        console.log(result);
        this.bankAccounts = result;
      }, error => console.error(error));
  }

  transfer(bankAccountId: string) {
    this.router.navigate(['/bankaccounts/transfer-from/:id', { id: bankAccountId }]);
  }
}

