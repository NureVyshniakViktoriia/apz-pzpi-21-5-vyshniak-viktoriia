import { Component, Inject } from '@angular/core';
import { Observable, of } from 'rxjs';
import { PetListItem } from '../../../core/models/pet/pet-list-item';
import { PetService } from '../../../core/services/pet.service';
import { L10N_LOCALE } from 'angular-l10n';
import { Guid } from 'guid-typescript';
import { Router } from '@angular/router';
import { StorageService } from '../../../core/services/storage.service';
import { ToastService } from '../../../core/services/toast.service';
import { EnumService } from '../../../core/services/enum.service';
import { PetType } from '../../../core/enums/pet-type';

@Component({
	selector: 'app-pet-list',
	templateUrl: './pet-list.component.html',
	styleUrl: './pet-list.component.scss'
})
export class PetListComponent {
	pets$!: Observable<PetListItem[]>;
	isLoading: boolean = false;
	locale: any;
	ownerId!: number;
	emptyGuid = Guid.createEmpty();

	constructor(
		private router: Router,
		private petService: PetService,
		private storageService: StorageService,
		private toastService: ToastService,
		private enumService: EnumService,
		@Inject(L10N_LOCALE) locale: any
	) {
		this.locale = locale;
	}

	ngOnInit(): void {
		this.ownerId = this.storageService.getCurrentUserId();
		this.loadPets();
	}

	loadPets(): void {
		this.isLoading = true;
		this.petService.getPetsByOwnerId(this.ownerId).subscribe({
			next: (result) => {
				this.pets$ = of(result);
			},
			complete: () => {
				this.isLoading = false;
			}
		});
	}

	editPet(petId: Guid): void {
		this.router.navigate(['/pet-create-update', petId]);
	}

	deletePet(petId: Guid): void {
		this.petService.delete(petId).subscribe({
			next: () => {
				this.loadPets();
			},
			error: () => {
				this.toastService.showErrorToast();
			},
			complete: () => {
				this.toastService.showSuccessToast();
			}
		});
	}

	addPet(): void {
		this.router.navigate(['/pet-create-update', this.emptyGuid.toString()]);
	}

	getPetTypeName(key: PetType): string {
		return this.enumService.getEnumKeyByValue(PetType, key);
	}
}
