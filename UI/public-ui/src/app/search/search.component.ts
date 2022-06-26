import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { SearchGroup } from '../services/models/search';
import { SearchService } from '../services/search.service';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.scss']
})
export class SearchComponent implements OnInit {

  public query: string = "";
  public entity: SearchGroup[] = [];

  constructor(
    private search: SearchService,
    private route: ActivatedRoute,
    private router: Router
  ) {
    
  }

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      this.query = params["q"];
      this.search.searchOverview(this.query).subscribe(r => this.entity = r);
    });
  }
}
