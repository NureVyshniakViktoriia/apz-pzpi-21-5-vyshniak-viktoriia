import { Injectable } from "@angular/core";
import { L10nConfig, L10nProvider, L10nTranslationLoader } from "angular-l10n";
import { Observable, from } from "rxjs";
import { StorageService } from "./core/services/storage.service";

export const l10nConfig: L10nConfig = {
	format: 'language-region',
	providers: [
		{ name: 'app', asset: 'app' }
	],
	cache: true,
	keySeparator: '.',
	defaultLocale: { language: 'uk-UA', currency: 'UAH', timeZone: 'Europe/Kiev' },
	schema: [
		{ locale: { language: 'uk-UA', currency: 'UAH', timeZone: 'Europe/Kiev' } },
		{ locale: { language: 'en-US', currency: 'USD', timeZone: 'America/Los_Angeles' } },
	]
};

@Injectable() export class TranslationLoader implements L10nTranslationLoader {
	savedLocale!: string;

	constructor(private storageService: StorageService) {}

	public get(language: string, provider: L10nProvider): Observable<{ [key: string]: any }> {
		this.savedLocale = this.storageService.getLocale() ?? language;
		const data = import(`../i18n/${this.savedLocale}/${provider.asset}.json`);
		return from(data);
	}

	public getCurrentLocale(): string {
		return this.savedLocale;
	}
}
