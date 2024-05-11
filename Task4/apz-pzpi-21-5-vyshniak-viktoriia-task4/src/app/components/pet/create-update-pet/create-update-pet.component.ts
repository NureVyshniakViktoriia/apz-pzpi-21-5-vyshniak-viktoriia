import { Component, Inject } from '@angular/core';
import { Guid } from 'guid-typescript';
import { PetModel } from '../../../core/models/pet/pet-model';
import { Observable, of } from 'rxjs';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { PetService } from '../../../core/services/pet.service';
import { DiaryNoteService } from '../../../core/services/diary-note.service';
import { StorageService } from '../../../core/services/storage.service';
import { L10N_LOCALE } from 'angular-l10n';
import { EnumService } from '../../../core/services/enum.service';
import { DiaryNoteListItem } from '../../../core/models/diary-note/diary-note-list-item';
import { PetType } from '../../../core/enums/pet-type';
import { HttpErrorResponse } from '@angular/common/http';
import { DatePipe } from '@angular/common';
import { ToastService } from '../../../core/services/toast.service';

@Component({
	selector: 'app-create-update-pet',
	templateUrl: './create-update-pet.component.html',
	styleUrl: './create-update-pet.component.scss'
})
export class CreateUpdatePetComponent {
	isLoading = false;
	locale: any;

	activeTab = 'pet';
	emptyGuid = Guid.createEmpty().toString();

	petId: Guid = Guid.createEmpty();
	pet$!: Observable<PetModel>;
	petForm!: FormGroup;

	diaryNotes$!: Observable<DiaryNoteListItem[]>;

	constructor(
		private fb: FormBuilder,
		private router: Router,
		private petService: PetService,
		private diaryNoteService: DiaryNoteService,
		private storageService: StorageService,
		private enumService: EnumService,
		private route: ActivatedRoute,
		private toastService: ToastService,
		private datePipe: DatePipe,
		@Inject(L10N_LOCALE) locale: any
	) {
		this.locale = locale;
	}

	ngOnInit(): void {
		this.isLoading = true;
		setTimeout(() => {
			this.route.params.subscribe(params => {
				this.petId = params['petId'];
				if (this.petId) {
					this.initPet();
				}

				this.initializeForms();
				this.isLoading = false;
			});
		}, 1500);
	}

	initPet(): void {
		this.pet$ = this.petService.getPetById(this.petId);
		this.pet$.subscribe((pet: PetModel) => {
			this.fillFormWithData(pet);
			
			if (this.petId == Guid.createEmpty())
				return;

			this.loadDiaryNotes();
		});
	}

	loadDiaryNotes(): void {
		this.diaryNoteService.getAllForPet(this.petId).subscribe({
			next: (result: DiaryNoteListItem[]) => {
				this.diaryNotes$ = of(result);
			},
			complete: () => {
				this.isLoading = false;
			}
		});
	}

	onTabClick(tabName: string) {
        this.activeTab = tabName;
    }

	getPetTypeOptions() {
		return this.enumService.getEnumOptions(PetType);
	}

	initializeForms(): void {
		this.petForm = this.fb.group({
			petId: [null],
			ownerId: [null],
			nickName: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(50)]],
			petType: [PetType.Cat, [Validators.required]],
			birthDate: [new Date(), [Validators.required]],
			breed: [''],
			weight: [0, [Validators.required]],
			height: [0, [Validators.required]],
			characteristics: ['', [Validators.required, Validators.minLength(15), Validators.maxLength(200)]],
			illnesses: ['', [Validators.required, Validators.minLength(15), Validators.maxLength(200)]],
			preferences: ['', [Validators.required, Validators.minLength(15), Validators.maxLength(200)]],
		});
	}

	fillFormWithData(pet: PetModel): void {
		this.petForm.patchValue({
			petId: pet.petId,
			ownerId: this.storageService.getCurrentUserId(),
			nickName: pet.nickName,
			petType: pet.petType,
			birthDate: this.datePipe.transform(pet.birthDate, 'yyyy-MM-dd'),
			breed: pet.breed,
			weight: pet.weight,
			height: pet.height,
			characteristics: pet.characteristics,
			illnesses: pet.illnesses,
			preferences: pet.preferences
		});
	}

	applyPet(): void {
		if (this.petForm.valid) {
			this.petService.apply(this.petForm.value).subscribe({
				next: () => {
					this.router.navigate(['/pet-list']);
				},
				error: (error) => {
					if (error instanceof HttpErrorResponse) {
						const errorMessage = error.error instanceof ErrorEvent ? error.error.message : error.statusText;
						if (error.status === 400) {
							this.toastService.showErrorToast();
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
}
