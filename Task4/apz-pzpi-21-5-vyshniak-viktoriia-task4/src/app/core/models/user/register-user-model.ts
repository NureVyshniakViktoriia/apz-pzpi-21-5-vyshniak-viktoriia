import { Gender } from "../../enums/gender";
import { Region } from "../../enums/region";

export interface RegisterUserModel {
	login: string,
	password: string,
	region: Region,
	gender: Gender,
	email: string
}
