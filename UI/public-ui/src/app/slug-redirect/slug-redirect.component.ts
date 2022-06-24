import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ForumsService } from '../services/forums.service';

@Component({
  selector: 'app-slug-redirect',
  templateUrl: './slug-redirect.component.html',
  styleUrls: ['./slug-redirect.component.scss']
})
export class SlugRedirectComponent implements OnInit {

  constructor(
    private router: Router,
    private forums: ForumsService,
    private route: ActivatedRoute
  ) { }

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      const slug = this.route.snapshot.params["slug"];
      this.forums.getIdBySlug(slug).subscribe(r => this.router.navigateByUrl("/r/" + r.id));
    });
  }

}
