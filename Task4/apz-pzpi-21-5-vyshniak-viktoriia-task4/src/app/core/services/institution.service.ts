import { Injectable } from '@angular/core';
import { ApiService, extractData } from './api.service';
import { Observable } from 'rxjs';
import { HttpHeaders, HttpParams } from '@angular/common/http';
import { api } from '../constants/api-constants';
import { InstitutionFilter } from '../models/institution/institution-filter';
import { InstitutionListItem } from '../models/institution/institution-list-item';
import { InstitutionModel } from '../models/institution/institution-model';
import { CreateUpdateInstitutionModel } from '../models/institution/create-update-institution-model';
import { UploadInstitutionLogoFileModel } from '../models/institution/upload-logo-model';
import { SetRatingModel } from '../models/institution/set-rating-model';
import { RemoveFacilityFromInstitution } from '../models/institution/remove-facility-institution-model';
import { AddFacilityToInstitutionModel } from '../models/institution/add-facility-institution-model';
import { ConfigureRfidReaderModel } from '../models/institution/configure-rfid-reader-model';
import { RfidReaderSettingsModel } from '../models/institution/rfid-reader-settings-model';

@Injectable({
	providedIn: 'root'
})
export class InsitutionService {

	constructor(
		private http: ApiService
	) { }

	public getAllByOwnerId(ownerId: number): Observable<InstitutionListItem[]> {
		const url = api.institution.getAllByOwnerId;
		const options = {
			params: new HttpParams().append('ownerId', ownerId.toString()),
			headers: new HttpHeaders()
		}

		return this.http.get(url, options).pipe(extractData(true));
	}

	public getInstitutionById(institutionId: number): Observable<InstitutionModel> {
		const url = api.institution.getInstitutionById;
		const options = {
			params: new HttpParams().append('institutionId', institutionId.toString()),
			headers: new HttpHeaders()
		}

		return this.http.get(url, options).pipe(extractData(true));
	}

	public list(filter: InstitutionFilter): Observable<InstitutionListItem[]> {
		const url = api.institution.list;
		var params = new HttpParams()
			.append('sortByRatingAscending', filter.sortByRatingAscending.toString())
			.append('pageSize', filter.pageSize.toString())
			.append('pageCount', filter.pageCount.toString());

		if (filter.searchQuery) {
			params = params.append('searchQuery', filter.searchQuery);
		}

		if (filter.type != null) {
			params = params.append('type', filter.type);
		}

		const options = {
			params: params,
			headers: new HttpHeaders()
		}

		return this.http.get(url, options).pipe(extractData(true));
	}

	public delete(institutionId: number) {
		const url = api.institution.delete;
		const options = {
			params: new HttpParams().append('institutionId', institutionId.toString()),
			headers: new HttpHeaders()
		}

		return this.http.delete(url, options).pipe(extractData(true));
	}

	public apply(institutionModel: CreateUpdateInstitutionModel) {
		const url = api.institution.apply;
		return this.http.post(url, institutionModel).pipe(extractData(true));
	}

	public uploadLogo(uploadModel: UploadInstitutionLogoFileModel): Observable<ImageData> {
		const formData = new FormData();
		formData.append('institutionId', uploadModel.institutionId.toString());
		formData.append('file', uploadModel.file);

		const url = api.institution.uploadLogo;
		return this.http.post(url, formData).pipe(extractData(true));
	}

	public setRating(model: SetRatingModel) {
		const url = api.institution.setRating;
		return this.http.post(url, model).pipe(extractData(true));
	}

	public removeFacilityInstitution(removeFacilityFromInstitution: RemoveFacilityFromInstitution) {
		const url = api.institution.removeFacilityInstitution;
		return this.http.post(url, removeFacilityFromInstitution).pipe(extractData(true));
	}

	public addFacilityInstitution(facilityToInstitutionModel: AddFacilityToInstitutionModel) {
		const url = api.institution.addFacilityInstitution;
		return this.http.post(url, facilityToInstitutionModel).pipe(extractData(true));
	}

	public configureRfidReader(configureRfidReaderModel: ConfigureRfidReaderModel) {
		const url = api.arduino.configureRfidReader;
		return this.http.post(url, configureRfidReaderModel).pipe(extractData(true));
	}

	public getRfidSettingsById(institutionId: number): Observable<RfidReaderSettingsModel> {
		const url = api.institution.getRfidSettingsById;
		const options = {
			params: new HttpParams().append('institutionId', institutionId.toString()),
			headers: new HttpHeaders()
		}

		return this.http.get(url, options).pipe(extractData(true));
	}
}
