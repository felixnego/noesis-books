import { BrowserModule } from '@angular/platform-browser'
import { HttpClientModule } from '@angular/common/http'
import { FormsModule } from '@angular/forms'
import { ReactiveFormsModule } from '@angular/forms'
import { NgModule } from '@angular/core'
import { AppRoutingModule } from './app-routing.module'
import { AppComponent } from './app.component'
import { LandingComponent } from './components/landing/landing.component'
import { AngularFontAwesomeModule } from 'angular-font-awesome'
import { NavMenuComponent } from './components/nav-menu/nav-menu.component'
import { BookListComponent } from './components/book-list/book-list.component'
import { BrowserAnimationsModule } from '@angular/platform-browser/animations'
import { MatChipsModule } from '@angular/material/chips'
import { MatIconModule } from '@angular/material/icon'
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner'
import { TopBooksComponent } from './components/top-books/top-books.component'
import { SearchResultsComponent } from './components/search-results/search-results.component'
import { MatTableModule } from '@angular/material/table'
import { MatFormFieldModule } from '@angular/material/form-field'
import { BookDetailComponent } from './components/book-detail/book-detail.component'
import { MatDialogModule } from '@angular/material/dialog'
import { BookUpsertComponent } from './components/book-upsert/book-upsert.component'
import { MatAutocompleteModule } from '@angular/material/autocomplete'


@NgModule({
  declarations: [
    AppComponent,
    LandingComponent,
    NavMenuComponent,
    BookListComponent,
    TopBooksComponent,
    SearchResultsComponent,
    BookDetailComponent,
    BookUpsertComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    AppRoutingModule,
    HttpClientModule,
    ReactiveFormsModule,
    AngularFontAwesomeModule,
    MatChipsModule,
    MatFormFieldModule,
    MatAutocompleteModule,
    MatTableModule,
    MatDialogModule,
    MatIconModule,
    MatProgressSpinnerModule,
    BrowserAnimationsModule
  ],
  providers: [],
  bootstrap: [AppComponent],
  entryComponents: [BookUpsertComponent]
})
export class AppModule { }
