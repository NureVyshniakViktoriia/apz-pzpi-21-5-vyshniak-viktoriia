import { WifiSettingsModel } from "../shared/wifi-settings-model";

export interface ConfigureRfidReaderModel {
    rfidSettingsId: number;
    wifiSettings: WifiSettingsModel;
}