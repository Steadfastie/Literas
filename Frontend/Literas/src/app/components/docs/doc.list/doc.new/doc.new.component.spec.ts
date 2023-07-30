import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DocNewComponent } from './doc.new.component';

describe('DocNewComponent', () => {
  let component: DocNewComponent;
  let fixture: ComponentFixture<DocNewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DocNewComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DocNewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
