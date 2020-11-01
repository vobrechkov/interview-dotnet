import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { SSL_OP_ALLOW_UNSAFE_LEGACY_RENEGOTIATION } from 'constants';

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

  get(bankAccountId): Observable<BankAccount> {
    return this.httpClient.get<BankAccount>(`${baseUrl}/${bankAccountId}`);
  }

  create(newBankAccount): Observable<BankAccount> {
    console.log('[BankAccountsService] creating new customer: ', newBankAccount);
    return this.httpClient.post<BankAccount>(baseUrl, newBankAccount);
  }

  update(id, bankAccount): Observable<any> {
    return this.httpClient.put<BankAccount>(`${baseUrl}/${id}`, bankAccount);
  }

  delete(id): Observable<any> {
    return this.httpClient.delete(`${baseUrl}/${id}`);
  }

  transfer(sourceAccountId: string, destinationAccountId: string, amount: number): Observable<any> {
    return this.httpClient.post(`${baseUrl}/${sourceAccountId}/transfer/${destinationAccountId}/amount/${amount}`, null);
  }
}

export interface BankAccountDetails extends BankAccount {
  description: '';
  customerName: '';
  institutionName: '';
}

export interface BankAccount extends NewBankAccount {
  id: string;
  createdAt: string;
  updatedAt: string;
}

export interface NewBankAccount {
  customerId: string;
  displayName: string;
  type: number;
  postedBalance: number;
}

export interface BankAccountListItem {
  id: string;
  displayName: string;
}
