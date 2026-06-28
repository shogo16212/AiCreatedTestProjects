package com.example.productmanagementapp_20260404

import android.os.Bundle
import android.os.StrictMode
import android.widget.Button
import android.widget.EditText
import android.widget.ListView
import android.widget.Toast
import androidx.activity.enableEdgeToEdge
import androidx.appcompat.app.AlertDialog
import androidx.appcompat.app.AppCompatActivity
import androidx.core.view.ViewCompat
import androidx.core.view.WindowInsetsCompat

class ListActivity : AppCompatActivity(), ItemNotifer {
    private val alSearchEditText by lazy { findViewById<EditText>(R.id.alSearchEditText) }
    private val laSearchButton by lazy { findViewById<Button>(R.id.laSearchButton) }
    private val laProductListView by lazy { findViewById<ListView>(R.id.laProductListView) }
    private val laUpdateButton by lazy { findViewById<Button>(R.id.laUpdateButton) }
    private val laDeleteButton by lazy { findViewById<Button>(R.id.laDeleteButton) }
    private var productList = listOf<DisplayProductData>()

//        private var selectProductId = 0;
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_list)
        StrictMode.setThreadPolicy(StrictMode.ThreadPolicy.Builder().permitAll().build())

        laDeleteButton.setOnClickListener {
            if (productList.any { it.isSelected }) {
                AlertDialog.Builder(this).apply {
                    setTitle("Confirm")
                    setMessage("Delete this item?")
                    setPositiveButton("OK"){_,_ ->
                        val product = productList.first{it.isSelected}
                        try{
                            val result = Api.delete<ResponseMessage>("api/products/${product.product.productId}")
                        }catch (e: Api.ApiException){

                        }catch (e: Exception){
                            Toast.makeText(this@ListActivity, e.message, Toast.LENGTH_SHORT).show()
                        }
                    }
                    setNegativeButton("Cancel"){_,_ -> }
                    show()
                }
            }

        }

        laUpdateButton.setOnClickListener {

        }

        laSearchButton.setOnClickListener {
            refresh()
        }


    }
    fun refresh() {
        val originalProductList = Api.get<List<ResponseGetProduct>>("api/products")
        productList = filter(
            originalProductList,
            alSearchEditText.text.toString()
        ).map { DisplayProductData(it, false) }.toList()
//        if(selectProductId != 0){
//            productList.forEach {
//                if(it.product.productId == selectProductId) it.isSelected = true
//            }
//        }
        laProductListView.adapter = ListViewAdapter(this, R.layout.list_product, productList)
    }

    fun filter(products: List<ResponseGetProduct>, content: String): List<ResponseGetProduct> {
        if (content.isNotEmpty()) {
            return filter(products.filter { it.productName.contains(content) }.toList(), "")
        }

        return products
    }

    override fun select(item: Any?) {
        val product = item as DisplayProductData
        productList.forEach { it.isSelected = false }
        product.isSelected = true
        refresh()
    }

}