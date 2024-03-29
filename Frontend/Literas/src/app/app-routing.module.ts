import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {PageNotFoundComponent} from "./components/system/page-not-found/page-not-found.component";
import {DocsComponent} from "./components/docs/docs/docs.component";
import {DocEditComponent} from "./components/docs/doc.edit/doc.edit.component";
import {DocCreateComponent} from "./components/docs/doc.create/doc.create.component";
import {LoginComponent} from "./components/auth/login/login.component";
import {AuthGuard} from "./guards/auth.guard";
import {FetchGuard} from "./guards/fetch.guard";
import {UnauthGuard} from "./guards/unauth.guard";
import {SigninCallbackComponent} from "./components/auth/signin-callback/signin-callback.component";

const routes: Routes = [
  { path:'docs',
    component: DocsComponent,
    canActivate: [AuthGuard],
    children: [
      {
        path: 'create',
        component: DocCreateComponent
      },
      {
        path: ':id',
        component: DocEditComponent,
        canActivate: [FetchGuard]
      },
      {
        path: '',
        redirectTo: 'create',
        pathMatch: 'full'
      }
    ]
  },
  {
    path: 'login',
    component: LoginComponent,
    canActivate: [UnauthGuard]
  },
  {path:'signin-callback', component: SigninCallbackComponent, pathMatch: 'full'},
  {path:'', redirectTo: '/docs/create', pathMatch: 'full'},
  {path:'**', component: PageNotFoundComponent},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
