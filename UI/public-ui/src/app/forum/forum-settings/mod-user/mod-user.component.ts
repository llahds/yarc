import { Component, Input, OnInit } from '@angular/core';
import { UserInfo } from 'src/app/services/models/users';

@Component({
  selector: 'app-mod-user',
  templateUrl: './mod-user.component.html',
  styleUrls: ['./mod-user.component.scss']
})
export class ModUserComponent implements OnInit {

  @Input() item!: UserInfo;

  constructor() { }

  ngOnInit(): void {
  }

}
