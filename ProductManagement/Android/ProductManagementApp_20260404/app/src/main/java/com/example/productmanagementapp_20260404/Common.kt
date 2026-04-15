package com.example.productmanagementapp_20260404

import android.health.connect.GetMedicalDataSourcesRequest
import com.google.gson.Gson
import com.google.gson.reflect.TypeToken

val gson= Gson()

inline fun<reified T> fromJson(json: String):T{
    return gson.fromJson<T>(json, object : TypeToken<T>(){}.type)
}