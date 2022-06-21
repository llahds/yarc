import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { debounceTime, Subject } from 'rxjs';
import { ForumUserManagementService } from 'src/app/services/forum-user-management.service';
import { UserInfo } from 'src/app/services/models/users';

@Component({
  selector: 'app-mod-search-users',
  templateUrl: './mod-search-users.component.html',
  styleUrls: ['./mod-search-users.component.scss']
})
export class ModSearchUsersComponent implements OnInit {
  @Input() show: boolean = false;
  @Output() onCancel = new EventEmitter<void>();
  @Output() onConfirm = new EventEmitter<UserInfo>();

  users: any[] = [];

  private _queryChanged = new Subject<string>();
  public isSearching: boolean = false;
  public queryText: string = "";

  constructor(
    private api: ForumUserManagementService
  ) {
    this._queryChanged
      .pipe(
        debounceTime(300)
      )
      .subscribe(m => this.search(m));
  }

  ngOnInit(): void {
  }

  cancel() {
    this.users = [];
    this.queryText = "";
    this.onCancel.emit();
  }

  clear() {
    this.users = [];
    this.search("");
  }

  search(m: string) {
    this.users = [];
    this.queryText = m;
    this.isSearching = true;
    if (m) {
      this.api.searchUsers(m)
        .subscribe(r => {
          this.isSearching = false;
          this.users = r;
        });
    } else {
      this.isSearching = false;
    }
  }

  queryChanged(text: string) {
    this._queryChanged.next(text);
  }

  select(item: UserInfo) {
    this.clear();
    this.onConfirm.emit(item);
  }
}
