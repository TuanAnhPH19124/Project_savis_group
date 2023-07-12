import { Injectable } from '@angular/core';
import { BookingDto } from './Models/BookingPostRequestDto';

@Injectable({
  providedIn: 'root'
})
export class SharedService {
  booking: BookingDto = {};
  postBookingRequest: boolean = false;
  constructor() { }

  getBooking():BookingDto{
    return this.booking;
  }

  setBooking(value: BookingDto){
    this.booking = value;
  }

  getSignalPostBooking():any{
    return this.postBookingRequest;
  }

  setSignalPostBooking(value: any){
    this.postBookingRequest = value;
  }
}
