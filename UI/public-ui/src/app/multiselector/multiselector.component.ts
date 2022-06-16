import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-multiselector',
  templateUrl: './multiselector.component.html',
  styleUrls: ['./multiselector.component.scss']
})
export class MultiselectorComponent implements OnInit {

  @Input() placeholder: string = "";
  @Input() selectedItems: any[] = [];
  @Input() emptyMessage: string = "";

  @Output() onQueryChanged = new EventEmitter<any>();
  @Output() onSelectedItemsChanged = new EventEmitter<any[]>();

  public query: string = "";
  @Input() public typeaheadItems: any[] = [];

  constructor() { }

  ngOnInit(): void {
  }

  search(query: string) {
    this.query = query;
    this.onQueryChanged.emit(query);
  }

  select(item: any) {
    this.query = "";
    this.selectedItems.push(item);
    this.onSelectedItemsChanged.emit(this.selectedItems);
  }

  remove(index: number){
    this.selectedItems.splice(index, 1);
    this.onSelectedItemsChanged.emit(this.selectedItems);
  }
}
