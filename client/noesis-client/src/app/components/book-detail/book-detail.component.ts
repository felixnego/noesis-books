import { Component, OnInit } from '@angular/core'
import { BookService } from '../../../services/book.service'
import { AuthService } from '../../../services/auth.service'
import { BookDetail } from '../../../models/BookDetail'
import { ActivatedRoute, Router } from '@angular/router'
import { MatDialog, MatDialogConfig } from '@angular/material'
import { BookUpsertComponent } from '../book-upsert/book-upsert.component'
import { CommentEditComponent } from '../comment-edit/comment-edit.component'
import { NoteEditComponent } from '../note-edit/note-edit.component'
import { Comment } from '../../../models/Comment'
import { Note } from '../../../models/Note'
import { Rating } from '../../../models/Rating'

@Component({
  selector: 'app-book-detail',
  templateUrl: './book-detail.component.html',
  styleUrls: ['./book-detail.component.css']
})
export class BookDetailComponent implements OnInit {

  public book: BookDetail
  private bookId: number
  public dataSource: any[]
  public displayedColumns: string[] = ['Username', 'Comment Text', 'Added On', 'Actions']
  public isLoggedIn: boolean
  private errorMessages = []
  public loggedUser
  public loggedUserId
  public loggedUserRole: string
  public userHasNotes: boolean
  public ratings: number[] = [1, 2, 3, 4, 5]
  public hoverIndex: number
  public lastRatingValue: number
  public userRating: Rating = new Rating()

  constructor(
    private _bookService: BookService,
    private route: ActivatedRoute,
    private _authService: AuthService,
    private dialog: MatDialog,
    private router: Router
  ) { }

  getBook() {
    this._bookService.getBookDetails(this.bookId).subscribe(
      result => {
        this.book = result
        this.dataSource = this.book.userComments
        this.userHasNotes = this.book.userNotes.map(e => e.userId).includes(Number(this.loggedUserId))
      }
    )
  }

  constructEmptyArray(n: number): number[] {
    return Array(Math.round(n))
  }

  openUpsertDialog(book: BookDetail) {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.disableClose = true // closes on ESC or clickng outside
    dialogConfig.autoFocus = true

    let ref = this.dialog.open(BookUpsertComponent, { data: {book: book}, maxHeight: '90vh', width: '600px' })
    ref.afterClosed().subscribe(_ => this.getBook())
  }

  delete(id: number): void {
    if (confirm('Are you sure you want to delete this book? This action cannot be reverted!')) {
      this._bookService.deleteBook(id).subscribe(
        _ => this.router.navigateByUrl('books')
      )
    }
  }

  addComment(bookId: number, form: any): void {
    let comment: Comment = { text: form.value.text }

    this._bookService.addComment(bookId, comment).subscribe(_ => {
      this.getBook()
      form.resetForm()
    }, err => { this.errorMessages = err.error.errors; console.log(err.error.errors) })
  }

  deleteComment(bookId: number, commentId: number): void {
    this._bookService.deleteComment(bookId, commentId).subscribe(_ => {
      this.getBook()
    })
  }

  updateComment(bookId: number, comment: any): void {
    let ref = this.dialog.open(CommentEditComponent, { data: {comment: comment, bookId: bookId}, width:'400px' })
    ref.afterClosed().subscribe(_ => this.getBook())
  }

  addNote(bookId: number, form: any){
    let note: Note = { text: form.value.text }

    this._bookService.addNote(bookId, note).subscribe(_ => {
      this.getBook()
      form.resetForm()
    }, err => { this.errorMessages = err.error.errors; console.log(err.error.errors) })

  }

  deleteNote(bookId: number, noteId: number) {
    this._bookService.deleteNote(bookId, noteId).subscribe(_ => {
      this.getBook()
    })
  }

  updateNote(bookId: number, note: any): void {
    let ref = this.dialog.open(NoteEditComponent, { data: { note: note, bookId: bookId }, width: '400px' })
    ref.afterClosed().subscribe(_ => this.getBook())
  }

  sendRating(rating: number): void {
    localStorage.setItem(this.loggedUserId.toString(), rating.toString())
    localStorage.setItem(this.loggedUser.toString(), this.bookId.toString())
    this.lastRatingValue = rating

    this.userRating.userId = this.loggedUserId
    this.userRating.bookId = this.bookId
    this.userRating.ratingValue = rating

    this._bookService.addRating(this.bookId, this.userRating).subscribe(_ => {
      this._bookService.reTrainModel().subscribe(_ => {
        console.log('Your new rating will re train the ML model!')
      })
      this.getBook()
    })
  }

  ngOnInit() {
    this.bookId = Number(this.route.snapshot.paramMap.get('id'))

    this.isLoggedIn = this._authService.isLoggedIn()

    if (this.isLoggedIn) {
      this.loggedUser = this._authService.decodedToken.unique_name
      this.loggedUserId = this._authService.decodedToken.nameid
      this.loggedUserRole = this._authService.decodedToken.role
    }
    this.getBook()
    if (localStorage.getItem(this.loggedUser.toString()) == this.bookId.toString()) {
      this.lastRatingValue = parseInt(localStorage.getItem(this.loggedUserId.toString()), 10)
    }
    
  }

}