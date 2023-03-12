import {AfterViewInit, Component, OnDestroy, OnInit, ViewChild} from '@angular/core';
import {FormBuilder, Validators} from "@angular/forms";
import {DocService} from "../../../services/docs/doc.service";
import {QuillEditorComponent} from "ngx-quill";

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
  @ViewChild('contentQuill') content?: QuillEditorComponent;
  constructor(private fb: FormBuilder,
              private docService: DocService){}

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
