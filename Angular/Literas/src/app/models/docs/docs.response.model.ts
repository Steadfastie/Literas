import {Guid} from "guid-typescript";

export interface DocResponseModel {
    id: Guid,
    creatorId:string,
    title: string,
    content: string
}
