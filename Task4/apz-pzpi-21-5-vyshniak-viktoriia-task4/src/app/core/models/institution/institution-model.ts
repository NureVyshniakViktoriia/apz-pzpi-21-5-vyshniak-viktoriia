import { InstitutionType } from "../../enums/institution-type";
import { Region } from "../../enums/region";
import { FacilityListItem } from "../facility/facility-list-item";
import { RatingModel } from "./rating-model";

export interface InstitutionModel {
	institutionId: number;
	ownerId: number;
	name: string;
	description: string;
	phoneNumber: string;
	address: string;
	latitude: number;
	longitude: number;
	institutionType: InstitutionType; 
	logo: string;
	rating: RatingModel;
	region: Region;
	regionName: string;
	institutionTypeName: string;
	websiteUrl: string;
	facilities: FacilityListItem[];
}
