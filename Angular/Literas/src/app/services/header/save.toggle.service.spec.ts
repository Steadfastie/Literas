import { TestBed } from '@angular/core/testing';

import { SaveToggleService } from './save.toggle.service';

describe('SaveToggleService', () => {
  let service: SaveToggleService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(SaveToggleService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
