import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-forum',
  templateUrl: './forum.component.html',
  styleUrls: ['./forum.component.scss']
})
export class ForumComponent implements OnInit {

  constructor(
    private route: ActivatedRoute
  ) { }

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      if (+this.route.snapshot.params['id'] === 0) {
        console.log(this.route.snapshot.params['id']);
        // this.headerLabel = "Add Article";
        // this.entity = { title: "", text: "", keywords: [], topics: [], isNew: true };
        // this.isLoading = false;
      }
      else {
        // this.headerLabel = "Edit Article";
        // this.entity = { title: "", text: "", keywords: [], categories: [], isNew: true };
        // this.api.getArticle(this.route.snapshot.params.id)
        //   .subscribe(
        //     data => this.handleSuccess(data),
        //     err => this.handleErrors(err)
        //   );
      }
    });
  }
}
