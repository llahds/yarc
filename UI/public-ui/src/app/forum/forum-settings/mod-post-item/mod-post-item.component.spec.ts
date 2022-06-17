import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ModPostItemComponent } from './mod-post-item.component';

describe('ModPostItemComponent', () => {
  let component: ModPostItemComponent;
  let fixture: ComponentFixture<ModPostItemComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ModPostItemComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ModPostItemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
