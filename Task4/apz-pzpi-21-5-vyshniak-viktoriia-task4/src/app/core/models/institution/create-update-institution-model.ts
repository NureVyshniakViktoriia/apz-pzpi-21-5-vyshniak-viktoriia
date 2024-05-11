import { InstitutionType } from "../../enums/institution-type";
import { Region } from "../../enums/region";

export interface CreateUpdateInstitutionModel {
	institutionId?: number | null;
	ownerId: number;
	name: string;
	description: string;
	phoneNumber: string;
	address: string;
	websiteUrl?: string | null;
	region: Region; 
	institutionType: InstitutionType
}
