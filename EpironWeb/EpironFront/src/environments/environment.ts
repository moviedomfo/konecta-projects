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
  
  iddleTimeout_seconds:8000,//idle timeout of 10' 
  iddle_waite_Timeout_seconds : 10 ,//period of time in seconds. after 10 seconds of inactivity, the user will be considered timed out.
  version: '1.0 beta'  ,
  culture:'es-AR',
  appInstanceGUID :'75CFEFE4-5A79-E411-BD73-0022640637C2',
  appId: 'epiron',
  fwkServiceProvider_Epiron:'epiron',//fwk service metadata provider : en la configuracion por defecto que uilizaran los servicios SVC del FWK

};
/*
 * For easier debugging in development mode, you can import the following file
 * to ignore zone related error stack frames such as `zone.run`, `zoneDelegate.invokeTask`.
 *
 * This import should be commented out in production mode because it will have a negative impact
 * on performance if an error is thrown.
 */
// import 'zone.js/dist/zone-error';  // Included with Angular CLI.
