import { Injectable } from '@angular/core';
import { ApiService, extractData } from './api.service';
import { HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { FacilityListItem } from '../models/facility/facility-list-item';
import { api } from '../constants/api-constants';

@Injectable({
	providedIn: 'root'
})
export class FacilityService {

	constructor(
		private http: ApiService
	) { }

	public getAllByInstitutionId(institutionId: number): Observable<FacilityListItem[]> {
		const url = api.facility.getAllByInstitutionId;
		const options = {
			params: new HttpParams().append('institutionId', institutionId.toString()),
			headers: new HttpHeaders()
		}

		return this.http.get(url, options).pipe(extractData(true));
	}

	public getAll(institutionId?: number | null): Observable<FacilityListItem[]> {
		const url = api.facility.getAll;
		var params = new HttpParams();
		if (institutionId !== undefined && institutionId !== null) {
			params = params.append('institutionId', institutionId.toString());
		}

		const options = {
			params: params,
			headers: new HttpHeaders()
		};

		return this.http.get(url, options).pipe(extractData(true));
	}
}
