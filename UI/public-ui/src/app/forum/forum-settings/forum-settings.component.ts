import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-forum-settings',
  templateUrl: './forum-settings.component.html',
  styleUrls: ['./forum-settings.component.scss']
})
export class ForumSettingsComponent implements OnInit {

  public forumId!: number;

  constructor(
    private route: ActivatedRoute
  ) { }

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      this.forumId = +this.route.snapshot.params["forumId"];
    });    
  }

}
