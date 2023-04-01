import { TestBed } from '@angular/core/testing';

import { DocCompareService } from './doc.compare.service';

describe('DocCompareService', () => {
  let service: DocCompareService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(DocCompareService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
