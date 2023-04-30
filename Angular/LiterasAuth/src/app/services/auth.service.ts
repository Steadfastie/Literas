import {Injectable} from '@angular/core';
import {ApiService} from "./api.service";
import {UserCredentials} from "../models/userCredentials";
import {Observable} from "rxjs";
import {OperationResponse} from "../models/operationResponse";
import {OperationsService} from "./operations.service";

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  returnUrl: string = 'www.google.com'
  constructor(private apiService: ApiService,
              private operations: OperationsService) {
    this.operations.getReturnUrl().subscribe(url => this.returnUrl = url);
  }

  login(user: UserCredentials): Observable<OperationResponse>{
    let data = {...user, returnUrl: this.returnUrl};
    return this.apiService.post(`auth/login`, data).pipe();
  }

  signup(user: UserCredentials): Observable<OperationResponse>{
    return this.apiService.post('auth/signup', {...user, returnUrl: this.returnUrl})
  }

  redirect(): Observable<any>{
    return this.apiService.request('GET', this.returnUrl, {}, {});
  }
}
