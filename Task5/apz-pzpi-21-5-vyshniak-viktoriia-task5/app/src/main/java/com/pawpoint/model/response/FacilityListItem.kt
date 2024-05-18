package com.pawpoint.model.response

import com.google.gson.annotations.SerializedName

data class FacilityListItem(

    @SerializedName("facilityId")
    val facilityId: Long,

    @SerializedName("name")
    val name: String,

    @SerializedName("isForInstitution")
    val isForInstitution: Boolean
)
