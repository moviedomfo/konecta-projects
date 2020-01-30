## npm first installings 
# install Bootstrap 4

1)
   
  1.1  `run`  npm install  bootstrap jquery popper.js --save

  1.2  `agregar en angular.json : guarda que a pooper no lo toma si no le pones ./`
    
      "styles": [
              "src/styles.scss",
              "node_modules/bootstrap/dist/css/bootstrap.min.css"
            ],
       "scripts": [
              "node_modules/jquery/dist/jquery.min.js",
              "node_modules/bootstrap/dist/js/bootstrap.min.js",
              "./node_modules/popper.js/dist/popper.min.js"  
            ]
# install AdminLTE 3.0.1
   1.1 `descargar`  https://github.com/ColorlibHQ/AdminLTE/releases
   1.2  `run` npm install admin-lte --save

# install  jwt-decode ng-idle
    npm install jwt-decode
    #actualizar si es necesario 

#  iddle 
    npm  install --save ng-idle

    angular/cli    usar el doc en g drive pelsoft https://docs.google.com/document/d/1TOSaHK82uXvB6S1jx6BkggssBo2ROrDuwS4GF9QNdiI/edit

    npm install --save @ng-idle/core @ng-idle/keepalive angular2-moment
    npm install rxjs
#  grid
    npm install ag-grid-community

ng-grid-community


1) `manejo de numericos DigitOnly Directive`
    https://codeburst.io/digit-only-directive-in-angular-3db8a94d80c3
    https://www.npmjs.com/package/@uiowa/digit-only
    npm i -S @uiowa/digit-only


# ---------------------------------ng creating component---------------------------------


ng g c functionalComponents\QuotationRequest
ng g c functionalComponents\QuotationRequest\orderManagement
ng g c functionalComponents\QuotationRequest\orderCard
ng g c functionalComponents\QuotationRequest\orderGrid


ng g c functionalComponents\QuotationRequest
ng g c functionalComponents\QuotationRequest\orderManagement
ng g c functionalComponents\QuotationRequest\orderCard
ng g c functionalComponents\QuotationRequest\orderGrid





# Celamdash3

This project was generated with [Angular CLI](https://github.com/angular/angular-cli) version 8.3.8.

## Development server

Run `ng serve` for a dev server. Navigate to `http://localhost:4200/`. The app will automatically reload if you change any of the source files.

## Code scaffolding

Run `ng generate component component-name` to generate a new component. You can also use `ng generate directive|pipe|service|class|guard|interface|enum|module`.

## Build

Run `ng build` to build the project. The build artifacts will be stored in the `dist/` directory. Use the `--prod` flag for a production build.

## Running unit tests

Run `ng test` to execute the unit tests via [Karma](https://karma-runner.github.io).

## Running end-to-end tests

Run `ng e2e` to execute the end-to-end tests via [Protractor](http://www.protractortest.org/).

## Further help

To get more help on the Angular CLI use `ng help` or go check out the [Angular CLI README](https://github.com/angular/angular-cli/blob/master/README.md).
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



---------------------------------------------------------------------------------
Admin LTE v2.4.18




---------------------------------ng c---------------------------------


ng g c functionalComponents\QuotationRequest
ng g c functionalComponents\QuotationRequest\orderManagement
ng g c functionalComponents\QuotationRequest\orderCard
ng g c functionalComponents\QuotationRequest\orderGrid


ng g c functionalComponents\QuotationRequest
ng g c functionalComponents\QuotationRequest\orderManagement
ng g c functionalComponents\QuotationRequest\orderCard
ng g c functionalComponents\QuotationRequest\providerAssignment

ng g c functionalComponents\stock\stockAlerts


ng g c functionalComponents\security\userRessetPwd
ng g c functionalComponents\security\userGrid
ng g c functionalComponents\security\dashboard


management es page

<!-- Main content -->
<section class="content">
    <div class="container-fluid">
        <div class="row">

            <div class="col-md-12">
                <div class="box">
                    <div class="box-header with-border">

                    </div>


                </div>

            </div>
        </div>
    </div><!-- /.container-fluid -->

</section> <!-- /Main .content -->