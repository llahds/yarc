import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ForumEditComponent } from './forum/forum-edit/forum-edit.component';
import { ForumListComponent } from './forum/forum-list/forum-list.component';
import { ForumPostComponent } from './forum/forum-post/forum-post.component';
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
      }
    ]
  }  
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
