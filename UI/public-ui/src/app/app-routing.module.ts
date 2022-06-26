import { NgModule } from '@angular/core';
import { ExtraOptions, RouterModule, Routes } from '@angular/router';
import { ForumCreateComponent } from './forum/forum-create/forum-create.component';
import { ForumEditComponent } from './forum/forum-edit/forum-edit.component';
import { ForumListComponent } from './forum/forum-list/forum-list.component';
import { ForumPostComponent } from './forum/forum-post/forum-post.component';
import { ForumSettingsApprovedComponent } from './forum/forum-settings/forum-settings-approved/forum-settings-approved.component';
import { ForumSettingsAutomaticComponent } from './forum/forum-settings/forum-settings-automatic/forum-settings-automatic.component';
import { ForumSettingsBannedComponent } from './forum/forum-settings/forum-settings-banned/forum-settings-banned.component';
import { ForumSettingsMutedComponent } from './forum/forum-settings/forum-settings-muted/forum-settings-muted.component';
import { ForumSettingsPostsComponent } from './forum/forum-settings/forum-settings-posts/forum-settings-posts.component';
import { ForumSettingsReportsComponent } from './forum/forum-settings/forum-settings-reports/forum-settings-reports.component';
import { ForumSettingsSpamComponent } from './forum/forum-settings/forum-settings-spam/forum-settings-spam.component';
import { ForumSettingsComponent } from './forum/forum-settings/forum-settings.component';
import { ForumComponent } from './forum/forum.component';
import { PopularComponent } from './popular/popular.component';
import { PrivateForumComponent } from './private-forum/private-forum.component';
import { SearchTypeComponent } from './search/search-type/search-type.component';
import { SearchComponent } from './search/search.component';
import { ForumGuardService } from './services/forum-guard.service';
import { SlugRedirectComponent } from './slug-redirect/slug-redirect.component';
import { UserSettingsComponent } from './user-settings/user-settings.component';

export const routingConfiguration: ExtraOptions = {
  paramsInheritanceStrategy: 'always'
};

const routes: Routes = [
  {
    path: "",
    redirectTo: "popular",
    pathMatch: "full"
  },
  {
    path: "popular",
    component: PopularComponent,
  },
  {
    path: "r/create",
    component: ForumCreateComponent
  },
  {
    path: "private",
    component: PrivateForumComponent
  },
  {
    path: "s/:slug",
    component: SlugRedirectComponent
  },
  {
    path: "search",
    component: SearchComponent
  },  
  {
    path: "search/:type",
    component: SearchTypeComponent
  },    
  {
    path: "r/:forumId",
    component: ForumComponent,
    canActivate: [ForumGuardService],
    children: [
      {
        path: "",
        component: ForumListComponent
      },
      {
        path: "p/:postId",
        component: ForumPostComponent
      },
      {
        path: "edit",
        component: ForumEditComponent
      },
      {
        path: "settings",
        component: ForumSettingsComponent,
        children: [
          { path: "", redirectTo: "reports", pathMatch: "full" },
          { path: "spam", component: ForumSettingsSpamComponent },
          { path: "reports", component: ForumSettingsReportsComponent },
          { path: "banned", component: ForumSettingsBannedComponent },
          { path: "muted", component: ForumSettingsMutedComponent },
          { path: "approved", component: ForumSettingsApprovedComponent },
          { path: "posts", component: ForumSettingsPostsComponent },
          { path: "automatic", component: ForumSettingsAutomaticComponent },
        ]
      }
    ]
  },
  {
    path: "user-settings",
    component: UserSettingsComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes, routingConfiguration)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
