import { Component, Input, OnInit } from '@angular/core';
import { ForumsService } from 'src/app/services/forums.service';
import { KeyValueModel } from 'src/app/services/models/common';
import { SimilarForum } from 'src/app/services/models/forums';

@Component({
  selector: 'app-similar-forums',
  templateUrl: './similar-forums.component.html',
  styleUrls: ['./similar-forums.component.scss']
})
export class SimilarForumsComponent implements OnInit {

  public list: SimilarForum[] = [];

  constructor(
    private forums: ForumsService
  ) { }

  ngOnInit(
  ): void {
  }

  @Input() set forumId(id: number) {
    if (id) {
      this.forums.similarForums(id)
        .subscribe(r => {
          this.list = r;
        });
    }
  }

}
