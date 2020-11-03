import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

const baseUrl = environment.baseApiUrl + 'bankaccounts';

@Injectable({
  providedIn: 'root'
})
export class BankAccountsService {
  constructor(private httpClient: HttpClient) { }

  getAll(): Observable<BankAccountDetails[]> {
    return this.httpClient.get<BankAccountDetails[]>(baseUrl);
  }

  getList(customerId): Observable<any> {
    let resourceUrl = `${baseUrl}/list`;

    if (customerId !== null && customerId !== '') {
      resourceUrl += `/customer/${customerId}`;
    }

    return this.httpClient.get(resourceUrl);
  }

  get(accountNumber): Observable<BankAccount> {
    return this.httpClient.get<BankAccount>(`${baseUrl}/${accountNumber}`);
  }

  create(newBankAccount: NewBankAccount): Observable<BankAccount> {
    console.log('[BankAccountsService] creating new bank account: ', newBankAccount);
    return this.httpClient.post<BankAccount>(baseUrl, newBankAccount);
  }

  update(accountNumber, bankAccount): Observable<any> {
    return this.httpClient.put<BankAccount>(`${baseUrl}/${accountNumber}`, bankAccount);
  }

  delete(accountNumber): Observable<any> {
    return this.httpClient.delete(`${baseUrl}/${accountNumber}`);
  }

  deposit(accountNumber: string, amount: number): Observable<any> {
    return this.httpClient.post(`${baseUrl}/${accountNumber}/deposit/${amount}`, null);
  }

  transfer(sourceAccountNumber: string, destinationAccountNumber: string, amount: number): Observable<any> {
    return this.httpClient.post(`${baseUrl}/${sourceAccountNumber}/transfer/${destinationAccountNumber}/amount/${amount}`, null);
  }
}

export enum AccountTypes {
  Checking = 1,
  Savings = 2
};

export interface BankAccountDetails extends BankAccount {
  description: '';
  customerName: '';
  institutionName: '';
}

export interface BankAccount extends NewBankAccount {
  number: string;
  createdAt: string;
  updatedAt: string;
}

export interface NewBankAccount {
  routingNumber: string;
  customerId: string;
  displayName: string;
  type: number;
  postedBalance: number;
}

export interface BankAccountListItem {
  number: string;
  displayName: string;
}
