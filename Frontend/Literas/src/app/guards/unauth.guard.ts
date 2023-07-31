import { Injectable } from '@angular/core';
import {ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree} from '@angular/router';
import {async, map, Observable} from 'rxjs';
import {AuthService} from "../services/auth/auth.service";

@Injectable({
  providedIn: 'root'
})
export class UnauthGuard implements CanActivate {
  token: string | null = null;
  constructor(private authService: AuthService,
              private router: Router) {}
  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    return this.authService.getUser().then(user => {
      if (user) {
        this.router.navigate(['/docs']);
        return false;
      }
      return true;
    })
  }

}
