// This file can be replaced during build by using the `fileReplacements` array.
// `ng build --prod` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.

export const environment = {
  production: false,
  AppAPI_BaseURL:"http://localhost:54900/",
  AppOAuth_BaseUrl:"http://localhost:54900/", 
  //  AppAPI_BaseURL:"http://10.200.1.239:50009/",
  //  AppOAuth_BaseUrl:"http://10.200.1.239:50009/", 
  oaut_client_id:'reteteosClient',
  oaut_client_secret:'pletorico28',
  oaut_securityProviderName:'sec_reseteos',
  version: '1.7'  ,
  recaptcha_key:'6LeJ__wUAAAAAIK8kLmcJ47jq0Y7o5W1czIMdY3b'
  
};

/*
 * For easier debugging in development mode, you can import the following file
 * to ignore zone related error stack frames such as `zone.run`, `zoneDelegate.invokeTask`.
 *
 * This import should be commented out in production mode because it will have a negative impact
 * on performance if an error is thrown.
 */
// import 'zone.js/dist/zone-error';  // Included with Angular CLI.
