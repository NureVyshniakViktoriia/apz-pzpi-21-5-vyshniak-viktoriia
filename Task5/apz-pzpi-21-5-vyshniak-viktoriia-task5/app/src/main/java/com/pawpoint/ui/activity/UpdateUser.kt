package com.pawpoint.ui.activity

import android.content.Intent
import android.os.Bundle
import android.widget.ArrayAdapter
import android.widget.AutoCompleteTextView
import android.widget.Button
import android.widget.Toast
import androidx.activity.enableEdgeToEdge
import androidx.appcompat.app.AppCompatActivity
import androidx.core.widget.doOnTextChanged
import com.google.android.material.textfield.TextInputEditText
import com.google.android.material.textfield.TextInputLayout
import com.pawpoint.R
import com.pawpoint.model.enums.Gender
import com.pawpoint.model.enums.Region
import com.pawpoint.model.request.UpdateUserRequest
import com.pawpoint.model.response.UserResponse
import com.pawpoint.network.ApiRepositoryImpl
import com.pawpoint.util.Validator
import retrofit2.Call
import retrofit2.Callback
import retrofit2.Response

class UpdateUser : AppCompatActivity() {

    private val activity: AppCompatActivity = this@UpdateUser
    private lateinit var token: String
    private lateinit var userId: String

    private val apiRepository = ApiRepositoryImpl()
    private val validator = Validator()

    private lateinit var genderInput: AutoCompleteTextView
    private lateinit var regionInput: AutoCompleteTextView
    private lateinit var emailInput: TextInputEditText
    private lateinit var genderContainer: TextInputLayout
    private lateinit var regionContainer: TextInputLayout
    private lateinit var emailContainer: TextInputLayout
    private lateinit var updateButton: Button
    private var profile: Int = 0


    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        enableEdgeToEdge()
        setContentView(R.layout.activity_update_user)
        init()
    }

    private fun init() {
        if (MainUserPage.getUserId() == null) {
            userId = MainAdminPage.getUserId().toString()
            token = MainAdminPage.getToken().toString()
            profile = 1
        } else {
            userId = MainUserPage.getUserId().toString()
            token = MainUserPage.getToken().toString()
            profile = 0
        }

        genderInput = findViewById(R.id.genderText)
        regionInput = findViewById(R.id.regionText)
        emailInput = findViewById(R.id.emailEditText)

        genderContainer = findViewById(R.id.genderContainer)
        regionContainer = findViewById(R.id.regionContainer)
        emailContainer = findViewById(R.id.emailContainer)

        updateButton = findViewById(R.id.update)

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

        updateButton.let { s ->
            s.setOnClickListener {
                update()
            }
        }

        getUser()
        validate()
    }

    private fun getUser() {
        apiRepository.getUser("Bearer " + token, userId.toInt(), object :
            Callback<UserResponse> {
            override fun onResponse(
                call: Call<UserResponse>,
                response: Response<UserResponse>
            ) {
                if (response.isSuccessful) {
                    val user = response.body()
                    if (user != null) {
                        fillData(user)
                    } else {
                        Toast.makeText(activity, "Something went wrong!", Toast.LENGTH_LONG)
                            .show()
                    }
                } else {
                    Toast.makeText(activity, "Something went wrong!", Toast.LENGTH_LONG)
                        .show()
                }
            }

            override fun onFailure(call: Call<UserResponse>, t: Throwable) {
                Toast.makeText(activity, "Something went wrong!", Toast.LENGTH_LONG).show()
            }
        })
    }

    fun fillData(user: UserResponse) {

        emailInput.setText(user.email)
        genderInput.setText(Gender.entries[user.gender - 1].name)
        regionInput.setText(
            Region.entries[user.region - 1].name
                .replace("__", "-")
                .replace("_", " ")
        )
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
    }

    private fun validate() {
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

    private fun validateGender(): String {
        return validator.validateGender(genderInput.text.toString().trim())
    }

    private fun validateRegion(): String {
        return validator.validateRegion(regionInput.text.toString().trim())
    }

    private fun validateEmail(): String {
        return validator.validateEmail(emailInput.text.toString().trim())
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

    private fun update() {
        val user = UpdateUserRequest(
            userId.toLong(),
            emailInput.text.toString().trim(),
            getGender(genderInput.text.toString().trim()),
            getRegion(regionInput.text.toString().trim()),
        )

        apiRepository.updateUser("Bearer " + token, user,
            object :
                Callback<Void> {
                override fun onResponse(
                    call: Call<Void>,
                    response: Response<Void>
                ) {
                    if (!response.isSuccessful) {
                        Toast.makeText(
                            activity,
                            "Something went wrong during updating!",
                            Toast.LENGTH_LONG
                        ).show()
                    } else {
                        Toast.makeText(activity, "User is updated!", Toast.LENGTH_LONG)
                            .show()
                        val intent1 = if (profile == 0) {
                            Intent(activity, MainUserPage::class.java)
                        } else {
                            Intent(activity, MainAdminPage::class.java)
                        }
                        intent1.putExtra("userId", userId)
                        intent1.putExtra("token", token)
                        startActivity(intent1)
                        activity.finish()
                    }

                }

                override fun onFailure(call: Call<Void>, t: Throwable) {
                    Toast.makeText(
                        activity,
                        "Something went wrong during updating!",
                        Toast.LENGTH_LONG
                    ).show()
                }
            })

    }
}
