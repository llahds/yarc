import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable, Subject } from 'rxjs';
import { environment } from 'src/environments/environment';
import { CriticalErrorService } from './critical-error.service';
import { AuthenticationToken } from './models/users';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {

  onNewSignIn = new Subject<AuthenticationToken>();
  onChallenge = new Subject<void>();
  onSignOut = new Subject<void>();

  constructor(
    private http: HttpClient,
    private criticalErrors: CriticalErrorService,
    private router: Router
  ) {

  }

  authenticate(userName: string, password: string): Observable<AuthenticationToken> {
    return this.criticalErrors.wrap(this.http.post(
      `${environment.service.url}/api/1.0/authenticate`,
      { userName: userName, password: password }
    ));
  }

  challengeCredentials() {
    this.onChallenge.next();
  }

  setIdentity(userInfo: AuthenticationToken) {
    sessionStorage.setItem('token', userInfo.token);
    sessionStorage.setItem('userName', userInfo.userName);
    this.onNewSignIn.next(userInfo);
  }

  clearIdentity() {
    sessionStorage.clear();
    this.onSignOut.next();
  }

  get token() {
    return sessionStorage.getItem('token') || '';
  }

  get userName(): any {
    return sessionStorage.getItem('userName') || '';
  }
}
