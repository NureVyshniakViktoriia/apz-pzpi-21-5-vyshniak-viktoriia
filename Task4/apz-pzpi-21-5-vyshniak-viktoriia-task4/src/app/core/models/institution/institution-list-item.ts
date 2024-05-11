import { InstitutionType } from "../../enums/institution-type";
import { Region } from "../../enums/region";
import { RatingModel } from "./rating-model";

export interface InstitutionListItem {
	institutionId: number;
	name: string;
	rating: RatingModel;
	weightedRating: number;
	institutionType: InstitutionType;
	region: Region;
	logo: string;
}
