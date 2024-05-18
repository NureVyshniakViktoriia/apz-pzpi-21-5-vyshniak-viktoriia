package com.pawpoint.ui.activity

import android.content.Intent
import android.os.Bundle
import android.util.Log
import android.view.View
import androidx.activity.enableEdgeToEdge
import androidx.appcompat.app.AlertDialog
import androidx.appcompat.app.AppCompatActivity
import androidx.core.widget.doOnTextChanged
import com.auth0.jwt.JWT
import com.auth0.jwt.interfaces.DecodedJWT
import com.google.android.material.textfield.TextInputEditText
import com.google.android.material.textfield.TextInputLayout
import com.pawpoint.R
import com.pawpoint.model.request.LoginRequest
import com.pawpoint.model.response.LoginResponse
import com.pawpoint.network.ApiRepositoryImpl
import com.pawpoint.util.Validator
import retrofit2.Call
import retrofit2.Callback
import retrofit2.Response

class LoginPage : AppCompatActivity() {

    private val activity: AppCompatActivity = this@LoginPage
    private val apiRepository = ApiRepositoryImpl()
    private val validator = Validator()
    private lateinit var username: TextInputEditText
    private lateinit var password: TextInputEditText
    private lateinit var usernameContainer: TextInputLayout
    private lateinit var passwordContainer: TextInputLayout
    private lateinit var userId: String
    private lateinit var userRole: String

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        enableEdgeToEdge()
        setContentView(R.layout.activity_login_page)
        init()
    }

    private fun init() {
        username = findViewById(R.id.loginEditText)
        password = findViewById(R.id.passwordEditText)

        usernameContainer = findViewById(R.id.loginContainer)
        passwordContainer = findViewById(R.id.passwordContainer)

        username.let { u ->
            u.doOnTextChanged { _, _, _, _ ->
                usernameContainer.let { c -> c.helperText = validateUsername() }
            }
        }
        password.let { u ->
            u.doOnTextChanged { _, _, _, _ ->
                passwordContainer.let { c -> c.helperText = validatePassword() }
            }
        }
    }

    private fun validateUsername(): String {
        return validator.validateLogin(username.text.toString().trim())
    }

    private fun validatePassword(): String {
        return validator.validatePassword(password.text.toString().trim())
    }

    fun login(view: View) {
        val user = LoginRequest(username.text.toString().trim(), password.text.toString().trim())

        apiRepository.login(
            user,
            object :
                Callback<LoginResponse> {
                override fun onResponse(
                    call: Call<LoginResponse>,
                    response: Response<LoginResponse>
                ) {
                    if (response.isSuccessful) {
                        val tokens = response.body()
                        if (tokens != null) {
                            userId = parseToken(tokens)
                            userRole = parseTokenForRole(tokens)
                            if (userRole == "User") {
                                val intent = Intent(activity, MainUserPage::class.java)
                                intent.putExtra("userId", userId)
                                intent.putExtra("token", tokens.accessToken)
                                startActivity(intent)
                                activity.finish()
                            } else {
                                val intent = Intent(activity, MainAdminPage::class.java)
                                intent.putExtra("userId", userId)
                                intent.putExtra("token", tokens.accessToken)
                                startActivity(intent)
                                activity.finish()
                            }
                        } else {
                            AlertDialog.Builder(activity)
                                .setTitle("Sing in")
                                .setMessage("Something went wrong. Try again later.")
                                .setPositiveButton("Okay") { _, _ ->
                                    username.setText("")
                                    password.setText("")
                                }
                                .show()
                            Log.d("error", response.code().toString());
                        }
                    } else {
                        Log.d("Error status code", response.code().toString())
                        Log.d("Error message", response.message().toString())
                        AlertDialog.Builder(activity)
                            .setTitle("Sing in")
                            .setMessage("Something went wrong. Try again later.")
                            .setPositiveButton("Okay") { _, _ ->
                                username.setText("")
                                password.setText("")
                            }
                            .show()
                    }
                }

                override fun onFailure(call: Call<LoginResponse>, t: Throwable) {
                    Log.d("Error", t.message.toString())
                    AlertDialog.Builder(activity)
                        .setTitle("Sing in")
                        .setMessage("Something went wrong. Try again later.")
                        .setPositiveButton("Okay") { _, _ ->
                            username.setText("")
                            password.setText("")
                        }
                        .show()
                }
            })
    }

    fun registration(view: View) {
        val intent = Intent(activity, RegisterPage::class.java)
        startActivity(intent)
    }

    private fun parseToken(tokens: LoginResponse): String {
        try {
            val decodedJWT: DecodedJWT = JWT.decode(tokens.accessToken)

            val issuer = decodedJWT.issuer

            val id = decodedJWT.getClaim("id").asString()

            Log.d("Issuer", issuer)
            Log.d("CustomClaim", id)

            return id
        } catch (e: Exception) {
            Log.e("TokenParsingError", e.message ?: "Unknown error occurred")
        }
        return "";
    }

    private fun parseTokenForRole(tokens: LoginResponse): String {
        try {
            val decodedJWT: DecodedJWT = JWT.decode(tokens.accessToken)

            val issuer = decodedJWT.issuer

            val role = decodedJWT.getClaim("role").asString()

            Log.d("Issuer", issuer)
            Log.d("CustomClaim", role)

            return role
        } catch (e: Exception) {
            Log.e("TokenParsingError", e.message ?: "Unknown error occurred")
        }
        return "";
    }
}
