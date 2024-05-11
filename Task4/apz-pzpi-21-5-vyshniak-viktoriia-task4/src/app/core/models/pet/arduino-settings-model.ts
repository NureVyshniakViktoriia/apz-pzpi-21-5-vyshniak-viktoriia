import { Guid } from "guid-typescript";

export interface ArduinoSettingsModel {
	petId: Guid;
	petDeviceIpAddress: string;
}
