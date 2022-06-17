import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
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
    path: "r/:id",
    component: ForumComponent,
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
          { path: "", redirectTo: "spam", pathMatch: "full" },
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
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
