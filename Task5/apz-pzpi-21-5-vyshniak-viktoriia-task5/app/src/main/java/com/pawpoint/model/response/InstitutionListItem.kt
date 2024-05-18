package com.pawpoint.model.response

import com.google.gson.annotations.SerializedName

data class InstitutionListItem(

    @SerializedName("institutionId")
    val institutionId: Long,

    @SerializedName("name")
    val name: String,

    @SerializedName("rating")
    val rating: RatingModel?,

    @SerializedName("weightedRating")
    val weightedRating: Double,

    @SerializedName("institutionType")
    val institutionType: Int,

    @SerializedName("region")
    val region: Int,

    @SerializedName("Logo")
    val logo: String?
)
