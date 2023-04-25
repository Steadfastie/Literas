import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {PageNotFoundComponent} from "./components/system/page-not-found/page-not-found.component";
import {DocsComponent} from "./components/docs/docs/docs.component";
import {DocEditComponent} from "./components/docs/doc.edit/doc.edit.component";
import {DocCreateComponent} from "./components/docs/doc.create/doc.create.component";
import {LoginComponent} from "./components/auth/login/login.component";

const routes: Routes = [
  { path:'docs',
    component: DocsComponent,
    children: [
      {
        path: 'create',
        component: DocCreateComponent
      },
      {
        path: ':id',
        component: DocEditComponent
      },
    ]
  },
  {
    path: 'login',
    component: LoginComponent
  },
  {path:'', redirectTo: '/docs', pathMatch: 'full'},
  {path:'**', component: PageNotFoundComponent},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
