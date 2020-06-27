import { Component, OnInit, Inject } from '@angular/core'
import { MAT_DIALOG_DATA } from '@angular/material/dialog'
import { FormControl, FormGroup } from '@angular/forms'
import { HttpClient } from '@angular/common/http'
import { Router } from '@angular/router'
import { MatDialogRef } from '@angular/material'
import { BookService } from '../../../services/book.service'
import { Comment } from '../../../models/Comment'

@Component({
  selector: 'app-comment-edit',
  templateUrl: './comment-edit.component.html',
  styleUrls: ['./comment-edit.component.css']
})
export class CommentEditComponent implements OnInit {

  public editCommentForm: FormGroup
  private errorMessages = []

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: any,
    public dialogRef: MatDialogRef<CommentEditComponent>,
    private http: HttpClient,
    private router: Router,
    private _bookService: BookService
  ) { }

  initializeFormControls(): void {
    this.editCommentForm = new FormGroup({
      id: new FormControl(this.data.comment.id),
      text: new FormControl(this.data.comment.text)
    })
  }

  editComment(): void {
    const editedComment = this.editCommentForm.value as Comment

    this._bookService.updateComment(this.data.bookId, editedComment.id, editedComment)
      .subscribe(_ => {
        this.dialogRef.close()
      }, err => this.errorMessages = err.error.errors)
  }

  ngOnInit() {
    this.initializeFormControls()
  }

}
