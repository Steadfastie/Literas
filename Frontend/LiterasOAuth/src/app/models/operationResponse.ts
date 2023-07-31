export enum OperationType {
  Login = 'Login',
  Signup = 'Signup',
}
export interface OperationResponse {
  type: OperationType
  succeeded: boolean
  errorMessage?: string
  returnUrl?: string
}
