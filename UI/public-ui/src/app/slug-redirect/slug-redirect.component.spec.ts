import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SlugRedirectComponent } from './slug-redirect.component';

describe('SlugRedirectComponent', () => {
  let component: SlugRedirectComponent;
  let fixture: ComponentFixture<SlugRedirectComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SlugRedirectComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SlugRedirectComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
