import { Injectable } from '@angular/core';
import { L10nTranslationService } from 'angular-l10n';

@Injectable({
	providedIn: 'root'
})
export class EnumService {

	constructor(
		private translateService: L10nTranslationService
	) {}

	public getEnumOptions(enumType: any): any[] {
		const options = [];

		for (const key in enumType) {
			if (!isNaN(Number(enumType[key]))) {
				options.push({
					value: enumType[key],
					label: this.translateService.translate(key.toLowerCase())
				});
			}
		}

		return options;
	}

	public getEnumKeyByValue(enumType: any, enumValue: any): string {
		for (const key in enumType) {
			if (enumType[key] === enumValue) {
				return this.translateService.translate(key.toLowerCase());
			}
		}
		return '';
	}
}
