package com.pawpoint.model.response

import com.google.gson.annotations.SerializedName

data class RatingModel(

    @SerializedName("institutionId")
    val institutionId: Int,

    @SerializedName("isSetByCurrentUser")
    val isSetByCurrentUser: Boolean,

    @SerializedName("mark")
    val mark: Double
)
