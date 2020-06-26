export interface User {
    id: number,
    name?: string,
    username: string,
    password: string,
    token?: string,
    email?: string,
    creationDate: Date,
    userRole?: string
}