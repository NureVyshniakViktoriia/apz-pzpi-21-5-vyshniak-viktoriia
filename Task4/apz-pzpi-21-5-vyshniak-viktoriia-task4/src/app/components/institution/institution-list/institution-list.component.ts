import { Component, EventEmitter, Inject, Input, Output } from '@angular/core';
import { L10N_LOCALE } from 'angular-l10n';
import { Observable } from 'rxjs';
import { InstitutionListItem } from '../../../core/models/institution/institution-list-item';
import { EnumService } from '../../../core/services/enum.service';
import { InstitutionType } from '../../../core/enums/institution-type';
import { Region } from '../../../core/enums/region';

@Component({
	selector: 'app-institution-list',
	templateUrl: './institution-list.component.html',
	styleUrl: './institution-list.component.scss'
})
export class InstitutionListComponent {
	@Input()
	institutions$!: Observable<InstitutionListItem[]>;
	@Input()
	isAdmin!: boolean;

	@Output()
	editInstitution: EventEmitter<number> = new EventEmitter<number>();
	@Output()
	deleteInstitution: EventEmitter<number> = new EventEmitter<number>();
	@Output()
	viewInstitution: EventEmitter<number> = new EventEmitter<number>();

	locale: any;

	constructor(
		@Inject(L10N_LOCALE) locale: any,
		private enumService: EnumService
	) {
		this.locale = locale;
	}

	ngOnInit(): void {}
  
	edit(id: number): void {
		this.editInstitution.emit(id);
	}
  
	delete(id: number): void {
		this.deleteInstitution.emit(id);
	}
  
	onView(id: number): void {
		this.viewInstitution.emit(id);
	}

	getInstitutionTypeName(key: InstitutionType): string {
		return this.enumService.getEnumKeyByValue(InstitutionType, key);
	}

	getRegionName(key: Region): string {
		return this.enumService.getEnumKeyByValue(Region, key);
	}
}
