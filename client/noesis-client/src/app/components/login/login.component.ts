import { Component, OnInit, Output, EventEmitter, OnDestroy } from '@angular/core'
import { FormGroup, FormControl } from '@angular/forms'
import { Router } from '@angular/router'
import { AuthService } from '../../../services/auth.service'
import { User } from '../../../models/User'

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit, OnDestroy {

  public loginForm: FormGroup

  constructor(
    private _authService: AuthService,
    private router: Router
  ) { }

  initializeFormControls(): void {
    this.loginForm = new FormGroup({
      username: new FormControl(''),
      password: new FormControl('')
    })
  }

  login(): void {
    const login = this.loginForm.value as User

    this._authService.loginUser(login).subscribe(
      user => {
        localStorage.setItem('token', user.token)
        this._authService.decodeToken()
        this._authService.emitLoginData()
        this.router.navigateByUrl('books')
      }
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
