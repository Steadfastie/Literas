import { Injectable } from '@angular/core';
import {UserManager} from "oidc-client-ts";
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
}
