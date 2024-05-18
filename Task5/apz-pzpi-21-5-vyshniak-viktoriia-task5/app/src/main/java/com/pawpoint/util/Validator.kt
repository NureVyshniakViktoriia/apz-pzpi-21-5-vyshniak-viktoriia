package com.pawpoint.util

class Validator {
    fun validateEmail(email: String): String {
        email.trim()
        if (email == "") {
            return "Required"
        }
        if (email.length < 5) {
            return "Invalid email"
        }
        val pattern = "^[A-Za-z0-9+_.-]+@[A-Za-z0-9.-]+\$"
        val isMatch = Regex(pattern).matches(email)
        return if (isMatch) {
            ""
        } else {
            "Invalid email"
        }
    }

    fun validateLogin(login: String): String {
        login.trim()
        if (login == "") {
            return "Required"
        }
        val pattern = "^[a-zA-Z0-9_-]{4,32}\$"
        val isMatch = Regex(pattern).matches(login)
        return if (isMatch) {
            ""
        } else {
            "Invalid username"
        }
    }

    fun validatePassword(password: String): String {
        password.trim()
        if (password == "") {
            return "Required"
        }
        val pattern = "^[a-zA-Z0-9!@#\$%^&*-_]{4,32}\$"
        val isMatch = Regex(pattern).matches(password)
        return if (isMatch) {
            ""
        } else {
            "Invalid password"
        }
    }

    fun validateGender(gender: String): String {
        return if (gender.isNotEmpty()) "" else "Invalid gender"
    }

    fun validateRegion(region: String): String {
        return if (region.isNotEmpty()) "" else "Invalid region"
    }
}
