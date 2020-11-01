import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { InstitutionsRoutingModule } from './institutions-routing.module';
import { ListInstitutionsComponent } from './list/list.component';
import { CreateInstitutionComponent } from './create/create.component';

@NgModule({
  declarations: [ListInstitutionsComponent, CreateInstitutionComponent],
  imports: [
    CommonModule,
    InstitutionsRoutingModule,
    FormsModule
  ]
})
export class InstitutionsModule { }
