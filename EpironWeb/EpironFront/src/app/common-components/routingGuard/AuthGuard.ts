import { Injectable } from '@angular/core';
import { Router, CanActivate, CanDeactivate, UrlTree, RouterStateSnapshot, ActivatedRouteSnapshot } from '@angular/router';
import { AuthenticationService } from '../../service';
import { helperFunctions } from 'src/app/service/helperFunctions';

@Injectable()
export class AuthGuard implements CanActivate {

    constructor(private router: Router, private authService: AuthenticationService, ) { }


    // canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean | UrlTree {
    //     const url = 'target';
    //     const tree: UrlTree = this.router.parseUrl(url);

    //     if (this.authService.isAuth() === true) {

    //         return true;
    //     }
    //     this.router.navigate(['/login'], { queryParams: { returnUrl: 'stock' } });
    //    return false;
    //   }
    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {


        // this.router.routerState.root.url.subscribe(url => {


        //   });
        if (this.authService.isAuth() === true) {
            return true;
        }

           
        if (helperFunctions.string_IsNullOrEmpty(state.url))
            this.router.navigate(['']);
        else
            //this.router.navigate(['/'+returnUrl]);
            //this.router.navigate(['/login'], { queryParams: { returnUrl: returnUrl } });
            this.router.navigate(['login'], { queryParams: { returnUrl: state.url } });

        this.authService.signOut();

        // this.router.routerState.root.queryParams.subscribe(queryParams => {
        //     let returnUrl = queryParams["returnUrl"];
        //     //alert(returnUrl);
        //     if (helperFunctions.string_IsNullOrEmpty(returnUrl))
        //       this.router.navigate(['']);
        //     else
        //       //this.router.navigate(['/'+returnUrl]);
        //       this.router.navigate(['/login'], { queryParams: { returnUrl: returnUrl } });


        //       this.authService.signOut();
        //   });

        //this.router.navigate(['/login'],{ queryParams: { returnUrl: this.router.url } });

        return false;
    }
}

