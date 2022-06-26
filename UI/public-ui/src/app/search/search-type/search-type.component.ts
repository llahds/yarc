import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { SearchGroup } from 'src/app/services/models/search';
import { SearchService } from 'src/app/services/search.service';

@Component({
  selector: 'app-search-type',
  templateUrl: './search-type.component.html',
  styleUrls: ['./search-type.component.scss']
})
export class SearchTypeComponent implements OnInit {

  public query: string = "";
  public group!: SearchGroup;
  public startAt: number = 0;

  public type: string = "";

  constructor(
    private search: SearchService,
    private route: ActivatedRoute,
    private router: Router
  ) {

  }

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      this.query = params["q"];
      this.startAt = params["startAt"];
      if (this.type) {
        this.search.search(this.type, this.query, this.startAt).subscribe(r => this.group = r);
      }
    });

    this.route.paramMap.subscribe(r => {
      if (r.has("type")) {
        this.type = r.get("type")!;
      }
      this.search.search(this.type, this.query, this.startAt).subscribe(r => this.group = r);
    });
  }
}
