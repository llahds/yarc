import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { MarkdownModule } from 'ngx-markdown';
import { LMarkdownEditorModule } from 'ngx-markdown-editor';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HeaderComponent } from './header/header.component';
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
import { ConfirmModalComponent } from './confirm-modal/confirm-modal.component';
import { PostEditComponent } from './post-edit/post-edit.component';
import { ForumSettingsComponent } from './forum/forum-settings/forum-settings.component';
import { ForumSettingsSpamComponent } from './forum/forum-settings/forum-settings-spam/forum-settings-spam.component';
import { ForumSettingsReportsComponent } from './forum/forum-settings/forum-settings-reports/forum-settings-reports.component';
import { ForumSettingsBannedComponent } from './forum/forum-settings/forum-settings-banned/forum-settings-banned.component';
import { ForumSettingsMutedComponent } from './forum/forum-settings/forum-settings-muted/forum-settings-muted.component';
import { ForumSettingsApprovedComponent } from './forum/forum-settings/forum-settings-approved/forum-settings-approved.component';
import { ForumSettingsPostsComponent } from './forum/forum-settings/forum-settings-posts/forum-settings-posts.component';
import { ForumSettingsAutomaticComponent } from './forum/forum-settings/forum-settings-automatic/forum-settings-automatic.component';
import { AddRemoveStringsComponent } from './add-remove-strings/add-remove-strings.component';
import { ModPostItemComponent } from './forum/forum-settings/mod-post-item/mod-post-item.component';
import { ModUserComponent } from './forum/forum-settings/mod-user/mod-user.component';
import { ModSearchUsersComponent } from './forum/forum-settings/mod-search-users/mod-search-users.component';
import { ForumCreateComponent } from './forum/forum-create/forum-create.component';
import { SignInComponent } from './sign-in/sign-in.component';
import { SignUpComponent } from './sign-up/sign-up.component';
import { UserSettingsComponent } from './user-settings/user-settings.component';
import { HttpClientModule } from '@angular/common/http';
import { TimeAgoPipe } from './services/time-ago.pipe';
import { ControlValidationErrorsComponent } from './control-validation-errors/control-validation-errors.component';
import { ChangeUserNameComponent } from './user-settings/change-user-name/change-user-name.component';
import { ChangeEmailComponent } from './user-settings/change-email/change-email.component';
import { ChangePasswordComponent } from './user-settings/change-password/change-password.component';
import { VoteComponent } from './vote/vote.component';
import { SimilarForumsComponent } from './forum/similar-forums/similar-forums.component';
import { PrivateForumComponent } from './private-forum/private-forum.component';
import { SlugRedirectComponent } from './slug-redirect/slug-redirect.component';
import { SearchComponent } from './search/search.component';
import { SearchTypeComponent } from './search/search-type/search-type.component';
import { PopularForumsComponent } from './popular-forums/popular-forums.component';
import { FeedComponent } from './feed/feed.component';

@NgModule({
  declarations: [
    AppComponent,
    HeaderComponent,
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
    MultiselectorComponent,
    ConfirmModalComponent,
    PostEditComponent,
    ForumSettingsComponent,
    ForumSettingsSpamComponent,
    ForumSettingsReportsComponent,
    ForumSettingsBannedComponent,
    ForumSettingsMutedComponent,
    ForumSettingsApprovedComponent,
    ForumSettingsPostsComponent,
    ForumSettingsAutomaticComponent,
    AddRemoveStringsComponent,
    ModPostItemComponent,
    ModUserComponent,
    ModSearchUsersComponent,
    ForumCreateComponent,
    SignInComponent,
    SignUpComponent,
    UserSettingsComponent,
    TimeAgoPipe,
    ControlValidationErrorsComponent,
    ChangeUserNameComponent,
    ChangeEmailComponent,
    ChangePasswordComponent,
    VoteComponent,
    SimilarForumsComponent,
    PrivateForumComponent,
    SlugRedirectComponent,
    SearchComponent,
    SearchTypeComponent,
    PopularForumsComponent,
    FeedComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    MarkdownModule.forRoot(),
    FormsModule,
    LMarkdownEditorModule,
    HttpClientModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
