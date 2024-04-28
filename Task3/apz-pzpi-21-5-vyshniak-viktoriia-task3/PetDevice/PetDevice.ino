#include <ESP8266WebServer.h>
#include <ESP8266WebServerSecure.h>
#include <Uri.h>
#include <ESP8266mDNS.h>
#include <ArduinoJson.h>
#include <ArduinoJson.hpp>
#include <ESP_EEPROM.h>
#include <ESP8266WiFi.h>
#include <NTC_TEMP_MODULE.h>
#include <iostream>
#include <vector>
#include <chrono>
#include <ctime>
#include <iomanip>

#pragma region Constants

String ssid = "";
String password = "";

String jsonString = "";

const int eepromSize = 512;
const int ATTEMPT_COUNT = 20;

unsigned long previousMillis = 0;
const unsigned long interval = 3600000;

BearSSL::ESP8266WebServerSecure server(443);
NTC_TEMP_CLASS ntc(0);

#pragma endregion

#pragma region Certificate and key data

static const char serverCert[] PROGMEM = R"EOF(
-----BEGIN CERTIFICATE-----
MIIC4jCCAkugAwIBAgIUYjIfHHXP6czjoxaesbjbUZrIhQkwDQYJKoZIhvcNAQEL
BQAwdjELMAkGA1UEBhMCUk8xCjAIBgNVBAgMAUIxEjAQBgNVBAcMCUJ1Y2hhcmVz
dDEbMBkGA1UECgwST25lVHJhbnNpc3RvciBbUk9dMRYwFAYDVQQLDA1PbmVUcmFu
c2lzdG9yMRIwEAYDVQQDDAlsb2NhbGhvc3QwHhcNMjMxMTEzMjA0ODM5WhcNMjQx
MTEyMjA0ODM5WjB2MQswCQYDVQQGEwJSTzEKMAgGA1UECAwBQjESMBAGA1UEBwwJ
QnVjaGFyZXN0MRswGQYDVQQKDBJPbmVUcmFuc2lzdG9yIFtST10xFjAUBgNVBAsM
DU9uZVRyYW5zaXN0b3IxEjAQBgNVBAMMCWxvY2FsaG9zdDCBnzANBgkqhkiG9w0B
AQEFAAOBjQAwgYkCgYEA1z/fyJEMQ3iTNt7BmEXe5i50ui7Wt40s2Fk+k2brg3eP
dy2uOx3QDmXhtOyrnEGvjA3z+qZyF3GO282r8RtTPS8/HEjGd2j1hm4c4k7sAfy8
N6tsK/UaPeiup8Q61bwa1sICUffJ3nrferAp+xKXkf1bdiUSm3WIS+wNpF6gWD8C
AwEAAaNtMGswHQYDVR0OBBYEFDVLw8u4SAuTAsUvELJ+yF7kTFODMB8GA1UdIwQY
MBaAFDVLw8u4SAuTAsUvELJ+yF7kTFODMA8GA1UdEwEB/wQFMAMBAf8wGAYDVR0R
BBEwD4INMTkyLjE2OC4wLjEwNTANBgkqhkiG9w0BAQsFAAOBgQAk9e9XrzLADZaP
KaM6KsOMJPHSrcJYRGPBvXT1Ril95vn1RbagwciXSBYar5eYtnGa72R6ZMOMJ1LX
llz7bODA2GSlmXzVa7GiB1qSmUg027Iq/n01taF9ycZ7IaWa5UMtjV3R5hj/9yTz
XANLKpk9BBY+dgmLQWcsIOlTOx9u4Q==
-----END CERTIFICATE-----
)EOF";

