import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {LoginComponent} from "./components/login/login.component";
import {SignupComponent} from "./components/signup/signup.component";
import {SuccessComponent} from "./components/success/success.component";
import {ErrorComponent} from "./components/error/error.component";
import {ServerErrorComponent} from "./components/server-error/server-error.component";
import {QueryGuard} from "./guards/query.guard";
import {SucccessOperationGuard} from "./guards/succcess-operation-guard.service";
import {ErrorOperationGuard} from "./guards/error-operation.guard";
import {LogoutComponent} from "./components/logout/logout.component";

const routes: Routes = [
  {
    path: 'login',
    component: LoginComponent,
    canActivate: [QueryGuard],
    children: [
      {
        path:'success',
        component: SuccessComponent,
        canActivateChild: [SucccessOperationGuard]
      },
      {
        path:'error',
        component: ErrorComponent,
        canActivateChild: [ErrorOperationGuard]
      }
    ]
  },
  {
    path: 'signup',
    component: SignupComponent,
    canActivate: [QueryGuard],
    children: [
      {
        path:'success',
        component: SuccessComponent,
        canActivateChild: [SucccessOperationGuard]
      },
      {
        path:'error',
        component: ErrorComponent,
        canActivateChild: [ErrorOperationGuard]
      }
    ]
  },
  {
    path: 'logout',
    component: LogoutComponent,
    canActivate: [QueryGuard],
    children: [
      {
        path:'success',
        component: SuccessComponent,
        canActivateChild: [SucccessOperationGuard]
      },
      {
        path:'error',
        component: ErrorComponent,
        canActivateChild: [ErrorOperationGuard]
      }
    ]
  },
  {path: 'error', component: ServerErrorComponent},
  {path:'**', redirectTo: '/error', pathMatch: 'full'},
  {path:'', redirectTo: '/login', pathMatch: 'full'}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
