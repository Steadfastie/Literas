import {AfterViewInit, Component, ElementRef, OnDestroy, OnInit, ViewChild} from '@angular/core';
import {FormBuilder, Validators} from "@angular/forms";
import {DocService} from "../../../services/docs/doc.service";
import {QuillEditorComponent} from "ngx-quill";
import {Store} from "@ngrx/store";
import { SelectionChange} from "ngx-quill/lib/quill-editor.component";
import * as quillSelectionActions from 'src/app/state/actions/quill.selection.actions';
import * as quillSelectionsSelectors from "../../../state/selectors/quill.selection.selectors";
import {Subject, takeUntil} from "rxjs";

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
  linkInputOpenState: boolean = false;
  subManager$: Subject<any> = new Subject();
  constructor(private fb: FormBuilder,
              private docService: DocService,
              private el: ElementRef,
              private store: Store){
    this.store.select(quillSelectionsSelectors.selectLinkInputOpenState)
      .pipe(takeUntil(this.subManager$))
      .subscribe(status => {
        this.linkInputOpenState = status;
      });
  }

  adaptToolBar(selectionChange: SelectionChange){
    if (this.linkInputOpenState){
      selectionChange.editor.formatText(
        selectionChange.oldRange?.index!,
        selectionChange.oldRange?.length!,
        'background', '#338dfa'
      );
      selectionChange.editor.formatText(
        selectionChange.oldRange?.index!,
        selectionChange.oldRange?.length!,
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

  ngOnDestroy(): void {
   this.subManager$.next('destroyed');
  }

  ngAfterViewInit(): void {
    if (this.title){
      this.title.styles = {'min-width':'fit-content', 'font-family': 'Sanchez, serif', 'font-size': '2.5rem'};
    }
    if (this.content){
      this.content.styles = {'min-width':'fit-content', 'font-family': 'Sanchez, serif', 'font-size': '1rem'};
      this.content.writeValue(
        `This content was auto generated.
       Please, proceed with caution.
      `)
    }
  }
}
