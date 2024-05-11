import { Injectable } from '@angular/core';
import { ApiService, extractData } from './api.service';
import { Observable } from 'rxjs';
import { RegisterUserModel } from '../models/user/register-user-model';
import { UserProfileModel } from '../models/user/user-profile-model';
import { SetUserRoleModel } from '../models/user/set-user-role-model';
import { HttpHeaders, HttpParams } from '@angular/common/http';
import { api } from '../constants/api-constants';

@Injectable({
	providedIn: 'root'
})
export class UserService {

	PAGE_SIZE = 10;
	PAGE_COUNT = 1;

	constructor(
		private http: ApiService
	) { }

	public register(registerUserModel: RegisterUserModel): Observable<any> {
		const url = api.user.register;
		return this.http.post(url, registerUserModel).pipe(extractData(true));
	}

	public getAllUsers(): Observable<UserProfileModel[]> {
		const url = api.user.getAll;
		return this.http.get(url).pipe(extractData(true));
	}

	public setUserRole(setUserRoleModel: SetUserRoleModel) {
		const url = api.user.setUserRole;
		const params = new HttpParams()
			.append('pageSize', this.PAGE_SIZE)
			.append('pageCount', this.PAGE_COUNT);

		const options = {
			params: params,
			headers: new HttpHeaders()
		}

		return this.http.post(url, setUserRoleModel, options).pipe(extractData(true));
	}

	public getUserById(userId: number): Observable<UserProfileModel> {
		const url = api.user.getUserProfile;
		const options = {
			params: new HttpParams().append('userId', userId.toString()),
			headers: new HttpHeaders()
		}

		return this.http.get(url, options).pipe(extractData(true));
	}

	public doDatabaseBackup() {
		const url = api.user.doDbBackup;
		return this.http.post(url).pipe(extractData(true));
	}
}
