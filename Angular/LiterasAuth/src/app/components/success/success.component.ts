import {AfterViewInit, Component, OnDestroy, OnInit} from '@angular/core';
import {OperationsService} from "../../services/operations.service";
import {OperationResponse} from "../../models/operationResponse";
import {ActivatedRoute, Router} from "@angular/router";
import {Subject, takeUntil} from "rxjs";

@Component({
  selector: 'app-success',
  templateUrl: './success.component.html',
  styleUrls: ['./success.component.sass']
})
export class SuccessComponent implements AfterViewInit, OnDestroy{
  operation?: OperationResponse;
  returnUrl?: string;
  subManager$ = new Subject<void>();
  constructor(private operations: OperationsService,
              private router: Router,
              private activatedRoute: ActivatedRoute
  ) {
    this.operations.pick()
      .pipe(takeUntil(this.subManager$))
      .subscribe(operation => {
        if (operation != null && operation.succeeded) {
          this.operation = operation;
        }
        else{
          this.router.navigate(['../'], { relativeTo: this.activatedRoute });
        }
    })
  }
  ngOnDestroy(): void {
    this.subManager$.next();
    this.subManager$.complete();
  }

  ngAfterViewInit(): void {
    this.operations.getReturnUrl().pipe().subscribe(url => this.returnUrl = url);
    window.location.href = this.returnUrl!;
  }
}
