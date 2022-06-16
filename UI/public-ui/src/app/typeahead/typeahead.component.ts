import { Component, OnInit, Input, Output, EventEmitter, TemplateRef, ContentChild } from '@angular/core';
import { Subject } from 'rxjs';
import { debounceTime } from 'rxjs/operators';

@Component({
  selector: 'app-typeahead',
  templateUrl: './typeahead.component.html',
  styleUrls: ['./typeahead.component.scss']
})
export class TypeaheadComponent implements OnInit {

  @Input() placeholder: string = "";
  @Input() disabled: boolean = false;
  @Output() public onSelected = new EventEmitter<any>();

  @Input() public queryText: string = '';
  @Output() public onQueryChanged = new EventEmitter<string>();

  @ContentChild("item", { static: false }) itemTemplateRef: any;

  private _queryChanged = new Subject<string>();
  public _items: any[] = [];
  public isSearching: boolean = false;

  constructor() {
    this._queryChanged
      .pipe(
        debounceTime(300)
      )
      .subscribe(m => this.search(m));
  }

  ngOnInit(): void {
  }

  clear() {
    this._items = [];
    this.search("");
  }

  search(m: string) {
    this.isSearching = true;
    this.onQueryChanged.emit(m);
  }

  queryChanged(text: string) {
    this._queryChanged.next(text);
  }

  select(item: any) {
    this.clear();
    this.onSelected.emit(item);
  }

  @Input() set typeaheadItems(items: any[]) {
    this.isSearching = false;
    this._items = items;
  }
}