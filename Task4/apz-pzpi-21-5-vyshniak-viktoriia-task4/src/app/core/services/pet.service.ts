import { Injectable } from '@angular/core';
import { ApiService, extractData } from './api.service';
import { HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Guid } from 'guid-typescript';
import { api } from '../constants/api-constants';
import { CreateUpdatePetModel } from '../models/pet/create-update-pet-model';
import { PetModel } from '../models/pet/pet-model';
import { PetListItem } from '../models/pet/pet-list-item';
import { ArduinoSettingsModel } from '../models/pet/arduino-settings-model';
import { ConfigurePetDeviceModel } from '../models/pet/configure-pet-device-model';

@Injectable({
	providedIn: 'root'
})
export class PetService {

	constructor(
		private http: ApiService,
	) { }

	public getPetsByOwnerId(ownerId: number): Observable<PetListItem[]> {
		const url = api.pet.getPetsByOwnerId;
		const options = {
			params: new HttpParams().append('ownerId', ownerId.toString()),
			headers: new HttpHeaders()
		}

		return this.http.get(url, options).pipe(extractData(true));
	}

	public getAll(): Observable<PetListItem[]> {
		const url = api.pet.getAll;
		return this.http.get(url).pipe(extractData(true));
	}

	public getPetById(petId: Guid): Observable<PetModel> {
		const url = api.pet.getPetById;
		const options = {
			params: new HttpParams().append('petId', petId.toString()),
			headers: new HttpHeaders()
		}

		return this.http.get(url, options).pipe(extractData(true));
	}

	public getPetSettingsById(petId: Guid): Observable<ArduinoSettingsModel> {
		const url = api.pet.getPetSettings;
		const options = {
			params: new HttpParams().append('petId', petId.toString()),
			headers: new HttpHeaders()
		}

		return this.http.get(url, options).pipe(extractData(true));
	}

	public delete(petId: Guid) {
		const url = api.pet.delete;
		const options = {
			params: new HttpParams().append('petId', petId.toString()),
			headers: new HttpHeaders()
		}

		return this.http.delete(url, options).pipe(extractData(true));
	}

	public apply(petModel: CreateUpdatePetModel) {
		const url = api.pet.apply;
		return this.http.post(url, petModel).pipe(extractData(true));
	}

	public configurePetDevice(configurePetDeviceModel: ConfigurePetDeviceModel) {
		const url = api.arduino.configurePetDevice;
		return this.http.post(url, configurePetDeviceModel).pipe(extractData(true));
	}
}
