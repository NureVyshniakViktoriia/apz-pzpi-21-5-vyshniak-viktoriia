import { HTTP_INTERCEPTORS, HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, switchMap, throwError } from 'rxjs';
import { StorageService } from '../services/storage.service';
import { AuthService } from '../services/auth.service';
import { Router } from '@angular/router';
import { Token } from '../models/auth/token';
import { L10nTranslationService } from 'angular-l10n';

@Injectable({
	providedIn: 'root'
})
export class HttpRequestInterceptor implements HttpInterceptor {
	private isRefreshing = false;

	constructor(
		private storageService: StorageService,
		private translation: L10nTranslationService,
		private authService: AuthService,
		private router: Router
	) { }

	intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
		var token: Token = this.storageService.getToken();
		if (!token) {
			return next.handle(req);
		}

		req = req.clone({
			headers: req.headers
				.set('Authorization', `Bearer ${token.accessToken}`)
				.set('Accept-Language', this.translation.getLocale().language),
		});

		return next.handle(req).pipe(
			catchError((error) => {
				if (error instanceof HttpErrorResponse
					&& error.status === 401) {
					return this.handle401Error(req, next);
				}

				return throwError(() => error);
			})
		);
	}

	private handle401Error(request: HttpRequest<any>, next: HttpHandler) {
		if (!this.isRefreshing) {
			this.isRefreshing = true;

			if (this.storageService.isLoggedIn()) {
				var token: Token = this.storageService.getToken();

				return this.authService.refresh(token.refreshToken).pipe(
					switchMap((result) => {
						this.isRefreshing = false;
						this.storageService.saveToken(result);

						return next.handle(request.clone({
							headers: request.headers.delete('Authorization')
								.set('Authorization', `Bearer ${result.accessToken}`),
					}));
				}),
				catchError((error) => {
					this.isRefreshing = false;

					if (error.status == '403') {
						this.storageService.clear();
						this.router.navigate(['login']);
					}

					return throwError(() => error);
				}));
			}
		}

		return next.handle(request);
	}
}

export const httpInterceptorProviders = [
	{
		provide: HTTP_INTERCEPTORS,
		useClass: HttpRequestInterceptor,
		multi: true
	},
];
