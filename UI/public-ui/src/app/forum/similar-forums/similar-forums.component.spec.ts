import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SimilarForumsComponent } from './similar-forums.component';

describe('SimilarForumsComponent', () => {
  let component: SimilarForumsComponent;
  let fixture: ComponentFixture<SimilarForumsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SimilarForumsComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SimilarForumsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
