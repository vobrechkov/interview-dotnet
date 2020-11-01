import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ListInstitutionsComponent } from './list.component';

describe('ListInstitutionsComponent', () => {
    let component: ListInstitutionsComponent;
    let fixture: ComponentFixture<ListInstitutionsComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ListInstitutionsComponent]
        })
            .compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(ListInstitutionsComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
