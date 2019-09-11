Authenticacion contra Dominio
http://localhost:50009/api/domainAuth/authenticate

Es un POST dado que podria en futuro implicar la creacion de una session. 
Por el momento solo la crea en Active Direcory. 
Desarrollarla GET haría que los clientes uncen http.Get en sus Front-End y luego en futuro siullegace a cambiar a post 
deberiamos modificar todos los desarrollos.


Los metodos Deberian requerir autenticacion con jwt

HttpGet
    userExist
    retriveUserByName
    retriveUserGroups
    retriveGroupUsers
    retriveGroups
    retriveDomainsUrl

HttpPost
    userResetPassword
    userSetActivation
    userUnlock
    userLock


JWT Concepts
 JWT typically looks like the following. xxxxx.yyyyy.zzzzz

Where yyyyy is Payload  which contains the claims :statements about an entity


iss (Issuer) : Quien autoriza  Identifica el proveedor de identidad que emitió el JWT

exp  (xpiration time)
aud (Audience) : dentifica la audiencia o receptores para lo que el JWT fue emitido. Cada servicio que recibe un JWT 
           para su validación tiene que controlar la audiencia a la que el JWT está destinado. 
           Si el proveedor del servicio no se encuentra presente en el campo aud, entonces el JWT tiene que ser 
           rechazado



jwt validation errors:

	IDX10214: Audience validation failed. Audiences: '[PII is hidden]'. Did not match: validationParameters.ValidAudience: '[PII is hidden]' or validationParameters.ValidAudiences: '[PII is hidden]'.

	IDX10230: Lifetime validation failed. Delegate returned false, securitytoken: '[PII is hidden]'.

	IDX10205: Issuer validation failed. Issuer: '[PII is hidden]'. Did not match: validationParameters.ValidIssuer: '[PII is hidden]' or validationParameters.ValidIssuers: '[PII is hidden]'.


	Active Directory Error
	The server is not operational :
	Usually indicates that the hostname or IP address listed in the Primary Server path is not correct, or that an LDAP server is not listening on that address. However, if this authentication source has been configured to use SSL (i.e., Primary Server Path begins "ldaps://"), this error could also mean that SSL was either not configured or not configured properly on the remote server.

