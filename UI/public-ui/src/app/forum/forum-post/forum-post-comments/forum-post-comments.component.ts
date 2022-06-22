import { Component, Input, OnInit } from '@angular/core';
import { Comment } from 'src/app/services/models/posts';
import { PostsService } from 'src/app/services/posts.service';

@Component({
  selector: 'app-forum-post-comments',
  templateUrl: './forum-post-comments.component.html',
  styleUrls: ['./forum-post-comments.component.scss']
})
export class ForumPostCommentsComponent implements OnInit {

  @Input() public depth: number = 0;
  @Input() forumId!: number;
  @Input() postId!: number;
  @Input() list: Comment[] = [];

  constructor(
    private posts: PostsService
  ) { }

  ngOnInit(): void {
    //this.refresh();
  }

  refresh() {
    // this.posts.getComments(this.forumId, this.postId, undefined)
    //   .subscribe(r => {
    //     this.list = r;
    //   })
  }
}
