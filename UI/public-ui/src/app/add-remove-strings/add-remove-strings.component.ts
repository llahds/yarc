import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-add-remove-strings',
  templateUrl: './add-remove-strings.component.html',
  styleUrls: ['./add-remove-strings.component.scss']
})
export class AddRemoveStringsComponent implements OnInit {

  @Input() selectedItems: string[] = [];
  @Input() emptyMessage: string = "";
  public query: string = "";

  constructor() { }

  ngOnInit(): void {
  }

  select() {
    this.selectedItems.push(this.query);
    this.query = "";    
  }

  remove(index: number){
    this.selectedItems.splice(index, 1);
  }
}
