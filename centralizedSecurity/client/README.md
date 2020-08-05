Renderer2 proporciona una API para acceder de forma segura a elementos nativos, incluso cuando no están soportados por la plataforma (web workers, server-side rendering, etc).

Migrar a 8

errores 
1 An unhandled exception occurred: Could not find the implementation for builder @angular-devkit/build-angular:dev-server
  solution
  1) npm install @angular-devkit/build-angular 
  2) "@angular-devkit/build-angular": "^0.801.0",  le cambie la version a la actual

2 ) Si tira Error :  in The Angular Compiler requires TypeScript >=3.4.0 and <3.5.0 but 3.5.2 was found instead.
    Solucion:
    npm install typescript@">=3.4.0 <3.5.0"

**Directivas 

1) manejo de numericos DigitOnly Directive
    https://codeburst.io/digit-only-directive-in-angular-3db8a94d80c3
    https://www.npmjs.com/package/@uiowa/digit-only
    npm i -S @uiowa/digit-only




-------------------Validacion de password Cambio Contraseña
validation in 
https://regex101.com/
^(?=.{8,}$)(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*\W).*$
9 Por definición, una contraseña compleja está conformada por al menos lo
siguiente:
 Letras minúsculas
 Letras mayúsculas
 Números (por ejemplo: 1, 2, 3)
 Símbolos (tales como: @, =, -, etc.) 
 longitud mínima de 8 caracteres.
 compuesta por letras, números y símbolos