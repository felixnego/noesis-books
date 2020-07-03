import { Injectable, Output, EventEmitter } from '@angular/core'
import { environment } from 'src/environments/environment'
import { JwtHelperService } from '@auth0/angular-jwt'
import { HttpClient } from '@angular/common/http'
import { User } from '../models/User'

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private baseURL = environment.apiUrl + '/users'
  private jwtHelper = new JwtHelperService();
  decodedToken: any;
  @Output() fireIsLoggedIn: EventEmitter<any> = new EventEmitter<any>();

  constructor(
    private http: HttpClient
  ) { }

  registerUser(user: User) {
    return this.http.post<any>(this.baseURL, user);
  }

  loginUser(user: User) {
    return this.http.post<User>(this.baseURL + '/authenticate', user);
  }

  logout() {
    localStorage.removeItem('token')
    this.decodedToken = null
    this.fireIsLoggedIn.emit(null)
  }

  emitLoginData() {
    this.fireIsLoggedIn.emit(this.decodedToken);
  }

  getEmitter() {
    return this.fireIsLoggedIn;
  }

  isLoggedIn() {
    const token = localStorage.getItem('token');
    return !this.jwtHelper.isTokenExpired(token);
  }

  decodeToken() {
    this.decodedToken = this.jwtHelper.decodeToken(localStorage.getItem('token'));
    return this.decodedToken;
  }

  getDecodedToken() {
    this.decodeToken();
    return this.decodedToken;
  }

  getUserName() {
    console.log('getUserName was called')
    console.log('decoded token is: ', this.decodedToken)
    return this.decodedToken
  }

  getToken() {
    return localStorage.getItem('token');
  }

}
