import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { L10nTranslationService } from 'angular-l10n';
import { ApiErrorTypeEnum } from '../../core/enums/api-error-type';
import { RegisterUserModel } from '../../core/models/user/register-user-model';
import { EnumService } from '../../core/services/enum.service';
import { Region } from '../../core/enums/region';
import { Gender } from '../../core/enums/gender';
import { UserService } from '../../core/services/user.service';
import { ToastService } from '../../core/services/toast.service';

@Component({
	selector: 'app-registration',
	templateUrl: './registration.component.html',
	styleUrls: ['./registration.component.scss']
})
export class RegistrationComponent implements OnInit {
	registerForm!: FormGroup;
	isLoading = false;
	isRegisterError = false;
	errorMessage = '';

	constructor(
		private userService: UserService,
		private enumService: EnumService,
		private formBuilder: FormBuilder,
		private translateService: L10nTranslationService,
		private toastService: ToastService,
		private router: Router,
	) {}

	ngOnInit(): void {
		this.initRegisterForm();
	}

	submitRegistration(): void {
		this.isLoading = true;
		setTimeout(() => {
			this.userService.register(this.registerForm.value as RegisterUserModel).subscribe({
				next: () => this.handleRegistrationSuccess(),
				error: (error: any) => this.handleRegistrationError(error),
				complete: () => this.handleRegistrationComplete(),
			});
		}, 1500);
	}

	initRegisterForm(): void {
		this.registerForm = this.formBuilder.group({
			'login': ['', [Validators.required, Validators.minLength(4)]],
			'password': ['', [Validators.required, Validators.minLength(4)]],
			'region': [Region.Kyiv, Validators.required],
			'gender': [Gender.Male, Validators.required],
			'email': ['', [Validators.required, Validators.email]],
		});
	}

	handleRegistrationSuccess(): void {
		this.router.navigate(['login']);
	}

	handleRegistrationError(error: any): void {
		if (error.errorType === ApiErrorTypeEnum.ValidationError) {
			this.isRegisterError = true;
			this.errorMessage = this.translateService.translate('loginAlreadyTaken');
		}
		this.isLoading = false;
	}

	handleRegistrationComplete(): void {
		this.isLoading = false;
		this.toastService.showSuccessToast();
	}

	getRegionOptions(): any[] {
		return this.enumService.getEnumOptions(Region);
	}

	getGenderOptions(): any[] {
		return this.enumService.getEnumOptions(Gender);
	}
}
