package com.pawpoint.model.response

import com.google.gson.annotations.SerializedName

data class AddressData(

    @SerializedName("address")
    val address: String,

    @SerializedName("city")
    val city: String,

    @SerializedName("state")
    val state: String,

    @SerializedName("zip")
    val zip: String,

    @SerializedName("country")
    val country: String
)
