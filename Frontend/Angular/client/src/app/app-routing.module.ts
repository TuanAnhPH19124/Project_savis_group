import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { CalenderComponent } from './calender/calender.component';
import { DetailComponent } from './detail/detail.component';
import { EditComponent } from './edit/edit.component';
import { InsertComponent } from './insert/insert.component';
import { LoginComponent } from './login/login.component';
import { AppComponent } from './app.component';
import { BookingComponent } from './booking/booking.component';



const routes: Routes = [
  {component: HomeComponent, path:'', title: 'Home'},
  {component: CalenderComponent, path:'calendar', title: 'Calendar'},
  {component: DetailComponent, path: 'details/:id', title: 'Detail'},
  {component: EditComponent, path: 'edit/:id', title: 'Edits'},
  {component: InsertComponent, path: 'insert', title: 'Insert'},
  {component: LoginComponent, path: 'login',title: 'Login'},
  {component: AppComponent, path:'reload'},
  {component: BookingComponent, path:'booking', title:'Booking'}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
