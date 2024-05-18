package com.pawpoint.ui.activity

import android.annotation.SuppressLint
import android.content.Intent
import android.os.Bundle
import android.widget.TextView
import android.widget.Toast
import androidx.activity.enableEdgeToEdge
import androidx.appcompat.app.AppCompatActivity
import androidx.recyclerview.widget.LinearLayoutManager
import androidx.recyclerview.widget.RecyclerView
import com.pawpoint.R
import com.pawpoint.model.enums.PetType
import com.pawpoint.model.response.NotificationModel
import com.pawpoint.network.ApiRepositoryImpl
import com.pawpoint.ui.adapter.NoteAdapter
import retrofit2.Call
import retrofit2.Callback
import retrofit2.Response
import java.time.LocalDate
import java.time.LocalDateTime
import java.time.format.DateTimeFormatter

class MessageInfo : AppCompatActivity() {

    private val activity: AppCompatActivity = this@MessageInfo
    private val apiRepository = ApiRepositoryImpl()
    private lateinit var token: String
    private lateinit var userId: String
    private lateinit var messageId: String
    private lateinit var noteIds: ArrayList<String>
    private lateinit var titles: ArrayList<String>
    private lateinit var dates: ArrayList<String>
    private lateinit var recyclerView: RecyclerView
    private lateinit var dateField: TextView
    private lateinit var nameField: TextView
    private lateinit var typeField: TextView
    private lateinit var charactField: TextView
    private lateinit var breedField: TextView
    private lateinit var illnessField: TextView
    private lateinit var prefField: TextView
    private lateinit var noteAdapter: NoteAdapter

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        enableEdgeToEdge()
        setContentView(R.layout.activity_message_info)
        init()
    }

    private fun init() {
        getExtra()
        userId = MainAdminPage.getUserId().toString()
        token = MainAdminPage.getToken().toString()

        noteIds = ArrayList()
        titles = ArrayList()
        dates = ArrayList()

        dateField = findViewById(R.id.date)
        nameField = findViewById(R.id.name)
        typeField = findViewById(R.id.type)
        charactField = findViewById(R.id.charact)
        breedField = findViewById(R.id.breed)
        illnessField = findViewById(R.id.illnesses)
        prefField = findViewById(R.id.pref)

        recyclerView = findViewById(R.id.recycle_view)

        displayAllData()

        setAdapterOnRecycleView()
    }

    private fun getExtra() {
        val arguments = intent.extras
        if (arguments != null) {
            if (arguments.containsKey("messageId")) {
                messageId = arguments.getString("messageId").toString()
            }
        } else {
            Toast.makeText(this, "Error", Toast.LENGTH_LONG).show()
            val intent = Intent(this, InstitutionsPage::class.java)
            startActivity(intent)
        }
    }

    private fun setAdapterOnRecycleView() {
        noteAdapter = NoteAdapter(
            activity, noteIds, titles, dates
        )

        recyclerView.adapter = noteAdapter

        recyclerView.layoutManager = LinearLayoutManager(activity)
    }

    private fun displayAllData() {
        apiRepository.getNotificationById("Bearer " + token, messageId,
            object :
                Callback<NotificationModel> {
                override fun onResponse(
                    call: Call<NotificationModel>,
                    response: Response<NotificationModel>
                ) {
                    if (response.isSuccessful) {
                        val dataList = response.body()
                        if (dataList != null) {
                            clearRecycleView()
                            setAdapterOnRecycleView()
                            fillData(dataList)
                            setAdapterOnRecycleView()
                        } else {
                            Toast.makeText(
                                activity,
                                "Something went wrong!",
                                Toast.LENGTH_LONG
                            ).show()
                        }
                        setAdapterOnRecycleView()
                    } else {
                        Toast.makeText(activity, "Something went wrong!", Toast.LENGTH_LONG)
                            .show()
                    }
                }

                override fun onFailure(call: Call<NotificationModel>, t: Throwable) {
                    Toast.makeText(activity, "Something went wrong!", Toast.LENGTH_LONG)
                        .show()
                }
            })
    }

    private fun clearRecycleView() {
        noteIds = ArrayList()
        titles = ArrayList()
        dates = ArrayList()
    }

    @SuppressLint("Range")
    private fun fillData(data: NotificationModel) {
        if (data.petProfile.documents.isNotEmpty()) {
            for (item in data.petProfile.documents) {
                val date = LocalDate.parse(item.lastUpdateOnUtc.split("T")[0])

                noteIds.add(item.diaryNoteId)
                titles.add(item.title)
                dates.add(date.format(DateTimeFormatter.ofPattern("dd.MM.YYYY")))
            }
        }

        val date = LocalDateTime.parse(data.createdOnUtc)

        dateField.text = date.format(DateTimeFormatter.ofPattern("dd.MM.YYYY HH:mm:ss"))
        nameField.text = data.petProfile.nickName
        typeField.text = PetType.entries[data.petProfile.petType - 1].name
        charactField.text = charactField.text.toString() + ": " + data.petProfile.characteristics
        breedField.text = breedField.text.toString() + ": " + data.petProfile.breed
        illnessField.text = illnessField.text.toString() + ": " + data.petProfile.illnesses
        prefField.text = prefField.text.toString() + ": " + data.petProfile.preferences
    }
}
