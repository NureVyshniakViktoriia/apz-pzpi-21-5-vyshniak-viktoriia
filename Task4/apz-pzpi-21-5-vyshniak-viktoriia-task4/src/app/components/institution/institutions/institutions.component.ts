import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { Observable, of } from 'rxjs';
import { InstitutionListItem } from '../../../core/models/institution/institution-list-item';
import { InsitutionService } from '../../../core/services/institution.service';
import { StorageService } from '../../../core/services/storage.service';
import { InstitutionFilter } from '../../../core/models/institution/institution-filter';
import { InstitutionType } from '../../../core/enums/institution-type';

@Component({
	selector: 'app-institutions',
	templateUrl: './institutions.component.html',
	styleUrl: './institutions.component.scss'
})
export class InstitutionsComponent {
	institutions$!: Observable<InstitutionListItem[]>;
	isLoading: boolean = false;
	currentUserId!: number;
	isAdmin!: boolean;
	
	institutionFilter: InstitutionFilter = {
		searchQuery: '',
		type: InstitutionType.All,
		sortByRatingAscending: true,
		pageSize: 10,
		pageCount: 1
	};

	constructor(
		private router: Router,
		private institutionService: InsitutionService,
		private storageService: StorageService
	) { }

	ngOnInit(): void {
		this.currentUserId = this.storageService.getCurrentUserId();
		this.isAdmin = this.storageService.isAdmin();

		this.loadInstitutions();
	}

	loadInstitutions() {
		this.isLoading = true;
		if (this.isAdmin) {
			this.institutionService.getAllByOwnerId(this.currentUserId).subscribe({
				next: (result: InstitutionListItem[]) => {
					this.institutions$ = of(result);
				},
				complete: () => {
					this.isLoading = false;
				}
			});

			return;
		}

		this.institutionService.list(this.institutionFilter).subscribe({
			next: (result: InstitutionListItem[]) => {
				this.institutions$ = of(result);
			},
			complete: () => {
				this.isLoading = false;
			}
		});
	}

	onEditInstitution(institutionId: number) {
		this.router.navigate(['/institution-create-update', institutionId]);
	}

	onDeleteInstitution(institutionId: number) {
		this.institutionService.delete(institutionId).subscribe(
			() => {
				this.loadInstitutions();
			},
		);
	}

	onAddNewInstitution() {
		this.router.navigate(['/institution-create-update', -1]);
	}

	onViewInstitution(institutionId: number) {
		this.router.navigate(['/institution-view', institutionId]);
	}

	onFilterChange(institutionFilter: InstitutionFilter) {
		this.institutionFilter = institutionFilter;
		this.loadInstitutions();
	}
}
