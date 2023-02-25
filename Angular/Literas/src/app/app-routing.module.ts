import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {DocListComponent} from "./components/docs/doc.list/doc.list.component";
import {AppComponent} from "./app.component";
import {PageNotFoundComponent} from "./components/system/page-not-found/page-not-found.component";
import {DocsComponent} from "./components/docs/docs/docs.component";

const routes: Routes = [
  {path:'docs', component: DocsComponent},
  {path:'', redirectTo: '/docs', pathMatch: 'full'},
  {path:'**', component: PageNotFoundComponent},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
