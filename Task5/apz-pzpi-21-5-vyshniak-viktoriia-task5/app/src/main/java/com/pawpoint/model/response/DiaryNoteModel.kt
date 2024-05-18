package com.pawpoint.model.response

import com.google.gson.annotations.SerializedName

data class DiaryNoteModel(

    @SerializedName("diaryNoteId")
    val diaryNoteId: String,

    @SerializedName("petId")
    val petId: String,

    @SerializedName("title")
    val title: String,

    @SerializedName("comment")
    val comment: String,

    @SerializedName("createdOnUtc")
    val createdOnUtc: String,

    @SerializedName("lastUpdatedOnUtc")
    val lastUpdatedOnUtc: String
)
