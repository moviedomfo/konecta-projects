import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AddreesComponent } from './addrees.component';

describe('AddreesComponent', () => {
  let component: AddreesComponent;
  let fixture: ComponentFixture<AddreesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AddreesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AddreesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
