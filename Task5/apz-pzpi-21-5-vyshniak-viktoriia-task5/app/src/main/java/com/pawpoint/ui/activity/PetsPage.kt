package com.pawpoint.ui.activity

import android.annotation.SuppressLint
import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Toast
import androidx.fragment.app.Fragment
import androidx.recyclerview.widget.LinearLayoutManager
import androidx.recyclerview.widget.RecyclerView
import com.pawpoint.R
import com.pawpoint.model.enums.PetType
import com.pawpoint.model.response.PetListItem
import com.pawpoint.network.ApiRepositoryImpl
import com.pawpoint.ui.adapter.PetAdapter
import retrofit2.Call
import retrofit2.Callback
import retrofit2.Response

class PetsPage : Fragment() {

    private val activity: Fragment = this@PetsPage
    private val apiRepository = ApiRepositoryImpl()
    private lateinit var userId: String
    private lateinit var token: String
    private lateinit var petsIds: ArrayList<String>
    private lateinit var nickNames: ArrayList<String>
    private lateinit var types: ArrayList<String>
    private lateinit var recyclerView: RecyclerView
    private lateinit var petAdapter: PetAdapter

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        arguments?.let {}
    }

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {
        val v: View = inflater.inflate(R.layout.activity_pets_page, container, false)
        init(v)
        return v
    }

    private fun init(v: View) {
        userId = MainUserPage.getUserId().toString()
        token = MainUserPage.getToken().toString()

        petsIds = ArrayList()
        nickNames = ArrayList()
        types = ArrayList()

        recyclerView = v.findViewById(R.id.recycle_view)

        displayAllData()

        setAdapterOnRecycleView()
    }

    private fun setAdapterOnRecycleView() {
        petAdapter = PetAdapter(getActivity(), petsIds, nickNames, types)

        recyclerView.adapter = petAdapter

        recyclerView.layoutManager = LinearLayoutManager(getActivity())
    }

    private fun displayAllData() {
        apiRepository.getPetsByOwnerId("Bearer " + token, userId.toInt(), object :
            Callback<List<PetListItem>> {
            override fun onResponse(
                call: Call<List<PetListItem>>,
                response: Response<List<PetListItem>>
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
                            getActivity(),
                            "Something went wrong!",
                            Toast.LENGTH_LONG
                        )
                            .show()
                    }
                    setAdapterOnRecycleView()
                } else {
                    Toast.makeText(getActivity(), "Something went wrong!", Toast.LENGTH_LONG)
                        .show()
                }
            }

            override fun onFailure(call: Call<List<PetListItem>>, t: Throwable) {
                Toast.makeText(getActivity(), "Something went wrong!", Toast.LENGTH_LONG)
                    .show()
            }
        })
    }

    private fun clearRecycleView() {
        petsIds = ArrayList()
        nickNames = ArrayList()
        types = ArrayList()
    }

    @SuppressLint("Range")
    private fun fillData(dataList: List<PetListItem>) {
        if (dataList.isEmpty()) {
            Toast.makeText(activity.context, "No data!", Toast.LENGTH_LONG).show()
        } else {
            for (item in dataList) {
                petsIds.add(item.petId)
                nickNames.add(item.nickName)
                types.add(PetType.entries[item.petType - 1].name)
            }
        }
    }
}
