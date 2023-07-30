import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor, HttpResponse
} from '@angular/common/http';
import {Observable, tap, throwError} from 'rxjs';

@Injectable()
export class StatusCodeInterceptor implements HttpInterceptor {

  constructor() {}

  // This interceptor is used to catch 304 status code and throw an error
  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(
      tap(event => {
        if (event instanceof HttpResponse && event.status === 304) {
          throwError(() => request);
        }
      })
    );
  }
}
