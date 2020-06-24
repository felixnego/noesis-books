import { Component, OnInit } from '@angular/core'
import { BookService } from '../../../services/book.service'
import { BookDetail } from '../../../models/BookDetail'
import { ActivatedRoute, Router } from '@angular/router'

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

  constructor(
    private _bookService: BookService,
    private route: ActivatedRoute
  ) { }

  getBook() {
    this._bookService.getBookDetails(this.bookId).subscribe(
      result => {
        this.book = result;
        this.dataSource = this.book.userComments;
      }
    )
  }

  constructEmptyArray(n: number): number[] {
    return Array(Math.round(n))
  }

  ngOnInit() {
    this.bookId = Number(this.route.snapshot.paramMap.get('id'))
    this.getBook()
  }

}
