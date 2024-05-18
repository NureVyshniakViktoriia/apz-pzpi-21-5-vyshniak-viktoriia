package com.pawpoint.ui.adapter

import android.content.Context
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.TextView
import androidx.recyclerview.widget.RecyclerView
import com.pawpoint.R

class FacilityAdapter(
    var context: Context?,
    private var facilityIds: ArrayList<Long>?,
    private var names: ArrayList<String>?,
) : RecyclerView.Adapter<FacilityAdapter.MyViewHolder>() {

    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): MyViewHolder {
        val inflater = LayoutInflater.from(context)
        val view: View = inflater.inflate(R.layout.row_facility, parent, false)
        return MyViewHolder(view)
    }

    override fun onBindViewHolder(holder: MyViewHolder, position: Int) {
        holder.facilityId.text = facilityIds!![position].toString()
        holder.name.text = names!![position]
    }

    override fun getItemCount(): Int {
        return facilityIds!!.size
    }

    class MyViewHolder(private val itemView: View) : RecyclerView.ViewHolder(itemView) {
        var facilityId: TextView = itemView.findViewById(R.id.facilityId)
        var name: TextView = itemView.findViewById(R.id.name)
        var view: View = itemView

        fun getItemView(): View {
            return view
        }
    }
}
