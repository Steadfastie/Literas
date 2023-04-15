import {Delta} from "quill";

export interface DocResponseModel {
    id: string,
    creatorId:string,
    title: string,
    titleDelta: Delta,
    content: string
    contentDeltas: Delta
}
