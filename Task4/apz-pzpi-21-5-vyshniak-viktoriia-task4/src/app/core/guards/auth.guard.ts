import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { StorageService } from '../services/storage.service';

@Injectable({
	providedIn: 'root'
})
export class AuthGuard implements CanActivate {

	constructor (
		private storageService: StorageService,
		private router: Router
	) { }

	canActivate(
		route: ActivatedRouteSnapshot,
		state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
		const roles = route.data['roles'] as string[];

		if (roles) {
			if (roles.includes('admin')) {
				return this.canActivateAdmin(route, state);
			}

			if (roles.includes('user')) {
				return this.canActivateUser(route, state);
			}

			if (roles.includes('sysAdmin')) {
				return this.canActivateSysAdmin(route, state);
			}
		}

		if (this.storageService.isLoggedIn()){
			return true;
		}
	  
		this.router.navigate(['/']);
		return false;
	}

	canActivateUser(
		route: ActivatedRouteSnapshot,
		state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
		if (this.storageService.isLoggedIn() && this.storageService.getUserRole() == 'User') {
			return true;
		}

		if (!this.storageService.isLoggedIn()) {
			return this.router.navigate(['/']);
		}
		
		return false;
	}

	canActivateAdmin(
		route: ActivatedRouteSnapshot,
		state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
		if (this.storageService.isLoggedIn() && this.storageService.getUserRole() == 'Admin') {
			return true;
		}

		if (!this.storageService.isLoggedIn()) {
			return this.router.navigate(['/']);
		}
		
		return false;
	}

	canActivateSysAdmin(
		route: ActivatedRouteSnapshot,
		state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
		if (this.storageService.isLoggedIn() && this.storageService.getUserRole() == 'SysAdmin') {
			return true;
		}

		if (!this.storageService.isLoggedIn()) {
			return this.router.navigate(['/']);
		}

		return false;
	}
}
