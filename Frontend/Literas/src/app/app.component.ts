import { Component } from '@angular/core';
import {AuthService} from "./services/auth/auth.service";
import {map} from "rxjs";
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.sass']
})
export class AppComponent {
  title = 'Literas';
  userToken: string | null = null;

  constructor(private authService: AuthService) {
    authService.getToken().subscribe(token => this.userToken = token);
  }
}
