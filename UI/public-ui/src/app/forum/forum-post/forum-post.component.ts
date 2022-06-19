import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { PostsService } from 'src/app/services/posts.service';
import { Post } from 'src/app/services/models/posts';

@Component({
  selector: 'app-forum-post',
  templateUrl: './forum-post.component.html',
  styleUrls: ['./forum-post.component.scss']
})
export class ForumPostComponent implements OnInit {

  public showRemoveModal: boolean = false;
  public showSpamModal: boolean = false;
  public showReportModal: boolean = false;
  public showPostModal: boolean = false;

  public forumId!: number;
  public item!: Post;

  constructor(
    private route: ActivatedRoute,
    private api: PostsService
  ) { }

  ngOnInit(): void {
    this.route.parent?.params.subscribe(params => {
      this.forumId = +params["id"];
    });

    this.route.paramMap.subscribe(params => {
      const id = +this.route.snapshot.params["postId"];
      this.api.getView(this.forumId, id).subscribe(r => this.item = r);
    });
  }

  confirmRemove() {
    this.showRemoveModal = true;
  }

  confirmSpam() {
    this.showSpamModal = true;
  }

  confirmReport() {
    this.showReportModal = true;
  }

}
