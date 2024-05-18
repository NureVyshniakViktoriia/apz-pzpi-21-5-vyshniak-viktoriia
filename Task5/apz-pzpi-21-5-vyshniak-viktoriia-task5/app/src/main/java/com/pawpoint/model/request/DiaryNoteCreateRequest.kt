package com.pawpoint.model.request

data class DiaryNoteCreateRequest(

    var petId: String,

    var title: String,

    var comment: String
)
