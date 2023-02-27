import {Guid} from "guid-typescript";

export interface IUserResponseModel{
  id: Guid,
  login: string,
  fullName: string
}