static const char serverKey[] PROGMEM =  R"EOF(
-----BEGIN PRIVATE KEY-----
MIICdgIBADANBgkqhkiG9w0BAQEFAASCAmAwggJcAgEAAoGBANc/38iRDEN4kzbe
wZhF3uYudLou1reNLNhZPpNm64N3j3ctrjsd0A5l4bTsq5xBr4wN8/qmchdxjtvN
q/EbUz0vPxxIxndo9YZuHOJO7AH8vDerbCv1Gj3orqfEOtW8GtbCAlH3yd5633qw
KfsSl5H9W3YlEpt1iEvsDaReoFg/AgMBAAECgYEAxWzShpVzWBcpf3REwLOlRB5r
9bCsQ+J6tVz1piEJ9qUcs0T0v0gS4oHTySwiIsmkh6LsmoM7tgDz9Orpv9U+O/n5
aKzAFsHYGnqB15WX1D+afFt9siyeI7xtUknbjwuPJgM3D+s4pqbwK3OVjxmgtBVS
qRW7Ozdk13J6T3kat5ECQQD/28a9q3agMwP1R67CD9TprC32jbrFIB6VSRfeHtVN
X3d6fjuob54LTvYUQ9NRsMYL2Z72uiOv5IPrCdOjuMFbAkEA115ZOOxctSyFETr/
CEr1S1pl02M+SMD1XXXNZnjXSYaUyT6nhv8R8HfZWlEb/nfsObVuPbf9ckj+8AgX
aaC17QJATH7e+pIrE1ZtVoYp/6dQFtT9oPta3djui9+gnS5Gw61JQXxARempLORc
uk5U/dWGQMC9wDKCvPHrZvg3KHnyhQJAfJcXNIgAJctIe6xOh2SdvObhy0U5bes6
xsN0fNhhayaRAk7dqYn7vcoHkmkjsbV8kfH0gX3j/+5clU2JBdE0DQJAfaKEFvAr
oHoZbSREE+Y6b9nbohO26EW1dFdtrrdIqOCNBJ8qvgYNkXf5DSvPlK/254g57p/Q
JUOWVQM0mS/PnQ==
-----END PRIVATE KEY-----
)EOF";

#pragma endregion

#pragma region Types

enum CommandType {
  Configure,
  GetCurrentTemperature,
  GetAverageTemperature,
  GetCurrentLocation
};

class MonitorData {
private:
    int day;
    std::vector<double> temperatures;

public:
    MonitorData() {
      day = getTodaysDay();
      addTemperature(ntc.GetTemp());
    }

    void addTemperature(double temp) {
        temperatures.push_back(temp);
    }

    void updateDay(int newDay) {
        day = newDay;
        temperatures.clear();
    }

    int getDay() const {
        return day;
    }

    std::vector<double>& getTemperatures() {
        return temperatures;
    }

    double getAverageTemperature() const {
        double sum = 0;

        for (double temperature : temperatures) {
            sum += temperature;
        }

        double average = sum / temperatures.size();
        return std::round(average * 100) / 100.0;
    }

    int getTodaysDay() {
        auto now = std::chrono::system_clock::now();

        std::time_t now_time = std::chrono::system_clock::to_time_t(now);
        std::tm* now_tm = std::localtime(&now_time);

        int day = now_tm->tm_mday;

        return day;
    }

    void checkDate() {
      int todaysDay = getTodaysDay();

      if (day != todaysDay) {
        previousMillis = 0;
        day = todaysDay;
        addTemperature(ntc.GetTemp());
      }
    }
};

class BaseArduinoResponse {
public: 
  BaseArduinoResponse(
    bool isSuccess = false,
    String errorCode = "") {
      this->isSuccess = isSuccess;
      this->errorCode = errorCode;
  }

protected: 
  bool isSuccess;
  String errorCode;
};

class ArduinoResponse : public BaseArduinoResponse {
public:
  ArduinoResponse(
    bool isSuccess = false, 
    String errorCode = "", 
    String payload = "")
    : BaseArduinoResponse(isSuccess, errorCode) {
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
  String payload;
};

class LocationResponse : public BaseArduinoResponse {
public:
  LocationResponse(
    bool isSuccess = false,
    String errorCode = "",
    double latitude = 0,
    double longtitude = 0)
    : BaseArduinoResponse(isSuccess, errorCode) {
      this->latitude = latitude;
      this->longtitude = longtitude;
  }

