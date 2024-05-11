import { JwtHelperService, JwtModule } from "@auth0/angular-jwt";
import { AppComponent } from "./app.component";
import { AuthGuard } from "./core/guards/auth.guard";
import { httpInterceptorProviders } from "./core/helpers/http-interceptor.service";
import { L10nDateDirective, L10nIntlModule, L10nTranslationModule } from "angular-l10n";
import { HttpClient, HttpClientModule } from "@angular/common/http";
import { AppRoutingModule } from "./app-routing.module";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { TranslateLoader, TranslateModule } from "@ngx-translate/core";
import { BrowserModule } from "@angular/platform-browser";
import { TranslateHttpLoader } from "@ngx-translate/http-loader";
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule } from "@angular/core";
import { ToastrModule } from "ngx-toastr";
import { TranslationLoader, l10nConfig } from "./l10n-config";
import { FooterComponent } from './components/shared/footer/footer.component';
import { NavbarComponent } from "./components/shared/navbar/navbar.component";
import { LoginComponent } from './components/login/login.component';
import { UserListComponent } from './components/user/user-list/user-list.component';
import { RegistrationComponent } from './components/registration/registration.component';
import { InstitutionsComponent } from "./components/institution/institutions/institutions.component";
import { PetAdminListComponent } from './components/pet/pet-admin-list/pet-admin-list.component';
import { InstitutionFilterComponent } from './components/institution/institution-filter/institution-filter.component';
import { InstitutionListComponent } from "./components/institution/institution-list/institution-list.component";
import { InstitutionViewComponent } from './components/institution/institution-view/institution-view.component';
import { FacilityListComponent } from './components/facility/facility-list/facility-list.component';
import { CreateUpdateInstitutionComponent } from './components/institution/create-update-institution/create-update-institution.component';
import { PetListComponent } from './components/pet/pet-list/pet-list.component';
import { CreateUpdatePetComponent } from './components/pet/create-update-pet/create-update-pet.component';
import { DiaryNoteListComponent } from './components/diary-note/diary-note-list/diary-note-list.component';
import { CreateUpdateDiaryNoteComponent } from './components/diary-note/create-update-diary-note/create-update-diary-note.component';
import { DatePipe } from "@angular/common";
import { ConfigurePetComponent } from './components/pet/configure-pet/configure-pet.component';
import { SetUserRoleComponent } from './components/user/set-user-role/set-user-role.component';
import { SetInstitutionRatingComponent } from './components/institution/set-institution-rating/set-institution-rating.component';

export function createTranslateLoader(http: HttpClient) {
    return new TranslateHttpLoader(http, './assets/i18n/', '.json');
}

@NgModule({
	declarations: [
		AppComponent,
		NavbarComponent,
		FooterComponent,
		LoginComponent,
		RegistrationComponent,
		UserListComponent,
		InstitutionsComponent,
		PetAdminListComponent,
		InstitutionFilterComponent,
		InstitutionListComponent,
  		InstitutionViewComponent,
    	FacilityListComponent,
     CreateUpdateInstitutionComponent,
     PetListComponent,
     CreateUpdatePetComponent,
     DiaryNoteListComponent,
     CreateUpdateDiaryNoteComponent,
     ConfigurePetComponent,
     SetUserRoleComponent,
     SetInstitutionRatingComponent
	],
	imports: [
		BrowserModule,
		BrowserAnimationsModule,
		ToastrModule.forRoot({
			positionClass: 'toast-bottom-right',
			closeButton: true,
			progressBar: true
		}),
		TranslateModule.forRoot({
			loader: {
				provide: TranslateLoader,
				useFactory: createTranslateLoader,
				deps: [HttpClient]
			}
		}),
		L10nTranslationModule.forRoot(
			l10nConfig,
			{
				translationLoader: TranslationLoader
			}
		),
		L10nIntlModule,
		L10nDateDirective,
		FormsModule,
		ReactiveFormsModule,
		AppRoutingModule,
		HttpClientModule,
		JwtModule.forRoot({
			config: {
				tokenGetter: () => localStorage.getItem('access_token'),
				allowedDomains: ['example.com'],
				disallowedRoutes: ['example.com/login'],
				},
		})
	],
	exports: [
		L10nTranslationModule
	],
	providers: [
		httpInterceptorProviders,
		JwtHelperService,
		AuthGuard,
		DatePipe 
	],
	bootstrap: [
		AppComponent
	]
})

export class AppModule { }