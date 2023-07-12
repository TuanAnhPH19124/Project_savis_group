import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HomeComponent } from './home.component';
import { HousingLocationModule } from '../housing-location/housing-location.module';
import {MatSlideToggleModule} from '@angular/material/slide-toggle';
import {MatSelectModule} from '@angular/material/select';
import {MatFormFieldModule} from '@angular/material/form-field';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {MatSliderModule} from '@angular/material/slider';
import {MatPaginatorModule} from '@angular/material/paginator';
import {MatButtonModule} from '@angular/material/button';
@NgModule({
  declarations: [HomeComponent],
  imports: [
    CommonModule,
    HousingLocationModule,
    MatSlideToggleModule,
    MatSelectModule,
    MatFormFieldModule,
    FormsModule, 
    ReactiveFormsModule,
    MatSliderModule,
    MatPaginatorModule,
    MatButtonModule
  ],
  exports:[
    HomeComponent
  ]
})
export class HomeModule { }
