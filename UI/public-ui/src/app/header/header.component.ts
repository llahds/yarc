import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
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

  public queryText: string = "";

  constructor(
    private authentication: AuthenticationService,
    private router: Router
  ) {
    this.authentication.onNewSignIn.subscribe(u => {
      this.userName = u.userName;
      this.isAuthenticated = true;
    });

    this.authentication.onChallenge.subscribe(r => {
      this.userName = "";
      this.isAuthenticated = false;
      this.showSignIn = true;
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

  search() {
    this.router.navigateByUrl(`/search?q=${this.queryText}&s=0`);
  }
}
