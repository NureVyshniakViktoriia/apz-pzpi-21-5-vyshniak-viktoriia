import { Guid } from "guid-typescript";

export interface DiaryNoteListItem {
	diaryNoteId: Guid;
	petId: Guid;
	title: string;
	createdOnUtc: Date; 
	lastUpdatedOnUtc: Date; 
}
