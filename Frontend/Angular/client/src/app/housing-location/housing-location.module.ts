import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HousingLocationComponent } from './housing-location.component';
import { RouterModule } from '@angular/router';

@NgModule({
  declarations: [HousingLocationComponent],
  imports: [
    CommonModule,
    RouterModule
  ],
  exports:[
    HousingLocationComponent
  ]
})
export class HousingLocationModule { }
