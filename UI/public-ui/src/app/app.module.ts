import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { MarkdownModule } from 'ngx-markdown';
import { LMarkdownEditorModule } from 'ngx-markdown-editor';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HeaderComponent } from './header/header.component';
import { PopularComponent } from './popular/popular.component';
import { PostItemComponent } from './post-item/post-item.component';
import { ForumComponent } from './forum/forum.component';
import { PostListComponent } from './post-list/post-list.component';
import { ForumListComponent } from './forum/forum-list/forum-list.component';
import { ForumPostComponent } from './forum/forum-post/forum-post.component';
import { FormsModule } from '@angular/forms';
import { ForumPostCommentsComponent } from './forum/forum-post/forum-post-comments/forum-post-comments.component';
import { ForumPostCommentComponent } from './forum/forum-post/forum-post-comments/forum-post-comment/forum-post-comment.component';
import { ModalComponent } from './modal/modal.component';
import { ForumEditComponent } from './forum/forum-edit/forum-edit.component';
import { TypeaheadComponent } from './typeahead/typeahead.component';
import { MultiselectorComponent } from './multiselector/multiselector.component';

@NgModule({
  declarations: [
    AppComponent,
    HeaderComponent,
    PopularComponent,
    PostItemComponent,
    ForumComponent,
    PostListComponent,
    ForumListComponent,
    ForumPostComponent,
    ForumPostCommentsComponent,
    ForumPostCommentComponent,
    ModalComponent,
    ForumEditComponent,
    TypeaheadComponent,
    MultiselectorComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    MarkdownModule.forRoot(),
    FormsModule,
    LMarkdownEditorModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
