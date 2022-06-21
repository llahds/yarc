import { TestBed } from '@angular/core/testing';

import { ForumUserManagementService } from './forum-user-management.service';

describe('ForumUserManagementService', () => {
  let service: ForumUserManagementService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ForumUserManagementService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
