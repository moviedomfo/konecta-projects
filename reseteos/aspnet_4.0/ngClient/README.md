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