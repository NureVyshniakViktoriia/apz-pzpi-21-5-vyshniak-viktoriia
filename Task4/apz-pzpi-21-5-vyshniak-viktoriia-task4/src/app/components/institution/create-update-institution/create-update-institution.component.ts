import { Component, Inject, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { InstitutionModel } from '../../../core/models/institution/institution-model';
import { Observable, of } from 'rxjs';
import { SafeUrl } from '@angular/platform-browser';
import { L10N_LOCALE } from 'angular-l10n';
import { ActivatedRoute, Router } from '@angular/router';
import { InsitutionService } from '../../../core/services/institution.service';
import { ImageService } from '../../../core/services/image.service';
import { StorageService } from '../../../core/services/storage.service';
import { FacilityService } from '../../../core/services/facility.service';
import { InstitutionType } from '../../../core/enums/institution-type';
import { Region } from '../../../core/enums/region';
import { HttpErrorResponse } from '@angular/common/http';
import { FacilityListItem } from '../../../core/models/facility/facility-list-item';
import { EnumService } from '../../../core/services/enum.service';
import { ToastService } from '../../../core/services/toast.service';

@Component({
	selector: 'app-create-update-institution',
	templateUrl: './create-update-institution.component.html',
	styleUrl: './create-update-institution.component.scss'
})
export class CreateUpdateInstitutionComponent {
	isLoading = false;
	locale: any;

	activeTab = 'institution';

	institutionId!: number;
	institution$!: Observable<InstitutionModel>;
	institutionForm!: FormGroup;
	configureRfidReaderForm!: FormGroup;
	logoForm!: FormGroup;
	uploadedImageUrl: SafeUrl | undefined;

	facilities$!: Observable<FacilityListItem[]>;

	@ViewChild('logoFileInput') 
	logoFileInput: any;

	constructor(
		private fb: FormBuilder,
		private router: Router,
		private institutionService: InsitutionService,
		private imageService: ImageService,
		private storageService: StorageService,
		private facilityService: FacilityService,
		private enumService: EnumService,
		private route: ActivatedRoute,
		private toastService: ToastService,
		@Inject(L10N_LOCALE) locale: any
	) {
		this.locale = locale;
	}

	ngOnInit(): void {
		this.isLoading = true;
		this.route.params.subscribe(params => {
			this.institutionId = +params['institutionId'];
			if (this.institutionId) {
				this.initInstitution();
			}

			this.initializeForms();
			this.isLoading = false;
		});
	}

	initInstitution(): void {
		this.institution$ = this.institutionService.getInstitutionById(this.institutionId);
		this.institution$.subscribe((institution: InstitutionModel) => {
			this.fillFormWithData(institution);
			
			if (this.institutionId == undefined || this.institutionId <= 0)
				return;

			this.loadFacilities();
			this.getRfidReaderSettings();

			if (institution.logo)
				this.uploadedImageUrl = this.imageService.base64ToSafeUrl(institution.logo);
		});
	}

	loadFacilities(): void {
		this.facilityService.getAll(this.institutionId).subscribe({
			next: (result: FacilityListItem[]) => {
				this.facilities$ = of(result);
			},
			complete: () => {
				this.isLoading = false;
			}
		});
	}

	applyInstitution(): void {
		if (this.institutionForm.valid) {
			this.institutionService.apply(this.institutionForm.value).subscribe({
				next: () => {
					this.router.navigate(['/institutions']);
				},
				error: (error) => {
					if (error instanceof HttpErrorResponse) {
						const errorMessage = error.error instanceof ErrorEvent ? error.error.message : error.statusText;
						if (error.status === 400) {
							this.toastService.showErrorToast('Validation Error', errorMessage);
						} else if (error.status === 401) {
							this.toastService.showErrorToast('Unauthorized', errorMessage);
						} else {
							this.toastService.showErrorToast('Server Error', errorMessage);
						}
					} else {
						this.toastService.showErrorToast();
					}
				},
				complete: () => {
					this.toastService.showSuccessToast();
				}
			});
		} else {
			this.toastService.showErrorToast('Validation Error', 'Please check the form for errors.');
		}
	}

	onLogoFileChange(event: any): void {
		this.logoForm.patchValue({ logo: event.target.files[0] });
	}

	uploadLogo(): void {
		const uploadModel = {
			institutionId: this.logoForm.value.institutionId,
			file: this.logoForm.value.logo
		};

		this.institutionService.uploadLogo(uploadModel).subscribe({
			next: (result: any) => {
				this.uploadedImageUrl = this.imageService.base64ToSafeUrl(result.data);
			},
			error: () => {
				this.toastService.showErrorToast();
			},
			complete: () => {
				this.toastService.showSuccessToast();
			}
		});
	}

	removeFacility(facilityId: number): void {
		const removeFacilityModel = {
			institutionId: this.institutionId,
			facilityId: facilityId
		};

		this.institutionService.removeFacilityInstitution(removeFacilityModel).subscribe({
			next: () => {
				this.loadFacilities();
			},
			error: () => {
				this.toastService.showErrorToast();
			},
			complete: () => {
				this.toastService.showSuccessToast();
			}
		});
	}
	
	addFacility(facilityId: number): void {
		const addFacilityModel = {
			institutionId: this.institutionId,
			facilityId: facilityId
		};

		this.institutionService.addFacilityInstitution(addFacilityModel).subscribe({
			next: () => {
				this.loadFacilities();
			},
			error: () => {
				this.toastService.showErrorToast();
			},
			complete: () => {
				this.toastService.showSuccessToast();
			}
		});
	}

	onTabClick(tabName: string) {
    	this.activeTab = tabName;
    }

	getInstitutionTypeOptions() {
		return this.enumService.getEnumOptions(InstitutionType);
	}

	getRegionOptions() {
		return this.enumService.getEnumOptions(Region);
	}

	getRfidReaderSettings() {
		this.institutionService.getRfidSettingsById(this.institutionId).subscribe(settings => {
			this.configureRfidReaderForm.patchValue({
				rfidSettingsId: settings.institutionId,
				rfidReaderIpAddress: settings.rfidReaderIpAddress
			});
		});
	}

	configureRfidReader(): void {
		this.institutionService.configureRfidReader(this.configureRfidReaderForm.value).subscribe({
			next: () => {
				this.getRfidReaderSettings();
			}
		});
	}

	initializeForms(): void {
		this.institutionForm = this.fb.group({
			institutionId: [null],
			ownerId: [null],
			name: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(50)]],
			description: ['', [Validators.required, Validators.minLength(15), Validators.maxLength(200)]],
			phoneNumber: ['', [Validators.required]],
			address: [''],
			websiteUrl: [''],
			institutionType: [InstitutionType.Cafe, Validators.required],
			region: [Region.Kyiv, Validators.required],
		});

		if (this.institutionId <= 0)
			return;

		this.logoForm = this.fb.group({
			logo: [null],
			institutionId: this.institutionId
		});

		this.configureRfidReaderForm = this.fb.group({
			rfidSettingsId: this.institutionId,
			rfidReaderIpAddress: '',
			wifiSettings: this.fb.group({
				ssid: [''],
				password: [''],
				callbackUrl: ['']
			})
		});
	}

	fillFormWithData(institution: InstitutionModel): void {
		this.institutionForm.patchValue({
			institutionId: institution.institutionId,
			ownerId: this.storageService.getCurrentUserId(),
			name: institution.name,
			description: institution.description,
			phoneNumber: institution.phoneNumber,
			address: institution.address,
			websiteUrl: institution.websiteUrl,
			institutionType: institution.institutionType,
			region: institution.region,
		});
	}
}
