package com.example.reviewproject

import com.google.gson.Gson
import java.io.BufferedReader
import java.io.InputStreamReader
import java.io.OutputStreamWriter
import java.net.HttpURLConnection
import java.net.URL
import java.nio.charset.StandardCharsets

object Api {
    val gson = Gson()
    val token:String = "";

    inline fun<reified T> request(arg:String, method: String, json: String = ""):T{
        val url = URL("http://172.17.1.101:2024/$arg")
        val con = url.openConnection() as HttpURLConnection

        con.apply {
            requestMethod = method
            connectTimeout = 3000
            if(!token.isNullOrEmpty()){
                setRequestProperty("Authorization", "Bearer $token")
            }
            if(!json.isNullOrEmpty()){
                setRequestProperty("Content-Type", "application/json;charset=utf-8")
                OutputStreamWriter(outputStream).use { it.write(json) }
            }
        }

        if(con.responseCode != 200 && con.responseCode != 201){

        }

        val jsonText = BufferedReader(
            InputStreamReader(
                con.inputStream,
                StandardCharsets.UTF_8
            )
        ).use { it.readText() }

        return fromJson(jsonText)
    }

    inline fun<reified T> get(arg: String){
        return request(arg, "GET")
    }
    inline fun<reified T> put(arg: String, json: String = ""){
        return request(arg, "PUT", json)
    }
    inline fun<reified T> post(arg: String, json: String = ""){
        return request(arg, "POST", json)
    }
    inline fun<reified T> delete(arg: String){
        return request(arg, "DELETE")
    }

    class ApiException(val responseCode:Int, val responseMessage:String, val errorJson: String):
        Exception()
}