package com.pawpoint.ui.activity

import android.annotation.SuppressLint
import android.content.Intent
import android.os.Bundle
import android.util.Log
import android.widget.Button
import android.widget.TextView
import android.widget.Toast
import androidx.activity.enableEdgeToEdge
import androidx.appcompat.app.AppCompatActivity
import androidx.recyclerview.widget.LinearLayoutManager
import androidx.recyclerview.widget.RecyclerView
import com.pawpoint.R
import com.pawpoint.model.enums.PetType
import com.pawpoint.model.response.GPSTrackerResponse
import com.pawpoint.model.response.PetModel
import com.pawpoint.model.response.TemperatureMonitorResponse
import com.pawpoint.network.ApiRepositoryImpl
import com.pawpoint.ui.adapter.HealthAdapter
import retrofit2.Call
import retrofit2.Callback
import retrofit2.Response
import java.time.LocalDate
import java.time.LocalDateTime
import java.time.format.DateTimeFormatter

class PetInfo : AppCompatActivity() {

    private val activity: AppCompatActivity = this@PetInfo
    private val apiRepository = ApiRepositoryImpl()
    private lateinit var token: String
    private lateinit var userId: String
    private lateinit var petId: String
    private lateinit var healthIds: ArrayList<String>
    private lateinit var dates: ArrayList<String>
    private lateinit var temps: ArrayList<Double>
    private lateinit var recyclerView: RecyclerView
    private lateinit var nameField: TextView
    private lateinit var typeField: TextView
    private lateinit var currentTempField: TextView
    private lateinit var currentLocationField: TextView
    private lateinit var birthField: TextView
    private lateinit var charactField: TextView
    private lateinit var breedField: TextView
    private lateinit var weightField: TextView
    private lateinit var heightField: TextView
    private lateinit var illnessField: TextView
    private lateinit var prefField: TextView
    private lateinit var notesButton: Button
    private lateinit var getCurrentTempButton: Button
    private lateinit var getCurrentLocationButton: Button
    private lateinit var healthAdapter: HealthAdapter
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        enableEdgeToEdge()
        setContentView(R.layout.activity_pet_info)
        init()
    }

    private fun init() {
        getExtra()
        userId = MainUserPage.getUserId().toString()
        token = MainUserPage.getToken().toString()

        healthIds = ArrayList()
        dates = ArrayList()
        temps = ArrayList()

        nameField = findViewById(R.id.name)
        typeField = findViewById(R.id.type)
        currentTempField = findViewById(R.id.currentTemp)
        currentLocationField = findViewById(R.id.currentLocation)
        birthField = findViewById(R.id.dateOfBirth)
        charactField = findViewById(R.id.charact)
        breedField = findViewById(R.id.breed)
        weightField = findViewById(R.id.weight)
        heightField = findViewById(R.id.height)
        illnessField = findViewById(R.id.illnesses)
        prefField = findViewById(R.id.pref)
        notesButton = findViewById(R.id.notesButton)
        getCurrentTempButton = findViewById(R.id.getCurrentTemp)
        getCurrentLocationButton = findViewById(R.id.getCurrentLocation)

        recyclerView = findViewById(R.id.recycle_view)

        notesButton.let { s ->
            s.setOnClickListener {
                val intent = Intent(activity, DiaryNotesPage::class.java)
                intent.putExtra("petId", petId)
                intent.putExtra("petName", nameField.text.toString())
                intent.putExtra("petType", typeField.text.toString())
                startActivity(intent)
            }
        }

        getCurrentTempButton.let { s ->
            s.setOnClickListener {
                getCurrentTemp()
            }
        }

        getCurrentLocationButton.let { s ->
            s.setOnClickListener {
                getCurrentLocation()
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
        } else {
            Toast.makeText(this, "Error", Toast.LENGTH_LONG).show()
            val intent = Intent(this, InstitutionsPage::class.java)
            startActivity(intent)
        }
    }

    private fun setAdapterOnRecycleView() {
        healthAdapter = HealthAdapter(
            activity, healthIds, dates, temps
        )

        recyclerView.adapter = healthAdapter

        recyclerView.layoutManager = LinearLayoutManager(activity)
    }

    private fun displayAllData() {

        apiRepository.getPetById("Bearer " + token, petId,
            object :
                Callback<PetModel> {
                override fun onResponse(
                    call: Call<PetModel>,
                    response: Response<PetModel>
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

                override fun onFailure(call: Call<PetModel>, t: Throwable) {
                    Toast.makeText(activity, "Something went wrong!", Toast.LENGTH_LONG)
                        .show()
                }
            })
    }

    private fun getCurrentTemp() {
        apiRepository.getCurrentPetTemp("Bearer " + token, petId,
            object :
                Callback<Double> {
                override fun onResponse(
                    call: Call<Double>,
                    response: Response<Double>
                ) {
                    if (response.isSuccessful) {
                        val dataList = response.body()
                        if (dataList != null) {
                            currentTempField.text = dataList.toString() + " °C"
                        }
                    } else {
                        Log.d("TEMP_ERROR", response.code().toString());
                        Toast.makeText(activity, "Something went wrong!", Toast.LENGTH_LONG)
                            .show()
                    }
                }

                override fun onFailure(call: Call<Double>, t: Throwable) {
                    Toast.makeText(activity, "Something went wrong!", Toast.LENGTH_LONG)
                        .show()
                }
            })
    }

    private fun getCurrentLocation() {
        apiRepository.getCurrentPetLocation("Bearer " + token, petId,
            object :
                Callback<GPSTrackerResponse> {
                override fun onResponse(
                    call: Call<GPSTrackerResponse>,
                    response: Response<GPSTrackerResponse>
                ) {
                    if (response.isSuccessful) {
                        val dataList = response.body()
                        if (dataList != null) {
                            currentLocationField.text = dataList.addressData.address
                        }
                    } else {
                        Toast.makeText(activity, "Something went wrong!", Toast.LENGTH_LONG)
                            .show()
                    }
                }

                override fun onFailure(call: Call<GPSTrackerResponse>, t: Throwable) {
                    Toast.makeText(activity, "Something went wrong!", Toast.LENGTH_LONG)
                        .show()
                }
            })
    }

    private fun clearRecycleView() {
        healthIds = ArrayList()
        dates = ArrayList()
        temps = ArrayList()
    }

    @SuppressLint("Range")
    private fun fillData(data: PetModel) {
        if (data.healthRecords.isNotEmpty()) {
            for (item in data.healthRecords) {
                val date = LocalDateTime.parse(item.createdOnUtc)

                healthIds.add(item.healthRecordId)
                dates.add(date.format(DateTimeFormatter.ofPattern("dd.MM.YYYY HH:mm:ss")))
                temps.add(item.temperature)
            }
        }

        val date = LocalDate.parse(data.birthDate.split("T")[0])

        nameField.text = data.nickName
        typeField.text = PetType.entries[data.petType - 1].name
        birthField.text = birthField.text.toString() + ": " +
                date.format(DateTimeFormatter.ofPattern("dd.MM.YYYY"))
        charactField.text = charactField.text.toString() + ": " + data.characteristics
        breedField.text = breedField.text.toString() + ": " + data.breed
        weightField.text = weightField.text.toString() + ": " + data.weight
        heightField.text = heightField.text.toString() + ": " + data.height
        illnessField.text = illnessField.text.toString() + ": " + data.illnesses
        prefField.text = prefField.text.toString() + ": " + data.preferences
    }
}