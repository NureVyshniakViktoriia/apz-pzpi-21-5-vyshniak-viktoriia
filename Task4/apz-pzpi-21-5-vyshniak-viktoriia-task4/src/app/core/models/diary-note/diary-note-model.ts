import { Guid } from "guid-typescript";

export interface DiaryNoteModel {
	diaryNoteId: Guid;
	petId: Guid;
	title: string;
	comment: string;
	createdOnUtc: Date;
	lastUpdatedOnUtc?: Date | null;
}
