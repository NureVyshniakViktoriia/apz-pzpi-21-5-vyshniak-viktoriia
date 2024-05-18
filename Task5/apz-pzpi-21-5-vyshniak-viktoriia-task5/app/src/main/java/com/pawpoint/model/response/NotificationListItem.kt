package com.pawpoint.model.response

import com.google.gson.annotations.SerializedName

data class NotificationListItem(

    @SerializedName("notificationId")
    val notificationId: String,

    @SerializedName("petId")
    val petId: String,

    @SerializedName("createdOnUtc")
    val createdOnUtc: String
)
