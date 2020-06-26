import { Component, OnInit } from '@angular/core'
import { MatDialog, MatDialogConfig } from '@angular/material'
import { BookUpsertComponent } from '../book-upsert/book-upsert.component'
import { AuthService } from '../../../services/auth.service'
import { Router } from '@angular/router'

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent implements OnInit {

  isExpanded = false
  public username: string

  constructor(
    private dialog: MatDialog,
    private _authService: AuthService,
    private router: Router
  ) { }


  collapse() {
    this.isExpanded = false
  }

  openUpsertDialog() {
    const dialogConfig = new MatDialogConfig()

    let ref = this.dialog.open(BookUpsertComponent, { data: null, maxHeight: '90vh', width: '600px'})
  }

  logout() {
    this._authService.logout()
    this.router.navigateByUrl('/')
  }

  authServiceSubscribe() {
    this._authService.getEmitter().subscribe(
      data => this.username = data
    )
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }

  ngOnInit() {
    this.username = this._authService.getDecodedToken()['unique_name']
    this.authServiceSubscribe()
  }
}
