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
import com.pawpoint.ui.activity.PetInfo

class PetAdapter(
    var context: Context?,
    private var petsIds: ArrayList<String>?,
    private var nickNames: ArrayList<String>?,
    private var types: ArrayList<String>?
) : RecyclerView.Adapter<PetAdapter.MyViewHolder>() {

    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): MyViewHolder {
        val inflater = LayoutInflater.from(context)
        val view: View = inflater.inflate(R.layout.row_pet, parent, false)
        return MyViewHolder(view)
    }

    override fun onBindViewHolder(holder: MyViewHolder, position: Int) {
        holder.petId.text = petsIds!![position]
        holder.nickName.text = nickNames!![position]
        holder.type.text = types!![position]

        val petId = holder.view.findViewById<TextView>(R.id.petId).text.toString()
        holder.button.setOnClickListener {
            val intent = Intent(context, PetInfo::class.java)
            intent.putExtra("petId", petId)
            intent.setFlags(Intent.FLAG_ACTIVITY_NEW_TASK or Intent.FLAG_ACTIVITY_MULTIPLE_TASK)
            context!!.startActivity(intent)
        }
    }

    override fun getItemCount(): Int {
        return petsIds!!.size
    }

    class MyViewHolder(private val itemView: View) : RecyclerView.ViewHolder(itemView) {
        var petId: TextView = itemView.findViewById(R.id.petId)
        var nickName: TextView = itemView.findViewById(R.id.nickName)
        var type: TextView = itemView.findViewById(R.id.type)
        var button: ImageButton = itemView.findViewById(R.id.openButton)
        var view: View = itemView

        fun getItemView(): View {
            return view
        }
    }
}
