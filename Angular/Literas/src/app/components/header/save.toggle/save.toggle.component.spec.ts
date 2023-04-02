import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SaveToggleComponent } from './save.toggle.component';

describe('SaveToggleComponent', () => {
  let component: SaveToggleComponent;
  let fixture: ComponentFixture<SaveToggleComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SaveToggleComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SaveToggleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
