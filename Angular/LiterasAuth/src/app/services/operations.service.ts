import {Injectable} from '@angular/core';
import {BehaviorSubject} from "rxjs";
import {OperationResponse} from "../models/operationResponse";

@Injectable({
  providedIn: 'root'
})
export class OperationsService {
  private latestOperation = new BehaviorSubject<OperationResponse | null>(null);
  constructor() {
  }
  push(operation: OperationResponse){
    this.latestOperation.next(operation);
  }
  pick() {
   return this.latestOperation.asObservable();
  }
}