  String toJson() {
      StaticJsonDocument<256> doc;
      doc["IsSuccess"] = this->isSuccess;
      doc["ErrorCode"] = this->errorCode;
      doc["Latitutde"] = this->latitude;
      doc["Longtitude"] = this->longtitude;

      String jsonString;
      serializeJson(doc, jsonString);

      return jsonString;
  }

private:
  double latitude;
  double longtitude;
};

#pragma endregion

MonitorData md;

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

  ssid = esid;
  password = epass;

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

  configTime(3 * 3600, 0, "pool.ntp.org", "time.nist.gov");
  server.getServer().setRSACert(new BearSSL::X509List(serverCert), new BearSSL::PrivateKey(serverKey));

  server.on("/data", HTTP_POST, handleHttp);
  server.begin();
  Serial.println("Server listening");
}

void handleHttp() {
  String jsonStr = server.arg("plain");
  StaticJsonDocument<256> doc;
  parseJsonData(doc, jsonStr);

  CommandType command = (CommandType)doc["CommandType"];
  String locale = doc["Locale"];
  String jsonResponse;
  switch(command) {
      case GetCurrentTemperature:
        jsonResponse = getCurrentTemperature(locale);
        break;
      case GetAverageTemperature:
        jsonResponse = getAverageTemperature(locale);
        break;
      case GetCurrentLocation:
        jsonResponse = getCurrentLocation();
        break;
      default:
        ArduinoResponse response(false, "UNKOWN_COMMAND", "");
        jsonResponse = response.toJson();
        break;
    }

  server.send(200, "application/json", jsonResponse);
}

const double CELSIUS_TO_FAHRENHEIT_SCALE = 9.0 / 5.0;
const double FAHRENHEIT_FREEZING_POINT = 32.0;

double celsiusToFahrenheit(double celsius) {
    double tempInFahrenheit = (celsius * CELSIUS_TO_FAHRENHEIT_SCALE) + FAHRENHEIT_FREEZING_POINT;
    return std::round(tempInFahrenheit * 100) / 100.0;
}

#pragma endregion

#pragma region Commands

void configurePetDevice(ArduinoJson::JsonDocument & doc) {
  String newSsid = doc["WifiSettings"]["Ssid"];
  String newPassword = doc["WifiSettings"]["Password"];

  EEPROM.begin(eepromSize);
  for (int i = 0; i < 64; i++) {
    EEPROM.write(i, 0);
  }

  Serial.println("writing eeprom ssid:");
  writeInMemory(0, newSsid);

  Serial.println("writing eeprom pass:");
  writeInMemory(32, newPassword);

  EEPROM.commit();
  EEPROM.end();

  ESP.restart();
}

String getCurrentTemperature(String locale) {
  double currentTemperature = ntc.GetTemp();
  if (locale == "en-US") {
    currentTemperature = celsiusToFahrenheit(currentTemperature);
  }

  ArduinoResponse response(
    true, 
    "", 
    String(currentTemperature)
  );

  return response.toJson();
}

String getAverageTemperature(String locale) {
  md.checkDate();
  
  double averageTemperature = md.getAverageTemperature();
  if (locale == "en-US") {
    averageTemperature = celsiusToFahrenheit(averageTemperature);
  }

  ArduinoResponse response(
    true, 
    "", 
    String(averageTemperature)
  );

  return response.toJson();
}

String getCurrentLocation() {
  LocationResponse response(
    true,
    "",
    30.31,
    50.27
  );

  return response.toJson();
}

#pragma endregion

void setup() {
  Serial.begin(9600);
  loadSavedConfig();
  connectToWiFi();
  Serial.println("Ended setup...");
}

void loop() {
  if (WiFi.status() == WL_CONNECTED) {
    server.handleClient();
    MDNS.update();
  }

  while (Serial.available()) {
    jsonString = Serial.readStringUntil('\n');
    Serial.println(jsonString);
    StaticJsonDocument<256> doc;
    parseJsonData(doc, jsonString);

    CommandType command = (CommandType)doc["CommandType"];
    switch(command) {
      case Configure:
        configurePetDevice(doc);
        break;
      default:
        ArduinoResponse response(false, "UNKOWN_COMMAND", "");
        String jsonResponse = response.toJson();
        Serial.println(jsonResponse);
        break;
    }

    jsonString = "";
  }
}

