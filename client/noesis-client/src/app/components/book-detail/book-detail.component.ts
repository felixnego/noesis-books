import { Component, OnInit } from '@angular/core'
import { BookService } from '../../../services/book.service'
import { BookDetail } from '../../../models/BookDetail'
import { ActivatedRoute, Router } from '@angular/router'
import { MatDialog, MatDialogConfig } from '@angular/material'
import { BookUpsertComponent } from '../book-upsert/book-upsert.component'

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
    private route: ActivatedRoute,
    private dialog: MatDialog
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

  openUpsertDialog(book: BookDetail) {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.disableClose = true // closes on ESC or clickng outside
    dialogConfig.autoFocus = true

    let ref = this.dialog.open(BookUpsertComponent, { data: {book: book}, maxHeight: '90vh', width: '600px' })
    ref.afterClosed().subscribe(_ => this.getBook())
  }

  ngOnInit() {
    this.bookId = Number(this.route.snapshot.paramMap.get('id'))
    this.getBook()
  }

}
