import { Component, Input } from '@angular/core';
import { HousingLocation } from '../housing-location';
import { AuthenticationService } from '../authentication.service';
@Component({
  selector: 'app-housing-location',
  templateUrl: './housing-location.component.html',
  styleUrls: ['./housing-location.component.css']
})
export class HousingLocationComponent {
  @Input() housingLocation: HousingLocation | undefined;
  constructor(public authenticationService: AuthenticationService){}
}
