import { Injectable } from '@angular/core';
import {ApiService} from "../api.service";
import {DocResponseModel} from "../../models/docs/docs.response.model";
import {Observable} from "rxjs";
import {IUserResponseModel} from "../../models/users/user.response.model";
import {DocRequestModel} from "../../models/docs/docs.request.model";
import {Guid} from "guid-typescript";
import {DocThumbnail} from "../../models/docs/doc.thumbnail";

@Injectable({
  providedIn: 'root'
})
export class DocService {

  constructor(private apiService: ApiService) { }

  getAllDocs(): Observable<DocResponseModel[]>{
    return this.apiService.get('docs').pipe();
  }

  getDocById(docId: string): Observable<DocResponseModel>{
    return this.apiService.get(`docs/${docId}`).pipe();
  }

  getDocThumbnails(): Observable<DocThumbnail[]>{
    return this.apiService.get(`docs/thumbnails`).pipe();
  }

  getEditorsByDocId(docId: string): Observable<IUserResponseModel[]>{
    return this.apiService.get(`docs/${docId}/editors`).pipe();
  }

  create(doc: DocRequestModel): Observable<DocResponseModel>{
    return this.apiService.post(`docs`, doc).pipe();
  }

  patch(doc: DocRequestModel): Observable<DocResponseModel>{
    return this.apiService.patch(`docs/${doc.id}`, doc).pipe();
  }

  delete(docId: Guid | string): Observable<DocResponseModel>{
    if (docId instanceof Guid) {
      docId = docId.toString();
    }
    return this.apiService.delete(`docs/${docId}`).pipe();
  }
}
