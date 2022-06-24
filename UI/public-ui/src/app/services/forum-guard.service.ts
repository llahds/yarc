import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { map, of } from 'rxjs';
import { ForumsService } from './forums.service';

@Injectable({
  providedIn: 'root'
})
export class ForumGuardService {

  constructor(
    private forums: ForumsService,
    private router: Router
  ) { }

  canActivate(next: ActivatedRouteSnapshot, state: RouterStateSnapshot)  {

    const forumId = +next.params["forumId"];

    return this.forums.checkForumAccess(forumId).pipe(
      map(r => {
        if (r.canAccessForum) {
          return true;
        }

        this.router.navigateByUrl("/private");

        return false;
      })
    );
  }  
}