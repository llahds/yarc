import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ControlValidationErrorsComponent } from './control-validation-errors.component';

describe('ControlValidationErrorsComponent', () => {
  let component: ControlValidationErrorsComponent;
  let fixture: ComponentFixture<ControlValidationErrorsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ControlValidationErrorsComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ControlValidationErrorsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
