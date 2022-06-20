import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ChangePassword } from 'src/app/services/models/users';
import { UserSettingsService } from 'src/app/services/user-settings.service';

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.scss']
})
export class ChangePasswordComponent implements OnInit {

  @Input() show: boolean = false;
  @Output() onCancel = new EventEmitter<void>();
  @Output() onUpdate = new EventEmitter<void>();

  public entity: ChangePassword = { oldPassword: "", password: "", confirmPassword: "" }
  public errors: any = {};
  public isSaving: boolean = false;

  constructor(
    private api: UserSettingsService
  ) { }

  ngOnInit(): void {
  }

  cancel() {
    this.isSaving = false;
    this.errors = {};
    this.entity = { oldPassword: "", password: "", confirmPassword: "" }
    this.onCancel.emit();
  }

  update() {
    this.errors = {};
    this.isSaving = true;
    this.api.changePassword(this.entity)
      .subscribe(r => {
        this.isSaving = false;
        this.entity = { oldPassword: "", password: "", confirmPassword: "" }
        this.onUpdate.emit();
      }, err => {
        this.isSaving = false;
        this.errors = err.error;
      })
  }

  get hasInvalidData() {
    return !this.entity.oldPassword
      || !this.entity.password
      || !this.entity.confirmPassword;
  }
}
