package com.pawpoint.ui.activity

import android.content.Intent
import android.os.Bundle
import android.util.Log
import android.view.View
import android.widget.ArrayAdapter
import android.widget.AutoCompleteTextView
import androidx.activity.enableEdgeToEdge
import androidx.appcompat.app.AlertDialog
import androidx.appcompat.app.AppCompatActivity
import androidx.core.widget.doOnTextChanged
import com.google.android.material.textfield.TextInputEditText
import com.google.android.material.textfield.TextInputLayout
import com.pawpoint.R
import com.pawpoint.model.enums.Gender
import com.pawpoint.model.enums.Region
import com.pawpoint.model.request.UserRequest
import com.pawpoint.network.ApiRepositoryImpl
import com.pawpoint.util.Validator
import retrofit2.Call
import retrofit2.Callback
import retrofit2.Response


class RegisterPage : AppCompatActivity() {

    private val activity: AppCompatActivity = this@RegisterPage
    private val apiRepository = ApiRepositoryImpl()
    private val validator = Validator()
    private lateinit var loginInput: TextInputEditText
    private lateinit var passwordInput: TextInputEditText
    private lateinit var genderInput: AutoCompleteTextView
    private lateinit var regionInput: AutoCompleteTextView
    private lateinit var emailInput: TextInputEditText
    private lateinit var loginContainer: TextInputLayout
    private lateinit var passwordContainer: TextInputLayout
    private lateinit var genderContainer: TextInputLayout
    private lateinit var regionContainer: TextInputLayout
    private lateinit var emailContainer: TextInputLayout

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        enableEdgeToEdge()
        setContentView(R.layout.activity_register_page)
        init()
    }

    private fun init() {
        loginInput = findViewById(R.id.loginEditText)
        passwordInput = findViewById(R.id.passwordEditText)
        genderInput = findViewById(R.id.genderText)
        regionInput = findViewById(R.id.regionText)
        emailInput = findViewById(R.id.emailEditText)

        loginContainer = findViewById(R.id.loginContainer)
        passwordContainer = findViewById(R.id.passwordContainer)
        genderContainer = findViewById(R.id.genderContainer)
        regionContainer = findViewById(R.id.regionContainer)
        emailContainer = findViewById(R.id.emailContainer)

        val genders = Gender.entries.map { g -> g.name }.toList()
        val regions = Region.entries.map { r ->
            r.name
                .replace("__", "-")
                .replace("_", " ")
        }.toList()

        val genderAdapter = ArrayAdapter(this, R.layout.dropdown_menu_popup_item, genders)
        val regionAdapter = ArrayAdapter(this, R.layout.dropdown_menu_popup_item, regions)

        genderInput.setAdapter(genderAdapter)
        regionInput.setAdapter(regionAdapter)

        validate()
    }

    private fun validate() {
        loginInput.let { u ->
            u.doOnTextChanged { _, _, _, _ ->
                loginContainer.let { c -> c.helperText = validateUsername() }
            }
        }
        passwordInput.let { u ->
            u.doOnTextChanged { _, _, _, _ ->
                passwordContainer.let { c -> c.helperText = validatePassword() }
            }
        }
        genderInput.let { u ->
            u.doOnTextChanged { _, _, _, _ ->
                genderContainer.let { c -> c.helperText = validateGender() }
            }
        }
        regionInput.let { u ->
            u.doOnTextChanged { _, _, _, _ ->
                regionContainer.let { c -> c.helperText = validateRegion() }
            }
        }
        emailInput.let { u ->
            u.doOnTextChanged { _, _, _, _ ->
                emailContainer.let { c -> c.helperText = validateEmail() }
            }
        }
    }

    private fun validateUsername(): String {
        return validator.validateLogin(loginInput.text.toString().trim())
    }

    private fun validatePassword(): String {
        return validator.validatePassword(passwordInput.text.toString().trim())
    }

    private fun validateGender(): String {
        return validator.validateGender(genderInput.text.toString().trim())
    }

    private fun validateRegion(): String {
        return validator.validateRegion(regionInput.text.toString().trim())
    }

    private fun validateEmail(): String {
        return validator.validateEmail(emailInput.text.toString().trim())
    }

    fun register(view: View) {
        val user = UserRequest(
            null,
            loginInput.text.toString().trim(),
            passwordInput.text.toString().trim(),
            1,
            getRegion(regionInput.text.toString().trim()),
            getGender(genderInput.text.toString().trim()),
            emailInput.text.toString().trim(),
        )

        Log.d("User", user.toString())

        apiRepository.register(user,
            object :
                Callback<Void> {
                override fun onResponse(
                    call: Call<Void>,
                    response: Response<Void>
                ) {
                    if (!response.isSuccessful) {
                        Log.d("Error1", response.code().toString())
                        AlertDialog.Builder(activity)
                            .setTitle("Sing up")
                            .setMessage("Something went wrong. Try again later.")
                            .setPositiveButton("Okay") { _, _ ->
                                val intent = Intent(activity, LoginPage::class.java)
                                startActivity(intent)
                                activity.finish()
                            }
                            .show()
                    } else {
                        AlertDialog.Builder(activity)
                            .setTitle("Sign up")
                            .setMessage("Registration was successful. You can try to log into your account.")
                            .setPositiveButton("Okay") { _, _ ->
                                val intent = Intent(activity, LoginPage::class.java)
                                startActivity(intent)
                                activity.finish()
                            }
                            .show()
                    }

                }

                override fun onFailure(call: Call<Void>, t: Throwable) {
                    Log.d("Error2", t.message.toString())
                    AlertDialog.Builder(activity)
                        .setTitle("Sing up")
                        .setMessage("Something went wrong. Try again later.")
                        .setPositiveButton("Okay") { _, _ ->
                            val intent = Intent(activity, LoginPage::class.java)
                            startActivity(intent)
                            activity.finish()
                        }
                        .show()
                }
            })
    }

    private fun getGender(gender: String): Int {
        return Gender.valueOf(gender).ordinal + 1
    }

    private fun getRegion(region: String): Int {
        return Region.valueOf(
            region
                .replace("-", "__")
                .replace(" ", "_")
        ).ordinal + 1
    }
}
