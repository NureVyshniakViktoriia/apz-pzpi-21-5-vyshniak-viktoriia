package com.pawpoint.model.response

import com.google.gson.annotations.SerializedName

data class InstitutionModel(

    @SerializedName("institutionId")
    val institutionId: Int,

    @SerializedName("ownerId")
    val ownerId: Int,

    @SerializedName("name")
    val name: String,

    @SerializedName("description")
    val description: String,

    @SerializedName("phoneNumber")
    val phoneNumber: String,

    @SerializedName("address")
    val address: String,

    @SerializedName("latitude")
    val latitude: Double,

    @SerializedName("longitude")
    val longitude: Double,

    @SerializedName("institutionType")
    val institutionType: Int,

    @SerializedName("logo")
    val logo: String,

    @SerializedName("rating")
    val rating: RatingModel?,

    @SerializedName("region")
    val region: Int,

    @SerializedName("websiteUrl")
    val websiteUrl: String,

    @SerializedName("facilities")
    val facilities: List<FacilityListItem>,

    @SerializedName("RFIDSettings")
    val rfidSettings: RFIDSettingsModel
)
