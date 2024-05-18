package com.pawpoint.ui.activity

import android.content.Intent
import android.os.Bundle
import android.util.Log
import android.view.MenuItem
import android.widget.Toast
import androidx.activity.enableEdgeToEdge
import androidx.appcompat.app.AppCompatActivity
import com.google.android.material.bottomnavigation.BottomNavigationView
import com.pawpoint.R

class MainAdminPage : AppCompatActivity(), BottomNavigationView.OnNavigationItemSelectedListener {

    private lateinit var bottomNavigationView: BottomNavigationView
    private val activity: BottomNavigationView.OnNavigationItemSelectedListener = this@MainAdminPage

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        enableEdgeToEdge()
        setContentView(R.layout.activity_main_admin_page)

        getExtra()

        bottomNavigationView = findViewById(R.id.bottomMenu);

        bottomNavigationView.let { b ->
            b.setOnNavigationItemSelectedListener(activity)
            b.setSelectedItemId(R.id.messages)
            b.setItemIconTintList(null)
        }
    }

    companion object {
        private var userId: String? = null
        private var token: String? = null

        fun getUserId(): String? {
            return userId
        }

        fun getToken(): String? {
            Log.d("token", token + " ")
            return token
        }
    }


    private fun setUserId(userIdValue: String) {
        userId = userIdValue
    }

    private fun setToken(tokenValue: String) {
        token = tokenValue
    }

    private fun getExtra() {
        val arguments = intent.extras
        if (arguments != null) {
            if (arguments.containsKey("userId")) {
                userId = arguments.getString("userId")
                setUserId(userId.toString())
            }
            if (arguments.containsKey("token")) {
                token = arguments.getString("token")
                setToken(token.toString())
            }
        } else {
            Toast.makeText(this, "Error", Toast.LENGTH_LONG).show()
            val intent = Intent(this, LoginPage::class.java)
            startActivity(intent)
        }
    }

    override fun onNavigationItemSelected(item: MenuItem): Boolean {
        when (item.itemId) {

            R.id.messages -> {
                supportFragmentManager.beginTransaction().replace(
                    R.id.Fragment,
                    MessagesPage()
                ).commit()
                return true
            }

            R.id.profile -> {
                supportFragmentManager.beginTransaction().replace(
                    R.id.Fragment,
                    ProfileUserPage()
                ).commit()
                return true
            }
        }
        return false
    }
}
