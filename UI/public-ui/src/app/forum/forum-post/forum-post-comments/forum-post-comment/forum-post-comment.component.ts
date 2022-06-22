import { Component, Input, OnInit } from '@angular/core';
import { Comment } from 'src/app/services/models/posts';
import { PostsService } from 'src/app/services/posts.service';

@Component({
  selector: 'app-forum-post-comment',
  templateUrl: './forum-post-comment.component.html',
  styleUrls: ['./forum-post-comment.component.scss']
})
export class ForumPostCommentComponent implements OnInit {

  public showReply: boolean = false;
  public showChildren: boolean = false;
  @Input() depth: number = 0;
  @Input() item!: Comment;
  @Input() forumId!: number;
  @Input() postId!: number;

  public showRemoveModal: boolean = false;
  public showSpamModal: boolean = false;
  public showReportModal: boolean = false;
  public replyText: string = "";
  public isReplying: boolean = false;

  public showEdit: boolean = false;
  public editText: string = "";
  public isEditing: boolean = false;

  public replies: Comment[] = [];
  public isRemoving: boolean = false;

  constructor(
    private api: PostsService
  ) { }

  ngOnInit(): void {
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

  reply() {
    this.isReplying = true;
    this.api.createComment(this.forumId, this.postId, this.replyText, this.item.id)
      .subscribe(r => {
        this.replyText = "";
        this.isReplying = false;
        this.showReply = false;
        this.replies.unshift(r);
        this.item.replyCount++;
      });
  }

  getReplies() {
    this.api.getComments(this.forumId, this.postId, this.item.id)
      .subscribe(r => {
        this.replies = r;
        this.showChildren = true;
      });
  }

  edit() {
    this.showEdit = true;
    this.editText = this.item.text;
  }

  update() {
    this.isEditing = true;
    this.api.updateComment(this.forumId, this.postId, this.item.id, this.editText)
      .subscribe(r => {
        this.isEditing = false;
        this.item.text = this.editText;
        this.editText = "";
        this.showEdit = false;
      });
  }

  up() {
    this.api.upvoteComment(this.forumId, this.postId, this.item.id).subscribe();
  }

  down() {
    this.api.downvoteComment(this.forumId, this.postId, this.item.id).subscribe();
  }

  remove() {
    this.isRemoving = true;
    this.api.removeComment(this.forumId, this.postId, this.item.id).subscribe(r => {
      this.isRemoving = false;
      this.showRemoveModal = false;
      this.item.isDeleted = true;
    });
  }
}
