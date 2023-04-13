import { Injectable } from '@angular/core';
import {Subject} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class SaveToggleService {
  public manualSave = new Subject<void>();
  constructor() { }
  activateManual(){
    this.manualSave.next();
  }
}
