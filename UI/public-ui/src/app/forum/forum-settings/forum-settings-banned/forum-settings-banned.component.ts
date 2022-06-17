import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-forum-settings-banned',
  templateUrl: './forum-settings-banned.component.html',
  styleUrls: ['./forum-settings-banned.component.scss']
})
export class ForumSettingsBannedComponent implements OnInit {

  public showBanModal: boolean = false;

  constructor() { }

  ngOnInit(): void {
  }

}
