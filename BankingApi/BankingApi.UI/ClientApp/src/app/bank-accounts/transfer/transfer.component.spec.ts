import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { BankAccountTransferComponent } from './transfer.component';

describe('BankAccountTransferComponent', () => {
  let component: BankAccountTransferComponent;
  let fixture: ComponentFixture<BankAccountTransferComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [BankAccountTransferComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BankAccountTransferComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
