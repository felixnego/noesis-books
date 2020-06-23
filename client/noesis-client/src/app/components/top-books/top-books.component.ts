import { Component, OnInit } from '@angular/core'
import { BookService } from '../../../services/book.service'
import { BookCard } from '../../../models/BookCard'
import { ThrowStmt } from '@angular/compiler'

@Component({
  selector: 'app-top-books',
  templateUrl: './top-books.component.html',
  styleUrls: ['./top-books.component.css']
})
export class TopBooksComponent implements OnInit {

  public topCategories: any[]

  public topBooksByCategory: {
    [category: string]: BookCard[]
  } = {}

  constructor(
    private _bookService: BookService
  ) { }

  getTopBooks() {
    this._bookService.getTopCategories().subscribe(
      categories => {
        this.topCategories = categories
        this.topCategories.forEach(category =>
          this._bookService.getTopBooksByCategory(category.id).subscribe(
            results => this.topBooksByCategory[category.categoryDescription] = results
          )
        )
      }
    )  
  }

  constructEmptyArray(n: number): number[] {
    return Array(Math.round(n))
  }

  ngOnInit() {
    this.getTopBooks()
  }

}
