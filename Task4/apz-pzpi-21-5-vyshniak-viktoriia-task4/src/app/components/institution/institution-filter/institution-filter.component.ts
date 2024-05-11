import { Component, EventEmitter, Inject, Output } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { L10N_LOCALE } from 'angular-l10n';
import { EnumService } from '../../../core/services/enum.service';
import { InstitutionType } from '../../../core/enums/institution-type';
import { InstitutionFilter } from '../../../core/models/institution/institution-filter';

@Component({
	selector: 'app-institution-filter',
	templateUrl: './institution-filter.component.html',
	styleUrl: './institution-filter.component.scss'
})
export class InstitutionFilterComponent {
	@Output() 
	filterChange: EventEmitter<InstitutionFilter> = new EventEmitter<InstitutionFilter>();

	institutionFilterForm!: FormGroup;
	locale: any;

	PAGE_COUNT = 1;
	PAGE_SIZE = 10;

	constructor(
		private fb: FormBuilder,
		private enumService: EnumService,
		@Inject(L10N_LOCALE) locale: any
	) {
		this.locale = locale;
	}
	
	ngOnInit(): void {
		this.initializeForm();
	}
	
	initializeForm(): void {
		this.institutionFilterForm = this.fb.group({
			searchQuery: [''],
			type: [InstitutionType.All],
			sortByRatingAscending: [true],
		});
	}

	getInstitutionTypeOptions() {
		return this.enumService.getEnumOptions(InstitutionType);
	}

	onFilterInstitutions() {
		const filter = this.institutionFilterForm.value as InstitutionFilter;
		filter.pageCount = this.PAGE_COUNT;
		filter.pageSize = this.PAGE_SIZE;

		this.filterChange.emit(filter); 
	}
}
