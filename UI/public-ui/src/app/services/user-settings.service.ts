import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ChangeEmail, ChangePassword, ChangeUserName, UserSettings } from './models/users';
import { RestApiService } from './rest-api.service';

@Injectable({
  providedIn: 'root'
})
export class UserSettingsService {

  constructor(
    private client: RestApiService) {

  }

  get() : Observable<UserSettings> {
    return this.client.get("api/1.0/user-settings");
  }

  update(model: UserSettings) {
    return this.client.put("api/1.0/user-settings", model);
  }

  changeUserName(model: ChangeUserName) {
    return this.client.put("api/1.0/user-settings/user-name", model);
  }

  changePassword(model: ChangePassword) {
    return this.client.put("api/1.0/user-settings/password", model);
  }  

  changeEmail(model: ChangeEmail) {
    return this.client.put("api/1.0/user-settings/email", model);
  }
}
