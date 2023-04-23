import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {LoginComponent} from "./components/login/login.component";
import {SignupComponent} from "./components/signup/signup.component";
import {SuccessComponent} from "./components/success/success.component";
import {ErrorComponent} from "./components/error/error.component";

const routes: Routes = [
  {
    path: 'login',
    component: LoginComponent,
    children: [
      {
        path:'success',
        component: SuccessComponent
      },
      {
        path:'error',
        component: ErrorComponent
      }
    ]
  },
  {
    path: 'signup',
    component: SignupComponent,
    children: [
      {
        path:'success',
        component: SuccessComponent
      },
      {
        path:'error',
        component: ErrorComponent
      }
    ]
  },
  {path:'**', redirectTo: '/login', pathMatch: 'full'},
  {path:'', redirectTo: '/login', pathMatch: 'full'}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
