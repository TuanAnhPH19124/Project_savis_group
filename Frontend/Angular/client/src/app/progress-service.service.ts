import { Injectable } from '@angular/core';
import { BehaviorSubject, Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ProgressService {
  public isLoading: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(true);

  
}
