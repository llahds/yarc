import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ChangeEmail, ChangeUserName } from 'src/app/services/models/users';
import { UserSettingsService } from 'src/app/services/user-settings.service';

@Component({
  selector: 'app-change-email',
  templateUrl: './change-email.component.html',
  styleUrls: ['./change-email.component.scss']
})
export class ChangeEmailComponent implements OnInit {


  @Input() show: boolean = false;
  @Output() onCancel = new EventEmitter<void>();
  @Output() onUpdate = new EventEmitter<void>();

  public entity: ChangeEmail = { email: "", password: "" }
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
    this.entity = { email: "", password: "" };
    this.onCancel.emit();
  }

  update() {
    this.errors = {};
    this.isSaving = true;
    this.api.changeEmail(this.entity)
      .subscribe(r => {
        this.isSaving = false;
        this.entity = { email: "", password: "" };
        this.onUpdate.emit();
      }, err => {
        this.isSaving = false;
        this.errors = err.error;
      })
  }

  get hasInvalidData() {
    return !this.entity.email
      || !this.entity.password;
  }

}
