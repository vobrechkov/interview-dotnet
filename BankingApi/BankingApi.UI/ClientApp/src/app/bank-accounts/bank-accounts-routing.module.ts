import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CreateBankAccountComponent } from './create/create.component';
import { ListBankAccountsComponent } from './list/list.component';
import { BankAccountTransferComponent } from './transfer/transfer.component';

const routes: Routes = [
  { path: 'bankaccounts', component: ListBankAccountsComponent },
  { path: 'bankaccounts/create', component: CreateBankAccountComponent },
  { path: 'bankaccounts/transfer-from/:id', component: BankAccountTransferComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class BankAccountsRoutingModule { }
