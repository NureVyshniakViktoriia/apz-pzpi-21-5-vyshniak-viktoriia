import { Component, Inject, Input } from '@angular/core';
import { Observable, of } from 'rxjs';
import { DiaryNoteListItem } from '../../../core/models/diary-note/diary-note-list-item';
import { Guid } from 'guid-typescript';
import { L10N_LOCALE } from 'angular-l10n';
import { DiaryNoteService } from '../../../core/services/diary-note.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { CreateUpdateDiaryNoteComponent } from '../create-update-diary-note/create-update-diary-note.component';

@Component({
	selector: 'app-diary-note-list',
	templateUrl: './diary-note-list.component.html',
	styleUrl: './diary-note-list.component.scss'
})
export class DiaryNoteListComponent {
	@Input()
	petId!: Guid;

	locale: any;
	isLoading = false;

	diaryNotes$!: Observable<DiaryNoteListItem[]>;

	constructor(
		private diaryNoteService: DiaryNoteService,
		private modalService: NgbModal,
		@Inject(L10N_LOCALE) locale: any
	) {
		this.locale = locale;
	}

	ngOnInit(): void {
		this.loadDiaryNotes();
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

	openDiaryNoteModal(diaryNoteId: Guid): void {
		const modalRef = this.modalService.open(CreateUpdateDiaryNoteComponent, { size: 'md' });
		modalRef.componentInstance.onSaveCallback = () => this.loadDiaryNotes();
		modalRef.componentInstance.diaryNoteId = diaryNoteId;
		modalRef.componentInstance.petId = this.petId;
	}

	onDeleteNote(diaryNoteId: Guid) {
		this.diaryNoteService.delete(diaryNoteId).subscribe(
			() => {
				this.loadDiaryNotes();
			},
		);
	}

	onAddNote() {
		const modalRef = this.modalService.open(CreateUpdateDiaryNoteComponent, { size: 'md' });
		modalRef.componentInstance.onSaveCallback = () => this.loadDiaryNotes();
		modalRef.componentInstance.diaryNoteId = Guid.createEmpty();
		modalRef.componentInstance.petId = this.petId;
	}
}
