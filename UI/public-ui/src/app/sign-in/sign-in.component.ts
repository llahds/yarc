import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AuthenticationService } from '../services/authentication.service';

@Component({
  selector: 'app-sign-in',
  templateUrl: './sign-in.component.html',
  styleUrls: ['./sign-in.component.scss']
})
export class SignInComponent implements OnInit {

  @Input() showSignIn: boolean = false;
  @Output() onCancel = new EventEmitter<void>();
  @Output() onSignIn = new EventEmitter<void>();

  public userName: string = "";
  public password: string = "";
  public errors: any = {};
  public isSaving: boolean = false;

  constructor(
    private api: AuthenticationService
  ) { }

  ngOnInit(): void {
  }

  cancel() {
    this.errors = {};
    this.isSaving = false;
    this.userName = "";
    this.password = "";
    this.onCancel.emit();
  }

  signIn() {
    this.errors = {};
    this.isSaving = true;
    this.api.authenticate(this.userName, this.password).subscribe(r => {
      this.isSaving = false;
      this.userName = "";
      this.password = "";
      this.api.setIdentity(r);
      this.onSignIn.emit();
    }, e => {
      this.api.clearIdentity();
      this.errors = e.error;
      this.isSaving = false;
    });
  }

  get hasInvalidData() {
    return !this.userName
      || !this.password;
  }
}
