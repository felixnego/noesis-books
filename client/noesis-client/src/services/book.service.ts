import { Injectable } from '@angular/core'
import { HttpClient, HttpParams } from '@angular/common/http'
import { Observable, pipe } from 'rxjs'
import { BookCard } from '../models/BookCard'
import { BookDetail } from '../models/BookDetail'
import { Category } from '../models/Category'
import { Author } from '../models/Author'
import { Comment } from '../models/Comment'
import { Note } from '../models/Note'
import { Rating } from '../models/Rating'
import { environment } from 'src/environments/environment'

@Injectable({
  providedIn: 'root'
})
export class BookService {

  private bookURL = environment.apiUrl + '/api/books'
  private recommendationURL = environment.recommendationEngineUrl
  private reportsURL = environment.apiUrl + '/api/reports'
  private currentPage: number = -1

  constructor(private http: HttpClient) { }

  getBooks(): Observable<BookCard[]> {
    let params = new HttpParams()
    this.currentPage ++
    params = params.append('page', this.currentPage.toString())

    return this.http.get<BookCard[]>(this.bookURL, { params: params })
  }

  getTopCategories(): Observable<Category[]> {
    return this.http.get<Category[]>(this.reportsURL + '/topcategories')
  }

  getAllCategories(): Observable<Category[]> {
    return this.http.get<Category[]>(this.reportsURL + '/allcategories')
  }

  getAllAuthors(): Observable<Author[]> {
    return this.http.get<Author[]>(this.reportsURL + '/allauthors')
  }

  getTopBooksByCategory(id: number): Observable<BookCard[]> {
    return this.http.get<BookCard[]>(this.reportsURL + '/topcategories/' + `${id}`)
  }

  searchBooks(terms: string): Observable<BookCard[]> {
    let params = new HttpParams()
    params = params.append('terms', terms)

    return this.http.get<BookCard[]>(this.reportsURL + '/search', { params: params })
  }

  getBookDetails(id: number): Observable<BookDetail> {
    return this.http.get<BookDetail>(this.bookURL + `/${id}`)
  }

  updateBook(book: BookDetail): Observable<any> {
    return this.http.put(this.bookURL + `/${book.id}`, book)
  }

  addBook(book: BookDetail): Observable<any> {
    return this.http.post(this.bookURL, book)
  }

  deleteBook(id: number): Observable<BookDetail> {
    return this.http.delete<BookDetail>(this.bookURL + `/${id}`)
  }

  addComment(bookId: number, comment: Comment): Observable<any> {
    return this.http.post(this.bookURL + `/${bookId}/comments`, comment)
  }

  updateComment(bookId: number, commentId: number, comment: Comment): Observable<any> {
    return this.http.put(this.bookURL + `/${bookId}/comments/${commentId}`, comment)
  }

  deleteComment(bookId: number, commentId: number) {
    return this.http.delete(this.bookURL + `/${bookId}/comments/${commentId}`)
  }

  addNote(bookId: number, note: Note): Observable<any> {
    return this.http.post(this.bookURL + `/${bookId}/notes`, note)
  }

  updateNote(bookId: number, noteId: number, note: Note): Observable<any> {
    return this.http.put(this.bookURL + `/${bookId}/notes/${noteId}`, note)
  }

  deleteNote(bookId: number, noteId: number): Observable<any> {
    return this.http.delete(this.bookURL + `/${bookId}/notes/${noteId}`)
  }

  addRating(bookId: number, rating: Rating): Observable<any> {
    return this.http.post(this.bookURL + `/${bookId}/ratings`, rating)
  }

  getRecommendations(userId: number): Observable<any> {
    return this.http.get(this.recommendationURL + `/recommend/${userId}`)
  }

  reTrainModel(): Observable<any> {
    return this.http.get(this.recommendationURL + '/train')
  }

  resetPage() {
    this.currentPage = -1
  }
}
