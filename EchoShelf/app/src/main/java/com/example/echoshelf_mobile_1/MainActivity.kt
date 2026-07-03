package com.example.echoshelf_mobile_1

import android.content.Intent
import android.os.Bundle
import android.os.StrictMode
import android.widget.Toast
import androidx.activity.enableEdgeToEdge
import androidx.appcompat.app.AppCompatActivity
import androidx.core.app.ShareCompat
import androidx.core.view.ViewCompat
import androidx.core.view.WindowInsetsCompat
import androidx.recyclerview.widget.RecyclerView
import com.example.echoshelf_mobile_1.databinding.ActivityMainBinding

class MainActivity : AppCompatActivity() {
    private val b by lazy { ActivityMainBinding.inflate(layoutInflater) }
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(b.root)
        StrictMode.setThreadPolicy(StrictMode.ThreadPolicy.Builder().permitAll().build())

        UserId = getSharedPreferences("DATA", MODE_PRIVATE).getInt("UserID", 0)
        if(UserId != 0){
            startActivity(Intent(this, MenuActivity::class.java))
            finish()
        }

        b.bt1.setOnClickListener {
            try{
                val loginData = RequestPostLogin(b.tb1.text.toString(), b.tb2.text.toString())
                val response = Api.post<ResponsePostLogin>("api/auth/login", gson.toJson(loginData))

                if(b.cb1.isChecked){
                    getSharedPreferences("DATA", MODE_PRIVATE).edit().apply{
                        putInt("UserID", response.data)
                        apply()
                    }
                }

                UserId = response.data

                startActivity(Intent(this, MenuActivity::class.java))
                finish()
            }catch (ex: Api.ApiException){
                val error = fromJson<ResponseError>(ex.errorJson)
                Toast.makeText(this, error.error, Toast.LENGTH_SHORT).show()
            }catch (ex: Exception){
                Toast.makeText(this, ex.message, Toast.LENGTH_SHORT).show()
            }
        }
    }
}