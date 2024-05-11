import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FacilityListItem } from '../../../core/models/facility/facility-list-item';
import { Observable } from 'rxjs';

@Component({
	selector: 'app-facility-list',
	templateUrl: './facility-list.component.html',
	styleUrl: './facility-list.component.scss'
})
export class FacilityListComponent {
	@Input()
	facilities$!: Observable<FacilityListItem[]>;
	@Input()
	isAdmin!: boolean;

	@Output() 
	removeFacility: EventEmitter<number> = new EventEmitter<number>();
	@Output() 
	addFacility: EventEmitter<number> = new EventEmitter<number>();

	constructor() { }

	toggleFacilitySelection(facility: FacilityListItem): void {
		if (facility.isForInstitution) {
			this.removeFacility.emit(facility.facilityId);
		} else {
			this.addFacility.emit(facility.facilityId);
		}
	}
}
