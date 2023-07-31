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
}
