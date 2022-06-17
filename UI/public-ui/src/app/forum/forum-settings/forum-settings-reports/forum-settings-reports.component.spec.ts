import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ForumSettingsReportsComponent } from './forum-settings-reports.component';

describe('ForumSettingsReportsComponent', () => {
  let component: ForumSettingsReportsComponent;
  let fixture: ComponentFixture<ForumSettingsReportsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ForumSettingsReportsComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ForumSettingsReportsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
