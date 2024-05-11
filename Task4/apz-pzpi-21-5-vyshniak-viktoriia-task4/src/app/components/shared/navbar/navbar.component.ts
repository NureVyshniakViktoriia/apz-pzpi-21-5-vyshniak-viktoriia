import { Component, Inject } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { L10N_CONFIG, L10N_LOCALE, L10nConfig, L10nLocale, L10nTranslationService } from 'angular-l10n';
import { StorageService } from '../../../core/services/storage.service';
import { ToastService } from '../../../core/services/toast.service';

@Component({
	selector: 'app-navbar',
	templateUrl: './navbar.component.html',
	styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent {
	navbarCollapsed = true;
	schema: any[];
	selectedLocale: L10nLocale;
	activeTab = '';

	constructor(
		private router: Router,
		private toastService: ToastService,
		private storageService: StorageService,
		@Inject(L10N_LOCALE) public locale: L10nLocale,
		@Inject(L10N_CONFIG) private config: L10nConfig,
		private translation: L10nTranslationService
	) {
		this.schema = this.config.schema;
		this.selectedLocale = this.schema[0].locale;
	}

	ngOnInit(): void {
		this.router.events.subscribe((event) => {
			if (event instanceof NavigationEnd) {
				this.setActiveTab(event.url);
			}
		});
	}

	setActiveTab(url: string): void {
		if (url.includes('institutions')) {
			this.activeTab = 'institutions';
		} else if (url.includes('users')) {
			this.activeTab = 'users';
		} else if (url.includes('pets')) {
			this.activeTab = 'pets';
		} else if (url.includes('pet-list')) {
			this.activeTab = 'pet-list';
		} else {
			this.activeTab = '';
		}
	}

	getLocale(): L10nLocale {
		return this.translation.getLocale();
	}

	setLocale(): void {
		this.translation.setLocale(this.selectedLocale);
		this.storageService.saveLocale(this.selectedLocale.language);
	}

	logout(): void {
		this.storageService.clear();
		this.router.navigate(['login']);
		this.toastService.showWarningToast('logout', 'logoutMessage');
	}

	isLoggedIn(): boolean {
		return this.storageService.isLoggedIn();
	}

	isLoggedInUser(): boolean {
		return this.storageService.getUserRole() == "User";
	}

	isLoggedInAdmin(): boolean {
		return this.storageService.getUserRole() == "Admin";
	}

	isLoggedInSysAdmin(): boolean {
		return this.storageService.getUserRole() == "SysAdmin";
	}

	setActive(tabName: string): void {
		this.activeTab = tabName;
	}

	reloadPage(): void {
		window.location.reload();
	}
}
