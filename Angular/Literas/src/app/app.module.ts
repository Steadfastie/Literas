import { NgModule, isDevMode } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import {AppComponent } from './app.component';
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
import { DocNewComponent } from './components/docs/doc.list/doc.new/doc.new.component';
import { MaterialModule } from "./modules/material.module";
import { DocThumbnailComponent } from './components/docs/doc.list/doc.thumbnail/doc.thumbnail.component';
import {QuillModule} from "ngx-quill";
import {ReactiveFormsModule} from "@angular/forms";
import {toolbarOptions} from "./config/quill-toolbar";
import { ToolbarComponent } from './components/quill-toolbar/toolbar/toolbar.component';
import {quillSelectionReducer} from "./state/reducers/quill.selection.reducer";

@NgModule({
  declarations: [
    AppComponent,
    DocCreateComponent,
    DocListComponent,
    PageNotFoundComponent,
    DocsComponent,
    DocEditComponent,
    DocNewComponent,
    DocThumbnailComponent,
    ToolbarComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    HttpClientModule,
    MaterialModule,
    StoreModule.forRoot({
      'docs_crud': docsReducer,
      'quill': quillSelectionReducer
    }, {}),
    EffectsModule.forRoot([DocCrudEffects]),
    StoreDevtoolsModule.instrument({ maxAge: 25, logOnly: !isDevMode() }),
    QuillModule.forRoot({
      modules: {
        toolbar: toolbarOptions
      },
      theme: 'bubble',
      customOptions: [{
        import: 'formats/font',
        whitelist: ['Sanchez', 'serif']
      }],
    }),
    ReactiveFormsModule,
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
