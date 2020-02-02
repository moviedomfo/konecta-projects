//import { AnonymousSubject } from "rxjs/Subject";
import * as moment from 'moment'
import { Duration } from "moment";

import { AppoimantsStatus_SP } from './common.constants';
import { PersonBE } from './persons.model';


/// <summary>
/// Representa un intervalo de tiempo puntual generado a partir del la programacion de turnos del provider.
/// Una lista de TimespamView por ejemplo podria representar los intervalos de tiempo de disponibilidad 
/// horaria para un prefecional en un dia determinada y turno determinado
/// 
/// Este elemento es utilizado para rellenar las grillas donde aparecen los turnos otorgados y turnos disponible.
/// Un turno disponible es un turno inexistente o un turno no registrado en la base de datos.
/// </summary>
export class TimespamView {


    constructor(date?: Date) {
        //Menor que cero t1 es menor que t2.:Da
        //Para fechas date anterior a hoy no se usa nowTicks
        if (!date) {
            date = new Date();
        }

        //El pasado u hoy
        if (date <= new Date())
            this.nowTicks = date.getMilliseconds();
        //Futuro Siempre esta libre siempre y cuando no está en otro estado
        if (date > new Date())
            this.nowTicks = -1;
    }

    public IsExceptional: boolean;

    public Description: string;
    //public Appointment: AppointmentsBE;
    public Name: string;
    public Time: TimeSpan;

    public TimeString: string;




    public Duration: number;
    private nowTicks?: number = null;


    private status: number;

    get Status(): number {
        return this.GetStatus();
    }
    set Status(s: number) {
        this.status = s;
    }

    public GetStatus(): number {
        // if (!this.Appointment)
        //     return this.Appointment.Status;

        if (!this.nowTicks)
            return AppoimantsStatus_SP.Nulo;

        if (this.nowTicks == -1) return AppoimantsStatus_SP.Libre;

        //Si tiempo final del TimespamView es mayor a ahora elta libre
        let t: number = 0;
        //let t:number = Time.Add(TimeSpan.FromMinutes(Duration)).Ticks;


        if (this.nowTicks < t)
            return AppoimantsStatus_SP.Libre;
        else
            return AppoimantsStatus_SP.Nulo;
    }
}

export class TimeSpan {



    public Fecha: Date;
    public Milliseconds: number;

    public Days: number;
    public Hours: number;
    public Minutes: number;
    public Seconds: number;

    public TotalMilliseconds: number;
    public TotalDays: number;
    public TotalHours: number;
    public TotalMinutes: number;
    public TotalSeconds: number;

    public Tick: number;
    private currentDuration: Duration;

    constructor(ticks?: number) {

    }

    Set_hhmmss(hhmmss: string) {

        var startDate = new Date(1, 0, 1, 0, 0, 0, 0);
        let duration: Duration = moment.duration(hhmmss);

        var day = new Date();
        //alert(hhmmss +'  '+ duration.hours());
        this.Fecha = new Date(day.getFullYear(), day.getMonth(), day.getDate(), duration.hours(), duration.minutes(), duration.seconds(), duration.milliseconds());

        //alert(hhmmss +'  '+ duration.hours() + ' fecha = ' + this.Fecha.toString());

        var startMomentDate = moment(startDate);
        var endMomentDate = moment(this.Fecha);//moment('1/1/2013', 'DD/MM/YYYY');
        //var days = endMomentDate.diff(startMomentDate, 'days');


        this.currentDuration = moment.duration(endMomentDate.diff(startMomentDate));

        this.setFromDuration(this.currentDuration);

    }

    setFromDuration(duration: Duration) {

        this.Days = duration.days();  //number of days in a duration
        this.Hours = duration.hours();
        // console.log('hours:' + this.Hours);
        this.Minutes = duration.minutes();
        // console.log('Minutes:' +this.Minutes);
        this.Seconds = duration.seconds();
        // console.log('Seconds:' +this.Seconds);
        this.Milliseconds = duration.milliseconds();  //number of milliseconds in a duration


        this.TotalDays = this.currentDuration.asDays();//The length of the duration in days,
        this.TotalHours = this.currentDuration.asHours();//The length of the duration in minutes
        this.TotalMinutes = this.currentDuration.asMinutes();
        this.TotalSeconds = this.currentDuration.asSeconds();
        this.TotalMilliseconds = this.currentDuration.asMilliseconds();

        this.hhmm = moment(this.Fecha).format('HH:mm');
    }

