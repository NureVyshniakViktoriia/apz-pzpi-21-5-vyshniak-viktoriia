package com.pawpoint.ui.adapter

import android.content.Context
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.TextView
import androidx.recyclerview.widget.RecyclerView
import com.pawpoint.R

class HealthAdapter(
    var context: Context?,
    private var healthIds: ArrayList<String>?,
    private var dates: ArrayList<String>?,
    private var temps: ArrayList<Double>?
) : RecyclerView.Adapter<HealthAdapter.MyViewHolder>() {

    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): MyViewHolder {
        val inflater = LayoutInflater.from(context)
        val view: View = inflater.inflate(R.layout.row_health, parent, false)
        return MyViewHolder(view)
    }

    override fun onBindViewHolder(holder: MyViewHolder, position: Int) {
        holder.healthId.text = healthIds!![position]
        holder.date.text = dates!![position]
        holder.temp.text = temps!![position].toString() + " °C"
    }

    override fun getItemCount(): Int {
        return healthIds!!.size
    }

    class MyViewHolder(private val itemView: View) : RecyclerView.ViewHolder(itemView) {
        var healthId: TextView = itemView.findViewById(R.id.healthId)
        var date: TextView = itemView.findViewById(R.id.date)
        var temp: TextView = itemView.findViewById(R.id.temp)
        var view: View = itemView

        fun getItemView(): View {
            return view
        }
    }
}
