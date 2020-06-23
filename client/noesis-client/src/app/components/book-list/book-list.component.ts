import { Component, OnInit, HostListener } from '@angular/core'
import { BookService } from '../../../services/book.service'
import { BookCard } from '../../../models/BookCard'

@Component({
  selector: 'app-book-list',
  templateUrl: './book-list.component.html',
  styleUrls: ['./book-list.component.css']
})
export class BookListComponent implements OnInit {
  

  private books: BookCard[] = []


  constructor(
    private _bookService: BookService
  ) { }

  addToBooks() {
    this._bookService.getBooks().subscribe(
      results => this.books.push(...results)
    )
  }

  constructEmptyArray(n: number): number[] {
    return Array(Math.round(n))
  }

  @HostListener('window: scroll', ['$event'])
  onWindowScroll(event: any){
    const end =
      document.documentElement.scrollHeight -
      document.documentElement.scrollTop ===
      document.documentElement.clientHeight

    if (end) {
      this.addToBooks()
    }
  }

  ngOnInit() {
    this.books = []
    this._bookService.resetPage()
    this.addToBooks()
  }

}
