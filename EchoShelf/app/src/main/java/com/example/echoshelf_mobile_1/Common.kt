package com.example.echoshelf_mobile_1

import com.google.gson.Gson
import com.google.gson.reflect.TypeToken

val gson = Gson()

inline fun<reified T> fromJson(json: String):T{
    return gson.fromJson<T>(json, object : TypeToken<T>(){}.type)
}

var UserId:Int = 0;