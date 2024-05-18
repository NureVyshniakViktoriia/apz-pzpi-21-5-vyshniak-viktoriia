package com.pawpoint.model.response

import com.google.gson.annotations.SerializedName

data class NotificationModel(

    @SerializedName("notificationId")
    val notificationId: String,

    @SerializedName("createdOnUtc")
    val createdOnUtc: String,

    @SerializedName("petProfile")
    val petProfile: PetNotificationProfile,
)
