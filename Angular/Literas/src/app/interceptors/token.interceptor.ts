import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import {catchError, from, map, Observable, switchMap} from 'rxjs';
import {AuthService} from "../services/auth/auth.service";
import {Router} from "@angular/router";

@Injectable()
export class TokenInterceptor implements HttpInterceptor {
  constructor(private authService: AuthService,
              private router: Router) {
  }

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return from(this.authService.getToken().pipe(
      switchMap(token => {
        if (token != null){
          request = request.clone({
            setHeaders: {
              Authorization: `Bearer ${token}`
            }
          });

          // Try to send request with current access token
          return next.handle(request).pipe(
            catchError(error => {

              // If request failed due to expired access token
              if (error.status === 401) {

                // Try to refresh token
                from(this.authService.renewToken()).pipe(
                  switchMap(newToken => {
                    if (newToken !== null) {
                      // Clone the original request with the new access token
                      request = request.clone({
                        setHeaders: { Authorization: `Bearer ${newToken}` },
                      });

                      // Send the cloned request with the new access token
                      return next.handle(request);
                    }

                    // If new access token is null, log out
                    return from(this.authService.logout());
                  }),

                  // If token refresh fails, log out
                  catchError(() => {
                    return from(this.authService.logout());
                  })
                )}

              // If request failed due to other reasons forward the error
              return next.handle(error);
            })
          );
        }

        return next.handle(request).pipe(
          catchError(error => {
            if (error.status === 401) {
              this.router.navigate(['/login']);
            }
            return next.handle(error);
          })
        );
      })
    ));
  }
}
