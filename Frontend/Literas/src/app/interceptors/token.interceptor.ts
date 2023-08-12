import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import {from, Observable, switchMap} from 'rxjs';
import {AuthService} from "../services/auth/auth.service";

@Injectable()
export class TokenInterceptor implements HttpInterceptor {
  token?: string;
  constructor(private authService: AuthService) {
    this.authService.getUser().then(user => {
      this.token = user?.access_token;
    })
  }

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
      return from(this.authService.getUserToken()).pipe(
          switchMap(token => {
              if (token) {
                  request = request.clone({
                      setHeaders: {
                          Authorization: `Bearer ${token}`
                      }
                  });
              }
              return next.handle(request);
          })
      );
  }
}
