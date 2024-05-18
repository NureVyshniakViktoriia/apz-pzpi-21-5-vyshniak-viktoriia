package com.pawpoint.ui.activity

import android.annotation.SuppressLint
import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.ImageButton
import android.widget.Toast
import androidx.fragment.app.Fragment
import androidx.recyclerview.widget.LinearLayoutManager
import androidx.recyclerview.widget.RecyclerView
import com.pawpoint.R
import com.pawpoint.model.response.NotificationListItem
import com.pawpoint.network.ApiRepositoryImpl
import com.pawpoint.ui.adapter.MessageAdapter
import retrofit2.Call
import retrofit2.Callback
import retrofit2.Response
import java.time.LocalDateTime
import java.time.format.DateTimeFormatter

class MessagesPage : Fragment() {

    private val activity: Fragment = this@MessagesPage
    private val apiRepository = ApiRepositoryImpl()
    private lateinit var userId: String
    private lateinit var token: String
    private lateinit var messageIds: ArrayList<String>
    private lateinit var dates: ArrayList<String>
    private lateinit var updateButton: ImageButton
    private lateinit var recyclerView: RecyclerView
    private lateinit var messageAdapter: MessageAdapter

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        arguments?.let {}
    }

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {
        val v: View = inflater.inflate(R.layout.activity_messages_page, container, false)
        init(v)
        return v
    }

    private fun init(v: View) {
        userId = MainAdminPage.getUserId().toString()
        token = MainAdminPage.getToken().toString()

        messageIds = ArrayList()
        dates = ArrayList()

        updateButton = v.findViewById(R.id.updateButton)
        recyclerView = v.findViewById(R.id.recycle_view)

        updateButton.let { s ->
            s.setOnClickListener {
                clearRecycleView()
                displayAllData()
                setAdapterOnRecycleView()
            }
        }

        displayAllData()

        setAdapterOnRecycleView()
    }

    private fun setAdapterOnRecycleView() {
        messageAdapter = MessageAdapter(getActivity(), messageIds, dates)

        recyclerView.adapter = messageAdapter

        recyclerView.layoutManager = LinearLayoutManager(getActivity())
    }

    private fun displayAllData() {
        apiRepository.getNotificationsByUserId("Bearer " + token, userId.toInt(), object :
            Callback<List<NotificationListItem>> {
            override fun onResponse(
                call: Call<List<NotificationListItem>>,
                response: Response<List<NotificationListItem>>
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

            override fun onFailure(call: Call<List<NotificationListItem>>, t: Throwable) {
                Toast.makeText(getActivity(), "Something went wrong!", Toast.LENGTH_LONG)
                    .show()
            }
        })
    }

    private fun clearRecycleView() {
        messageIds = ArrayList()
        dates = ArrayList()
    }

    @SuppressLint("Range")
    private fun fillData(dataList: List<NotificationListItem>) {
        if (dataList.isEmpty()) {
            Toast.makeText(activity.context, "No data!", Toast.LENGTH_LONG).show()
        } else {
            for (item in dataList) {
                val date = LocalDateTime.parse(item.createdOnUtc)
                messageIds.add(item.notificationId)
                dates.add(date.format(DateTimeFormatter.ofPattern("dd.MM.YYYY HH:mm:ss")))
            }
        }
    }
}
