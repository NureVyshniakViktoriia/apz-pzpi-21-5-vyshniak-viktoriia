import { Component, Inject, Input, inject } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { UserService } from '../../../core/services/user.service';
import { L10N_LOCALE } from 'angular-l10n';
import { EnumService } from '../../../core/services/enum.service';
import { Role } from '../../../core/enums/role';
import { ToastService } from '../../../core/services/toast.service';

@Component({
	selector: 'app-set-user-role',
	templateUrl: './set-user-role.component.html',
	styleUrl: './set-user-role.component.scss'
})
export class SetUserRoleComponent {
	@Input()
	userLogin!: string;
	@Input()
	userId!: number;
	@Input()
	onSaveCallback!: () => void;

	userForm!: FormGroup;
	activeModal = inject(NgbActiveModal);

	locale: any;

	constructor(
		private fb: FormBuilder,
		private userService: UserService,
		private enumService: EnumService,
		private toastService: ToastService,
		@Inject(L10N_LOCALE) locale: any
	) {
		this.locale = locale;
	}

	ngOnInit(): void {
		this.initUserForm();
		this.initUser();
	}

	initUserForm(): void {
		this.userForm = this.fb.group({
			userId: [null],
			role: [null]
		});
	}

	initUser(): void {
		this.userService.getUserById(this.userId)
			.subscribe(user => {
				this.userForm.patchValue(user);
			});
	}

	setUserRole(): void {
		if (this.userForm.valid) {
			this.userService.setUserRole(this.userForm.value).subscribe({
				next: () => {
					this.onSaveCallback();
					this.activeModal.close(false);
				},
				error: () => {
					this.toastService.showErrorToast();
				},
				complete: () => {
					this.toastService.showSuccessToast();
				}
			});
		} else {
			this.toastService.showErrorToast();
		}
	}

	getRoleOptions(): any[] {
		return this.enumService.getEnumOptions(Role);
	}
}
