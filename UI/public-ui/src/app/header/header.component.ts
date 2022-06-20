import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from '../services/authentication.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {

  public showSignIn: boolean = false;
  public showSignUp: boolean = false;

  public userName: string = "";
  public isAuthenticated: boolean = false;

  constructor(
    private authentication: AuthenticationService
  ) {
    this.authentication.onNewSignIn.subscribe(u => {
      this.userName = u.userName;
      this.isAuthenticated = true;
    });

    this.authentication.onChallenge.subscribe(r => {
      this.userName = "";
      this.isAuthenticated = false;
    });
  }

  ngOnInit(): void {
    this.userName = this.authentication.userName;
    this.isAuthenticated = !!this.userName;
  }

  signOut() {
    this.authentication.clearIdentity();
    this.isAuthenticated = false;
    this.userName = "";
  }
}
