import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ModSearchUsersComponent } from './mod-search-users.component';

describe('ModSearchUsersComponent', () => {
  let component: ModSearchUsersComponent;
  let fixture: ComponentFixture<ModSearchUsersComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ModSearchUsersComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ModSearchUsersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
