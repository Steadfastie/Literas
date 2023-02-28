import { Injectable } from '@angular/core';
import {ApiService} from "../api.service";
import {IDocResponseModel} from "../../models/docs/docs.response.model";
import {Observable} from "rxjs";
import {IUserResponseModel} from "../../models/users/user.response.model";
import {IDocRequestModel} from "../../models/docs/docs.request.model";
import {Guid} from "guid-typescript";

@Injectable({
  providedIn: 'root'
})
export class DocService {

  constructor(private apiService: ApiService) { }

  getAllDocs(): Observable<IDocResponseModel[]>{
    return this.apiService.get('docs').pipe();
  }

  getDocById(docId: string): Observable<IDocResponseModel>{
    return this.apiService.get(`docs/${docId}`).pipe();
  }

  getEditorsByDocId(docId: string): Observable<IUserResponseModel[]>{
    return this.apiService.get(`docs/${docId}/editors`).pipe();
  }

  create(doc: IDocRequestModel): Observable<IDocResponseModel>{
    return this.apiService.post(`docs`, doc).pipe();
  }

  patch(doc: IDocRequestModel): Observable<IDocResponseModel>{
    return this.apiService.patch(`docs/${doc.id}`, doc).pipe();
  }

  delete(docId: Guid): Observable<IDocResponseModel>{
    return this.apiService.delete(`doc/${docId}`).pipe();
  }
}
