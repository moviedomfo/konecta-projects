import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { UserRessetPwdComponent } from './user-resset-pwd.component';

describe('UserRessetPwdComponent', () => {
  let component: UserRessetPwdComponent;
  let fixture: ComponentFixture<UserRessetPwdComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ UserRessetPwdComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UserRessetPwdComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
