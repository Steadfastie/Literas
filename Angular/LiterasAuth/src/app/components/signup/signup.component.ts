import { Component } from '@angular/core';
import {FormBuilder, Validators} from "@angular/forms";
import {AuthService} from "../../services/auth.service";
import {ActivatedRoute, Router} from "@angular/router";

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.sass']
})
export class SignupComponent {
  succeeded = false;
  constructor(private fb: FormBuilder,
              private authService: AuthService,
              private router: Router,
              private activatedRoute: ActivatedRoute) { }

  signUpForm = this.fb.group({
    username: ['', [Validators.required]],
    password: ['', [Validators.required]]
  });

  ngOnInit(): void {

  }

  submit() {
    if (this.signUpForm.valid) {
      let useCredentials = {
        username: this.signUpForm.controls.username.value!,
        password: this.signUpForm.controls.password.value!
      }
      this.authService.signup(useCredentials).pipe().subscribe(() => {
        this.succeeded = true;
        this.router.navigate(['~/success'], {relativeTo: this.activatedRoute})
      })
    }
  }
}
