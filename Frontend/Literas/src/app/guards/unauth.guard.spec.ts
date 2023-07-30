import { TestBed } from '@angular/core/testing';

import { UnauthGuard } from './unauth.guard';

describe('UnauthGuardGuard', () => {
  let guard: UnauthGuard;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    guard = TestBed.inject(UnauthGuard);
  });

  it('should be created', () => {
    expect(guard).toBeTruthy();
  });
});
