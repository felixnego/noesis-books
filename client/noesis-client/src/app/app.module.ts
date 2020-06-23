import { BrowserModule } from '@angular/platform-browser'
import { HttpClientModule } from '@angular/common/http'
import { FormsModule } from '@angular/forms'
import { NgModule } from '@angular/core'
import { AppRoutingModule } from './app-routing.module'
import { AppComponent } from './app.component'
import { LandingComponent } from './components/landing/landing.component'
import { AngularFontAwesomeModule } from 'angular-font-awesome'
import { NavMenuComponent } from './components/nav-menu/nav-menu.component'
import { BookListComponent } from './components/book-list/book-list.component'
import { BrowserAnimationsModule } from '@angular/platform-browser/animations'
import { MatChipsModule } from '@angular/material/chips'
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner'
import { TopBooksComponent } from './components/top-books/top-books.component'
import { SearchResultsComponent } from './components/search-results/search-results.component'
import { from } from 'rxjs'


@NgModule({
  declarations: [
    AppComponent,
    LandingComponent,
    NavMenuComponent,
    BookListComponent,
    TopBooksComponent,
    SearchResultsComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    AppRoutingModule,
    HttpClientModule,
    AngularFontAwesomeModule,
    MatChipsModule,
    MatProgressSpinnerModule,
    BrowserAnimationsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
