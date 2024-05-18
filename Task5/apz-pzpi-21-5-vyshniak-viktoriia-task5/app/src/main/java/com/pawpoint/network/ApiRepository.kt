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
import retrofit2.Callback
import java.time.LocalDateTime

interface ApiRepository {
    fun login(
        user: LoginRequest,
        callback: Callback<LoginResponse>
    )

    fun register(
        user: UserRequest,
        callback: Callback<Void>
    )

    fun getUser(
        token: String,
        userId: Int,
        callback: Callback<UserResponse>
    )

    fun updateUser(
        token: String,
        user: UpdateUserRequest,
        callback: Callback<Void>
    )

    fun getPetsByOwnerId(
        token: String,
        ownerId: Int,
        callback: Callback<List<PetListItem>>
    )

    fun getPetById(
        token: String,
        petId: String,
        callback: Callback<PetModel>
    )

    fun getHealthRecords(
        token: String,
        petId: String,
        startDate: LocalDateTime?,
        endDate: LocalDateTime,
        callback: Callback<List<HealthRecordModel>>
    )

    fun getAllForPet(
        token: String,
        petId: String,
        callback: Callback<List<DiaryNoteListItem>>
    )

    fun getNoteById(
        token: String,
        diaryNoteId: String,
        callback: Callback<DiaryNoteModel>
    )

    fun createNote(
        token: String,
        diaryNoteRequest: DiaryNoteCreateRequest,
        callback: Callback<Void>
    )

    fun uploadDocument(
        token: String,
        diaryNoteRequest: DiaryNoteRequest,
        callback: Callback<Void>
    )

    fun downloadDocument(
        token: String,
        diaryNoteId: String,
        callback: Callback<DiaryNoteDocumentModel>
    )

    fun getNotificationsByUserId(
        token: String,
        userId: Int,
        callback: Callback<List<NotificationListItem>>
    )

    fun getNotificationById(
        token: String,
        notificationId: String,
        callback: Callback<NotificationModel>
    )

    fun getInstitutionList(
        token: String,
        searchQuery: String?,
        type: Int?,
        sortByRatingAscending: Boolean,
        callback: Callback<List<InstitutionListItem>>
    )

    fun getInstitutionById(
        token: String,
        institutionId: Int,
        callback: Callback<InstitutionModel>
    )

    fun setRating(
        token: String,
        ratingRequest: RatingRequest,
        callback: Callback<Void>
    )

    fun getAllFacilitiesByInstitutionId(
        token: String,
        institutionId: Int,
        callback: Callback<List<FacilityListItem>>
    )

    fun getCurrentPetTemp(
        token: String,
        petId: String,
        callback: Callback<Double>
    )

    fun getCurrentPetLocation(
        token: String,
        petId: String,
        callback: Callback<GPSTrackerResponse>
    )
}
