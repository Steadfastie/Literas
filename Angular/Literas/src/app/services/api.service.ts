import { Injectable } from '@angular/core';
import {HttpClient, HttpRequest} from "@angular/common/http";
import {apiUrl} from "../../environment/dev";
import * as queryString from "querystring";
import {Observable} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  constructor(private http: HttpClient) { }

  private getFullUrl(url: string) {
    return apiUrl.url + url;
  }

  get(url: string, data: { [key: string]: any } = {}): Observable<any> {
    if (Object.keys(data).length > 0) {
      url += '?' + queryString.stringify(data);
    }
    return this.http.get(this.getFullUrl(url));
  }

  post(url: string, data: object): Observable<any> {
    return this.http.post(this.getFullUrl(url), data);
  }

  patch(url: string, data: object): Observable<any> {
    return this.http.patch(this.getFullUrl(url), data);
  }

  delete(url: string): Observable<any> {
    return this.http.delete(this.getFullUrl(url));
  }

  put(url: string, data: object): Observable<any> {
    return this.http.put(this.getFullUrl(url), data);
  }

  request(method: string, url: string, data: object, options: object): Observable<any> {
    const req = new HttpRequest(method, this.getFullUrl(url), data, options);
    return this.http.request(req);
  }
}
