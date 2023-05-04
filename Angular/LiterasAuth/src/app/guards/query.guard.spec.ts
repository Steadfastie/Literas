import { TestBed } from '@angular/core/testing';

import { QueryGuard } from './query.guard';

describe('QueryGuard', () => {
  let guard: QueryGuard;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    guard = TestBed.inject(QueryGuard);
  });

  it('should be created', () => {
    expect(guard).toBeTruthy();
  });
});
