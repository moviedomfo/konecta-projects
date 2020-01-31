// This file can be replaced during build by using the `fileReplacements` array.
// `ng build --prod` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.

export const environment = {
  production: false,
  App_BaseURL:"http://localhost:44345/",
  AppOAuth_BaseUrl:"http://localhost:44345/", 
  oaut_client_id: 'nodeJSClient',//'reteteosClient',
  oaut_client_secret:'pletorico28',
  oaut_securityProviderName:'epironWeb',
  
  iddleTimeout_seconds:800,//idle timeout of 10' 
  iddle_waite_Timeout_seconds : 10 ,//period of time in seconds. after 10 seconds of inactivity, the user will be considered timed out.
  version: '1.0 beta'  ,
  culture:'es-AR',
  appInstanceGUID :'0685749B-AB4B-E311-A348-000C292448BD'
  
};
/*
 * For easier debugging in development mode, you can import the following file
 * to ignore zone related error stack frames such as `zone.run`, `zoneDelegate.invokeTask`.
 *
 * This import should be commented out in production mode because it will have a negative impact
 * on performance if an error is thrown.
 */
// import 'zone.js/dist/zone-error';  // Included with Angular CLI.
