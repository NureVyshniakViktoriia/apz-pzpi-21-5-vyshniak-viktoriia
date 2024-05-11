import { Component, Inject, Input, inject } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { InsitutionService } from '../../../core/services/institution.service';
import { L10N_LOCALE } from 'angular-l10n';
import { StorageService } from '../../../core/services/storage.service';
import { ToastService } from '../../../core/services/toast.service';

@Component({
	selector: 'app-set-institution-rating',
	templateUrl: './set-institution-rating.component.html',
	styleUrl: './set-institution-rating.component.scss'
})
export class SetInstitutionRatingComponent {
	@Input()
	institutionId!: number;
	@Input()
	onSaveCallback!: () => void;

	setRatingForm!: FormGroup;
	activeModal = inject(NgbActiveModal);

	locale: any;

	constructor(
		private fb: FormBuilder,
		private institutionService: InsitutionService,
		private toastService: ToastService,
		private storageService: StorageService,
		@Inject(L10N_LOCALE) locale: any
	) {
		this.locale = locale;
	}

	ngOnInit(): void {
		this.initSetRatingForm();
	}

	initSetRatingForm(): void {
		this.setRatingForm = this.fb.group({
			institutionId: this.institutionId,
			mark: 0,
    		userId: this.storageService.getCurrentUserId()
		});
	}

	setRating() {
		if (this.setRatingForm.valid) {
			this.institutionService.setRating(this.setRatingForm.value).subscribe({
				next: () => {
					this.onSaveCallback();
					this.activeModal.close(false);
				},
				error: () => {
					this.toastService.showErrorToast();
				},
				complete: () => {
					this.toastService.showSuccessToast();
				}
			});
		} else {
			this.toastService.showErrorToast();
		}
	}
}
