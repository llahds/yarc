import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ChangeUserName } from 'src/app/services/models/users';
import { UserSettingsService } from 'src/app/services/user-settings.service';

@Component({
  selector: 'app-change-user-name',
  templateUrl: './change-user-name.component.html',
  styleUrls: ['./change-user-name.component.scss']
})
export class ChangeUserNameComponent implements OnInit {

  @Input() show: boolean = false;
  @Output() onCancel = new EventEmitter<void>();
  @Output() onUpdate = new EventEmitter<void>();

  public entity: ChangeUserName = { userName: "", password: "" }
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
    this.entity = { userName: "", password: "" };
    this.onCancel.emit();
  }

  update() {
    this.errors = {};
    this.isSaving = true;
    this.api.changeUserName(this.entity)
      .subscribe(r => {
        this.isSaving = false;
        this.entity = { userName: "", password: "" };
        this.onUpdate.emit();
      }, err => {
        this.isSaving = false;
        this.errors = err.error;
      })
  }

  get hasInvalidData() {
    return !this.entity.userName
      || !this.entity.password;
  }
}
