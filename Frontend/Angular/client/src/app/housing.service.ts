import { Injectable } from '@angular/core';
import { HousingLocation } from './housing-location';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthenticationService } from './authentication.service';
import { Province } from './province';
import { DynamicParam } from './dynamicParam';

@Injectable({
  providedIn: 'root'
})
export class HousingService {
 
  url = "http://localhost:5149/api/Rooms";
  host = "https://provinces.open-api.vn/api/";
  constructor(private http: HttpClient, private _authService: AuthenticationService) { }

  getAllHousingLocations(): Observable<HousingLocation[]> {
    return this.http.get<HousingLocation[]>(`${this.url}/getall`);
  }

  getHousingLocationById(id: string): Observable<HousingLocation | undefined> {
    return this.http.get<HousingLocation>(`${this.url}/get/${id}`);
  }

  updateHousingById(id: string | undefined, formData: FormData): Observable<string> {
    return this.http.put<string>(`${this.url}/update/${id}`, formData);
  }

  search(keyword: string): Observable<HousingLocation[]>{
    const apiUrl = `${this.url}/search?keyword=${encodeURIComponent(keyword)}`;
    return this.http.get<HousingLocation[]>(apiUrl);
  }

  removeHousing(id: string): Observable<string>{
    const token = this._authService.getToken();
    const options = {
      headers: new HttpHeaders({
        Authorization: `Bearer ${token}`
      })
    };
    return this.http.delete<string>(`${this.url}/delete/${id}`, options);
  }

  insertHousing(formData: FormData): Observable<string> {
    const token = this._authService.getToken();
    const options = {
      headers: new HttpHeaders({
        Authorization: `Bearer ${token}`
      })
    };
    return this.http.post<string>(`${this.url}/insert`, formData, options);
  }

  getProvince(): Observable<Province[]>{
    return this.http.get<Province[]>(`${this.host}`);
  }

  getDistrict(provices: any): Observable<Province>{
    return this.http.get<Province>(`${this.host}p/${provices}?depth=2`);
  }

  getWard(district: any): Observable<object>{
    return this.http.get<object>(`${this.host}d/${district}?depth=2`);
  }

  filter(params: DynamicParam): Observable<HousingLocation[]>{
    return this.http.post<HousingLocation[]>(`${this.url}/filter`,params);
  }
}
