import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CheckApisComponent } from './check-apis.component';

describe('CheckApisComponent', () => {
  let component: CheckApisComponent;
  let fixture: ComponentFixture<CheckApisComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CheckApisComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CheckApisComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
