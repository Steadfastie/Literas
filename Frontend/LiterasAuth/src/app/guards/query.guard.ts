import { Injectable } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  CanActivate,
  Router,
  RouterStateSnapshot,
  UrlTree
} from '@angular/router';
import { Observable } from 'rxjs';
import {OperationsService} from "../services/operations.service";

@Injectable({
  providedIn: 'root'
})
export class QueryGuard implements CanActivate {
  constructor(private operations: OperationsService,
              private router: Router) {
  }
  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    if (route.queryParams['ReturnUrl']){
      this.operations.saveReturnUrl(route.queryParams['ReturnUrl']);
      return true;
    }
    this.router.navigate(['./error']);
    return false;
  }

}
