import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BookingDto } from './Models/BookingPostRequestDto';
import { AuthenticationService } from './authentication.service';
import { BookingDetail } from './Models/bookingGetRequestDto';
import { TransactionDetailDto } from './Models/transactionDetail';
import { TransactionDto } from './Models/transaction';

@Injectable({
  providedIn: 'root'
})
export class BookingService {
  url = "http://localhost:5149/Bookings";
  counterBooking = 0;
  bookingDetailList: BookingDetail[] = [];

  constructor(private http: HttpClient, private _authService: AuthenticationService) { }

  AddToBooking(booking: BookingDto): Observable<any> {
    const token = this._authService.getToken();
    const options = {
      headers: new HttpHeaders({
        Authorization: `Bearer ${token}`
      })
    };
    return this.http.post(`${this.url}/add`, booking, options);
  }

  getBookingDetail(): Observable<BookingDetail[]>{
    const token = this._authService.getToken();
    var CustomerId = localStorage.getItem('CustomerId');
    const options = {
      headers: new HttpHeaders({
        Authorization: `Bearer ${token}`
      })
    };
    return this.http.get<BookingDetail[]>(`${this.url}/getBookingDetail/${CustomerId}`, options);
  }

  setBookingDetailList(list: BookingDetail[]){
    this.bookingDetailList = list;
  }

  getBookingDetailList() : BookingDetail[]{
    return this.bookingDetailList;
  }

  getCounterBooking(): number {
    return this.counterBooking;
  }

  setCounterBooking(value: number) {
    this.counterBooking = value;
  }

  getTotalItemInBooking(): Observable<number> {
    const token = this._authService.getToken();
    const cusomterId = localStorage.getItem('CustomerId');
    const options = {
      headers: new HttpHeaders({
        Authorization: `Bearer ${token}`
      })
    };
    return this.http.get<number>(`${this.url}/gettotalitem/${cusomterId}`, options);
  }

  setChecked(id: string, check: boolean){
    this.bookingDetailList = this.bookingDetailList.map((booking) => {
      if (booking.id === id){
        return { ...booking, selected:check}
      }
      return booking;
    });
  }

  calculateSubTotal(): number{
    const totalPrice = this.bookingDetailList.reduce((sum, item) => {
      if (item.selected) {
        return sum + item.price*item.amount;
      }
      return sum;
    }, 0);
    return totalPrice;
  }

  updateBookingAmout(id: string, amount: number) : Observable<any>{
    const token = this._authService.getToken();
    const options = {
      headers: new HttpHeaders({
        Authorization: `Bearer ${token}`
      })
    };
    return this.http.get<any>(`${this.url}/updateAmount/${id}?amount=${amount}`, options);
  }

  removeBooking(id: string) : Observable<void>{
    const token = this._authService.getToken();
    const options = {
      headers: new HttpHeaders({
        Authorization: `Bearer ${token}`
      })
    };
    return this.http.delete<void>(`${this.url}/remove/${id}`,options);
  }

  getSelectedItem(): TransactionDetailDto[]{
    var selectedItem = this.bookingDetailList
    .filter(item => item.selected)
    .map(item => ({BookingId: item.id, RoomId: item.roomId, Amount: item.amount}));
    return selectedItem;
  }

}
