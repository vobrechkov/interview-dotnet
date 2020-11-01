import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateBankAccountComponent } from './create.component';

describe('CreateBankAccountComponent', () => {
  let component: CreateBankAccountComponent;
  let fixture: ComponentFixture<CreateBankAccountComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CreateBankAccountComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateBankAccountComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
