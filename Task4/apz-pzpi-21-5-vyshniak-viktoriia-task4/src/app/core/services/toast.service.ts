import { Inject, Injectable } from '@angular/core';
import { L10N_LOCALE, L10nTranslationService } from 'angular-l10n';
import { ToastrService } from 'ngx-toastr';

@Injectable({
	providedIn: 'root'
})
export class ToastService {
	locale: any;

	constructor(
		private translateService: L10nTranslationService,
		private toastrService: ToastrService,
		@Inject(L10N_LOCALE) locale: any
	) {
		this.locale = locale;
	}

	public showErrorToast(title: string = 'failure', message: string = 'failureOperation'): void {
		const translatedMessages = this.translateService.translate([title, message]);
		this.toastrService.error(translatedMessages[title], translatedMessages[message]);
	}

	public showSuccessToast(): void {
		const translatedMessages = this.translateService.translate(['success', 'successfulOperation']);
		this.toastrService.success(translatedMessages['success'], translatedMessages['successfulOperation']);
	}

	public showWarningToast(title: string = 'warning', message: string = 'warning'): void {
		const translatedMessages = this.translateService.translate([title, message]);
		this.toastrService.warning(translatedMessages[title], translatedMessages[message]);
	}
}
