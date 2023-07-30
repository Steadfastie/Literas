import { TestBed } from '@angular/core/testing';

import { FetchGuard } from './fetch.guard';

describe('FetchGuard', () => {
  let guard: FetchGuard;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    guard = TestBed.inject(FetchGuard);
  });

  it('should be created', () => {
    expect(guard).toBeTruthy();
  });
});
