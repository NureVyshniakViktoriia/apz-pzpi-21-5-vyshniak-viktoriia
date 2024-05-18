package com.pawpoint.model.request

data class UpdateUserRequest (

    var userId: Long,

    var email: String,

    var gender: Int,

    var region: Int
)
