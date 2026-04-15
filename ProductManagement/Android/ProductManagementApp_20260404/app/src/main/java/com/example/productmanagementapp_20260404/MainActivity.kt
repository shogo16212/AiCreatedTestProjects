package com.example.productmanagementapp_20260404

import android.os.Bundle
import android.os.StrictMode
import android.widget.Button
import android.widget.ListView
import android.widget.TextView
import android.widget.Toast
import androidx.activity.enableEdgeToEdge
import androidx.appcompat.app.AppCompatActivity
import androidx.core.view.ViewCompat
import androidx.core.view.WindowInsetsCompat

class MainActivity : AppCompatActivity() {
    private val maListButton by lazy { findViewById<Button>(R.id.maListButton) }
    private val maSubmitButton by lazy { findViewById<Button>(R.id.maSubmitButton) }
    private val maHistoryButton by lazy { findViewById<Button>(R.id.maHistoryButton) }
    private val maSummaryTextView by lazy { findViewById<TextView>(R.id.maSummaryTextView) }
    private val maHistoryList by lazy { findViewById<ListView>(R.id.maHistoryList) }
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_main)
        StrictMode.setThreadPolicy(StrictMode.ThreadPolicy.Builder().permitAll().build())


    }

    override fun onResume() {
        super.onResume()
        refresh()
    }

    fun refresh(){
        try {
            val summaryData = Api.get<SummaryData>("api/dashboard")

            maSummaryTextView.text = "Submit Product Count:${summaryData.productCount}, Total Stock Amount:${summaryData.totalStock}"

            maHistoryList.adapter = ListViewAdapter(this, R.layout.list_summary, summaryData.recentOperations.sortedByDescending { it.GetCreated() }.toList())

        }catch (ex: Api.ApiException){
            Toast.makeText(this, ex.message, Toast.LENGTH_SHORT).show()
        }catch (ex: Exception){
            Toast.makeText(this, ex.message, Toast.LENGTH_SHORT).show()
        }
    }
}