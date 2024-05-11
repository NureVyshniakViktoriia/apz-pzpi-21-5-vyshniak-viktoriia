import { Component, Inject } from '@angular/core';
import { L10N_LOCALE } from 'angular-l10n';
import { Observable, of } from 'rxjs';
import { InstitutionModel } from '../../../core/models/institution/institution-model';
import { SafeUrl } from '@angular/platform-browser';
import { ActivatedRoute } from '@angular/router';
import { InsitutionService } from '../../../core/services/institution.service';
import { ImageService } from '../../../core/services/image.service';
import { FacilityListItem } from '../../../core/models/facility/facility-list-item';
import { FacilityService } from '../../../core/services/facility.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { SetInstitutionRatingComponent } from '../set-institution-rating/set-institution-rating.component';
import { EnumService } from '../../../core/services/enum.service';
import { InstitutionType } from '../../../core/enums/institution-type';
import { Region } from '../../../core/enums/region';

@Component({
	selector: 'app-institution-view',
	templateUrl: './institution-view.component.html',
	styleUrl: './institution-view.component.scss'
})
export class InstitutionViewComponent {
	isLoading = false;
	locale: any;

	activeTab = 'institution';
	institutionId!: number;
	institution$!: Observable<InstitutionModel>;

	facilities$!: Observable<FacilityListItem[]>;

	uploadedImageUrl: SafeUrl | undefined;

	constructor(
		private route: ActivatedRoute,
		private institutionService: InsitutionService,
		private imageService: ImageService,
		private facilityService: FacilityService,
		private modalService: NgbModal,
		private enumService: EnumService,
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

			this.isLoading = false;
		});
	}

	initInstitution(): void {
		this.institution$ = this.institutionService.getInstitutionById(this.institutionId);
		this.institution$.subscribe((institution: InstitutionModel) => {
			if (institution.logo)
				this.uploadedImageUrl = this.imageService.base64ToSafeUrl(institution.logo);

			this.loadFacilities();
		});
	}

	loadFacilities(): void {
		this.isLoading = true;
		this.facilityService.getAllByInstitutionId(this.institutionId).subscribe({
			next: (result: FacilityListItem[]) => {
				this.facilities$ = of(result);
			},
			complete: () => {
				this.isLoading = false;
			}
		});
	}

	onTabClick(tabName: string) {
		this.activeTab = tabName;
	}

	openSetRatingModal(institutionId: number): void {
		const modalRef = this.modalService.open(SetInstitutionRatingComponent, { size: 'md' });
		modalRef.componentInstance.onSaveCallback = () => this.initInstitution();
		modalRef.componentInstance.institutionId = institutionId;
	}

	getInstitutionTypeName(key: InstitutionType): string {
		return this.enumService.getEnumKeyByValue(InstitutionType, key);
	}

	getRegionName(key: Region): string {
		return this.enumService.getEnumKeyByValue(Region, key);
	}
}
