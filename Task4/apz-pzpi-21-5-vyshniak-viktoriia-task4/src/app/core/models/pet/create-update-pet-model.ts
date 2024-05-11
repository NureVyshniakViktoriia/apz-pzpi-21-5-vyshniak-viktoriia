import { Guid } from "guid-typescript";
import { PetType } from "../../enums/pet-type";

export interface CreateUpdatePetModel {
	petId?: Guid | null;
	ownerId: number;
	nickName: string;
	petType: PetType; 
	birthDate: Date;
	breed?: string;
	weight: number;
	height: number;
	characteristics: string;
	illnesses?: string;
	preferences: string;
	rfid: string;
}
 