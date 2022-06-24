import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PrivateForumComponent } from './private-forum.component';

describe('PrivateForumComponent', () => {
  let component: PrivateForumComponent;
  let fixture: ComponentFixture<PrivateForumComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PrivateForumComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PrivateForumComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
