import { TestBed } from '@angular/core/testing';

import { SelectionFraudService } from './selection.fraud.service';

describe('SelectionFraudService', () => {
  let service: SelectionFraudService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(SelectionFraudService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
