JWT Concepts
 JWT typically looks like the following. xxxxx.yyyyy.zzzzz

Where yyyyy is Payload  which contains the claims :statements about an entity


iss (Issuer) : Quien autoriza  Identifica el proveedor de identidad que emitió el JWT

exp  (xpiration time)
aud (Audience) : dentifica la audiencia o receptores para lo que el JWT fue emitido. Cada servicio que recibe un JWT 
           para su validación tiene que controlar la audiencia a la que el JWT está destinado. 
           Si el proveedor del servicio no se encuentra presente en el campo aud, entonces el JWT tiene que ser 
           rechazado




Install-Package System.IdentityModel.Tokens.Jwt

Errores que sualen salir 



Settings previos al crear web api

		To disable OWIN startup discovery, add the appSetting owin:AutomaticAppStartup = false
		 <add key="owin:AutomaticAppStartup" value="true"/>
		To specify the OWIN startup Assembly, Class, or Method, add the appSetting owin:AppStartup

Agregar la clase Startup
	Requiere dll bien configuradas en web.config assemblyBinding con la version a la que referencian en el poryecto
		System.IdentityModel.Tokens.Jwt  
		Microsoft.IdentityModel.Tokens
Reuiere dll referenciada		Microsoft.IdentityModel.Logging
    




No Entity Framework provider found for the ADO.NET provider with invariant name 'System.Data.SqlClient'. 
Make sure the provider is registered in the 'entityFramework' section of the application config file. 
See http://go.microsoft.com/fwlink/?LinkId=260882 for more information.

Solucion
	Install-Package EntityFramework