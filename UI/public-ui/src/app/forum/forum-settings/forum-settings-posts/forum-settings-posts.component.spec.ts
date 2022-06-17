import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ForumSettingsPostsComponent } from './forum-settings-posts.component';

describe('ForumSettingsPostsComponent', () => {
  let component: ForumSettingsPostsComponent;
  let fixture: ComponentFixture<ForumSettingsPostsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ForumSettingsPostsComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ForumSettingsPostsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
