import { Injectable } from '@angular/core';
import { TransactionDto } from './Models/transaction';
import { Observable } from 'rxjs';
import { AuthenticationService } from './authentication.service';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class TransactionService {
  url = "http://localhost:5149/api/Transaction";
  constructor(private _authService: AuthenticationService, private http: HttpClient) { }

  addTransaction(transaction: TransactionDto): Observable<void>{
    const token = this._authService.getToken();
    const options = {
      headers: new HttpHeaders({
        Authorization: `Bearer ${token}`
      })
    };
   return this.http.post<void>(`${this.url}/order`, transaction, options);
  }
}
