import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DocCreateComponent } from './doc.create.component';

describe('DocCreateComponent', () => {
  let component: DocCreateComponent;
  let fixture: ComponentFixture<DocCreateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DocCreateComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DocCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
