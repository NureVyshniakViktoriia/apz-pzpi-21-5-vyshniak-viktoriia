import { Routes } from '@angular/router';
import { AuthGuard } from './core/guards/auth.guard';
import { LoginComponent } from './components/login/login.component';
import { RegistrationComponent } from './components/registration/registration.component';
import { UserListComponent } from './components/user/user-list/user-list.component';
import { PetAdminListComponent } from './components/pet/pet-admin-list/pet-admin-list.component';
import { InstitutionsComponent } from './components/institution/institutions/institutions.component';
import { InstitutionViewComponent } from './components/institution/institution-view/institution-view.component';
import { CreateUpdateInstitutionComponent } from './components/institution/create-update-institution/create-update-institution.component';
import { PetListComponent } from './components/pet/pet-list/pet-list.component';
import { CreateUpdatePetComponent } from './components/pet/create-update-pet/create-update-pet.component';

export const routes: Routes = [
	{
		path: '',
		redirectTo: '/login',
		pathMatch: 'full'
	},
	{
		path: 'login',
		component: LoginComponent
	},
	{
		path: 'registration',
		component: RegistrationComponent
	},
	{
		path: 'users',
		component: UserListComponent,
		canActivate: [AuthGuard],
		data: { roles: ['sysAdmin'] }
	},
	{
		path: 'pets',
		component: PetAdminListComponent,
		canActivate: [AuthGuard],
	},
	{
		path: 'pet-list',
		component: PetListComponent,
		canActivate: [AuthGuard],
	},
	{
		path: 'institutions',
		component: InstitutionsComponent,
		canActivate: [AuthGuard],
	},
	{
		path: 'institution-view/:institutionId',
		component: InstitutionViewComponent,
		canActivate: [AuthGuard],
	},
	{
		path: 'institution-create-update/:institutionId',
		component: CreateUpdateInstitutionComponent,
		canActivate: [AuthGuard],
	},
	{
		path: 'pet-create-update/:petId',
		component: CreateUpdatePetComponent,
		canActivate: [AuthGuard],
	},
];
