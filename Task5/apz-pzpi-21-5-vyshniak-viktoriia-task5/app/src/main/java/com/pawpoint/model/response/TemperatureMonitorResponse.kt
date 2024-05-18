package com.pawpoint.model.response

import com.google.gson.annotations.SerializedName

data class TemperatureMonitorResponse(

    @SerializedName("Payload")
    val temperature: Double
)
