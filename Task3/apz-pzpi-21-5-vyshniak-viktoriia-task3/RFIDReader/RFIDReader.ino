#include <MFRC522.h>
#include <ESP8266HTTPClient.h>
#include <WiFiClientSecureBearSSL.h>
#include <ESP8266WebServerSecure.h>
#include <ArduinoJson.h>
#include <ArduinoJson.hpp>
#include <Wire.h>
#include <ESP8266WiFi.h>
#include <ESP8266mDNS.h>
#include <ESP8266WebServer.h>
#include <ESP_EEPROM.h>
#include <SPI.h>

#define RST_PIN D3
#define SS_PIN D4

MFRC522 rfid(SS_PIN, RST_PIN);

String ssid = "";
String password = "";
String callbackUrl = "";
int userId = 1;

const int eepromSize = 512;
const int ATTEMPT_COUNT = 20;

#pragma region Types

enum CommandType {
  ConfigureRFIDReader = 4
};

class Notification {
public:
  Notification(
    int userId = 0,
    String petRfid = ""
  ) {
    this->userId = userId;
    this->petRfid = petRfid;
  }

  String toJson() {
    StaticJsonDocument<256> doc;
    doc["UserId"] = this->userId;
    doc["PetRFID"] = this->petRfid;

    String jsonString;
    serializeJson(doc, jsonString);

    return jsonString;
  }

private:
  int userId;
  String petRfid;
};

class ArduinoResponse {
public:
  ArduinoResponse(
    bool isSuccess = false, 
    String errorCode = "", 
    String payload = "") {
      this->isSuccess = isSuccess;
      this->errorCode = errorCode;
      this->payload = payload;
  }

  String toJson() {
      StaticJsonDocument<256> doc;
      doc["IsSuccess"] = this->isSuccess;
      doc["ErrorCode"] = this->errorCode;
      doc["Payload"] = this->payload;

      String jsonString;
      serializeJson(doc, jsonString);

      return jsonString;
  }

private:
    bool isSuccess;
    String errorCode;
    String payload;
};

#pragma endregion

#pragma region Helpers

void parseJsonData(ArduinoJson::JsonDocument & doc, String & jsonString) {
  DeserializationError error = deserializeJson(doc, jsonString);

  if (error) {
    Serial.print("JSON parsing failed: ");
    Serial.println(error.c_str());
    return;
  }
}

String readFromMemory(int start, int end) {
  String data = "";
  for (int i = start; i < end; ++i) {
    char readChar = char(EEPROM.read(i));
    if (readChar == 0) {
      break;
    }

    data += readChar;
  }

  return data;
}

void writeInMemory(int start, String data) {
  Serial.println("writing eeprom:");
  for (int i = 0; i < data.length(); i++)
  {
    EEPROM.write(start + i, data[i]);
    Serial.print("Wrote: ");
    Serial.println(data[i]);
  }
}

void loadSavedConfig() {
  EEPROM.begin(eepromSize);

  String esid = readFromMemory(0, 32);
  Serial.println("Stored SSID: " + esid);

  String epass = readFromMemory(32, 64);
  Serial.println("Stored password: " + epass);

  String eCallbackURL = readFromMemory(64, 128);
  Serial.println("Stored callback url: " + eCallbackURL);

  String eUserId = readFromMemory(128, 160);
  Serial.println("Stored user id: " + eUserId);

  ssid = esid;
  password = epass;
  callbackUrl = eCallbackURL;
  userId = eUserId.toInt();

  EEPROM.end();
}

void connectToWiFi() {
  Serial.println("Connecting to WiFi");

  const char* wifi = ssid.c_str();
  const char* pass = password.c_str();
  
  WiFi.begin(wifi, pass);

  int attemptCount = 0;
  while (WiFi.status() != WL_CONNECTED 
    && attemptCount < ATTEMPT_COUNT) 
  {
    delay(1000);
    attemptCount++;
  }

  String jsonResponse;
  if (WiFi.status() != WL_CONNECTED) {
    ArduinoResponse response(false, "INVALID_WIFI_CREDS", "");
    jsonResponse = response.toJson();
  } else {
    ArduinoResponse response(true, "", WiFi.localIP().toString());
    jsonResponse = response.toJson();
  }

  Serial.println(jsonResponse);
}

String getDomainFromCallbackUrl() {
  String ipAddress = "";
  int colonIndex = callbackUrl.indexOf(':');
  
  if (colonIndex != -1) {
    ipAddress = callbackUrl.substring(colonIndex + 3, callbackUrl.indexOf(':', colonIndex + 3));
    Serial.print("IP address: ");
    Serial.println(ipAddress);
  } else {
    Serial.println("Invalid URL format");
  }

  return ipAddress;
}

String readRFID() {
  if (!rfid.PICC_IsNewCardPresent()) {
    return "";
  }

  if (!rfid.PICC_ReadCardSerial()) {
    return "";
  }

  String tag = "";
  for (int i = 0; i < rfid.uid.size; i++) {
    tag += rfid.uid.uidByte[i];
  }

  rfid.PICC_HaltA();
  rfid.PCD_StopCrypto1();

  return tag;
}

void processTag() {
  String tag = readRFID();
  if (tag == "") {
    return;
  }

  WiFiClientSecure client;
  String serverDomain = getDomainFromCallbackUrl();
  client.setInsecure();
  
  if (client.connect(serverDomain, 7114)){
    Serial.println("Connected to server");
  } else {
    Serial.println("ERROR connection to server");
  }

  HTTPClient https;
    
  https.begin(client, callbackUrl);
  https.addHeader("Content-Type", "application/json");

  Notification notification(userId, tag);
  String data = notification.toJson();

  int httpsResponseCode = https.POST(data);
  Serial.print("HTTP status code = ");
  Serial.println(httpsResponseCode);   
    
  https.end();
}

#pragma endregion

#pragma region Commands

void configureRFIDReader(ArduinoJson::JsonDocument & doc) {
  String newSsid = doc["WifiSettings"]["Ssid"];
  String newPassword = doc["WifiSettings"]["Password"];
  String newCallbackUrl = doc["WifiSettings"]["CallbackUrl"];
  String newUserId = doc["UserId"];

  EEPROM.begin(eepromSize);
  for (int i = 0; i < 160; i++) {
    EEPROM.write(i, 0);
  }

  Serial.println("writing eeprom ssid:");
  writeInMemory(0, newSsid);

  Serial.println("writing eeprom pass:");
  writeInMemory(32, newPassword);

  Serial.println("writing eeprom callback url:");
  writeInMemory(64, newCallbackUrl);

  Serial.println("writing eeprom user id:");
  writeInMemory(128, newUserId);

  EEPROM.commit();
  EEPROM.end();

  ESP.restart();
}

#pragma endregion

void setup() {
  Serial.begin(9600);
  Serial.println("Setup started.");
  loadSavedConfig();
  connectToWiFi();
  SPI.begin();
  rfid.PCD_Init();
  Serial.println("Setup finished.");
}

void loop() {
  if (Serial.available()) {
      String jsonString = Serial.readStringUntil('\n');
      StaticJsonDocument<256> doc;
      parseJsonData(doc, jsonString);

      CommandType command = (CommandType)doc["CommandType"];
      switch(command) {
        case ConfigureRFIDReader:
          configureRFIDReader(doc);
          break;
        default:
          break;
      }
  }
  
  if (WiFi.status() == WL_CONNECTED) {
      MDNS.update();
      processTag();
  }
}
