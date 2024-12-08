import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from '../../environments/environment.development';
import { userregister } from './../model/user.model';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  private baseUrl: string = environment.apiUrl;

  constructor(private http: HttpClient) {}

  Userregistration(data: userregister): Observable<any> {
    return this.http.post<any>(`${environment.apiUrl}User/register`, data, {
      headers: new HttpHeaders().set('Content-Type', 'application/json'),
      responseType: 'text' as 'json',
    });
  }

  UserLogin(loginData: {
    username: string;
    password: string;
  }): Observable<any> {
    return this.http.post<any>(`${environment.apiUrl}User/login`, loginData, {
      headers: new HttpHeaders().set('Content-Type', 'application/json'),
      responseType: 'json',
    });
  }
}
