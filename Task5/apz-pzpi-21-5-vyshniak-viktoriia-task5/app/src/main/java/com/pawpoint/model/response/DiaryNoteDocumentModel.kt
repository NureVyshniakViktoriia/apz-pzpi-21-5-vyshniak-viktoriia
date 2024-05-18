package com.pawpoint.model.response

import com.google.gson.annotations.SerializedName
import java.io.ByteArrayInputStream

data class DiaryNoteDocumentModel (

    @SerializedName("Title")
    val title: String?,

    @SerializedName("Document")
    val document: ByteArrayInputStream?
)
