import { Injectable } from '@angular/core';
import {User, UserManager} from "oidc-client-ts";
import {client} from "src/environment/oidc";

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  userManager: UserManager;
  constructor() {
    const settings = {
      authority: "https://localhost:7034",
      client_id: client.client_id,
      redirect_uri: client.redirect_uri,
      response_type: client.response_type,
      scope: client.scope
    };
    this.userManager = new UserManager(settings);
  }

  public login(): Promise<void> {
    return this.userManager.signinRedirect();
  }

  public getToken(): Promise<string | null> {
    return this.userManager.getUser().then(user => {
      return user?.access_token || null;
    });
  }

  public renewToken(): Promise<User | null> {
    return this.userManager.signinSilent();
  }

  public logout(): Promise<void> {
    return this.userManager.signoutRedirect();
  }
}
