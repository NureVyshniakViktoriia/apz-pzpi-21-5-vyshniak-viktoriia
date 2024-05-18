package com.pawpoint.model.response

import com.google.gson.annotations.SerializedName

data class ArduinoSettingsModel (

    @SerializedName("PetId")
    val petId: String?,

    @SerializedName("PetDeviceIpAddress")
    val petDeviceIpAddress: String?
)
