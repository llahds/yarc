import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddRemoveStringsComponent } from './add-remove-strings.component';

describe('AddRemoveStringsComponent', () => {
  let component: AddRemoveStringsComponent;
  let fixture: ComponentFixture<AddRemoveStringsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AddRemoveStringsComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddRemoveStringsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
