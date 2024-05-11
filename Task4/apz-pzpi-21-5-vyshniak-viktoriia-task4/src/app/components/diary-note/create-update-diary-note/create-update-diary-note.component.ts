import { Component, Inject, Input, inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { L10N_LOCALE } from 'angular-l10n';
import { DiaryNoteService } from '../../../core/services/diary-note.service';
import { Guid } from 'guid-typescript';
import { UploadDocumentFileModel } from '../../../core/models/diary-note/upload-document-model';
import { ImageService } from '../../../core/services/image.service';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { ToastService } from '../../../core/services/toast.service';

@Component({
	selector: 'app-create-update-diary-note',
	templateUrl: './create-update-diary-note.component.html',
	styleUrl: './create-update-diary-note.component.scss'
})
export class CreateUpdateDiaryNoteComponent {
	@Input()
	onSaveCallback!: () => void;
	@Input()
	diaryNoteId!: Guid;
	@Input()
	petId!: Guid;

	emptyGuid = Guid.createEmpty().toString();

	diaryNoteForm!: FormGroup;
	documentForm!: FormGroup;

	activeModal = inject(NgbActiveModal);
	locale: any;

	constructor(
		private fb: FormBuilder,
		private diaryNoteService: DiaryNoteService,
		private toastService: ToastService,
		private imageService: ImageService,
		@Inject(L10N_LOCALE) locale: any
	) {
		this.locale = locale;
	}

	ngOnInit(): void {
		this.initDiaryNote();
		this.initializeForms();
	}

	initializeForms(): void {
		this.diaryNoteForm = this.fb.group({
			diaryNoteId: [this.diaryNoteId],
			title: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(50)]],
			comment: ['', [Validators.required, Validators.minLength(15), Validators.maxLength(200)]],
			petId: this.petId,
		});

		if (this.diaryNoteId.toString() == this.emptyGuid)
			return;

		this.documentForm = this.fb.group({
			document: [null],
			diaryNoteId: this.diaryNoteId
		});
	}

	initDiaryNote() {
		this.diaryNoteService.getNoteById(this.diaryNoteId)
			.subscribe(note => {
				this.diaryNoteForm.patchValue(note);
			});
	}

	applyDiaryNote() {
		if (this.diaryNoteForm.valid) {
			this.diaryNoteForm.value.petId = this.petId;
			this.diaryNoteService.apply(this.diaryNoteForm.value).subscribe({
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

	onDocumentFileChange(event: any): void {
		this.documentForm.patchValue({ document: event.target.files[0] });
	}

	uploadDocument(): void {
		const uploadModel: UploadDocumentFileModel = {
			diaryNoteId: this.documentForm.value.diaryNoteId,
			file: this.documentForm.value.document
		};

		this.diaryNoteService.uploadDocument(uploadModel).subscribe({
			error: () => {
				this.toastService.showErrorToast();
			},
			complete: () => {
				this.toastService.showSuccessToast();
			}
		});
	}

	downloadDocument(): void {
		this.diaryNoteService.downloadDocument(this.diaryNoteId)
			.subscribe((result) => 
				this.imageService.downloadFile(result, `Document_${this.diaryNoteId}`)
			);
	}
}
