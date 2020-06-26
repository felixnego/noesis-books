import { Component, OnInit } from '@angular/core'
import { MatDialog, MatDialogConfig } from '@angular/material'
import { BookUpsertComponent } from '../book-upsert/book-upsert.component'

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent implements OnInit {
  isExpanded = false;

  constructor(
    private dialog: MatDialog
  ) { }

  ngOnInit() {
  }

  collapse() {
    this.isExpanded = false;
  }

  openUpsertDialog() {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.disableClose = true // closes on ESC or clickng outside
    dialogConfig.autoFocus = true

    let ref = this.dialog.open(BookUpsertComponent, { data: null, maxHeight: '90vh', width: '600px'})
    // ref.afterClosed().subscribe(_ => )
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }
}
