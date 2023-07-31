import {Component, OnInit} from '@angular/core';
import {AuthService} from "../../../services/auth/auth.service";
import {Router} from "@angular/router";

@Component({
  selector: 'signin-callback',
  templateUrl: './signin-callback.component.html',
  styleUrls: ['./signin-callback.component.sass']
})
export class SigninCallbackComponent implements OnInit {
  constructor(private readonly _router: Router, private readonly _authService: AuthService) {}

  async ngOnInit() {
    await this._authService.userManager.signinCallback();
    this._router.navigate(['']);
  }
}
