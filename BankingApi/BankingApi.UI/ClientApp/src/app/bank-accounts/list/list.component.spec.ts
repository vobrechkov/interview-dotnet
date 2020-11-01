import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ListBankAccountsComponent } from './list.component';

describe('ListBankAccountsComponent', () => {
    let component: ListBankAccountsComponent;
    let fixture: ComponentFixture<ListBankAccountsComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ListBankAccountsComponent]
        })
            .compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(ListBankAccountsComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
