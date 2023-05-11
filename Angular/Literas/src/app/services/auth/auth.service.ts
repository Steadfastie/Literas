import { Injectable } from '@angular/core';
import {UserManager} from "oidc-client-ts";
import {client} from "src/environment/oidc";
import {from, Observable} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  userManager: UserManager;
  constructor() {
    const settings = {
      authority: "https://localhost:7034",
      client_id: client.client_id,
      client_secret: client.client_secret,
      redirect_uri: client.redirect_uri,
      response_type: client.response_type,
      scope: client.scope
    };
    this.userManager = new UserManager(settings);
    this.userManager.signinRedirectCallback().then(user => {
      console.log(user);
      this.userManager.storeUser(user);
    });
  }

  public login(): Promise<void> {
    return this.userManager.signinRedirect();
  }

  public getToken(): Observable<string | null> {
    return from(this.userManager.getUser().then(user => {
      return user?.access_token || null;
    }));
  }

  public renewToken(): Promise<string | null> {
    return this.userManager.signinSilent().then(user => {
      return user?.access_token || null;
    });
  }

  public logout(): Promise<void> {
    return this.userManager.signoutRedirect();
  }
}
