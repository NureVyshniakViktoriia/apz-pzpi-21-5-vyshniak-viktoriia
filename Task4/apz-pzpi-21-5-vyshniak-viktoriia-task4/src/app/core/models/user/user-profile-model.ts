import { Gender } from "../../enums/gender";
import { Region } from "../../enums/region";
import { Role } from "../../enums/role";

export interface UserProfileModel {
	userId: number;
	email: string;
	login: string;
	role: Role;
	gender: Gender,
	region: Region,
	registeredOnUtc: Date
}
