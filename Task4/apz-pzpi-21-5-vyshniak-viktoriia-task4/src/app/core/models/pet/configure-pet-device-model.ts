import { Guid } from "guid-typescript";
import { WifiSettingsModel } from "../shared/wifi-settings-model";

export interface ConfigurePetDeviceModel {
    arduinoSettingsId: Guid;
    wifiSettings: WifiSettingsModel;
}
