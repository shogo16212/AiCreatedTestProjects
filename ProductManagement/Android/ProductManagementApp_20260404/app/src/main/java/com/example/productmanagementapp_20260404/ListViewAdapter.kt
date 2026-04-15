package com.example.productmanagementapp_20260404

import android.content.Context
import android.graphics.Color
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.BaseAdapter
import android.widget.TextView
import java.time.format.DateTimeFormatter

class ListViewAdapter(
    val context: Context,
    val resourceId:Int,
    val items: List<*>,
    val itemNotifer: ItemNotifer? = null
): BaseAdapter(){
    override fun getCount(): Int {
        return items.size
    }

    override fun getItem(p0: Int): Any? {
        return items[p0] as Any?
    }

    override fun getItemId(p0: Int): Long {
        return -1
    }

    override fun getView(
        p0: Int,
        p1: View?,
        p2: ViewGroup?
    ): View? {
        val layout = LayoutInflater.from(context).inflate(resourceId, p2, false)
        return when(resourceId){
            R.layout.list_summary -> layout.apply {
                val lsTextView = findViewById<TextView>(R.id.lsTextView)

                val history = items[p0] as SummaryHistory

                lsTextView.text = "- ${history.GetCreated().format(DateTimeFormatter.ofPattern("yyyy/MM/dd"))} ${history.productName} is ${history.actionType}"
            }
            R.layout.list_product -> layout.apply {
                val lpInfoTextView = findViewById<TextView>(R.id.lpInfoTextView)

                val product = items[p0] as DisplayProductData

                val productHistory = Api.get<List<HistoryData>>("api/history").filter { it.productId == product.product.productId }.sortedByDescending { it.GetCreated() }.toList()

                lpInfoTextView.text = "Name:${product.product.productName}, Stock:${product.product.stock}, Price:${product.product.price}, Created:${product.product.GetCreated().format(
                    DateTimeFormatter.ofPattern("yyyy-MM-dd"))}, LastUpdated:${productHistory.first().GetCreated().format(
                    DateTimeFormatter.ofPattern("yyyy-MM-dd"))}"

                setOnClickListener {
                    itemNotifer?.select(product)
                }

                setBackgroundColor(if(product.isSelected) Color.LTGRAY else Color.TRANSPARENT)
            }
            else -> View(context)
        }
    }
}