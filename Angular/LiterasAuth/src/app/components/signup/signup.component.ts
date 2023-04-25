import {Component, OnDestroy, OnInit} from '@angular/core';
import {FormBuilder, Validators} from "@angular/forms";
import {AuthService} from "../../services/auth.service";
import {ActivatedRoute, Router} from "@angular/router";
import {OperationResponse} from "../../models/operationResponse";
import {Subject, takeUntil} from "rxjs";
import {OperationsService} from "../../services/operations.service";

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.sass']
})
export class SignupComponent implements OnInit, OnDestroy {
  formSent = false;
  operationResponse: OperationResponse | null = null;
  subManager$ = new Subject<void>();
  constructor(private fb: FormBuilder,
              private authService: AuthService,
              private router: Router,
              private activatedRoute: ActivatedRoute,
              private operations: OperationsService) {
  }
  signupForm = this.fb.group({
    username: ['', [Validators.required]],
    password: ['', [Validators.required]]
  });
  ngOnInit(): void {
    this.operations.pick()
      .pipe(takeUntil(this.subManager$))
      .subscribe(operation => {
        this.operationResponse = operation;
      })
  }
  submit() {
    if (this.signupForm.valid) {
      let useCredentials = {
        username: this.signupForm.controls.username.value!,
        password: this.signupForm.controls.password.value!,
        returnUrl: ''
      }
      this.authService.signup(useCredentials).pipe().subscribe(response => {
        if (response.succeeded){
          this.formSent = true;
          this.operations.push(response);
          this.router.navigate(['./success'], {relativeTo: this.activatedRoute})
        }
        else{
          this.formSent = true;
          this.operations.push(response);
          this.router.navigate(['./error'], {relativeTo: this.activatedRoute})
        }
      })
    }
  }
  ngOnDestroy(): void {
    this.subManager$.next();
    this.subManager$.complete();
  }
}
