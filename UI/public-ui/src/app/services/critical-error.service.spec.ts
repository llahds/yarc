import { TestBed } from '@angular/core/testing';

import { CriticalErrorService } from './critical-error.service';

describe('CriticalErrorService', () => {
  let service: CriticalErrorService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CriticalErrorService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
