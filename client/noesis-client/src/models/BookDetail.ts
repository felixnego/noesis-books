export class BookDetail {
    id?: number
    title: string
    pages: number
    isbn: string
    year: number
    goodReadsId: number
    publisher: string
    thumbnailURL: string
    coverBigURL: string
    averageRating?: number
    bookCategories: any[]
    bookAuthors: any[]
    userComments?: any[]
    userNotes?: any[]
}