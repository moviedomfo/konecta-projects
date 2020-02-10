
import { HttpHeaders } from '@angular/common/http';
import { environment } from '../../environments/environment';


const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    headers.append('Access-Control-Allow-Headers', 'Content-Type');
    headers.append('Access-Control-Allow-Methods', '*');
    headers.append('Access-Control-Allow-Origin', '*'); 

let header_httpClient_contentTypeJson = new HttpHeaders({ 'Content-Type': 'application/json' });
     header_httpClient_contentTypeJson.append('Access-Control-Allow-Methods', '*');
     header_httpClient_contentTypeJson.append('Access-Control-Allow-Headers', 'Content-Type, Access-Control-Allow-Headers, Authorization, X-Requested-With');
     header_httpClient_contentTypeJson.append('Access-Control-Allow-Origin', '*');
     
let header_httpClient_form_urlencoded = new HttpHeaders({ 'Content-Type': 'application/x-www-form-urlencoded' });
    header_httpClient_form_urlencoded.append('Access-Control-Allow-Methods', '*');
    header_httpClient_form_urlencoded.append('Access-Control-Allow-Headers', 'Content-Type, Access-Control-Allow-Headers, Authorization, X-Requested-With');
    header_httpClient_form_urlencoded.append('Access-Control-Allow-Origin', '*');


//let options = new RequestOptions({ headers: headers });

export  const AppConstants={
    
    AppProducion:environment.production,
    AppVersion:environment.version,
     AppAPI_URL:environment.App_BaseURL + "api/",
     Culture:environment.culture,
     
     
     ImagesSrc_Woman:'/assets/images/User_Famele.bmp',
     ImagesSrc_Man:  '/assets/images/User_Male.bmp',
    
     //httpOptions:options,
     httpClientOption_form_urlencoded:{headers:header_httpClient_form_urlencoded},
     httpClientOption_contenttype_json:{headers:header_httpClient_contentTypeJson},
     
     
     iddleTimeout_seconds:environment.iddleTimeout_seconds,
     iddle_waite_Timeout_seconds:environment.iddle_waite_Timeout_seconds,
     emptyGuid :'00000000-0000-0000-0000-000000000000'

     
     
}

export const Sexo =
{
    Masculino : 0,
    Femenino : 1
};


  export  const contextInfo ={
        Culture: "ES-AR",
        ProviderNameWithCultureInfo:"",
        HostName : 'localhost',
        HostIp : '10.10.200.168',
        HostTime : new Date(),
        ServerName : 'WebAPIDispatcherClienteWeb',
        ServerTime : new Date(),
        UserName : 'mrenaudo',
        UserId : '466541AB-0DB6-47CB-A19E-46152EDEE4A3',
        AppId : 'Healt',
        ProviderName: 'health'
      };

   
export  const EventType = 
      {
              //
        // Summary:
        //     Representa mensajes de información.
        Information : 0,
        //
        // Summary:
        //     Representa mensajes de advertencia.
        Warning : 1,
        //
        // Summary:
        //     Representa mensajes de error.
        Error : 2,
     
        //     Representa la ausencia de tipo de evento.
        Success : 4,
        
      }
export const EntityStatus =
{
    New : -1,
    Added : 0,
    Updated : 1,
    Removed : 2
};
export  enum  ParamTypeEnum  
      {
        EntityStatus =500,
        OrderStatus=520,
        StockType=600,
        TipoDocumento = 700,
        Especialidad = 550,
        Profesion = 100,
        EstadoCivil=  750,
       
        TipoRecepcion =  200,
  
        TipoMedioContacto =  1000,
        Paises =  1050,
        Localidad =  1200,
        Provincia =  1100,
        SubCategoriaLibro =  5000,
        CategoriaLibro =  1000,
        Parentesco= 2000
      };



export const CommonValuesEnum =
    {
        TodosComboBoxValue : -1000,
        VariosComboBoxValue : -2000,
        SeleccioneUnaOpcion : -3000,
        Ninguno : -4000,
        /// <summary>
        /// Esta opcion es usada para seleccion de Mutuales .- Caso Sin mutual particular
        /// </summary>
        Particular : -5000
    };

    export const DayNamesIndex_Value_ES =[ 
        {"name" : 'Sabado' ,"index"  : 0,"bidValue": 1},
        {"name" : 'Viernes' ,"index"  : 1,"bidValue": 2 },
        {"name" : 'Jueves' ,"index"  : 2,"bidValue": 4 },
        {"name" : 'Miercoles' ,"index"  : 3,"bidValue": 8 },
        {"name" : 'Martes' ,"index"  : 4,"bidValue": 16 },
        {"name" : 'Lunes' ,"index"  : 5,"bidValue": 32 },
        {"name" : 'Domingo' ,"index"  : 6,"bidValue": 64 },
     ];

