import { Guid } from "guid-typescript";

export interface CreateUpdateDiaryNoteModel {
	diaryNoteId?: Guid | null;
	petId: Guid;
	title: string;
	comment: string;
}
