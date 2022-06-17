import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ForumSettingsAutomaticComponent } from './forum-settings-automatic.component';

describe('ForumSettingsAutomaticComponent', () => {
  let component: ForumSettingsAutomaticComponent;
  let fixture: ComponentFixture<ForumSettingsAutomaticComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ForumSettingsAutomaticComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ForumSettingsAutomaticComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
