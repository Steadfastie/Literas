import {AfterViewInit, Component, ElementRef, Input, OnDestroy, OnInit, ViewChild} from '@angular/core';
import {Store} from "@ngrx/store";
import * as quillSelectionsSelectors from "src/app/state/selectors/quill.selection.selectors";
import * as quillSelectionActions from 'src/app/state/actions/quill.selection.actions';
import {Subject, takeUntil} from "rxjs";
import {FormBuilder, Validators} from "@angular/forms";
import {RangeStatic} from "quill";
import {QuillEditorComponent} from "ngx-quill";

@Component({
  selector: 'quill-link',
  templateUrl: './link.component.html',
  styleUrls: ['./link.component.sass']
})
export class LinkComponent implements OnInit, AfterViewInit, OnDestroy{
  @Input() editor!: QuillEditorComponent;
  hasValidUrl: boolean = false;
  currentSelectionRange?: RangeStatic | null;
  url?: string;
  inputOpened: boolean = false;
  @ViewChild('saveButton', { read: ElementRef }) save?: ElementRef;
  subManager$: Subject<any> = new Subject();

  constructor(private store: Store,
              private fb: FormBuilder) {
    this.store.select(quillSelectionsSelectors.selectCurrentSelectionFormats)
      .pipe(takeUntil(this.subManager$))
      .subscribe(formats => {
        this.hasValidUrl = formats['link']?.length > 3;
        if (this.hasValidUrl) this.url = formats['link'];
      });

    this.store.select(quillSelectionsSelectors.selectLinkInputOpened)
      .pipe(takeUntil(this.subManager$))
      .subscribe(status => {
        this.inputOpened = status;
      });

    this.store.select(quillSelectionsSelectors.selectCurrentRange)
      .pipe(takeUntil(this.subManager$))
      .subscribe(range => {
        this.currentSelectionRange = range;
      });
  }



  urlForm = this.fb.group({
    url: ['', [Validators.minLength(3),Validators.pattern('(https?://)?([\\da-z.-]+)\\.([a-z.]{2,6})[/\\w .-]*/?')]]
  })

  switchInput(){
    this.urlForm.reset();
    this.editor.quillEditor.formatText(
      this.currentSelectionRange!.index,
      this.currentSelectionRange!.length,
      'background', ''
    );
    this.editor.quillEditor.formatText(
      this.currentSelectionRange!.index,
      this.currentSelectionRange!.length,
      'color', ''
    );

    this.store.dispatch(quillSelectionActions.quill_switchLinkInput());
    this.editor.quillEditor.setSelection(this.currentSelectionRange!.index, this.currentSelectionRange!.length);
  }

  submit(){
    if(this.urlForm.invalid) return;
    if(this.urlForm.value?.url?.length! <= 3) return;

    let formValue = this.urlForm.value.url!;
    if (!/^https?:\/\//i.test(formValue)) {
      formValue = 'https://' + formValue;
    }

    this.editor.quillEditor.formatText(
      this.currentSelectionRange!.index,
      this.currentSelectionRange!.length,
      'link', formValue
    );

    this.store.dispatch(
      quillSelectionActions.quill_formatChange(
      {format: 'link', value: formValue}
      )
    );
    this.switchInput();
  }
  ngOnInit(): void {
  }
  ngAfterViewInit(): void {
    this.urlForm.statusChanges
      .pipe(takeUntil(this.subManager$))
      .subscribe(status => {
        if (this.save){
          if (status === 'INVALID') {
            this.save.nativeElement.disabled = true;
            this.save.nativeElement.style.color = '#ffc2c2';
          }
          else if (status === 'VALID') {
            this.save.nativeElement.disabled = false;
            this.save.nativeElement.style = `
            {
                color: #e0e0e0
                transition: 0.5s
                 &:hover
                   color: #5f5f5f
                   transition: 0.5s
            }`;
          }
        }
      })
  }
  ngOnDestroy(): void {
    this.subManager$.next('destroyed');
  }
}
