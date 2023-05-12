import {AfterViewInit, Component, OnDestroy, OnInit} from '@angular/core';
import {AuthService} from "../../services/auth.service";
import {Subject, take, takeUntil} from "rxjs";
import {OperationsService} from "../../services/operations.service";
import {OperationResponse} from "../../models/operationResponse";
import {ActivatedRoute, Router} from "@angular/router";

@Component({
  selector: 'logout',
  templateUrl: './logout.component.html',
  styleUrls: ['./logout.component.sass']
})
export class LogoutComponent  implements OnInit, AfterViewInit, OnDestroy {
  operationResponse: OperationResponse | null = null;
  logoutId: string = '';
  subManager$ = new Subject<void>();

  constructor(private authService: AuthService,
              private operations: OperationsService,
              private activatedRoute: ActivatedRoute,
              private router: Router){
  }

  ngOnInit(): void {
    this.operations.pick()
      .pipe(takeUntil(this.subManager$))
      .subscribe(operation => {
        this.operationResponse = operation;
      });
    const logoutId = this.activatedRoute.snapshot.queryParamMap.get('logoutId');
    if (!logoutId){
      this.router.navigate(['./error'])
    }
    this.logoutId = logoutId!;
  }

  ngAfterViewInit(): void {
    this.authService.logout(this.logoutId!)
      .pipe(takeUntil(this.subManager$))
      .subscribe(response => {
        if (response.succeeded){
          /*It is vital to first push a response to service and then redirect due to guard setup*/
          this.operations.push(response);
          this.router.navigate(['./success'], {
            relativeTo: this.activatedRoute,
            queryParamsHandling: 'preserve',
            preserveFragment: true
          });
        } else {
          /*It is vital to first push a response to service and then redirect due to guard setup*/
          this.operations.push(response);
          this.router.navigate(['./error'], {
            relativeTo: this.activatedRoute,
            queryParamsHandling: 'preserve',
            preserveFragment: true
          });
        }});
  }

  ngOnDestroy(): void {
    this.subManager$.next();
    this.subManager$.complete();
  }

}
