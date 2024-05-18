package com.pawpoint.ui.activity

import android.content.Intent
import android.content.res.Configuration
import android.os.Bundle
import android.util.Log
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.ImageButton
import android.widget.TextView
import android.widget.Toast
import androidx.fragment.app.Fragment
import com.pawpoint.R
import com.pawpoint.model.enums.Region
import com.pawpoint.model.response.UserResponse
import com.pawpoint.network.ApiRepositoryImpl
import retrofit2.Call
import retrofit2.Callback
import retrofit2.Response
import java.util.Locale


class ProfileUserPage : Fragment() {

    private val activity: Fragment = this@ProfileUserPage
    private val apiRepository = ApiRepositoryImpl()
    private lateinit var token: String
    private lateinit var userId: String
    private lateinit var username: TextView
    private lateinit var email: TextView
    private lateinit var region: TextView
    private lateinit var signout: TextView
    private lateinit var language: TextView
    private lateinit var lang: String
    private lateinit var editButton: ImageButton

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        arguments?.let {
        }
    }

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {
        val v: View = inflater.inflate(R.layout.activity_profile_user_page, container, false)
        init(v)
        return v
    }

    private fun init(v: View) {
        if (MainUserPage.getUserId() == null) {
            userId = MainAdminPage.getUserId().toString()
            token = MainAdminPage.getToken().toString()
        } else {
            userId = MainUserPage.getUserId().toString()
            token = MainUserPage.getToken().toString()
        }

        username = v.findViewById(R.id.loginText)
        email = v.findViewById(R.id.emailText)
        region = v.findViewById(R.id.regionText)
        language = v.findViewById(R.id.language)
        editButton = v.findViewById(R.id.editUserButton)

        if (Locale.getDefault().language == "en") {
            lang = "uk"
            language.text = getString(R.string.msg11)
        } else {
            lang = "en"
            language.text = getString(R.string.msg112)
        }

        signout = v.findViewById(R.id.singOut)

        signout.let { s ->
            s.setOnClickListener {
                logout()
            }
        }

        language.let { s ->
            s.setOnClickListener {
                setLocale(lang)
                if (lang == "en") {
                    lang = "uk"
                } else {
                    lang = "en"
                }
            }
        }

        editButton.let { s ->
            s.setOnClickListener {
                val intent = Intent(activity.context, UpdateUser::class.java)
                startActivity(intent)
            }
        }

        getUser()
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
                        Toast.makeText(getActivity(), "Something went wrong!", Toast.LENGTH_LONG)
                            .show()
                    }
                } else {
                    Toast.makeText(
                        getActivity(),
                        "Something went wrong!"
                                + "Response " + response.code() + " " + response.message(),
                        Toast.LENGTH_LONG
                    )
                        .show()
                }
            }

            override fun onFailure(call: Call<UserResponse>, t: Throwable) {
                Toast.makeText(getActivity(), "Something went wrong!", Toast.LENGTH_LONG).show()
            }
        })
    }

    fun fillData(user: UserResponse) {
        val usernameText = user.login
        val emailText = user.email
        val regionText = getRegion(user.region)

        username.text = username.text.toString() + ": $usernameText"
        email.text = email.text.toString() + ": $emailText"
        region.text = region.text.toString() + ": $regionText"
    }

    private fun getRegion(region: Int): String {
        return Region.entries[region - 1].name
            .replace("__", "-")
            .replace("_", " ")
    }

    fun logout() {
        val intent = Intent(activity.context, LoginPage::class.java)
        startActivity(intent)
        activity.requireParentFragment().requireActivity().finish()
    }

    private fun setLocale(languageCode: String) {
        val locale = Locale(languageCode)
        Locale.setDefault(locale)
        val resources = activity.resources
        val config: Configuration = resources.configuration
        config.setLocale(locale)
        resources.updateConfiguration(config, resources.displayMetrics)
    }
}
