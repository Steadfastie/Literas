import { NgModule, isDevMode } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { DocCreateComponent } from './components/docs/doc.create/doc.create.component';
import { DocListComponent } from './components/docs/doc.list/doc.list.component';
import { PageNotFoundComponent } from './components/system/page-not-found/page-not-found.component';
import { DocsComponent } from './components/docs/docs/docs.component';
import { StoreDevtoolsModule } from '@ngrx/store-devtools';
import { StoreModule } from '@ngrx/store';
import { docsReducer } from "./state/reducers/docs.reducer";
import { EffectsModule } from '@ngrx/effects';
import { DocEditComponent } from './components/docs/doc.edit/doc.edit.component';
import { DocCrudEffects } from "./state/effects/doc.effects";
import { HttpClientModule } from "@angular/common/http";

@NgModule({
  declarations: [
    AppComponent,
    DocCreateComponent,
    DocListComponent,
    PageNotFoundComponent,
    DocsComponent,
    DocEditComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    HttpClientModule,
    StoreModule.forRoot({ 'docs_crud': docsReducer }, {}),
    EffectsModule.forRoot([DocCrudEffects]),
    StoreDevtoolsModule.instrument({ maxAge: 25, logOnly: !isDevMode() }),
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
