import { Component, Input, OnInit } from '@angular/core';
import { BookingDetail } from '../Models/bookingGetRequestDto';
import { BookingService } from '../booking.service';
import { Router } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { DialogConfirmDeleteComponent } from '../dialog-confirm-delete/dialog-confirm-delete.component';


@Component({
  selector: 'app-booking-detail',
  templateUrl: './booking-detail.component.html',
  styleUrls: ['./booking-detail.component.css']
})
export class BookingDetailComponent implements OnInit{
  @Input() bookingDetail: BookingDetail | undefined;
  price: number | undefined;
  constructor(private bookingService: BookingService, private router: Router, public dialog: MatDialog){
    
  }
  ngOnInit(): void {
    this.getPrice();
  }

  openDialog(enterAnimationDuration: string, exitAnimationDuration: string): void {
    const dialogRef = this.dialog.open(DialogConfirmDeleteComponent, {
      width: '250px',
      enterAnimationDuration,
      exitAnimationDuration
    });

    dialogRef.afterClosed().subscribe((confirmed: boolean) => {
      if (confirmed && this.bookingDetail){
        this.bookingService.removeBooking(this.bookingDetail?.id).subscribe(() =>{
          this.bookingService.getBookingDetail().subscribe((result: BookingDetail[]) => {
            this.bookingService.setBookingDetailList(result);
            this.bookingService.getTotalItemInBooking().subscribe((result: number) => {
              this.bookingService.setCounterBooking(result);
            })
          })
        });
      }
    });
    
  }

  setSelectedBooking(){
    if (this.bookingDetail){
      this.bookingService.setChecked(this.bookingDetail?.id, !this.bookingDetail.selected);
    }
  }

  getPrice(){
    if (this.bookingDetail)
    this.price = this.bookingDetail?.amount * this.bookingDetail?.price;
  }

  plus(){
    if (this.bookingDetail !== undefined){
      this.bookingDetail.amount++;
      this.getPrice();
      this.bookingService.updateBookingAmout(this.bookingDetail.id, this.bookingDetail.amount).subscribe();
      
    }
  }
  devide(){
    if (this.bookingDetail !== undefined && this.bookingDetail.amount > 1){
      this.bookingDetail.amount--;
      this.getPrice();
      this.bookingService.updateBookingAmout(this.bookingDetail.id, this.bookingDetail.amount).subscribe();
    }
  }
}
