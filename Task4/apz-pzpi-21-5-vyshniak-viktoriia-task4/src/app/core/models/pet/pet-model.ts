import { Guid } from 'guid-typescript';
import { PetType } from '../../enums/pet-type';
import { DiaryNoteListItem } from '../diary-note/diary-note-list-item';
import { ArduinoSettingsModel } from './arduino-settings-model';

export interface PetModel {
	petId: Guid;
	ownerId: number;
	nickName: string;
	petType: PetType;
	birthDate: string;
	breed: string;
	weight: number;
	height: number;
	characteristics: string;
	illnesses: string;
	preferences: string;
	rfid: string;
	diaryNotes: DiaryNoteListItem[];
	arduinoSettings: ArduinoSettingsModel
}
