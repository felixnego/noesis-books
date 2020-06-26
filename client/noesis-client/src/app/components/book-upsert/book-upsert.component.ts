import { Component, OnInit, Inject, ElementRef, ViewChild } from '@angular/core'
import { MAT_DIALOG_DATA } from '@angular/material/dialog'
import { FormControl, FormGroup } from '@angular/forms'
import { HttpClient } from '@angular/common/http'
import { Router } from '@angular/router'
import { MatDialogRef } from '@angular/material'
import { COMMA, ENTER } from '@angular/cdk/keycodes'
import { MatChipInputEvent } from '@angular/material/chips'
import { MatAutocompleteSelectedEvent, MatAutocomplete } from '@angular/material/autocomplete'
import { Observable } from 'rxjs'
import { map, startWith } from 'rxjs/operators'
import { Category } from '../../../models/Category'
import { Author } from '../../../models/Author'
import { BookService } from '../../../services/book.service'
import { BookDetail } from 'src/models/BookDetail'


@Component({
  selector: 'app-book-upsert',
  templateUrl: './book-upsert.component.html',
  styleUrls: ['./book-upsert.component.css']
})
export class BookUpsertComponent implements OnInit {

  public bookFrom: FormGroup
  private errorMessages = []
  visible = true
  selectable = true
  removable = true
  readonly separatorKeysCodes: number[] = [ENTER, COMMA]

  categories: Category[] = []
  categoryCtrl = new FormControl()
  filteredCategories: Observable<Category[]>
  allCategories: Category[]

  authors: Author[] = []
  authorCtrl = new FormControl()
  filteredAuthors: Observable<Author[]>
  allAuthors: Author[]

  @ViewChild('categoryInput', { static: false }) categoryInput: ElementRef<HTMLInputElement>
  @ViewChild('authorInput', { static: false }) authorInput: ElementRef<HTMLInputElement>
  @ViewChild('auto', { static: false }) matAutocomplete: MatAutocomplete
  @ViewChild('autoAuthor', { static: false }) matAuthorAutocomplete: MatAutocomplete

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: any,
    public dialogRef: MatDialogRef<BookUpsertComponent>,
    private http: HttpClient,
    private router: Router,
    private _bookService: BookService
  ) { }


  add(event: MatChipInputEvent): void {
    const input = event.input;
    const value = event.value;

    if ((value || '').trim()) {
      
      this.categories.push({ categoryDescription: value.trim() })
    }

    if (input) {
      input.value = '';
    }

    this.categoryCtrl.setValue(null);
  }

  addAuthor(event: MatChipInputEvent): void {
    const input = event.input;
    const value = event.value;

    if ((value || '').trim()) {

      this.authors.push({ name: value.trim() })
    }

    if (input) {
      input.value = '';
    }

    this.authorCtrl.setValue(null);
  }

  remove(cat: Category): void {
    const index = this.categories.indexOf(cat);

    if (index >= 0) {
      this.categories.splice(index, 1);
    }
  }

  removeAuthor(author: Author): void {
    const index = this.authors.indexOf(author);

    if (index >= 0) {
      this.authors.splice(index, 1);
    }
  }

  selected(event: MatAutocompleteSelectedEvent): void {
    let catObj: any = {}
    catObj.categoryDescription = event.option.viewValue

    let exists = this.allCategories.find(e => e.categoryDescription == catObj.categoryDescription)

    if (exists) {
      catObj.id = exists.id
    }
    this.categories.push(catObj);
    this.categoryInput.nativeElement.value = '';
    this.categoryCtrl.setValue(null);
  }

  selectedAuthor(event: MatAutocompleteSelectedEvent): void {
    let authorObj: any = {}
    authorObj.name = event.option.viewValue

    let exists = this.allAuthors.find(e => e.name == authorObj.name)

    if (exists) {
      authorObj.id = exists.id
    }
    this.authors.push(authorObj);
    this.authorInput.nativeElement.value = '';
    this.authorCtrl.setValue(null);
  }

  initializeFormControls(): void {
    this.bookFrom = new FormGroup({
      title: this.data != null ? new FormControl(this.data.book.title) : new FormControl(''),
      pages: this.data != null ? new FormControl(this.data.book.pages) : new FormControl(''),
      isbn: this.data != null ? new FormControl(this.data.book.isbn) : new FormControl(''),
      year: this.data != null ? new FormControl(this.data.book.year) : new FormControl(''),
      goodReadsId: this.data != null ? new FormControl(this.data.book.goodReadsId) : new FormControl(''),
      publisher: this.data != null ? new FormControl(this.data.book.publisher) : new FormControl(''),
      thumbnailURL: this.data != null ? new FormControl(this.data.book.thumbnailURL) : new FormControl(''),
      coverBigURL: this.data != null ? new FormControl(this.data.book.coverBigURL) : new FormControl('')
    })
  }

  getAllPossiblreCategories() {
    this._bookService.getAllCategories().subscribe(
      results => {
        this.allCategories = results
        this.filteredCategories = this.categoryCtrl.valueChanges.pipe(
          startWith(null),
          map((cat: string | null) => cat ? this._filter(cat) : this.allCategories.slice()))
      }
    )
  }

  getAllPossibleAuthors() {
    this._bookService.getAllAuthors().subscribe(
      results => {
        this.allAuthors = results
        this.filteredAuthors = this.authorCtrl.valueChanges.pipe(
          startWith(null),
          map((author: string | null) => author ? this._filter_authors(author) : this.allAuthors.slice())
        )
      }
    )
  }

  private _filter(value: string): Category[] {
    const filterValue = value.toString().toLowerCase();

    return this.allCategories.filter(cat => cat.categoryDescription.toLowerCase().indexOf(filterValue) === 0);
  }

  private _filter_authors(value: string): Author[] {
    const filterValue = value.toString().toLowerCase();

    return this.allAuthors.filter(author => author.name.toLowerCase().indexOf(filterValue) === 0);
  }

  handle() {
    let book = this.bookFrom.value as BookDetail

    if (this.data) {
      book.id = this.data.book.id
      book.bookCategories = this.categories
      book.bookAuthors = this.authors
      this._bookService.updateBook(book).subscribe(_ => {
        this.dialogRef.close()
      }, err => this.errorMessages = err.error.errors)
    } else {
      book.bookCategories = this.categories
      book.bookAuthors = this.authors
      this._bookService.addBook(book).subscribe(_ => {
        this.dialogRef.close()
      }, err => {this.errorMessages = err.error.errors; console.log(this.errorMessages)})
    }
  }

  ngOnInit() {
    this.initializeFormControls()

    if (this.data != null) {
      this.data.book.bookCategories.forEach(cat => {
        this.categories.push(cat)
      })
      this.data.book.bookAuthors.forEach(author => {
        this.authors.push(author)
      })
    }

    this.getAllPossiblreCategories()
    this.getAllPossibleAuthors()
  }

}
