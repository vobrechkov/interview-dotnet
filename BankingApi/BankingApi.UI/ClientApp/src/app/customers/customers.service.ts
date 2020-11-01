import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

const baseUrl = environment.baseApiUrl + 'customers';

@Injectable({
  providedIn: 'root'
})
export class CustomersService {
  constructor(private httpClient: HttpClient) { }

  getAll(): Observable<Customer[]> {
    return this.httpClient.get<Customer[]>(baseUrl);
  }

  create(newCustomer): Observable<Customer> {
    console.log('[CustomersService] creating new customer: ', newCustomer);
    return this.httpClient.post<Customer>(baseUrl, newCustomer);
  }

  update(id, customer): Observable<any> {
    return this.httpClient.put<Customer>(`${baseUrl}/${id}`, customer);
  }

  delete(id): Observable<any> {
    return this.httpClient.delete(`${baseUrl}/${id}`);
  }
}

export interface Customer extends NewCustomer {
  id: string;
  createdAt: string;
  updatedAt: string;
}

export interface NewCustomer {
  institutionId: string,
  firstName: string,
  lastName: string,
  phone: string;
  email: string;
}
