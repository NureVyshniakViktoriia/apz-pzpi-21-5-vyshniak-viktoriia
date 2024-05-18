package com.pawpoint.model.response

import com.google.gson.annotations.SerializedName

data class RFIDSettingsModel (

    @SerializedName("RFIDSettingsId")
    val rfidSettingsId: Int?,

    @SerializedName("RFIDReaderIpAddress")
    val rfidReaderIpAddress: String?,

    @SerializedName("InstitutionId")
    val institutionId: Int?
)
