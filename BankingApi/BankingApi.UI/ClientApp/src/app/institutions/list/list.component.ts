import { Component, Inject } from '@angular/core';
import { InstitutionsService, Institution } from '../institutions.service';

@Component({
  selector: 'app-list-institutions',
  templateUrl: './list.component.html'
})
export class ListInstitutionsComponent {
  public institutions: Institution[];

  constructor(institutionsService: InstitutionsService) {
    institutionsService.getAll().subscribe(result => {
        console.log(result);
        this.institutions = result;
      }, error => console.error(error));
    }
}

