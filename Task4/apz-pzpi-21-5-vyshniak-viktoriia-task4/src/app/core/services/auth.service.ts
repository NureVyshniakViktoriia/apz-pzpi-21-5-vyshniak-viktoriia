import { Injectable } from '@angular/core';
import { LoginModel } from '../models/auth/login-model';
import { Observable } from 'rxjs';
import { HttpHeaders, HttpParams } from '@angular/common/http';
import { Token } from '../models/auth/token';
import { ApiService, extractData } from './api.service';
import { api } from '../constants/api-constants';

@Injectable({
	providedIn: 'root'
})
export class AuthService {

	constructor(
		private http: ApiService
	) { }

	public login(loginModel: LoginModel): Observable<Token> {
		const url = api.auth.login;
		return this.http.post(url, loginModel).pipe(extractData(true));
	}

	public refresh(refreshTokenString: string): Observable<Token> {
		const url = api.auth.refresh;
		const options = {
			params: new HttpParams(),
			headers: new HttpHeaders().append('refreshTokenString', refreshTokenString)
		}

		return this.http.get(url, options).pipe(extractData(true));
	}
}
