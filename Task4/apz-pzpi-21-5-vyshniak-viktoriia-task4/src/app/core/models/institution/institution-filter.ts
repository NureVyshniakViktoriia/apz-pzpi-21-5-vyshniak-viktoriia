import { InstitutionType } from "../../enums/institution-type";
import { FilterBase } from "../shared/filter-base";

export interface InstitutionFilter extends FilterBase {
	searchQuery: string;
	type?: InstitutionType | null;
	sortByRatingAscending: boolean;
}
