import { Component, Inject } from '@angular/core';
import { UserProfileModel } from '../../../core/models/user/user-profile-model';
import { Observable, of } from 'rxjs';
import { L10N_LOCALE } from 'angular-l10n';
import { UserService } from '../../../core/services/user.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { SetUserRoleComponent } from '../set-user-role/set-user-role.component';
import { EnumService } from '../../../core/services/enum.service';
import { Role } from '../../../core/enums/role';
import { ToastService } from '../../../core/services/toast.service';

@Component({
	selector: 'app-users',
	templateUrl: './user-list.component.html',
	styleUrls: ['./user-list.component.scss']
})
export class UserListComponent {
	users$!: Observable<UserProfileModel[]>;

	isLoading: boolean = false;
	locale: any;

	constructor(
		private userService: UserService,
		private modalService: NgbModal,
		private enumService: EnumService,
		private toastService: ToastService,
		@Inject(L10N_LOCALE) locale: any
	) {
		this.locale = locale;
	}

	ngOnInit(): void {
		this.loadUsers();
	}

	loadUsers(): void {
		this.isLoading = true;
		setTimeout(() => {
			this.userService.getAllUsers().subscribe({
				next: (result) => {
					this.users$ = of(result);
				},
				complete: () => {
					this.isLoading = false;
				}
			});
		}, 1500);
	}

	doDatabaseBackup(): void {
		this.userService.doDatabaseBackup().subscribe({
			error: () => {
				this.toastService.showErrorToast();
			},
			complete: () => {
				this.toastService.showSuccessToast();
			}
		});
	}

	openSetUserRoleModal(userId: number, userLogin: string): void {
		const modalRef = this.modalService.open(SetUserRoleComponent, { size: 'md' });
		modalRef.componentInstance.onSaveCallback = () => this.loadUsers();
		modalRef.componentInstance.userId = userId;
		modalRef.componentInstance.userLogin = userLogin;
	}

	getUserRoleName(key: Role) {
		return this.enumService.getEnumKeyByValue(Role, key);
	}
}
