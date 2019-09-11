import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { UserResetComponentComponent } from './user-reset-component.component';

describe('UserResetComponentComponent', () => {
  let component: UserResetComponentComponent;
  let fixture: ComponentFixture<UserResetComponentComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ UserResetComponentComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UserResetComponentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
