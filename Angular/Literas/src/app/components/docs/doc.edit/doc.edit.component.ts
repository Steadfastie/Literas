import {AfterViewInit, Component, OnDestroy, OnInit, ViewChild} from '@angular/core';
import {FormBuilder, Validators} from "@angular/forms";
import {QuillEditorComponent} from "ngx-quill";
import {Subject} from "rxjs";
import {SelectionChange} from "ngx-quill/lib/quill-editor.component";
import * as quillSelectionActions from 'src/app/state/actions/quill.selection.actions';
import {Store} from "@ngrx/store";

@Component({
  selector: 'doc-edit',
  templateUrl: './doc.edit.component.html',
  styleUrls: ['./doc.edit.component.sass']
})
export class DocEditComponent implements OnInit, OnDestroy, AfterViewInit{
  editForm = this.fb.group({
    title: ['', [Validators.required, Validators.minLength(3)]],
    content: ['', [Validators.required, Validators.minLength(3)]]
  });
  @ViewChild('titleQuill') title?: QuillEditorComponent;
  @ViewChild('contentQuill') content!: QuillEditorComponent;
  linkInputOpenState: boolean = false;
  subManager$: Subject<any> = new Subject();
  constructor(private fb: FormBuilder,
              private store: Store){

  }

  adaptToolBar(selectionChange: SelectionChange){
    if (this.linkInputOpenState && selectionChange.oldRange){
      selectionChange.editor.formatText(
        selectionChange.oldRange.index,
        selectionChange.oldRange.length,
        'background', '#338dfa'
      );
      selectionChange.editor.formatText(
        selectionChange.oldRange.index,
        selectionChange.oldRange.length,
        'color', 'white'
      );
      return;
    }

    let range = selectionChange.range!;
    if (range.length !==0 ){
      let selectedText = selectionChange.editor.getText(range.index, range.length);
      let selectedTextFormats = selectionChange.editor.getFormat(range.index, range.length);

      const selection = window.getSelection()!;
      const rangeRect = selection.getRangeAt(0).getBoundingClientRect();

      this.store.dispatch(quillSelectionActions.quill_newSelection({
        range: range,
        bounds: {left: rangeRect.x, top: rangeRect.y},
        text: selectedText,
        formats: selectedTextFormats,
        linkInputOpenState: false
      }));
    }
    else {
      this.store.dispatch(quillSelectionActions.quill_focusOff());
    }
  }


  ngOnInit(): void {
  }
  ngAfterViewInit(): void {
    if (this.title){
      this.title.styles = {'min-width':'fit-content', 'font-family': 'Sanchez, serif', 'font-size': '2.5rem'};
    }
    if (this.content){
      this.content.styles = {'min-width':'fit-content', 'font-family': 'Sanchez, serif', 'font-size': '1rem'};
    }

    this.content!.elementRef.nativeElement.addEventListener('click', (event: Event) => {
      const target = event.target as HTMLElement;

      if (target.tagName === 'A') {
        event.preventDefault();
        window.open(target.getAttribute('href')!, '_blank');
      }
    });
  }

  ngOnDestroy(): void {
    this.subManager$.next('destroyed');
  }
}
