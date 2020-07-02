import { Component, OnInit } from '@angular/core'
import { ActivatedRoute, Router } from '@angular/router'
import { BookService } from '../../../services/book.service'
import { AuthService } from '../../../services/auth.service'
import { BookDetail } from '../../../models/BookDetail'

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {

  public userId: number
  public username: string
  public isLoggedIn: boolean
  public recommendations: number[]
  private books: BookDetail[] = []

  constructor(
    private _bookService: BookService,
    private route: ActivatedRoute,
    private _authService: AuthService
  ) { }

  getRecommendations() {
    this._bookService.getRecommendations(this.userId).subscribe(
      data => {
        this.recommendations = data
        this.getBooksData()
      }
    )
  }

  getBooksData(){
    this.recommendations.forEach(bookId => {
      this._bookService.getBookDetails(bookId).subscribe(
        bookData => this.books.push(bookData)
      )
    })
  }

  constructEmptyArray(n: number): number[] {
    return Array(Math.round(n))
  }

  ngOnInit() {
    this.isLoggedIn = this._authService.isLoggedIn()

    if (this.isLoggedIn) {
      this.userId = Number(this.route.snapshot.paramMap.get('id'))
      this.username = this._authService.getDecodedToken()['unique_name']
      this.getRecommendations()
    }
  }

}
