import { Component } from '@angular/core';
import {OperationResponse} from "../../models/operationResponse";
import {OperationsService} from "../../services/operations.service";
import {ActivatedRoute, Router} from "@angular/router";
import {Subject, takeUntil} from "rxjs";

@Component({
  selector: 'error',
  templateUrl: './error.component.html',
  styleUrls: ['./error.component.sass']
})
export class ErrorComponent {
  operation?: OperationResponse;
  subManager$ = new Subject<void>();
  constructor(private operations: OperationsService,
              private router: Router,
              private activatedRoute: ActivatedRoute
  ) {
    this.operations.pick()
      .pipe(takeUntil(this.subManager$))
      .subscribe(operation => {
        if (operation != null && !operation.succeeded) {
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
}
