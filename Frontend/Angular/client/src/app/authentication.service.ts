import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Authentication } from './Models/authentication';
import { LoginRequestDto } from './Models/LoginRequestDto';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {
  url = "http://localhost:5149/api/Authentication";
  constructor(private http: HttpClient) { }

  login(loginRequest: LoginRequestDto): Observable<Authentication>{
    return this.http.post<Authentication>(`${this.url}/signin`, loginRequest);
  }
  loggedIn(){
    return !!localStorage.getItem('jwtToken');
  }
  logOut(){
    localStorage.clear();
  }
  userRoles(){
    return localStorage.getItem('role');
  }
  getToken(){
    return localStorage.getItem('jwtToken');
  }
}
