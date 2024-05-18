package com.pawpoint.ui.adapter

import android.content.Context
import android.content.Intent
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.ImageButton
import android.widget.TextView
import androidx.recyclerview.widget.RecyclerView
import com.pawpoint.R
import com.pawpoint.ui.activity.MessageInfo

class MessageAdapter(
    var context: Context?,
    private var messageIds: ArrayList<String>?,
    private var dates: ArrayList<String>?
) : RecyclerView.Adapter<MessageAdapter.MyViewHolder>() {

    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): MyViewHolder {
        val inflater = LayoutInflater.from(context)
        val view: View = inflater.inflate(R.layout.row_message, parent, false)
        return MyViewHolder(view)
    }

    override fun onBindViewHolder(holder: MyViewHolder, position: Int) {
        holder.messageId.text = messageIds!![position]
        holder.date.text = dates!![position]

        val messageId = holder.view.findViewById<TextView>(R.id.messageId).text.toString()

        holder.button.setOnClickListener {
            val intent = Intent(context, MessageInfo::class.java)
            intent.putExtra("messageId", messageId)
            intent.setFlags(Intent.FLAG_ACTIVITY_NEW_TASK or Intent.FLAG_ACTIVITY_MULTIPLE_TASK)
            context!!.startActivity(intent)
        }
    }

    override fun getItemCount(): Int {
        return messageIds!!.size
    }

    class MyViewHolder(private val itemView: View) : RecyclerView.ViewHolder(itemView) {
        var messageId: TextView = itemView.findViewById(R.id.messageId)
        var date: TextView = itemView.findViewById(R.id.date)
        var button: ImageButton = itemView.findViewById(R.id.openButton)
        var view: View = itemView

        fun getItemView(): View {
            return view
        }
    }
}
