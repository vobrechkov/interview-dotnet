import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CreateInstitutionComponent } from './create/create.component';
import { ListInstitutionsComponent } from './list/list.component';

const routes: Routes = [
  { path: 'institutions', component: ListInstitutionsComponent },
  { path: 'institutions/create', component: CreateInstitutionComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class InstitutionsRoutingModule { }
