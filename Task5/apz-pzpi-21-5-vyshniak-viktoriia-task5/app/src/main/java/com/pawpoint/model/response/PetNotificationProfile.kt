package com.pawpoint.model.response

import com.google.gson.annotations.SerializedName

data class PetNotificationProfile(

    @SerializedName("petId")
    val petId: String,

    @SerializedName("nickName")
    val nickName: String,

    @SerializedName("petType")
    val petType: Int,

    @SerializedName("breed")
    val breed: String,

    @SerializedName("characteristics")
    val characteristics: String,

    @SerializedName("illnesses")
    val illnesses: String,

    @SerializedName("preferences")
    val preferences: String,

    @SerializedName("documents")
    val documents: List<DiaryNoteListItem>
)
