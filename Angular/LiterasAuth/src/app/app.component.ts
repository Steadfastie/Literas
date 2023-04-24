import {Component, OnInit} from '@angular/core';
import {OperationsService} from "./services/operations.service";
import {ActivatedRoute} from "@angular/router";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.sass']
})
export class AppComponent implements OnInit {
  title = 'LiterasAuth';

  constructor(private operationsService: OperationsService,
              private activatedRoute: ActivatedRoute) {
  }

  ngOnInit(): void {
    this.activatedRoute.queryParams.subscribe(params => {
      this.operationsService.saveReturnUrl(params['ReturnUrl']);
    });
  }
}
