import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot, UrlTree } from '@angular/router';
import {Observable} from 'rxjs';
import {Store} from "@ngrx/store";
import * as selectors from "../state/selectors/docs.crud.selectors";
import {DocThumbnail} from "../models/docs/doc.thumbnail";

@Injectable({
  providedIn: 'root'
})
export class FetchGuard implements CanActivate {
  docs: DocThumbnail[] = [];
  constructor(private store: Store) {
    this.store.select(selectors.selectDocThumbnails)
      .pipe()
      .subscribe(thumbnails => this.docs = thumbnails);
  }
  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    let id = route.paramMap.get('id');
    if (id === null) return false;
    return !!this.docs.find(doc => doc.id === id);
  }

}
