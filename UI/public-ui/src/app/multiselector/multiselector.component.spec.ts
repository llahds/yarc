import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MultiselectorComponent } from './multiselector.component';

describe('MultiselectorComponent', () => {
  let component: MultiselectorComponent;
  let fixture: ComponentFixture<MultiselectorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MultiselectorComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MultiselectorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
