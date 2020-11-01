import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { BankAccountsRoutingModule } from './bank-accounts-routing.module';
import { ListBankAccountsComponent } from './list/list.component';
import { CreateBankAccountComponent } from './create/create.component';
import { BankAccountTransferComponent } from './transfer/transfer.component';

@NgModule({
  declarations: [
    ListBankAccountsComponent,
    CreateBankAccountComponent,
    BankAccountTransferComponent
  ],
  imports: [
    CommonModule,
    BankAccountsRoutingModule,
    FormsModule
  ]
})
export class BankAccountsModule { }
