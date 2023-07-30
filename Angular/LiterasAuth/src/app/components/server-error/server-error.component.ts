import { Component } from '@angular/core';

@Component({
  selector: 'server-error',
  templateUrl: './server-error.component.html',
  styleUrls: ['./server-error.component.sass']
})
export class ServerErrorComponent {
  constructor() { }
  returnToMainApp() {
    window.location.assign('https://localhost:4200');
  }
}
