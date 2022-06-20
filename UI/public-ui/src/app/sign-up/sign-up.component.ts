import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AuthenticationService } from '../services/authentication.service';
import { Register } from '../services/models/users';
import { RegistrationService } from '../services/registration.service';

@Component({
  selector: 'app-sign-up',
  templateUrl: './sign-up.component.html',
  styleUrls: ['./sign-up.component.scss']
})
export class SignUpComponent implements OnInit {

  @Input() showSignUp: boolean = false;
  @Output() onCancel = new EventEmitter<void>();
  @Output() onSignUp = new EventEmitter<void>();

  public entity!: Register;
  public errors: any = {};
  public isSaving: boolean = false;

  constructor(
    private api: RegistrationService,
    private authentication: AuthenticationService
  ) { }

  ngOnInit(): void {
    this.entity = { email: "", userName: "", password: "", confirmPassword: "" };
  }

  cancel() {
    this.errors = {};
    this.isSaving = false;
    this.entity = { email: "", userName: "", password: "", confirmPassword: "" };
    this.onCancel.emit();
  }

  signUp() {
    this.errors = {};
    this.isSaving = true;
    this.api.register(this.entity).subscribe(r => {
      this.isSaving = false;
      this.entity = { email: "", userName: "", password: "", confirmPassword: "" };
      this.authentication.setIdentity(r);
      this.onSignUp.emit();
    }, e => {
      this.authentication.clearIdentity();
      this.errors = e.error;
      this.isSaving = false;
    });
  }

  get hasInvalidData() {
    return !this.entity.email
      || !this.entity.userName
      || !this.entity.password
      || !this.entity.confirmPassword;
  }
}
