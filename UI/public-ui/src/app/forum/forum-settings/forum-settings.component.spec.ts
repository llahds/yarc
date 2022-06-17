import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ForumSettingsComponent } from './forum-settings.component';

describe('ForumSettingsComponent', () => {
  let component: ForumSettingsComponent;
  let fixture: ComponentFixture<ForumSettingsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ForumSettingsComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ForumSettingsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
