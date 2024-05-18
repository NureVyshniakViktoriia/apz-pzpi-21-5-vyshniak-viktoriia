package com.pawpoint.model.response

import com.google.gson.annotations.SerializedName

data class HealthRecordModel(

    @SerializedName("healthRecordId")
    val healthRecordId: String,

    @SerializedName("temperature")
    val temperature: Double,

    @SerializedName("createdOnUtc")
    val createdOnUtc: String
)
