import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PopularForumsComponent } from './popular-forums.component';

describe('PopularForumsComponent', () => {
  let component: PopularForumsComponent;
  let fixture: ComponentFixture<PopularForumsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PopularForumsComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PopularForumsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
