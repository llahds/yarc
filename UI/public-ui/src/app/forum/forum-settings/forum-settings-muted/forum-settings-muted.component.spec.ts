import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ForumSettingsMutedComponent } from './forum-settings-muted.component';

describe('ForumSettingsMutedComponent', () => {
  let component: ForumSettingsMutedComponent;
  let fixture: ComponentFixture<ForumSettingsMutedComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ForumSettingsMutedComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ForumSettingsMutedComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
