package com.pawpoint.ui.activity

import android.annotation.SuppressLint
import android.content.Intent
import android.os.Bundle
import android.widget.EditText
import android.widget.ImageButton
import android.widget.TextView
import android.widget.Toast
import androidx.activity.enableEdgeToEdge
import androidx.appcompat.app.AppCompatActivity
import androidx.recyclerview.widget.LinearLayoutManager
import androidx.recyclerview.widget.RecyclerView
import com.pawpoint.R
import com.pawpoint.model.request.DiaryNoteCreateRequest
import com.pawpoint.model.response.DiaryNoteListItem
import com.pawpoint.network.ApiRepositoryImpl
import com.pawpoint.ui.adapter.NoteAdapter
import retrofit2.Call
import retrofit2.Callback
import retrofit2.Response
import java.time.LocalDate
import java.time.format.DateTimeFormatter

class DiaryNotesPage : AppCompatActivity() {

    private val activity: AppCompatActivity = this@DiaryNotesPage
    private val apiRepository = ApiRepositoryImpl()
    private lateinit var token: String
    private lateinit var userId: String
    private lateinit var petId: String
    private lateinit var petName: String
    private lateinit var petType: String
    private lateinit var noteIds: ArrayList<String>
    private lateinit var titles: ArrayList<String>
    private lateinit var dates: ArrayList<String>
    private lateinit var recyclerView: RecyclerView
    private lateinit var nameField: TextView
    private lateinit var typeField: TextView
    private lateinit var titleField: EditText
    private lateinit var commentField: EditText
    private lateinit var noteButton: ImageButton
    private lateinit var noteAdapter: NoteAdapter
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        enableEdgeToEdge()
        setContentView(R.layout.activity_diary_notes_page)
        init()
    }

    private fun init() {
        getExtra()
        userId = MainUserPage.getUserId().toString()
        token = MainUserPage.getToken().toString()

        noteIds = ArrayList()
        titles = ArrayList()
        dates = ArrayList()

        nameField = findViewById(R.id.name)
        typeField = findViewById(R.id.type)
        titleField = findViewById(R.id.title)
        commentField = findViewById(R.id.comment)
        noteButton = findViewById(R.id.addNoteButton)

        recyclerView = findViewById(R.id.recycle_view)

        noteButton.let { s ->
            s.setOnClickListener {
                addNote()
            }
        }

        displayAllData()

        setAdapterOnRecycleView()
    }

    private fun getExtra() {
        val arguments = intent.extras
        if (arguments != null) {
            if (arguments.containsKey("petId")) {
                petId = arguments.getString("petId").toString()
            }
            if (arguments.containsKey("petName")) {
                petName = arguments.getString("petName").toString()
            }
            if (arguments.containsKey("petType")) {
                petType = arguments.getString("petType").toString()
            }
        } else {
            Toast.makeText(this, "Error", Toast.LENGTH_LONG).show()
            val intent = Intent(this, InstitutionsPage::class.java)
            startActivity(intent)
        }
    }

    private fun addNote() {
        val noteRequest = DiaryNoteCreateRequest(
            petId, titleField.text.toString(),
            commentField.text.toString()
        )

        apiRepository.createNote("Bearer " + token, noteRequest, object :
            Callback<Void> {
            override fun onResponse(
                call: Call<Void>,
                response: Response<Void>
            ) {
                if (response.isSuccessful) {
                    titleField.setText("")
                    titleField.clearFocus()
                    commentField.setText("")
                    commentField.clearFocus()
                    clearRecycleView()
                    displayAllData()
                    setAdapterOnRecycleView()
                } else {
                    Toast.makeText(
                        activity, "Something went wrong! " + response.code()
                                + response.message(), Toast.LENGTH_LONG
                    )
                        .show()
                }
            }

            override fun onFailure(call: Call<Void>, t: Throwable) {
                Toast.makeText(activity, "Something went wrong!", Toast.LENGTH_LONG)
                    .show()
            }
        })
    }

    private fun setAdapterOnRecycleView() {
        noteAdapter = NoteAdapter(
            activity, noteIds, titles, dates
        )

        recyclerView.adapter = noteAdapter

        recyclerView.layoutManager = LinearLayoutManager(activity)
    }

    private fun displayAllData() {
        apiRepository.getAllForPet("Bearer " + token, petId,
            object :
                Callback<List<DiaryNoteListItem>> {
                override fun onResponse(
                    call: Call<List<DiaryNoteListItem>>,
                    response: Response<List<DiaryNoteListItem>>
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

                override fun onFailure(call: Call<List<DiaryNoteListItem>>, t: Throwable) {
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
    private fun fillData(data: List<DiaryNoteListItem>) {
        if (data.isNotEmpty()) {
            for (item in data) {
                val date = LocalDate.parse(item.lastUpdateOnUtc.split("T")[0])

                noteIds.add(item.diaryNoteId)
                titles.add(item.title)
                dates.add(date.format(DateTimeFormatter.ofPattern("dd.MM.YYYY")))
            }
        }
        nameField.text = petName
        typeField.text = petType
    }
}
