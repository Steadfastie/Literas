import { Injectable } from '@angular/core';
import {ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot, UrlTree} from '@angular/router';
import { Observable } from 'rxjs';
import {OperationsService} from "../services/operations.service";

@Injectable({
  providedIn: 'root'
})
export class SucccessOperationGuard implements CanActivate {
  constructor(private operations: OperationsService) {
  }
  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    this.operations.pick().subscribe(operation => {
      return !!operation?.succeeded;
    });
    return false;
  }

}
