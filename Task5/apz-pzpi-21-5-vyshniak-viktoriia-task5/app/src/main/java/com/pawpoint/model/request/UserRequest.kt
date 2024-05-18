package com.pawpoint.model.request

data class UserRequest(

    var userId: Long?,

    var login: String,

    var password: String,

    var role: Int,

    var region: Int,

    var gender: Int,

    var email: String,
)
