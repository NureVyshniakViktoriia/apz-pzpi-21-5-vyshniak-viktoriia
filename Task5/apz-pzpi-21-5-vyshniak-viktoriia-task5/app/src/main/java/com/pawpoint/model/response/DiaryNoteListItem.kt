package com.pawpoint.model.response

import com.google.gson.annotations.SerializedName

data class DiaryNoteListItem(

    @SerializedName("diaryNoteId")
    val diaryNoteId: String,

    @SerializedName("petId")
    val petId: String,

    @SerializedName("title")
    val title: String,

    @SerializedName("createdOnUtc")
    val createdOnUtc: String,

    @SerializedName("lastUpdatedOnUtc")
    val lastUpdateOnUtc: String
)
