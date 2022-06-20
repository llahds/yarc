import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AuthenticationToken, Register } from './models/users';
import { RestApiService } from './rest-api.service';

@Injectable({
  providedIn: 'root'
})
export class RegistrationService {

  constructor(
    private client: RestApiService
  ) { }

  register(model: Register): Observable<AuthenticationToken> {
    return this.client.post("api/1.0/register", model);
  }
}
