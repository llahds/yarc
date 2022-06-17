import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ForumSettingsApprovedComponent } from './forum-settings-approved.component';

describe('ForumSettingsApprovedComponent', () => {
  let component: ForumSettingsApprovedComponent;
  let fixture: ComponentFixture<ForumSettingsApprovedComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ForumSettingsApprovedComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ForumSettingsApprovedComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
