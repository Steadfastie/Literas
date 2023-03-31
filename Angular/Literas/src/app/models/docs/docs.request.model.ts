import {Guid} from "guid-typescript";

export interface DocRequestModel {
    id: Guid,
    title: string,
    content: string
}