    setDate(day: Date) {
        this.Fecha = day;
        var dayWrapper = moment(day);
        //'7.23:59:59.999'
        let duration: Duration = moment.duration(dayWrapper.day + '.' + dayWrapper.hour + ':' + + ':' + dayWrapper.minute + ':' + dayWrapper.second + '.' + dayWrapper.milliseconds);

        this.setFromDuration(duration);

    }

    addMinutes(m: number) {
        //console.log('antes this.currentDuration.minutes == ' + this.currentDuration.minutes());
        this.currentDuration.add(m, 'minutes');

        this.Fecha = new Date(
            this.Fecha.getFullYear(), this.Fecha.getMonth(), this.Fecha.getDate(),
            this.currentDuration.hours(),
            this.currentDuration.minutes(),
            this.currentDuration.seconds(),
            this.currentDuration.milliseconds());


        this.setFromDuration(this.currentDuration);
        // console.log('despues hh:mm == ' + this.currentDuration.hours() + ':' +  this.currentDuration.minutes());
        // console.log('despues hh:mm == ' + this.hhmm);
        // console.log('despues hh:mm == ' + this.getHHMM());
        // console.log('despues hh:mm == ' + moment(this.Fecha).format('HH:mm'));

    }

    public hhmm: string;

    // get HHMM(): string {
    //     return this.getHHMM();
    // }

    getHHMM() {
        return moment(this.Fecha).format('HH:mm');
        //return this.Hours + ':' + this.Minutes;//  moment(this.TotalMinutes,'minutes').format('hh:mm');
    }
    public static FromString(hhmmss: string) {
        var t: TimeSpan = new TimeSpan();
        t.Set_hhmmss(hhmmss);

        return t;
    }

    public static FromHHMM(hhmm: string) {
        var t: TimeSpan = new TimeSpan();

        t.Set_hhmmss(hhmm);
        return t;
    }

    public static FromMinutes(mm: number) {
        var t: TimeSpan = new TimeSpan();
        let duration: Duration = moment.duration(mm, 'minutes');
        t.setFromDuration(duration);
        return t;
    }
    public static FromSeconds(s: number) {
        var t: TimeSpan = new TimeSpan();
        let duration: Duration = moment.duration(s, 'seconds');
        t.setFromDuration(duration);
        return t;
    }
}

export class chkDays {
    public chkDomingo: boolean = false;
    public chkLunes: boolean = false;
    public chkMartes: boolean = false;
    public chkMiercoles: boolean = false;
    public chkJueves: boolean = false;
    public chkViernes: boolean = false;
    public chkSabado: boolean = false;
    public chkTodos: boolean = false;
}
export interface IContextInformation {
    Culture?: string;
    ProviderNameWithCultureInfo?: string;
    HostName?: string;
    HostIp?: string;
    HostTime: Date;
    ServerName: string;
    ServerTime?: Date;
    UserName?: string;
    userId?: string;
    AppId: string;
    ProviderName: string;
}
export class ContextInformation implements IContextInformation {
    Culture?: string;
    ProviderNameWithCultureInfo?: string;
    HostName?: string;
    HostIp?: string;
    HostTime: Date;
    ServerName: string;
    ServerTime?: Date;
    userName?: string;
    userId?: string;
    AppId: string;
    ProviderName: string;
}


export interface IRequest {

    SecurityProviderName?: string;
    Encrypt?: boolean;
    //Error?:ServiceError;
    ServiceName?: string;
    BusinessData?: object;
    CacheSettings?: object;
    ContextInformation: IContextInformation;
}

export class IpInfo {
    public ip: string;
    public loc: string;//"37.385999999999996,-122.0838",
    public city: string;//"Mountain View"
    public region: string;//"California"
    public country: string;//"US
}

