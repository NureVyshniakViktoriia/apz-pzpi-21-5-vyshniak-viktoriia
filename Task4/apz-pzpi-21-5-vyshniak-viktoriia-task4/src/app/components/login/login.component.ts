import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../core/services/auth.service';
import { StorageService } from '../../core/services/storage.service';
import { Router } from '@angular/router';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { L10nTranslationService } from 'angular-l10n';
import { ApiErrorTypeEnum } from '../../core/enums/api-error-type';
import { ToastService } from '../../core/services/toast.service';

@Component({
	selector: 'app-login',
	templateUrl: './login.component.html',
	styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
	loginForm!: FormGroup;
	isLoading = false;
	isLoginError = false;
	errorMessage = '';

	constructor(
		private authService: AuthService,
		private storageService: StorageService,
		private toastService: ToastService,
		private router: Router,
		private translateService: L10nTranslationService,
	) {}

	ngOnInit(): void {
		this.initLoginForm();
		this.checkLoggedIn();
	}

	checkLoggedIn(): void {
		if (this.storageService.isLoggedIn()) {
			this.router.navigate(['/']);
		}
	}

	submitLogin(): void {
		this.isLoading = true;
		setTimeout(() => {
			this.authService.login(this.loginForm.value).subscribe({
				next: (result) => {
					this.handleLoginSuccess(result);
				},
				error: (error) => {
					this.handleLoginError(error);
				},
				complete: () => {
					this.isLoading = false;
					this.toastService.showSuccessToast();
				}
			});
		}, 1500);
	}

	initLoginForm(): void {
		this.loginForm = new FormGroup({
			'login': new FormControl('', [Validators.required]),
			'password': new FormControl('', [Validators.required])
		});
	}

	navigateByRole(): void {
		const userRole = this.storageService.getUserRole();
		switch(userRole) {
			case 'User':
				this.router.navigate(['/institutions']);
				break;
			case 'SysAdmin':
				this.router.navigate(['/users']);
				break;
			case 'Admin':
				this.router.navigate(['/institutions']);
				break;
			default:
				break;
		}
	}

	handleLoginError(error: any): void {
		if (error.errorType === ApiErrorTypeEnum.Forbidden) {
			this.isLoginError = true;
			this.errorMessage = this.translateService.translate('invalidLoginOrPassword');
		}
		this.isLoading = false;
	}

	handleLoginSuccess(result: any): void {
		this.storageService.saveToken(result);
		this.navigateByRole();
	}
}
