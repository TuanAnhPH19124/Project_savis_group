import { Component, inject, OnInit } from '@angular/core';
import {PageEvent} from '@angular/material/paginator';
import { HousingLocation } from '../housing-location';
import { HousingService } from '../housing.service';
import {FormControl} from '@angular/forms';
import { EnumCity } from '../enums/city';
import { EnumDistrict } from '../enums/district';
import { Province } from '../province';
import { District } from '../district';
import { Ward } from '../ward';
import { DynamicParam } from '../dynamicParam';
import { ProgressService } from '../progress-service.service';
import { SharedService } from '../shared.service';
import { AuthenticationService } from '../authentication.service';
import { BookingService } from '../booking.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  housingLocationList: HousingLocation[] = [];
  pageContent: HousingLocation[] = [];
  params: DynamicParam = {};
  housingService: HousingService = inject(HousingService);
  city = new FormControl('');
  district = new FormControl('');
  cityList: Province[] | undefined;
  districtList: District[] | undefined;
  wardList: Ward[]|undefined;
  selectedCity: any | undefined;
  selectedDistrict: any|undefined;
  selectedWard:any|undefined;
  wifiCheck = false;
  laundrycheck = false;
  length = 0;
  pageSize = 6;
  pageIndex = 0;
  pageSizeOptions = [6, 12, 24];
  min = 0;
  max = 500;
  disableFilter = true;

  pageEvent: PageEvent | undefined;

  constructor(
    private sharedService: SharedService, 
    private authService: AuthenticationService, 
    private bookingService: BookingService) {}
  ngOnInit(): void {
    this.housingService.getAllHousingLocations().subscribe((result: HousingLocation[]) => {
      this.housingLocationList = result;
      this.length = result.length;
      this.pageContent = result.slice(this.pageIndex*this.pageSize, (this.pageIndex+1)*this.pageSize);
    });
    if (this.sharedService.getSignalPostBooking() && this.authService.loggedIn()){
      var booking = this.sharedService.getBooking();
      booking.customerId = localStorage.getItem("CustomerId");
      console.log(booking);
      this.bookingService.AddToBooking(booking).subscribe(()=>{
        this.sharedService.setSignalPostBooking(false);
        this.bookingService.getTotalItemInBooking().subscribe((result: number) =>{
          this.bookingService.setCounterBooking(result);
        })
      })
    }
  }


  handelSearch(key: string){
    if (key !== ''){
      this.housingService.search(key).subscribe((result: HousingLocation[]) => {
        this.pageIndex = 0;
        this.pageSize = 6;
        this.length = result.length;
        this.housingLocationList = result;
        this.pageContent = result.slice(this.pageIndex*this.pageSize, (this.pageIndex+1)*this.pageSize);
      });
    }else{
      this.housingService.getAllHousingLocations().subscribe((result: HousingLocation[]) => {
        this.pageIndex = 0;
        this.pageSize = 6;
        this.housingLocationList = result;
        this.length = result.length;
        this.pageContent = result.slice(this.pageIndex*this.pageSize, (this.pageIndex+1)*this.pageSize);
      });
    }
  }

  ClearFilter(){
    this.selectedCity = undefined;
    this.selectedDistrict = undefined;
    this.selectedWard = undefined;
    this.wifiCheck = false;
    this.laundrycheck = false;
    this.min = 0;
    this.max = 500;
    this.disableFilter = true;
    this.housingService.getAllHousingLocations().subscribe((result: HousingLocation[]) => {
      this.pageIndex = 0;
      this.pageSize = 6;
      this.housingLocationList = result;
      this.length = result.length;
      this.pageContent = result.slice(this.pageIndex*this.pageSize, (this.pageIndex+1)*this.pageSize);
    });
  }

  handlePageEvent(e: PageEvent) {
 
    this.pageEvent = e;
    this.pageSize = e.pageSize;
    this.pageIndex = e.pageIndex;
    this.pageContent = this.housingLocationList.slice(this.pageIndex*this.pageSize, (this.pageIndex+1)*this.pageSize);


  }

  formatLabel(value: number): string {
    if (value >= 1000) {
      return Math.round(value / 1000) + 'k';
    }

    return `${value}`;
  }

  getWard(){
    this.housingService.getWard(this.selectedDistrict.split('/')[0]).subscribe((result: any) => {
      this.wardList = result.wards;
    })
  }

  getProvince(){
    this.housingService.getProvince().subscribe((result: Province[]) => {this.cityList = result; console.log(result)});
  }

  getDictrict(){
    this.housingService.getDistrict(this.selectedCity.split('/')[0]).subscribe((result: Province) =>{
      this.districtList = result.districts;
    })
  }

  handleClickFilter(){
    this.params.city = this.selectedCity?.split('/')[1];
    this.params.district = this.selectedDistrict?.split('/')[1];
    this.params.ward = this.selectedWard?.split('/')[1];
    this.params.wifi = this.wifiCheck;
    this.params.laundry = this.laundrycheck;
    this.params.lowPrice = this.min;
    this.params.highPrice = this.max;
    this.disableFilter = false;
    this.housingService.filter(this.params).subscribe((result: HousingLocation[]) => {
      this.pageIndex = 0;
      this.pageSize = 6;
      this.length = result.length;
      this.housingLocationList = result;
      this.pageContent = result.slice(this.pageIndex*this.pageSize, (this.pageIndex+1)*this.pageSize);
    });
  

  }
}


