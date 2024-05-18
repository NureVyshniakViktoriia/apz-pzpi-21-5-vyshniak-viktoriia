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
import com.pawpoint.model.enums.InstitutionType
import com.pawpoint.model.request.RatingRequest
import com.pawpoint.model.response.InstitutionModel
import com.pawpoint.network.ApiRepositoryImpl
import com.pawpoint.ui.adapter.FacilityAdapter
import retrofit2.Call
import retrofit2.Callback
import retrofit2.Response

class InstitutionInfo : AppCompatActivity() {

    private val activity: AppCompatActivity = this@InstitutionInfo
    private val apiRepository = ApiRepositoryImpl()
    private lateinit var token: String
    private lateinit var userId: String
    private lateinit var institutionId: String
    private lateinit var facilityIds: ArrayList<Long>
    private lateinit var names: ArrayList<String>
    private lateinit var recyclerView: RecyclerView
    private lateinit var nameField: TextView
    private lateinit var typeField: TextView
    private lateinit var ratingField: EditText
    private lateinit var ratingButton: ImageButton
    private lateinit var descriptionField: TextView
    private lateinit var urlField: TextView
    private lateinit var numberField: TextView
    private lateinit var addressField: TextView
    private lateinit var facilityAdapter: FacilityAdapter

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        enableEdgeToEdge()
        setContentView(R.layout.activity_institution_info)
        init();
    }

    private fun init() {
        getExtra()
        userId = MainUserPage.getUserId().toString()
        token = MainUserPage.getToken().toString()

        facilityIds = ArrayList()
        names = ArrayList()

        nameField = findViewById(R.id.name)
        typeField = findViewById(R.id.type)
        ratingField = findViewById(R.id.rating)
        ratingButton = findViewById(R.id.setRatingButton)
        descriptionField = findViewById(R.id.description)
        urlField = findViewById(R.id.url)
        numberField = findViewById(R.id.number)
        addressField = findViewById(R.id.address)

        recyclerView = findViewById(R.id.recycle_view)

        ratingButton.let { s ->
            s.setOnClickListener {
                setRating()
            }
        }

        displayAllData()

        setAdapterOnRecycleView()
    }

    private fun getExtra() {
        val arguments = intent.extras
        if (arguments != null) {
            if (arguments.containsKey("institutionId")) {
                institutionId = arguments.getString("institutionId").toString()
            }
        } else {
            Toast.makeText(this, "Error", Toast.LENGTH_LONG).show()
            val intent = Intent(this, InstitutionsPage::class.java)
            startActivity(intent)
        }
    }

    private fun setRating() {
        var mark = 0
        if (ratingField.text != null) {
            mark = ratingField.getText().toString().toInt()
        }
        val ratingRequest = RatingRequest(institutionId.toInt(), userId.toInt(), mark)

        apiRepository.setRating("Bearer " + token, ratingRequest,
            object :
                Callback<Void> {
                override fun onResponse(
                    call: Call<Void>,
                    response: Response<Void>
                ) {
                    if (response.isSuccessful) {
                        Toast.makeText(
                            activity,
                            "Rating is successfully updated!",
                            Toast.LENGTH_LONG
                        ).show()
                    } else {
                        Toast.makeText(activity, "Something went wrong!", Toast.LENGTH_LONG)
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
        facilityAdapter = FacilityAdapter(
            activity, facilityIds, names
        )

        recyclerView.adapter = facilityAdapter

        recyclerView.layoutManager = LinearLayoutManager(activity)
    }

    private fun displayAllData() {
        apiRepository.getInstitutionById("Bearer " + token, institutionId.toInt(),
            object :
                Callback<InstitutionModel> {
                override fun onResponse(
                    call: Call<InstitutionModel>,
                    response: Response<InstitutionModel>
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

                override fun onFailure(call: Call<InstitutionModel>, t: Throwable) {
                    Toast.makeText(activity, "Something went wrong!", Toast.LENGTH_LONG)
                        .show()
                }
            })
    }

    private fun clearRecycleView() {
        facilityIds = ArrayList()
        names = ArrayList()
    }

    @SuppressLint("Range")
    private fun fillData(data: InstitutionModel) {
        if (data.facilities.isNotEmpty()) {
            for (item in data.facilities) {
                facilityIds.add(item.facilityId)
                names.add(item.name)
            }
        }

        var rat = 0.0

        if (data.rating != null && data.rating.isSetByCurrentUser) {
            rat = data.rating.mark
        }

        nameField.text = data.name
        typeField.text = InstitutionType.entries[data.institutionType - 1].name
        ratingField.setText(rat.toString())
        descriptionField.text = descriptionField.getText().toString() + ": " + data.description
        urlField.text = urlField.getText().toString() + ": " + data.websiteUrl
        numberField.text = numberField.getText().toString() + ": " + data.phoneNumber
        addressField.text = addressField.getText().toString() + ": " + data.address
    }
}
