import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { debounceTime, Subject } from 'rxjs';

@Component({
  selector: 'app-mod-search-users',
  templateUrl: './mod-search-users.component.html',
  styleUrls: ['./mod-search-users.component.scss']
})
export class ModSearchUsersComponent implements OnInit {
  @Input() show: boolean = false;
  @Output() onCancel = new EventEmitter<void>();
  @Output() onConfirm = new EventEmitter<void>();

  users: any[] = [];

  private _queryChanged = new Subject<string>();
  public isSearching: boolean = false;
  public queryText: string = "";

  constructor() {
    this._queryChanged
      .pipe(
        debounceTime(300)
      )
      .subscribe(m => this.search(m));
  }

  ngOnInit(): void {
  }

  cancel() {
    this.onCancel.emit();
  }

  clear() {
    this.users = [];
    this.search("");
  }

  search(m: string) {
    this.isSearching = true;
    this.users = [{ name: "foo" }, { name: "foo2" }];
    this.isSearching = false;
  }

  queryChanged(text: string) {
    this._queryChanged.next(text);
  }

  select(item: any) {
    this.clear();
    this.onConfirm.emit();
  }
}
