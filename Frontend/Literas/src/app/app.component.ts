import { Component } from '@angular/core';
import {AuthService} from "./services/auth/auth.service";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.sass']
})
export class AppComponent {
  title = 'Literas';
  userToken: string | null = null;
  constructor(private authService: AuthService) {
    this.authService.getUser().then(user => {
      this.userToken = user?.access_token || null;
    });
  }
}
