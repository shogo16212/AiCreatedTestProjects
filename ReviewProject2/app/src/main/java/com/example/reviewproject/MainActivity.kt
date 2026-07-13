package com.example.reviewproject

import android.content.Intent
import android.graphics.Bitmap
import android.os.Bundle
import android.provider.MediaStore
import androidx.activity.enableEdgeToEdge
import androidx.appcompat.app.AppCompatActivity
import androidx.core.view.ViewCompat
import androidx.core.view.WindowInsetsCompat
import com.example.reviewproject.databinding.ActivityMainBinding

class MainActivity : AppCompatActivity() {
    private val b by lazy { ActivityMainBinding.inflate(layoutInflater) }
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(b.root)

        b.b1.setOnClickListener {
            startActivityForResult(Intent(Intent.ACTION_GET_CONTENT).apply { type = "image/*" }, 1)
        }

        b.b1.setOnClickListener {
            startActivityForResult(Intent(MediaStore.ACTION_IMAGE_CAPTURE), 1000)
        }
    }

    override fun onActivityResult(requestCode: Int, resultCode: Int, data: Intent?) {
        super.onActivityResult(requestCode, resultCode, data)
        if(requestCode == 1 && resultCode == RESULT_OK){
            val bitmap = MediaStore.Images.Media.getBitmap(contentResolver, data?.data)
            b.im1.setImageBitmap(bitmap)
        }
    }
}