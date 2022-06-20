import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { AuthenticationService } from './authentication.service';
import { CriticalErrorService } from './critical-error.service';

@Injectable({
  providedIn: 'root'
})
export class RestApiService {

  constructor(
    private http: HttpClient,
    private criticalErrors: CriticalErrorService,
    private authentication: AuthenticationService
  ) { }

  public getHeaders(): HttpHeaders {
    if (this.authentication.token) {
      return new HttpHeaders({
        Authorization: `Bearer ${this.authentication.token}`
      });
    } else {
      return new HttpHeaders();
    }
  }

  public get(url: string) {
    return this.criticalErrors.wrap(this.http.get(
      `${environment.service.url}/${url}`,
      {
        headers: this.getHeaders()
      }
    ));
  }

  public put(url: string, model: any) {
    return this.criticalErrors.wrap(this.http.put(
      `${environment.service.url}/${url}`,
      model,
      {
        headers: this.getHeaders()
      }
    ));
  }

  public post(url: string, model: any) {
    return this.criticalErrors.wrap(this.http.post(
      `${environment.service.url}/${url}`,
      model,
      {
        headers: this.getHeaders()
      }
    ));
  }

  public delete(url: string) {
    return this.criticalErrors.wrap(this.http.delete(
      `${environment.service.url}/${url}`,
      {
        headers: this.getHeaders()
      }
    ));
  }
}
