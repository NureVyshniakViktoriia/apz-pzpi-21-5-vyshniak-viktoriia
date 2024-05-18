package com.pawpoint.ui.adapter

import android.content.Context
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.TextView
import androidx.recyclerview.widget.RecyclerView
import com.pawpoint.R

class NoteAdapter(
    var context: Context?,
    private var noteIds: ArrayList<String>?,
    private var titles: ArrayList<String>?,
    private var dates: ArrayList<String>?
) : RecyclerView.Adapter<NoteAdapter.MyViewHolder>() {

    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): MyViewHolder {
        val inflater = LayoutInflater.from(context)
        val view: View = inflater.inflate(R.layout.row_note, parent, false)
        return MyViewHolder(view)
    }

    override fun onBindViewHolder(holder: MyViewHolder, position: Int) {
        holder.noteId.text = noteIds!![position]
        holder.title.text = titles!![position]
        holder.date.text = dates!![position]
    }

    override fun getItemCount(): Int {
        return noteIds!!.size
    }

    class MyViewHolder(private val itemView: View) : RecyclerView.ViewHolder(itemView) {
        var noteId: TextView = itemView.findViewById(R.id.noteId)
        var title: TextView = itemView.findViewById(R.id.title)
        var date: TextView = itemView.findViewById(R.id.updateDate)
        var view: View = itemView

        fun getItemView(): View {
            return view
        }
    }
}
