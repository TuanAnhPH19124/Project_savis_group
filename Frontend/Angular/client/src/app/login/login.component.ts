import { Component, inject, OnInit } from '@angular/core';
import { LoginRequestDto } from '../Models/LoginRequestDto';
import { AuthenticationService } from '../authentication.service';
import { Router } from '@angular/router';
import { Authentication } from '../Models/authentication';
import jwtDecode from 'jwt-decode';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { catchError } from 'rxjs';
import { jwtDecodeDto } from '../Models/jwtDecoded';
import { BookingService } from '../booking.service';


@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  loginData!: LoginRequestDto;
  authenticationService: AuthenticationService = inject(AuthenticationService);
  groupData!: FormGroup;

  constructor(private router: Router, private snackBar: MatSnackBar, private bookingService: BookingService){}
  ngOnInit(): void {
    this.groupData = new FormGroup({
      email: new FormControl(null, Validators.required),
      password: new FormControl(null, Validators.required)
    });
  }
  onLoginSubmit(){
    if (this.groupData?.valid){
      this.loginData = this.groupData.value;
      console.log(this.loginData)
      this.authenticationService.login(this.loginData)
      .pipe(
        catchError((error) =>{
          this.snackBar.open(`${error.error}. Mã lỗi: ${error.status}`, 'Đóng')
          throw error;
        })
      )
      .subscribe((value: Authentication) =>{
        localStorage.setItem('jwtToken', value.token);
        const decodedToken = jwtDecode(value.token) as jwtDecodeDto;
        localStorage.setItem('role', decodedToken.role);
        localStorage.setItem('CustomerId', decodedToken.Id);
        this.router.navigate(['/']);
      });   
    }
  }
}
