import {Guid} from "guid-typescript";

export interface IDocResponseModel {
    id: Guid,
    creatorId:string,
    title: string,
    content: string
}
