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
  public userId: number
  public isLoggedIn: boolean
  public userRole: string

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
      data => this.getUserDetails(data)
    )
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }

  getUserDetails(decodedToken: any): void {
    decodedToken != null ? this.username = decodedToken.unique_name : this.username = null
    decodedToken != null ? this.userId = decodedToken.nameid : this.userId = null
    decodedToken != null ? this.userRole = decodedToken.role : this.userRole = null
  }

  ngOnInit() {
    this.isLoggedIn = this._authService.isLoggedIn()
    
    if (this.isLoggedIn) {
      this.getUserDetails(this._authService.getDecodedToken())
    }
    
    this.authServiceSubscribe()
  }
}
