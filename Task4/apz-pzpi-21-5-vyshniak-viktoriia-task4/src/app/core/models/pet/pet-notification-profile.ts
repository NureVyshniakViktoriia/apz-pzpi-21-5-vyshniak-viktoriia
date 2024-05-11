import { Guid } from "guid-typescript";
import { PetType } from "../../enums/pet-type";
import { DiaryNoteListItem } from "../diary-note/diary-note-list-item";

export interface PetNotificationProfile {
	petId: Guid;
	nickName: string;
	petType: PetType;
	breed: string;
	characteristics: string;
	illnesses: string;
	preferences: string;
	documents: DiaryNoteListItem[];
}
