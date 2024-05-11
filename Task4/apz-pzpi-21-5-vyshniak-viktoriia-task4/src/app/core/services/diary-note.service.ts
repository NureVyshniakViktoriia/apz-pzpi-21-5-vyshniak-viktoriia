import { Injectable } from '@angular/core';
import { ApiService, extractData } from './api.service';
import { Observable } from 'rxjs';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { api } from '../constants/api-constants';
import { Guid } from 'guid-typescript';
import { UploadDocumentFileModel } from '../models/diary-note/upload-document-model';
import { CreateUpdateDiaryNoteModel } from '../models/diary-note/create-update-diary-note-model';
import { DiaryNoteModel } from '../models/diary-note/diary-note-model';
import { DiaryNoteListItem } from '../models/diary-note/diary-note-list-item';

@Injectable({
	providedIn: 'root'
})
export class DiaryNoteService {

	constructor(
		private http: ApiService,
		private httpRaw: HttpClient,
	) { }

	public getAllForPet(petId: Guid): Observable<DiaryNoteListItem[]> {
		const url = api.diaryNote.getAllForPet;
		const options = {
			params: new HttpParams().append('petId', petId.toString()),
			headers: new HttpHeaders()
		}

		return this.http.get(url, options).pipe(extractData(true));
	}

	public getNoteById(diaryNoteId: Guid): Observable<DiaryNoteModel> {
		const url = api.diaryNote.getNoteById;
		const options = {
			params: new HttpParams().append('diaryNoteId', diaryNoteId.toString()),
			headers: new HttpHeaders()
		}

		return this.http.get(url, options).pipe(extractData(true));
	}

	public delete(diaryNoteId: Guid) {
		const url = api.diaryNote.delete;
		const options = {
			params: new HttpParams().append('diaryNoteId', diaryNoteId.toString()),
			headers: new HttpHeaders()
		}

		return this.http.delete(url, options).pipe(extractData(true));
	}

	public apply(diaryNoteModel: CreateUpdateDiaryNoteModel) {
		const url = api.diaryNote.apply;
		return this.http.post(url, diaryNoteModel).pipe(extractData(true));
	}

	public uploadDocument(uploadModel: UploadDocumentFileModel) {
		const formData = new FormData();
		formData.append('diaryNoteId', uploadModel.diaryNoteId.toString());
		formData.append('file', uploadModel.file);

		const url = api.diaryNote.uploadDocument;
		return this.http.post(url, formData).pipe(extractData(true));
	}

	// public downloadDocument(diaryNoteId: Guid) {
	// 	const url = this.baseUrl + api.diaryNote.downloadDocument + `?diaryNoteId=${diaryNoteId}`;
	// 	return this.httpRaw.get(url, { reportProgress: true, responseType: 'blob' });
	// }

	public downloadDocument(diaryNoteId: Guid) {
		const url = api.diaryNote.downloadDocument;
		const options = {
			params: new HttpParams().append('diaryNoteId', diaryNoteId.toString()),
			headers: new HttpHeaders()
		}

		return this.http.download(url, options);
	}
}
