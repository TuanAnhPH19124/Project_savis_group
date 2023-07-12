import { Component, OnInit } from '@angular/core';
import { BookingService } from '../booking.service';
import { BookingDetail } from '../Models/bookingGetRequestDto';
import { MatDatepickerInputEvent } from '@angular/material/datepicker';
import { TransactionDto } from '../Models/transaction';
import { TransactionService } from '../transaction.service';
import { Router } from '@angular/router';


@Component({
  selector: 'app-booking',
  templateUrl: './booking.component.html',
  styleUrls: ['./booking.component.css']
})
export class BookingComponent implements OnInit {
  // bookingDetails: BookingDetail[] = [];
  payMethods: PayMethod[] = [
    {value: 'Payment at the counter', viewValue: 'Payment at the counter'},
    {value: 'Credit card payment', viewValue: 'Credit card payment'},
   
  ];
  selectedPayMethod:string | undefined;
  discount = 0;
  minDate = Date.now();
  maxDate = Date.now() + 7;
  datetime: Date | undefined;
  constructor(public bookingService: BookingService
    , private transactionService: TransactionService
    , private router: Router){}
    
  ngOnInit(): void {
    this.bookingService.getBookingDetail().subscribe((result: BookingDetail[]) => {
      this.bookingService.setBookingDetailList(result);
    })
  }

  addEvent(type: string, event: MatDatepickerInputEvent<Date>) {
    if (event.value){
      const date = {year: event.value?.getFullYear(), month: event.value?.getMonth(), day: event.value?.getDay()}
      this.datetime = new Date(date.year,date.month,date.day);
      console.log(this.datetime);
    }
  }

  CheckOut(){
    if (this.datetime){
      var selectedItem = this.bookingService.getSelectedItem();
      var customerId = localStorage.getItem('CustomerId');
      var transaction: TransactionDto = {
        CheckInDate: this.datetime,
        Total: this.bookingService.calculateSubTotal() + this.discount,
        PayMethod: this.selectedPayMethod ? this.selectedPayMethod : '',
        Status: this.selectedPayMethod === "Credit card payment" ? "Paid" : "Payment at the counter",
        CustomerId: customerId ? customerId : '',
        TransactionDetails: selectedItem
      }
      console.log(transaction);
      this.transactionService.addTransaction(transaction).subscribe(() =>{
        alert("Order successfully");
        this.router.navigate(['/']);
      });
    }
  }
}

export interface PayMethod{
  value:string,
  viewValue: string
}
