import { Component, inject } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { HousingLocation } from '../housing-location';
import { HousingService } from '../housing.service';
import { BookingDto } from '../Models/BookingPostRequestDto';
import { Router } from '@angular/router';
import { SharedService } from '../shared.service';
import { BookingService } from '../booking.service';

@Component({
  selector: 'app-detail',
  templateUrl: './detail.component.html',
  styleUrls: ['./detail.component.css']
})
export class DetailComponent {
  route: ActivatedRoute = inject(ActivatedRoute);
  housingService: HousingService = inject(HousingService);
  housingLocation: HousingLocation | undefined;
  booking: BookingDto = {};
  housingLocationId: string;

  constructor(private router: Router, 
    private sharedService: SharedService, 
    private bookingService: BookingService){
    this.housingLocationId = this.route.snapshot.params['id'];
    this.housingService.getHousingLocationById(this.housingLocationId)
    .subscribe((result: HousingLocation|undefined) => (this.housingLocation = result));
  }

  handleBooking(){
    this.booking.roomId = this.housingLocationId;
    this.booking.amount = 1;
    if (localStorage.getItem("CustomerId")=== null){
      this.sharedService.setSignalPostBooking(true);
      this.sharedService.setBooking(this.booking);
      this.router.navigate(['/login']);
      return;
    }
    this.booking.customerId = localStorage.getItem("CustomerId");
    this.bookingService.AddToBooking(this.booking).pipe().subscribe(() =>{
      this.bookingService.getTotalItemInBooking().subscribe((result) => {
        this.bookingService.setCounterBooking(result);
      })
      this.router.navigate(['/']);
    })
  }
}