export const AppoimantsStatus_SP={
    Reservado : 630,
    EnAtencion : 631,
    Cerrado : 632,
    Cancelado : 633,
    Expirado : 634,
    EnEspera : 635,
    Libre : 636,
    Nulo : 637
}
export const AppoimantsStatus_SP_type=
{

    Entreturno : 638,
    Sobreturno : 639
}

  /// <summary>
    /// Estados de una subscripcion enviada para pertenecer a una institución
    /// </summary>
    export const SubscriptionRequestStatus=
    {
        EnEspera : 650,
        Rechazado : 651,
        Expirado : 652,
        Null : 653

    }
    export const DayNamesIndex_ES =
    {
        //SAB	VIE	JUE	MIE	MAR	LUN	DOM
        Sabado : 0,
        Viernes : 1,
        Jueves : 2,
        Miercoles : 3,
        Martes : 4,
        Lunes : 5,
        Domingo : 6
    }
    export const  WeekDays_EN =
    {

        Sunday : 1,

        Monday : 2,

        Tuesday : 4,

        Wednesday : 8,

        Thursday : 16,

        Friday : 32,
        //
        // Summary:
        //     Specifies work days (Monday, Tuesday, Wednesday, Thursday and Friday).
        WorkDays : 62,
        //
        // Summary:
        //     Specifies Saturday.
        Saturday : 64,
        //
        // Summary:
        //     Specifies Saturday and Sunday.
        WeekendDays : 65,
        //
        // Summary:
        //     Specifies every day of the week.
        EveryDay : 127
    }

     //
    // Summary:
    //     Specifies the day of the week c# System
   export const DayOfWeek=
    {
        Sunday : 0,
        Monday : 1,
        Tuesday : 2,
        Wednesday : 3,
        Thursday : 4,
        Friday : 5,
        Saturday : 6
    }

    export const MonthsShortName_ES=
    {
        ENE : 1,
        FEB : 2,
        MAR : 3,
        ABR : 4,
        MAY : 5,
        JUN : 6,
        JUL : 7,
        AGO : 8,
        SET : 9,
        OCT : 10,
        NOV : 11,
        DIC : 12

    }

    export const MonthsNames_ES=
    {
        Enero : 1,
        Febrero : 2,
        Marzo : 3, Abril : 4, Mayo : 5, Junio : 6, Julio : 7, Agosto : 8,
        Septiembre : 9,
        Octubre : 10,
        Noviembre : 11,
        Diciembre : 12

    }

export const CommonParams={
    TodosComboBoxValue:{
        paramId:-1000,
        name:'Todos'
    },
    VariosComboBoxValue: {
        paramId:-2000,
        name:'Varios'
},
    SeleccioneUnaOpcion : {
        paramId:-3000,
        name:'Seleccione una opcion'
},
    Ninguno : {
        paramId:-4000,
        name:'Ninguno'
            },
   
    Particular :  {
        paramId:-5000,
        name:'Ninguno'
            }

};
export enum  PersonStatus
    {
        Activo = 501,
        Inactivo = 502,
        Desvinculado = 503,
        PendienteAuth = 304

    }
export enum  MotivoConsultaEnum 
    {
        CreateStock = 0,
        UpdateStock= 1,
        CreateProvider= 2,
        UpdateProvider= 3,
        QueryPerson_NoUpdate= 4,
        QueryStock_NoUpdate = 5,
        /// <summary>
        /// Asocia a la institucion un provider ya existente
        /// </summary>
        AsociateProvider= 6,
        CreateOrder = 7,
        UpdateOrder= 8,
        QueryOrder_NoUpdate = 9,
        QueryProviderPage = 10,
        CreateAccount= 11,
        UpdateAccount= 12,
        QueryAccount= 13
    }

 
//module.exports =  AppConstants;
//module.exports =  contextInfo;
// const  CNN_STRING_HEALTH  = {
//     user: 'sa',
//     password: 'as',
//     server: 'SANTANA\\SQLEXPRESS2008',
//     database: 'health3',

//     options: {
//         encrypt: true // Use this if you're on Windows Azure 
//     }
// }