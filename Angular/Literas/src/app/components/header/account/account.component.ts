import { Component } from '@angular/core';
import {AuthService} from "../../../services/auth/auth.service";
import {User} from "oidc-client-ts";

@Component({
  selector: 'account',
  templateUrl: './account.component.html',
  styleUrls: ['./account.component.sass']
})
export class AccountComponent {

  constructor(private authService: AuthService) { }
  logout(){
    this.authService.logout();
  }

}
