import { TestBed } from '@angular/core/testing';

import { ForumGuardService } from './forum-guard.service';

describe('ForumGuardService', () => {
  let service: ForumGuardService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ForumGuardService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
