package com.pawpoint.model.response

import com.google.gson.annotations.SerializedName

data class GPSTrackerResponse(

    @SerializedName("latitutde")
    val latitutde: Double,

    @SerializedName("longtitude")
    val longtitude: Double,

    @SerializedName("addressData")
    val addressData: AddressData
)
