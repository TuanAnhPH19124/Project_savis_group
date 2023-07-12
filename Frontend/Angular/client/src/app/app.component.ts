import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthenticationService } from './authentication.service';
import { ProgressService } from './progress-service.service';
import { HousingService } from './housing.service';
import { BookingService } from './booking.service';


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{
  title = 'client';
  role!: string | null;
  islogin!: string |null;

  constructor(private route: Router, public _authService: AuthenticationService, public progressService: ProgressService
    ,public bookingService: BookingService){}

  ngOnInit(): void {
    this.bookingService.getTotalItemInBooking().subscribe((result: number) =>{
      this.bookingService.setCounterBooking(result);
    })
  }
  logOut(){
    localStorage.clear();
    console.log('123');
    this._authService
  }
}
