import { Injectable } from '@angular/core';
import {ApiService} from "./api.service";
import {User} from "../models/user";
import {Observable} from "rxjs";
import {OperationResponse} from "../models/operationResponse";

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  constructor(private apiService: ApiService) { }

  login(user: User): Observable<OperationResponse>{
    return this.apiService.post('login', user)
  }

  signup(user: User): Observable<OperationResponse>{
    return this.apiService.post('signup', user)
  }
}
