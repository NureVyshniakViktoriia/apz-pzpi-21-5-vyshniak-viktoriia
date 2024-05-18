package com.pawpoint.model.response

import com.google.gson.annotations.SerializedName

data class PetModel(

    @SerializedName("petId")
    val petId: String,

    @SerializedName("ownerId")
    val ownerId: Int,

    @SerializedName("nickName")
    val nickName: String,

    @SerializedName("petType")
    val petType: Int,

    @SerializedName("birthDate")
    val birthDate: String,

    @SerializedName("breed")
    val breed: String,

    @SerializedName("weight")
    val weight: Double,

    @SerializedName("height")
    val height: Double,

    @SerializedName("characteristics")
    val characteristics: String,

    @SerializedName("illnesses")
    val illnesses: String,

    @SerializedName("preferences")
    val preferences: String,

    @SerializedName("RFID")
    val rfid: String,

    @SerializedName("healthRecords")
    val healthRecords: List<HealthRecordModel>,

    @SerializedName("diaryNotes")
    val diaryNotes: List<DiaryNoteListItem>,

    @SerializedName("arduinoSettings")
    val arduinoSettings: ArduinoSettingsModel?
)
