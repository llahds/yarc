import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ForumPostCommentComponent } from './forum-post-comment.component';

describe('ForumPostCommentComponent', () => {
  let component: ForumPostCommentComponent;
  let fixture: ComponentFixture<ForumPostCommentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ForumPostCommentComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ForumPostCommentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
