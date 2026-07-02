package com.example.echoshelf_mobile_1

data class ResponseError(
    val error: String
)

data class RequestPostLogin(
    val email: String,
    val password: String
)

data class ResponsePostLogin(
    val message: String,
    val data: Int
)