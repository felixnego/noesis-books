import { Component, OnInit, Inject } from '@angular/core'
import { MAT_DIALOG_DATA } from '@angular/material/dialog'
import { FormControl, FormGroup } from '@angular/forms'
import { HttpClient } from '@angular/common/http'
import { Router } from '@angular/router'
import { MatDialogRef } from '@angular/material'
import { BookService } from '../../../services/book.service'
import { Note } from '../../../models/Note'

@Component({
  selector: 'app-note-edit',
  templateUrl: './note-edit.component.html',
  styleUrls: ['./note-edit.component.css']
})
export class NoteEditComponent implements OnInit {

  public editNoteForm: FormGroup
  private errorMessages = []

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: any,
    public dialogRef: MatDialogRef<NoteEditComponent>,
    private http: HttpClient,
    private router: Router,
    private _bookService: BookService
  ) { }

  initializeFormControls(): void {
    this.editNoteForm = new FormGroup({
      id: new FormControl(this.data.note.id),
      text: new FormControl(this.data.note.text)
    })
  }

  editNote(): void {
    const editedNote = this.editNoteForm.value as Note

    this._bookService.updateNote(this.data.bookId, editedNote.id, editedNote)
      .subscribe(_ => {
        this.dialogRef.close()
      }, err => this.errorMessages = err.error.errors)
  }

  ngOnInit() {
    this.initializeFormControls()
  }

}
