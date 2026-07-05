package com.example.echoshelf_mobile_1

import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.fragment.app.Fragment
import com.example.echoshelf_mobile_1.databinding.FragmentHomeBinding

class HomeFragment : Fragment() {
    private val b by lazy { FragmentHomeBinding.inflate(layoutInflater) }
    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {


        return View(requireContext())
    }
}