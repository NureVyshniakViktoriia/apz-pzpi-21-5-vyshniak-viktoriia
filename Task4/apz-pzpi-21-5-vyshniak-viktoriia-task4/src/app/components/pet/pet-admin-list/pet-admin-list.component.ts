import { Component, Inject } from '@angular/core';
import { PetListItem } from '../../../core/models/pet/pet-list-item';
import { Observable, of } from 'rxjs';
import { FormGroup } from '@angular/forms';
import { PetService } from '../../../core/services/pet.service';
import { L10N_LOCALE } from 'angular-l10n';
import { Guid } from 'guid-typescript';
import { ConfigurePetComponent } from '../configure-pet/configure-pet.component';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
	selector: 'app-pet-admin-list',
	templateUrl: './pet-admin-list.component.html',
	styleUrl: './pet-admin-list.component.scss'
})
export class PetAdminListComponent {
	pets$!: Observable<PetListItem[]>;

	isLoading: boolean = false;
	locale: any;

	configurePetForm!: FormGroup;

	constructor(
		private petService: PetService,
		private modalService: NgbModal,
		@Inject(L10N_LOCALE) locale: any
	) {
		this.locale = locale;
	}

	ngOnInit(): void {
		this.loadPets();
	}

	loadPets(): void {
		this.isLoading = true;
		setTimeout(() => {
			this.petService.getAll().subscribe({
				next: (result) => {
					this.pets$ = of(result);
				},
				complete: () => {
					this.isLoading = false;
				}
			});
		}, 1500);
	}

	openConfigurePetModal(petId: Guid): void {
		const modalRef = this.modalService.open(ConfigurePetComponent, { size: 'md' });
		modalRef.componentInstance.onSaveCallback = () => this.loadPets();
		modalRef.componentInstance.petId = petId;
	}
}
