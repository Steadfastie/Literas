import {Injectable} from '@angular/core';
import {BehaviorSubject, Subject} from "rxjs";
import {OperationResponse} from "../models/operationResponse";

@Injectable({
  providedIn: 'root'
})
export class OperationsService {
  private latestOperation = new BehaviorSubject<OperationResponse | null>(null);
  private returnUrl = new BehaviorSubject<string>('www.google.com');
  constructor() {
  }
  push(operation: OperationResponse){
    this.latestOperation.next(operation);
  }
  pick() {
   return this.latestOperation.asObservable();
  }
  saveReturnUrl(url: string){
    this.returnUrl.next(url);
  }
  getReturnUrl(){
    return this.returnUrl.asObservable();
  }
}
