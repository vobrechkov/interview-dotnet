import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

const baseUrl = environment.baseApiUrl + 'institutions';

@Injectable({
  providedIn: 'root'
})
export class InstitutionsService {
  constructor(private httpClient: HttpClient) { }

  getAll(): Observable<Institution[]> {
    return this.httpClient.get<Institution[]>(baseUrl);
  }

  create(institution): Observable<Institution> {
    return this.httpClient.post<Institution>(baseUrl, institution);
  }

  update(id, institution): Observable<any> {
    return this.httpClient.put<Institution>(`${baseUrl}/${id}`, institution);
  }

  delete(id): Observable<any> {
    return this.httpClient.delete(`${baseUrl}/${id}`);
  }
}

export interface Institution {
  id: string;
  name: string;
  phone: string;
  email: string;
  website: string;
  createdAt: string;
  updatedAt: string;
}
