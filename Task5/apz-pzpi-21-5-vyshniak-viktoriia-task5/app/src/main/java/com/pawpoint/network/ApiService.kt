package com.pawpoint.network

import com.pawpoint.model.request.DiaryNoteCreateRequest
import com.pawpoint.model.request.DiaryNoteRequest
import com.pawpoint.model.request.LoginRequest
import com.pawpoint.model.request.RatingRequest
import com.pawpoint.model.request.UpdateUserRequest
import com.pawpoint.model.request.UserRequest
import com.pawpoint.model.response.DiaryNoteDocumentModel
import com.pawpoint.model.response.DiaryNoteListItem
import com.pawpoint.model.response.DiaryNoteModel
import com.pawpoint.model.response.FacilityListItem
import com.pawpoint.model.response.GPSTrackerResponse
import com.pawpoint.model.response.HealthRecordModel
import com.pawpoint.model.response.InstitutionListItem
import com.pawpoint.model.response.InstitutionModel
import com.pawpoint.model.response.LoginResponse
import com.pawpoint.model.response.NotificationListItem
import com.pawpoint.model.response.NotificationModel
import com.pawpoint.model.response.PetListItem
import com.pawpoint.model.response.PetModel
import com.pawpoint.model.response.TemperatureMonitorResponse
import com.pawpoint.model.response.UserResponse
import retrofit2.Call
import retrofit2.http.Body
import retrofit2.http.GET
import retrofit2.http.Header
import retrofit2.http.Headers
import retrofit2.http.POST
import retrofit2.http.Query
import java.time.LocalDateTime

interface ApiService {

    @Headers("Content-Type: application/json")
    @POST("/api/auth/login")
    fun login(@Body loginData: LoginRequest): Call<LoginResponse>

    @POST("/api/user/register")
    fun register(@Body registrationData: UserRequest): Call<Void>

    @GET("/api/user/get-user-profile")
    fun getUser(
        @Header("Authorization") token: String,
        @Query("userId") userId: Int
    ): Call<UserResponse>

    @POST("/api/user/update-user-info")
    @Headers("Content-Type: application/json")
    fun updateUser(
        @Header("Authorization") token: String,
        @Body user: UpdateUserRequest
    ): Call<Void>

    @GET("/api/pet/get-all-by-owner-id")
    fun getPetsByOwnerId(
        @Header("Authorization") token: String,
        @Query("ownerId") ownerId: Int
    ): Call<List<PetListItem>>

    @GET("/api/pet/get-pet-by-id")
    fun getPetById(
        @Header("Authorization") token: String,
        @Query("petId") petId: String
    ): Call<PetModel>

    @GET("/api/health-record/get-health-records")
    fun getHealthRecords(
        @Header("Authorization") token: String,
        @Query("petId") petId: String,
        @Query("startDate") startDate: LocalDateTime?,
        @Query("endDate") endDate: LocalDateTime?
    ): Call<List<HealthRecordModel>>

    @GET("/api/diary-note/get-all-for-pet")
    fun getAllForPet(
        @Header("Authorization") token: String,
        @Query("petId") petId: String
    ): Call<List<DiaryNoteListItem>>

    @GET("/api/diary-note/get-note-by-id")
    fun getNoteById(
        @Header("Authorization") token: String,
        @Query("diaryNoteId") diaryNoteId: String
    ): Call<DiaryNoteModel>

    @POST("/api/diary-note/apply")
    @Headers("Content-Type: application/json")
    fun createNote(
        @Header("Authorization") token: String,
        @Body diaryNoteRequest: DiaryNoteCreateRequest
    ): Call<Void>

    @POST("/api/diary-note/upload-document")
    @Headers("Content-Type: application/json")
    fun uploadDocument(
        @Header("Authorization") token: String,
        @Body diaryNoteRequest: DiaryNoteRequest
    ): Call<Void>

    @GET("/api/diary-note/download-document")
    fun downloadDocument(
        @Header("Authorization") token: String,
        @Query("diaryNoteId") diaryNoteId: String
    ): Call<DiaryNoteDocumentModel>

    @GET("/api/notification/get-notifications-by-user-id")
    fun getNotificationsByUserId(
        @Header("Authorization") token: String,
        @Query("userId") userId: Int
    ): Call<List<NotificationListItem>>

    @GET("/api/notification/get-notification-by-id")
    fun getNotificationById(
        @Header("Authorization") token: String,
        @Query("notificationId") notificationId: String
    ): Call<NotificationModel>

    @GET("/api/institution/list")
    fun getInstitutionList(
        @Header("Authorization") token: String,
        @Query("searchQuery") searchQuery: String?,
        @Query("type") type: Int?,
        @Query("sortByRatingAscending") sortByRatingAscending: Boolean?
    ): Call<List<InstitutionListItem>>

    @GET("/api/institution/get-institution-by-id")
    fun getInstitutionById(
        @Header("Authorization") token: String,
        @Query("institutionId") institutionId: Int
    ): Call<InstitutionModel>

    @POST("/api/institution/set-rating")
    @Headers("Content-Type: application/json")
    fun setRating(
        @Header("Authorization") token: String,
        @Body ratingRequest: RatingRequest
    ): Call<Void>

    @GET("/api/facility/get-all-by-institution-id")
    fun getAllFacilitiesByInstitutionId(
        @Header("Authorization") token: String,
        @Query("institutionId") institutionId: Int
    ): Call<List<FacilityListItem>>

    @GET("/api/arduino/get-pet-temperature-data")
    fun getCurrentPetTemp(
        @Header("Authorization") token: String,
        @Query("petId") petId: String
    ): Call<Double>

    @GET("/api/arduino/get-pet-location-data")
    fun getCurrentPetLocation(
        @Header("Authorization") token: String,
        @Query("petId") petId: String
    ): Call<GPSTrackerResponse>
}
