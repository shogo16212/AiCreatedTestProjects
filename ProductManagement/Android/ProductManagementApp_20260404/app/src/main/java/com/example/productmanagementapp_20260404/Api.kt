package com.example.productmanagementapp_20260404

import android.R
import java.io.BufferedReader
import java.io.InputStreamReader
import java.io.OutputStreamWriter
import java.net.HttpURLConnection
import java.net.URL
import java.nio.charset.StandardCharsets

object Api {

    fun request(arg: String, method: String, json: String = ""): String{
        val url = URL("http://10.0.2.2:5001/${arg}")
        val con= url.openConnection() as HttpURLConnection

        con.apply {
            connectTimeout = 3000
            requestMethod = method
            if(json.isNotEmpty()){
                setRequestProperty("Content-Type", "application/json;charset=utf-8")
                OutputStreamWriter(getOutputStream()).use { it.write(json) }
            }
        }

        val code = con.responseCode

        if(code!=200&&code!=201&&code!=204){
            val json = BufferedReader(
                InputStreamReader(
                    con.errorStream,
                    StandardCharsets.UTF_8
                )
            ).use { it.readText() }

            throw ApiException(code, con.responseMessage, json)
        }

        val json = BufferedReader(
            InputStreamReader(
                con.getInputStream(),
                StandardCharsets.UTF_8
            )
        ).use { it.readText() }

        return json
    }

    inline fun<reified T> get(arg: String):T{
        return fromJson(request(arg, "GET"))
    }
    inline fun<reified T> post(arg: String, json: String = ""):T{
        return fromJson(request(arg, "POST", json))
    }
    inline fun<reified T> put(arg: String, json: String = ""):T{
        return fromJson(request(arg, "PUT", json))
    }
    inline fun<reified T> delete(arg: String):T{
        return fromJson(request(arg, "DELETE"))
    }
    class ApiException(val responseCode:Int, val responseMessage: String, val errorJson: String):
        Exception()
}