import { Component, OnInit } from '@angular/core';
import { UserSettings } from '../services/models/users';
import { UserSettingsService } from '../services/user-settings.service';

@Component({
  selector: 'app-user-settings',
  templateUrl: './user-settings.component.html',
  styleUrls: ['./user-settings.component.scss']
})
export class UserSettingsComponent implements OnInit {

  public showChangeEmail: boolean = false;
  public showChangePassword: boolean = false;
  public showChangeUserName: boolean = false;

  public entity: UserSettings = { displayName: "", about: "" };
  public isSaving: boolean = false;
  public errors: any = {};

  constructor(
    private api: UserSettingsService
  ) { }

  ngOnInit(): void {
    this.api.get().subscribe(r => this.entity = r);
  }

  save() {
    this.errors = {};
    this.isSaving = true;
    this.api.update(this.entity).subscribe(r => {
      this.isSaving = false;
    }, e => {
      this.errors = e.error;
      this.isSaving = false;
    });
  }
}
