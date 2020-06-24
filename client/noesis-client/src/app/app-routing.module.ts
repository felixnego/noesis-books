import { NgModule } from '@angular/core'
import { Routes, RouterModule } from '@angular/router'
import { LandingComponent } from './components/landing/landing.component'
import { BookListComponent } from './components/book-list/book-list.component'
import { BookDetailComponent } from './components/book-detail/book-detail.component'
import { SearchResultsComponent } from './components/search-results/search-results.component'

const routes: Routes = [
  { path: '', component: LandingComponent},
  { path: 'books', component: BookListComponent},
  { path: 'search', component: SearchResultsComponent, pathMatch: 'full'},
  { path: 'books/:id', component: BookDetailComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
