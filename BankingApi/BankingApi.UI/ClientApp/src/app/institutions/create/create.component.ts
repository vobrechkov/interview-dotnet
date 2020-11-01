import { Component, OnInit } from '@angular/core';
import { InstitutionsService, Institution } from '../institutions.service';

@Component({
  selector: 'app-create',
  templateUrl: './create.component.html',
  styleUrls: ['./create.component.css']
})
export class CreateInstitutionComponent implements OnInit {
  private institution: Institution;
  submitted = false;

  constructor(private service: InstitutionsService) {
    this.clearInstitution();
  }

  ngOnInit() {
  }

  createInstitution(): void {
    const data = {
      name: this.institution.name,
      phone: this.institution.phone,
      email: this.institution.email,
      website: this.institution.website
    };
    this.service.create(data)
      .subscribe(response => {
        console.log(response);
        this.institution.id = response.id;
        this.institution.createdAt = response.createdAt;
        this.submitted = true;
      },
      error => {
        console.log(error);
      });
  }

  clearInstitution(): void {
    this.submitted = false;
    this.institution = {
      id: '',
      name: '',
      phone: '',
      email: '',
      website: '',
      createdAt: '',
      updatedAt: ''
    };
  }
}
