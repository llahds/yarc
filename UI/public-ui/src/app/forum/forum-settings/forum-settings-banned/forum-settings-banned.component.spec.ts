import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ForumSettingsBannedComponent } from './forum-settings-banned.component';

describe('ForumSettingsBannedComponent', () => {
  let component: ForumSettingsBannedComponent;
  let fixture: ComponentFixture<ForumSettingsBannedComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ForumSettingsBannedComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ForumSettingsBannedComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
