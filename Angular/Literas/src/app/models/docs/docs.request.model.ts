import {Guid} from "guid-typescript";

export interface IDocRequestModel {
    id: Guid,
    title: string,
    content: string
}
