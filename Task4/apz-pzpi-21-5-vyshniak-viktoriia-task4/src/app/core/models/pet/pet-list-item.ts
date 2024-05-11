import { Guid } from "guid-typescript";
import { PetType } from "../../enums/pet-type";
import { ArduinoSettingsModel } from "./arduino-settings-model";

export interface PetListItem {
	petId: Guid;
	nickName: string;
	petType: PetType;
	arduinoSettings: ArduinoSettingsModel,
	ownerLogin: string,
}
