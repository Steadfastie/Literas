import { TestBed } from '@angular/core/testing';

import { DocSubmitService } from './doc.submit.service';

describe('DocSubmitService', () => {
  let service: DocSubmitService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(DocSubmitService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
