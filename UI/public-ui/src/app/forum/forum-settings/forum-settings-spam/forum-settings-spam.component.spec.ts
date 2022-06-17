import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ForumSettingsSpamComponent } from './forum-settings-spam.component';

describe('ForumSettingsSpamComponent', () => {
  let component: ForumSettingsSpamComponent;
  let fixture: ComponentFixture<ForumSettingsSpamComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ForumSettingsSpamComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ForumSettingsSpamComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
