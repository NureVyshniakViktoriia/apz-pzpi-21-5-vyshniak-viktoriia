import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment.development';
import { HttpClient, HttpHeaders, HttpParams, HttpResponse } from '@angular/common/http';
import { Observable, OperatorFunction, tap } from 'rxjs';

@Injectable({
	providedIn: 'root'
})
export class ApiService {
	private baseUrl = environment.webApi;

	constructor(
		public http: HttpClient,
	) { }

	public get<T>(
		url: string,
		options?: {
			headers?: HttpHeaders;
			params?: HttpParams;
		}
	) : Observable<HttpResponse<T>> {
		options ??= {};
		return this.http.get<HttpResponse<T>>(this.baseUrl + url, options);
	}

	public download(
		url: string,
		options?: {
			headers?: HttpHeaders;
			params?: HttpParams;
		}
	) : Observable<Blob> {
		options ??= {};
		return this.http.get(this.baseUrl + url, { ...options, reportProgress: true, responseType: 'blob' });
	}

	public post<T>(
		url: string,
		body?: any,
		options?: {
			headers?: HttpHeaders;
			params?: HttpParams;
		}
	): Observable<HttpResponse<T>> {
		options ??= {};
		return this.http.post<HttpResponse<T>>(this.baseUrl + url, body, options);
	}

	public put<T>(
		url: string,
		body: any,
		options?: {
			headers?: HttpHeaders
		}
	): Observable<HttpResponse<T>> {
		options ??= {};

		return this.http.put<HttpResponse<T>>(this.baseUrl + url, body, options);
	}

	public delete<T>(
		url: string,
		options?: {
			headers?: HttpHeaders;
			params?: HttpParams;
			body?: any;
		}
	): Observable<HttpResponse<T>> {
		options ??= {};

		return this.http.delete<HttpResponse<T>>(this.baseUrl + url, options);
	}

	public login<T>(
		url: string,
		body?: any,
		options?: {
			headers?: HttpHeaders;
			params?: HttpParams;
		}
	): Observable<HttpResponse<T>> {
		options ??= {};
		return this.http.post<HttpResponse<T>>(url, body, options);
	}

	public refresh<T>(
		url: string,
		options?: {
			headers?: HttpHeaders;
			params?: HttpParams;
		}
	): Observable<HttpResponse<T>> {
		options ??= {};

		return this.http.get<HttpResponse<T>>(url, options);
	}
}

export function extractData<T>(parseDate: boolean = false): OperatorFunction<any, any> {
	return (source: Observable<T>) =>
		source.pipe(tap(data => data));
}

export function formatString(source: string, args: { [key: string]: string | number | boolean } | Array<string | number>): string {
	if (!source || source === '')
		return source;

	return Array.isArray(args)
		? args.reduce((accumulator: string, value, index) => accumulator.replace(`{${index}}`, value?.toString()), source) as string
		: Object.entries(args).reduce((accumulator, [key, value]) => accumulator.replace(`{${key}}`, value?.toString()), source);
}
