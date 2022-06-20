import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-user-settings',
  templateUrl: './user-settings.component.html',
  styleUrls: ['./user-settings.component.scss']
})
export class UserSettingsComponent implements OnInit {

  public showChangeEmail: boolean = false;
  public showChangePassword: boolean = false;
  public showChangeUserName: boolean = false;

  constructor() { }

  ngOnInit(): void {
  }

}
