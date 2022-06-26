import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { SearchGroup } from './models/search';
import { RestApiService } from './rest-api.service';

@Injectable({
  providedIn: 'root'
})
export class SearchService {

  constructor(
    private client: RestApiService
  ) { }

  searchOverview(q: string) : Observable<SearchGroup[]> {
    return this.client.get(`api/1.0/search/overview?query=${q}`);
  }

  search(type: string, q: string, startAt: number) : Observable<SearchGroup> {
    return this.client.get(`api/1.0/search/${type}?query=${q}&startAt=${startAt}`);
  }
}
