import { Injectable } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Token } from '../models/auth/token';

const USER_TOKEN = 'auth-user';
const USER_CULTURE = 'locale';

@Injectable({
	providedIn: 'root'
})
export class StorageService {

	constructor(
		private jwtHelper: JwtHelperService
	) { }

	public clear(): void {
		window.sessionStorage.clear();
	}

	public saveToken(data: Token): void {
		window.sessionStorage.removeItem(USER_TOKEN);
		window.sessionStorage.setItem(USER_TOKEN, JSON.stringify(data));
	}

	public getToken(): Token {
		const data = window.sessionStorage.getItem(USER_TOKEN);
		if (data) {
			return JSON.parse(data) as Token;
		}

		return {
			accessToken: "",
			refreshToken: ""
		} as Token;
	}

	public isLoggedIn(): boolean {
		const user = window.sessionStorage.getItem(USER_TOKEN);
		if (user) {
			return true;
		}

		return false;
	}

	public isAdmin(): boolean {
		if (!this.isLoggedIn()){
			return false;
		}

		const user = window.sessionStorage.getItem(USER_TOKEN);
		if (user == null) {
			return false;
		}

		const token = JSON.parse(user)
		const accessToken = token['accessToken'];
		const decodedToken = this.jwtHelper.decodeToken(accessToken);
		decodedToken['role'];

		return (decodedToken['role'] === 'Admin');
	}

	public getUserRole(): string {
		if (!this.isLoggedIn()){
			return "";
		}

		const user = window.sessionStorage.getItem(USER_TOKEN);
		if (user == null) {
			return "";
		}

		const token = JSON.parse(user)
		const accessToken = token['accessToken'];
		const decodedToken = this.jwtHelper.decodeToken(accessToken);
		decodedToken['role'];

		return decodedToken['role'];
	}

	public getCurrentUserId(): number {
		const user = window.sessionStorage.getItem(USER_TOKEN);
		if (user == null) {
			return -1;
		}

		const token = JSON.parse(user);
		const accessToken = token['accessToken'];
		const decodedToken = this.jwtHelper.decodeToken(accessToken);

		return decodedToken['id'];
	}

	public saveLocale(locale: string) {
		window.sessionStorage.removeItem(USER_CULTURE);
		window.sessionStorage.setItem(USER_CULTURE, locale);
	}

	public getLocale() {
		const data = window.sessionStorage.getItem(USER_CULTURE);
		return data;
	}
}
