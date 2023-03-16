import {AfterViewInit, Component, ElementRef, OnDestroy, OnInit, Renderer2, ViewChild} from '@angular/core';
import {FormBuilder, Validators} from "@angular/forms";
import {DocService} from "../../../services/docs/doc.service";
import {Blur, QuillEditorComponent} from "ngx-quill";
import {Store} from "@ngrx/store";
import {ContentChange, SelectionChange} from "ngx-quill/lib/quill-editor.component";
import * as quillSelectionActions from 'src/app/state/actions/quill.selection.actions';
import {Observable, Subject} from "rxjs";

@Component({
  selector: 'doc-create',
  templateUrl: './doc.create.component.html',
  styleUrls: ['./doc.create.component.sass']
})
export class DocCreateComponent implements OnInit, OnDestroy, AfterViewInit {
  creationForm = this.fb.group({
    title: ['', Validators.required, Validators.minLength(3)],
    content: ['', Validators.required, Validators.minLength(3)]
  });
  @ViewChild('titleQuill') title?: QuillEditorComponent;
  @ViewChild('contentQuill', {static: true}) content!: QuillEditorComponent;
  constructor(private fb: FormBuilder,
              private docService: DocService,
              private el: ElementRef,
              private store: Store){}

  adaptToolBar(selectionChange: SelectionChange){
    let range = selectionChange.range!;
    if (range.length !==0 ){
      let selectedText = selectionChange.editor.getText(range.index, range.length);
      let selectedTextFormats = selectionChange.editor.getFormat(range.index, range.length);
      this.store.dispatch(quillSelectionActions.quill_newSelection(
        {range: range, text: selectedText, formats: selectedTextFormats}));
    }
  }

  focusOff(){
    this.store.dispatch(quillSelectionActions.quill_focusOff());
  }

  ngOnInit(): void {

  }

  ngOnDestroy(): void {
    throw new Error('Method not implemented.');
  }

  ngAfterViewInit(): void {
    if (this.title){
      this.title.styles = {'min-width':'fit-content', 'font-family': 'Sanchez, serif', 'font-size': '2.5rem'};
    }
    if (this.content){
      this.content.styles = {'min-width':'fit-content', 'font-family': 'Sanchez, serif', 'font-size': '1rem'};
    }
  }
}
