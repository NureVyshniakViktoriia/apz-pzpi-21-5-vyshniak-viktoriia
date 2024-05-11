const getAll = 'get-all';
const apply = 'apply';
const deleteById = 'delete';

const authArea = 'auth';
const login = 'login';
const refresh = 'get-user-by-refresh-token';

const userArea = 'user';
const register = 'register';
const setUserRole = 'set-user-role';
const getUserProfile = 'get-user-profile';
const doDbBackup = 'do-db-backup';

const petArea = 'pet';
const getAllByOwnerId = 'get-all-by-owner-id';
const getPetById = 'get-pet-by-id';
const getPetSettings = 'get-arduino-settings-by-pet-id';

const arduinoArea = 'arduino';
const configurePetDevice = 'configure-pet-device';
const configureRfidReader = 'configure-rfid-reader';

const facilityArea = 'facility';
const getFacilityById = 'get-facility-by-id';
const getAllByInstitutionId = 'get-all-by-institution-id';

const diaryNoteArea = 'diary-note';
const downloadDocument = 'download-document';
const uploadDocument = 'upload-document';
const getNoteById = 'get-note-by-id';
const getAllForPet = 'get-all-for-pet';

const institutionArea = 'institution';
const list = 'list';
const getInstitutionById = 'get-institution-by-id';
const uploadLogo = 'upload-logo';
const removeFacilityInstitution = 'remove-facility-institution';
const addFacilityInstitution = 'add-facility-institution';
const setRating = 'set-rating';
const getByOwnerId = 'get-by-owner-id';
const getRfidSettingsById = 'get-rfid-settings-by-id';

export const api = {
	auth: {
		login: `${authArea}/${login}`,
		refresh: `${authArea}/${refresh}`
	},
	user: {
		register: `${userArea}/${register}`,
		getAll: `${userArea}/${getAll}`,
		setUserRole: `${userArea}/${setUserRole}`,
		getUserProfile: `${userArea}/${getUserProfile}`,
		doDbBackup: `${userArea}/${doDbBackup}`
	},
    pet: {
		apply: `${petArea}/${apply}`,
		delete: `${petArea}/${deleteById}`,
		getPetsByOwnerId: `${petArea}/${getAllByOwnerId}`,
		getPetById: `${petArea}/${getPetById}`,
		getAll: `${petArea}/${getAll}`,
		getPetSettings: `${petArea}/${getPetSettings}`
	},
	arduino: {
		configurePetDevice: `${arduinoArea}/${configurePetDevice}`,
		configureRfidReader: `${arduinoArea}/${configureRfidReader}`,
	},
	facility: {
		apply: `${facilityArea}/${apply}`,
		delete: `${facilityArea}/${deleteById}`,
		getAllByInstitutionId: `${facilityArea}/${getAllByInstitutionId}`,
		getFacilityById: `${facilityArea}/${getFacilityById}`,
		getAll: `${facilityArea}/${getAll}`,
	},
	diaryNote: {
		apply: `${diaryNoteArea}/${apply}`,
		delete: `${diaryNoteArea}/${deleteById}`,
		downloadDocument: `${diaryNoteArea}/${downloadDocument}`,
		uploadDocument: `${diaryNoteArea}/${uploadDocument}`,
		getNoteById: `${diaryNoteArea}/${getNoteById}`,
		getAllForPet: `${diaryNoteArea}/${getAllForPet}`,
	},
	institution: {
		apply: `${institutionArea}/${apply}`,
		delete: `${institutionArea}/${deleteById}`,
		list: `${institutionArea}/${list}`,
		getInstitutionById: `${institutionArea}/${getInstitutionById}`,
		uploadLogo: `${institutionArea}/${uploadLogo}`,
		removeFacilityInstitution: `${institutionArea}/${removeFacilityInstitution}`,
		addFacilityInstitution: `${institutionArea}/${addFacilityInstitution}`,
		setRating: `${institutionArea}/${setRating}`,
		getAllByOwnerId: `${institutionArea}/${getByOwnerId}`,
		getRfidSettingsById: `${institutionArea}/${getRfidSettingsById}`
	}
}
