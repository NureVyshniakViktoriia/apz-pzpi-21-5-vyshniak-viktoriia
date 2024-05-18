package com.pawpoint.ui.adapter

import android.content.Context
import android.content.Intent
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.TextView
import androidx.recyclerview.widget.RecyclerView
import com.pawpoint.R
import com.pawpoint.ui.activity.InstitutionInfo

class InstitutionAdapter(
    var context: Context?,
    private var institutionsIds: ArrayList<Long>?,
    private var names: ArrayList<String>?,
    private var userRatings: ArrayList<Double>?,
    private var ratings: ArrayList<Double>?,
    private var types: ArrayList<String>?
) : RecyclerView.Adapter<InstitutionAdapter.MyViewHolder>() {

    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): MyViewHolder {
        val inflater = LayoutInflater.from(context)
        val view: View = inflater.inflate(R.layout.row_institution, parent, false)
        return MyViewHolder(view)
    }

    override fun onBindViewHolder(holder: MyViewHolder, position: Int) {
        holder.institutionId.text = institutionsIds!![position].toString()
        holder.name.text = names!![position]
        holder.userRating.text = userRatings!![position].toString()
        holder.rating.text = ratings!![position].toString()
        holder.type.text = types!![position]

        holder.view.setOnClickListener { v ->
            val txt = v.findViewById<TextView>(R.id.institutionId)
            val intent = Intent(context, InstitutionInfo::class.java)
            intent.putExtra("institutionId", txt.text.toString())
            intent.setFlags(Intent.FLAG_ACTIVITY_NEW_TASK or Intent.FLAG_ACTIVITY_MULTIPLE_TASK)
            context!!.startActivity(intent)
        }
    }

    override fun getItemCount(): Int {
        return names!!.size
    }

    class MyViewHolder(private val itemView: View) : RecyclerView.ViewHolder(itemView) {
        var institutionId: TextView = itemView.findViewById(R.id.institutionId)
        var name: TextView = itemView.findViewById(R.id.name)
        var userRating: TextView = itemView.findViewById(R.id.userRating)
        var rating: TextView = itemView.findViewById(R.id.rating)
        var type: TextView = itemView.findViewById(R.id.type)
        var view: View = itemView

        fun getItemView(): View {
            return view
        }
    }
}
