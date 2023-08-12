import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor, HttpResponse
} from '@angular/common/http';
import {from, Observable, switchMap, tap, throwError} from 'rxjs';
import {AuthService} from "../services/auth/auth.service";

@Injectable()
export class StatusCodeInterceptor implements HttpInterceptor {

  constructor(private authService: AuthService) {}

  // This interceptor is used to catch 304 status code and throw an error
  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(
      tap(event => {
        if (event instanceof HttpResponse && event.status === 304) {
          throwError(() => request);
        }
        if (event instanceof HttpResponse && event.status === 401) {
          from(this.authService.renewToken()).pipe(
              switchMap(newToken => {
                if (newToken) {
                  request = request.clone({
                    setHeaders: {
                      Authorization: `Bearer ${newToken}`
                    }
                  });
                }
                return next.handle(request);
              })
          );
        }
      }));
  }
}
