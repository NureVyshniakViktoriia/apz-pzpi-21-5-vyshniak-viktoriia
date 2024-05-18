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
import okhttp3.OkHttpClient
import retrofit2.Callback
import retrofit2.Retrofit
import retrofit2.converter.gson.GsonConverterFactory
import java.time.LocalDateTime

class ApiRepositoryImpl : ApiRepository {

    private val client = OkHttpClient.Builder().build()

    private val retrofit = Retrofit.Builder()
        .baseUrl("https://9982-91-142-173-12.ngrok-free.app")
        .addConverterFactory(GsonConverterFactory.create())
        .client(client)
        .build()

    private val service = retrofit.create(ApiService::class.java)

    override fun login(user: LoginRequest, callback: Callback<LoginResponse>) {
        service.login(user).enqueue(callback)
    }

    override fun register(user: UserRequest, callback: Callback<Void>) {
        service.register(user).enqueue(callback)
    }

    override fun getUser(token: String, userId: Int, callback: Callback<UserResponse>) {
        service.getUser(token, userId).enqueue(callback)
    }

    override fun updateUser(token: String, user: UpdateUserRequest, callback: Callback<Void>) {
        service.updateUser(token, user).enqueue(callback)
    }

    override fun getPetsByOwnerId(
        token: String, ownerId: Int,
        callback: Callback<List<PetListItem>>
    ) {
        service.getPetsByOwnerId(token, ownerId).enqueue(callback)
    }

    override fun getPetById(token: String, petId: String, callback: Callback<PetModel>) {
        service.getPetById(token, petId).enqueue(callback)
    }

    override fun getHealthRecords(
        token: String,
        petId: String,
        startDate: LocalDateTime?,
        endDate: LocalDateTime,
        callback: Callback<List<HealthRecordModel>>
    ) {
        service.getHealthRecords(token, petId, startDate, endDate).enqueue(callback)
    }

    override fun getAllForPet(
        token: String,
        petId: String,
        callback: Callback<List<DiaryNoteListItem>>
    ) {
        service.getAllForPet(token, petId).enqueue(callback)
    }

    override fun getNoteById(
        token: String,
        diaryNoteId: String,
        callback: Callback<DiaryNoteModel>
    ) {
        service.getNoteById(token, diaryNoteId).enqueue(callback)
    }

    override fun createNote(
        token: String,
        diaryNoteRequest: DiaryNoteCreateRequest,
        callback: Callback<Void>
    ) {
        service.createNote(token, diaryNoteRequest).enqueue(callback)
    }

    override fun uploadDocument(
        token: String,
        diaryNoteRequest: DiaryNoteRequest,
        callback: Callback<Void>
    ) {
        service.uploadDocument(token, diaryNoteRequest).enqueue(callback)
    }

    override fun downloadDocument(
        token: String,
        diaryNoteId: String,
        callback: Callback<DiaryNoteDocumentModel>
    ) {
        service.downloadDocument(token, diaryNoteId).enqueue(callback)
    }

    override fun getNotificationsByUserId(
        token: String,
        userId: Int,
        callback: Callback<List<NotificationListItem>>
    ) {
        service.getNotificationsByUserId(token, userId).enqueue(callback)
    }

    override fun getNotificationById(
        token: String,
        notificationId: String,
        callback: Callback<NotificationModel>
    ) {
        service.getNotificationById(token, notificationId).enqueue(callback)
    }

    override fun getInstitutionList(
        token: String,
        searchQuery: String?,
        type: Int?,
        sortByRatingAscending: Boolean,
        callback: Callback<List<InstitutionListItem>>
    ) {
        service.getInstitutionList(token, searchQuery, type, sortByRatingAscending)
            .enqueue(callback)
    }

    override fun getInstitutionById(
        token: String,
        institutionId: Int,
        callback: Callback<InstitutionModel>
    ) {
        service.getInstitutionById(token, institutionId).enqueue(callback)
    }

    override fun setRating(token: String, ratingRequest: RatingRequest, callback: Callback<Void>) {
        service.setRating(token, ratingRequest).enqueue(callback)
    }

    override fun getAllFacilitiesByInstitutionId(
        token: String,
        institutionId: Int,
        callback: Callback<List<FacilityListItem>>
    ) {
        service.getAllFacilitiesByInstitutionId(token, institutionId).enqueue(callback)
    }

    override fun getCurrentPetTemp(
        token: String,
        petId: String,
        callback: Callback<Double>
    ) {
        service.getCurrentPetTemp(token, petId).enqueue(callback)
    }

    override fun getCurrentPetLocation(
        token: String,
        petId: String,
        callback: Callback<GPSTrackerResponse>
    ) {
        service.getCurrentPetLocation(token, petId).enqueue(callback)
    }
}
