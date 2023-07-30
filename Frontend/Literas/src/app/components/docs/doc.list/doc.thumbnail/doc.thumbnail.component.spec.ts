import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DocThumbnailComponent } from './doc.thumbnail.component';

describe('DocThumbnailComponent', () => {
  let component: DocThumbnailComponent;
  let fixture: ComponentFixture<DocThumbnailComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DocThumbnailComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DocThumbnailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
