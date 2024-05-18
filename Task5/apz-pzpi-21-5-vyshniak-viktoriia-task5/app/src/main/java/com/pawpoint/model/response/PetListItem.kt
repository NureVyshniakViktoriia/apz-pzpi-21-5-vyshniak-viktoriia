package com.pawpoint.model.response

import com.google.gson.annotations.SerializedName

data class PetListItem(

    @SerializedName("petId")
    val petId: String,

    @SerializedName("nickName")
    val nickName: String,

    @SerializedName("petType")
    val petType: Int
)
