import {Component, OnDestroy, OnInit} from '@angular/core';
import {FormBuilder, Validators} from "@angular/forms";
import {AuthService} from "../../services/auth.service";
import {ActivatedRoute, Router} from "@angular/router";
import {Subject, takeUntil} from "rxjs";
import {OperationResponse} from "../../models/operationResponse";
import {OperationsService} from "../../services/operations.service";

@Component({
  selector: 'login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.sass']
})
export class LoginComponent implements OnInit, OnDestroy {
  formSent = false;
  operationResponse: OperationResponse | null = null;
  subManager$ = new Subject<void>();
  constructor(private fb: FormBuilder,
              private authService: AuthService,
              private router: Router,
              private activatedRoute: ActivatedRoute,
              private operations: OperationsService) {
  }
  loginForm = this.fb.group({
    username: ['', [Validators.required]],
    password: ['', [Validators.required]]
  });
  ngOnInit(): void {
    this.operations.pick()
      .pipe(takeUntil(this.subManager$))
      .subscribe(operation => {
        this.operationResponse = operation;
      });
  }
  submit() {
    if (this.loginForm.valid) {
      let useCredentials = {
        username: this.loginForm.controls.username.value!,
        password: this.loginForm.controls.password.value!,
        returnUrl: ''
      }
      this.authService.login(useCredentials).pipe().subscribe(response => {
        if (response.succeeded){
          this.formSent = true;

          /*It is vital to first push a response to service and then redirect due to guard setup*/
          this.operations.push(response);
          this.router.navigate(['./success'], {
            relativeTo: this.activatedRoute,
            queryParamsHandling: 'preserve',
            preserveFragment: true
          });
        }
        else{
          this.formSent = true;

          /*It is vital to first push a response to service and then redirect due to guard setup*/
          this.operations.push(response);
          this.router.navigate(['./error'], {
            relativeTo: this.activatedRoute,
            queryParamsHandling: 'preserve',
            preserveFragment: true
          });
        }
      })
    }
  }
  ngOnDestroy(): void {
    this.subManager$.next();
    this.subManager$.complete();
  }
}
