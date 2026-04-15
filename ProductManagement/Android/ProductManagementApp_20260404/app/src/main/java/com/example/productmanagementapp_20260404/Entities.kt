package com.example.productmanagementapp_20260404

import java.time.LocalDate
import java.time.LocalDateTime

data class HistoryData(
    val historyId:Int,
    val productId:Int,
    val productName: String,
    val actionType: String,
    val amount:Int,
    val memo: String,
    val createdAt: String
){
    fun GetCreated(): LocalDateTime{
        return LocalDateTime.parse(createdAt)
    }
}

data class SummaryData(
    val productCount:Int,
    val totalStock:Int,
    val recentOperations: List<SummaryHistory>
)

data class SummaryHistory(
    val createdAt: String,
    val productName: String,
    val actionType: String
){
    fun GetCreated(): LocalDateTime{
        return LocalDateTime.parse(createdAt)
    }
}

data class ResponseGetProduct(
    val productId:Int,
    val productName: String,
    val stock:Int,
    val price:Int,
    val createdAt:String
){
    fun GetCreated(): LocalDateTime{
        return LocalDateTime.parse(createdAt)
    }
}

data class DisplayProductData(
    val product: ResponseGetProduct,
    var isSelected: Boolean,
)