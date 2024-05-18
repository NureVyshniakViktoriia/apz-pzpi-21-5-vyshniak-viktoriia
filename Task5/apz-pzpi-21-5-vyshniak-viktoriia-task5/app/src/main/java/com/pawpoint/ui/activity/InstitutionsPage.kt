package com.pawpoint.ui.activity

import android.annotation.SuppressLint
import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.EditText
import android.widget.ImageButton
import android.widget.Toast
import androidx.fragment.app.Fragment
import androidx.recyclerview.widget.LinearLayoutManager
import androidx.recyclerview.widget.RecyclerView
import com.google.android.material.tabs.TabLayout
import com.google.android.material.tabs.TabLayout.OnTabSelectedListener
import com.pawpoint.R
import com.pawpoint.model.enums.InstitutionType
import com.pawpoint.model.response.InstitutionListItem
import com.pawpoint.network.ApiRepositoryImpl
import com.pawpoint.ui.adapter.InstitutionAdapter
import retrofit2.Call
import retrofit2.Callback
import retrofit2.Response

class InstitutionsPage : Fragment() {

    private val activity: Fragment = this@InstitutionsPage
    private val apiRepository = ApiRepositoryImpl()
    private lateinit var userId: String
    private lateinit var token: String
    private lateinit var institutionsIds: ArrayList<Long>
    private lateinit var names: ArrayList<String>
    private lateinit var userRatings: ArrayList<Double>
    private lateinit var ratings: ArrayList<Double>
    private lateinit var types: ArrayList<String>
    private lateinit var tabLayout: TabLayout
    private lateinit var recyclerView: RecyclerView
    private lateinit var searchField: EditText
    private lateinit var searchButton: ImageButton
    private lateinit var resetButton: ImageButton
    private lateinit var sortButton: ImageButton
    private lateinit var institutionAdapter: InstitutionAdapter
    private var sortByRatingAscending: Boolean = true
    private var type: Int = 1

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        arguments?.let {}
    }

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {
        val v: View = inflater.inflate(R.layout.activity_institutions_page, container, false)
        init(v)
        return v
    }

    private fun init(v: View) {
        userId = MainUserPage.getUserId().toString()
        token = MainUserPage.getToken().toString()

        institutionsIds = ArrayList()
        names = ArrayList()
        userRatings = ArrayList()
        ratings = ArrayList()
        types = ArrayList()

        tabLayout = v.findViewById(R.id.tabs);

        recyclerView = v.findViewById(R.id.recycle_view)
        searchField = v.findViewById(R.id.searchField)
        searchButton = v.findViewById(R.id.searchButton)
        resetButton = v.findViewById(R.id.resetButton)
        sortButton = v.findViewById(R.id.sortButton)

        searchButton.let { s ->
            s.setOnClickListener {
                clearRecycleView()
                displayAllData()
                setAdapterOnRecycleView()
            }
        }

        resetButton.let { s ->
            s.setOnClickListener {
                searchField.setText("")
                clearRecycleView()
                displayAllData()
                setAdapterOnRecycleView()
            }
        }

        sortButton.let { s ->
            s.setOnClickListener {
                sortByRatingAscending = !sortByRatingAscending
                clearRecycleView()
                displayAllData()
                setAdapterOnRecycleView()
            }
        }

        displayAllData()

        selectingTabs()
    }

    private fun setAdapterOnRecycleView() {
        institutionAdapter = InstitutionAdapter(
            getActivity(), institutionsIds, names, userRatings, ratings, types
        )

        recyclerView.adapter = institutionAdapter

        recyclerView.layoutManager = LinearLayoutManager(getActivity())
    }

    private fun displayAllData() {
        var search = ""

        if (searchField.text.isNotEmpty()) {
            search = searchField.text.toString()
        }

        apiRepository.getInstitutionList("Bearer " + token, search, type,
            sortByRatingAscending, object :
                Callback<List<InstitutionListItem>> {
                override fun onResponse(
                    call: Call<List<InstitutionListItem>>,
                    response: Response<List<InstitutionListItem>>
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

                override fun onFailure(call: Call<List<InstitutionListItem>>, t: Throwable) {
                    Toast.makeText(getActivity(), "Something went wrong!", Toast.LENGTH_LONG)
                        .show()
                }
            })
    }

    private fun clearRecycleView() {
        institutionsIds = ArrayList()
        names = ArrayList()
        userRatings = ArrayList()
        ratings = ArrayList()
        types = ArrayList()
    }

    @SuppressLint("Range")
    private fun fillData(dataList: List<InstitutionListItem>) {
        if (dataList.isEmpty()) {
            Toast.makeText(activity.context, "No data!", Toast.LENGTH_LONG).show()
        } else {
            for (item in dataList) {
                institutionsIds.add(item.institutionId)
                names.add(item.name)
                var rat = 0.0

                if (item.rating != null && item.rating.isSetByCurrentUser) {
                    rat = item.rating.mark
                }

                userRatings.add(rat)
                ratings.add(item.weightedRating)
                types.add(InstitutionType.entries[item.institutionType - 1].name)
            }
        }
    }

    private fun selectingTabs() {
        tabLayout.addOnTabSelectedListener(object : OnTabSelectedListener {
            override fun onTabSelected(tab: TabLayout.Tab) {
                when (tab.position) {
                    0 -> {
                        clearRecycleView()
                        type = 1
                        displayAllData()
                        setAdapterOnRecycleView()
                    }

                    1 -> {
                        clearRecycleView()
                        type = 2
                        displayAllData()
                        setAdapterOnRecycleView()
                    }

                    2 -> {
                        clearRecycleView()
                        type = 3
                        displayAllData()
                        setAdapterOnRecycleView()
                    }

                    3 -> {
                        clearRecycleView()
                        type = 4
                        displayAllData()
                        setAdapterOnRecycleView()
                    }

                    4 -> {
                        clearRecycleView()
                        type = 5
                        displayAllData()
                        setAdapterOnRecycleView()
                    }
                }
            }

            override fun onTabUnselected(tab: TabLayout.Tab) {}
            override fun onTabReselected(tab: TabLayout.Tab) {}
        })
    }
}
