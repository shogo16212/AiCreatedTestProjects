package com.example.reviewproject

import com.google.gson.Gson
import com.google.gson.reflect.TypeToken

var gson = Gson()

inline fun <reified  T> toJson(value:T): String{
    return gson.toJson(value)
}

inline fun <reified  T> fromJson(json: String): T{
    return gson.fromJson<T>(json, object : TypeToken<T>(){}.type)
}