export class ExecuteReq {
    serviceProviderName?: string;
    serviceName?: string;
    jsonRequest?: string;
}
export interface IResponse {

    SecurityProviderName?: string;
    Encrypt?: boolean;
    Error?: ServiceError;
    ServiceName?: string;
    BusinessData?: object;
    CacheSettings?: object;
    ContextInformation: IContextInformation;
}
export class Result implements IResponse {

    SecurityProviderName?: string;
    Encrypt?: boolean;
    Error?: ServiceError;
    ServiceName?: string;
    BusinessData?: object;
    CacheSettings?: object;
    ContextInformation: ContextInformation;
}

export class IAPIRequest  {
    SecurityProviderName?: string;
    UserId?: string;
    ClientIp?:string;
    Culture:string;
    
    AppId:string;
}

export class Request implements IRequest {

    SecurityProviderName?: string;
    Encrypt?: boolean;
    Error?: object;
    ServiceName?: string;
    BusinessData?: object;
    CacheSettings?: object;
    ContextInformation: ContextInformation;
}

export class ParamBE {

    paramId: number;
    parentId?: number;
    culture: string;
    name: string;
    description: string;
}


export class UbicacionItemBE {  

    id: number;
    nombre: string;
   
}

export class localidadesResponse {

    
    public cantidad: number;
    public  inicio: number;

    public localidades :UbicacionItemBE[];
   
}



export class EpironApiResponse {
    public StatusCode: number;
    public Errors: EpironApiError;
    public Result: any;

}

export class EpironApiError
{
    public  EventResponseId : number;
    public  EventResponseText :string;
    public  EventResponseInternalCode :string;
    public  Guid :string;
}

/// Contiene informacion del error de un servicio.-
// if(e instanceof EvalError)
export class ServiceError extends Error {


    Message: string;
    StackTrace: string;
    Type: string;
    Machine: string;
    Status: number;

}


export class FwkEvent {
    Message: string;
    Source: string;
    Machine: string;
    LogDate: Date;
    Type: string;
    User: string;
}


export class HealthInstitutionBE {

    public HealthInstitutionId: string;
    public HealthInstitutionType?: number;
    public Street: string;
    public StreetNumber?: number;
    public Floor: string;
    public CountryId?: number;
    public ProvinceId?: number;
    public CityId?: number;
    public RazonSocial: string;
    public Province: string;
    public City: string;
    public Neighborhood: string;
    public ZipCode: string;
    public CreatedDate: Date;
    public LastAccessTime?: Date;
    public LastAccessUserId?: string;
    public ActivationKey: string;
    public Description: string;
    public CUIT: string;
    public HealthInstitutionIdParent?: string;
}

export class UserTask {
    constructor (options?: {taskId:number; tittle: string; completedPercent: number;descripción:string;createdDate:Date,priority:string}) {
        if (options) {
            this.taskId = options.taskId;
            this.tittle = options.tittle;
            this.descripción = options.descripción;
            this.completedPercent = options.completedPercent;
            this.createdDate = options.createdDate;
            this.priority = options.priority;
            
            
        }
    }
    taskId:number;
    tittle: string;
    descripción: string;
    completedPercent: number;
    createdDate: Date;
    priority: string;
}
export class UserMessage {
    constructor (options?: {messageId:number; tittle: string; completedPercent: number;body:string;createdDate:Date}) {
        if (options) {
            this.messageId = options.messageId;
            this.tittle = options.tittle;
            this.body = options.body;
            this.completedPercent = options.completedPercent;
            this.createdDate = options.createdDate;
            
            
            var sinceDate = moment(this.createdDate).format('MMMM Do YYYY, h:mm:ss a');
            this.timeAgo = sinceDate;
            
        }
    }
    messageId:number;
    tittle: string;
    body: string;
    completedPercent: number;
    createdDate: Date;
    timeAgo: string;
}

export class HelperBE {
    public getFullName(name: string, lastName: string) {
        return lastName + ', ' + name;
    }
}



export class ApiServerInfo {
    public HostName: string;
    public SQLServerMeucci: string;
    public SQLServerSeguridad: string;
    public Ip: string;
    public email: string;
}