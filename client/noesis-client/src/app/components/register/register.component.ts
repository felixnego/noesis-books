import { Component, OnInit, OnDestroy } from '@angular/core'
import { FormGroup, FormControl } from '@angular/forms'
import { Router } from '@angular/router'
import { AuthService } from '../../../services/auth.service'
import { User } from '../../../models/User'

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit, OnDestroy {

  public registerForm: FormGroup

  constructor(
    private _authService: AuthService,
    private router: Router
  ) { }

  initializeFormControls(): void {
    this.registerForm = new FormGroup({
      name: new FormControl(''),
      username: new FormControl(''),
      password: new FormControl(''),
      email: new FormControl('')
    })
  }

  register(): void {
    const newUser = this.registerForm.value as User

    this._authService.registerUser(newUser).subscribe(
      _ => this.router.navigateByUrl('books')
    )
  }

  ngOnInit() {
    this.initializeFormControls()
    document.body.classList.add('login-background')
  }

  ngOnDestroy() {
    document.body.className = '';
  }

}
