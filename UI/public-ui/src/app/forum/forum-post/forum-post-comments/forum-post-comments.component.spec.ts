import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ForumPostCommentsComponent } from './forum-post-comments.component';

describe('ForumPostCommentsComponent', () => {
  let component: ForumPostCommentsComponent;
  let fixture: ComponentFixture<ForumPostCommentsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ForumPostCommentsComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ForumPostCommentsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
