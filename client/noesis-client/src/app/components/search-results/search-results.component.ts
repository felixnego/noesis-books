import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router'
import { BookService } from 'src/services/book.service'
import { BookCard } from '../../../models/BookCard'

@Component({
  selector: 'app-search-results',
  templateUrl: './search-results.component.html',
  styleUrls: ['./search-results.component.css']
})
export class SearchResultsComponent implements OnInit {

  private searchTerms: string;
  private books: BookCard[];

  constructor(
    private route: ActivatedRoute,
    private _bookService: BookService
  ) { }

  constructEmptyArray(n: number): number[] {
    return Array(Math.round(n))
  }

  ngOnInit() {
    this.route.queryParams.subscribe(params => {
      this.searchTerms = params.search
      this._bookService.searchBooks(this.searchTerms).subscribe(
        results => this.books = [...results]
      )
    })
  }

}
