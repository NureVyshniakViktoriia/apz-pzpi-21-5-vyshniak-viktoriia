package com.pawpoint.model.response

import com.google.gson.annotations.SerializedName

data class UserResponse(

    @SerializedName("userId")
    var userId: Long,

    @SerializedName("region")
    var region: Int,

    @SerializedName("gender")
    var gender: Int,

    @SerializedName("email")
    var email: String,

    @SerializedName("login")
    var login: String,

    @SerializedName("registeredOnUtc")
    var registeredOnUtc: String,

    @SerializedName("role")
    var role: Int
)
