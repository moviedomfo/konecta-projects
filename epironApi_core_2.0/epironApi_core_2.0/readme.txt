Install
Documentacion Host ASP.NET Core on Windows with IIS 
https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/iis/?view=aspnetcore-3.0

IIS configuration
	Enable the Web Server (IIS) server role and establish role services.
	Windows desktop operating systems -->Enable the IIS Management Console and World Wide Web Services.


 

Install the .NET Core Hosting Bundle (The bundle installs the .NET Core Runtime, .NET Core Library, and the ASP.NET Core Module)
	Descarga .NET Core 3.0 Runtime & Hosting Bundle for Windows (v3.0.0)
	https://dotnet.microsoft.com/download/thank-you/dotnet-runtime-3.0.0-windows-hosting-bundle-installer

Web Platform Installer

https://www.microsoft.com/web/downloads/platform.aspx

Se pueden configurar opiciones de IIS
		IIS options https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/iis/?view=aspnetcore-3.0#iis-options
		services.Configure<IISServerOptions>(options => 
		{
			options.AutomaticAuthentication = false;
			});
			
Reiniciar luego de instalar .NET Core Hosting Bundle


Crear una Site en IIS
	
Pegar epironApi_core_2.0.xml para que levante swagguer
crear carpeta logs
revisar el web.configurar
		stdoutLogEnabled="true" 
		stdoutLogFile=".\logs\stdout" -----------> crear carpeta
		hostingModel="InProcess"

Navegar http://172.22.14.46:55000/index.html
		http://localhost:55000/index.html

Errores cuando instalamos un server desde 0

	The specified version of Microsoft.NetCore.App or Microsoft.AspNetCore.App was not found. 

Download .NET Core 2.2 and install
		ASP.NET Core/.NET Core --> dotnet-hosting-2.2.7-win  
		ASP.NET Core Installer: -->aspnetcore-runtime-2.2.7-win-x64
		Debeemos tener instalado los anteiorres. Si llegamos a tener proeviamente la version 3. nonos va a sevir dado que el poryecto esta compilado en 2.2
