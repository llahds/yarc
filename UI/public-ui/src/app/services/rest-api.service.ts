import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { CriticalErrorService } from './critical-error.service';

@Injectable({
  providedIn: 'root'
})
export class RestApiService {

  constructor(
    private http: HttpClient,
    private criticalErrors: CriticalErrorService,
  ) { }

  public get(url: string) {
    return this.criticalErrors.wrap(this.http.get(
      `${environment.service.url}/${url}`,
      {
        headers: new HttpHeaders({
          //Authorization: `Bearer ${this.authenticationService.token}`
        })
      }
    ));
  }

  public put(url: string, model: any) {
    return this.criticalErrors.wrap(this.http.put(
      `${environment.service.url}/${url}`,
      model,
      {
        headers: new HttpHeaders({
          //Authorization: `Bearer ${this.authenticationService.token}`
        })
      }
    ));
  }

  public post(url: string, model: any) {
    return this.criticalErrors.wrap(this.http.post(
      `${environment.service.url}/${url}`,
      model,
      {
        headers: new HttpHeaders({
          //Authorization: `Bearer ${this.authenticationService.token}`
        })
      }
    ));
  }

  public delete(url: string) {
    return this.criticalErrors.wrap(this.http.delete(
      `${environment.service.url}/${url}`,
      {
        headers: new HttpHeaders({
          //Authorization: `Bearer ${this.authenticationService.token}`
        })
      }
    ));
  }
}
