import { TestBed } from '@angular/core/testing';

import { StatusCodeInterceptor } from './status-code.interceptor';

describe('StatusCodeInterceptor', () => {
  beforeEach(() => TestBed.configureTestingModule({
    providers: [
      StatusCodeInterceptor
      ]
  }));

  it('should be created', () => {
    const interceptor: StatusCodeInterceptor = TestBed.inject(StatusCodeInterceptor);
    expect(interceptor).toBeTruthy();
  });
});
