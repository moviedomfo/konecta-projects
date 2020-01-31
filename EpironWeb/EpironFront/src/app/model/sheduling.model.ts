
import { TimeSpan, TimespamView } from './common.model';
import {  DayNamesIndex_Value_ES, WeekDays_EN, DayOfWeek, AppConstants } from './common.constants';




export class ResourceSchedulingBE {

  constructor (){
      
      this.WeekDays = 0;
      this.DateStart = null;
      this.DateEnd = null;
      this.Duration = 30;
      this.Description = '';
      this.TimeStart = '08:30';
      this.TimeEnd = '18:30';
      this.TimeStart_timesp = TimeSpan.FromHHMM('08:30');
      this.TimeEnd_timesp = TimeSpan.FromHHMM('08:30');
      this.HealthInstitutionId = AppConstants.DefaultHealthInstitutionId;
      this.Generate_Attributes();

  }
    public IdSheduler: number;
    public DateStart?: Date;
    public DateEnd: Date;

    public Description: string;
    public ResourceId?: number;

    public WeekOfMonth?: number;
    public WeekDays?: number;
    public Duration?: number;
    public CreationUserId: string;
    public UpdateUserId: string;
    public ResourceType?: number;
    public HealthInstitutionId?: string;
    public TimeStart: string;
    public TimeEnd: string;
    public TimeStart_timesp: TimeSpan;

    public TimeEnd_timesp: TimeSpan;
 
    public WeekDays_BinArray: boolean[];

    //ejemplo "Miercoles|Jueves|Viernes"
    public WeekDays_List: string;

    public Generate_Attributes(log?:boolean){
        
        this.WeekDays_BinArray= this.Get_WeekDays_BinArray();
        if(log)
            console.log('this.WeekDays_BinArray = ' + this.WeekDays_BinArray);
        this.WeekDays_List = this.getDayNames().join("|");
        if(log)
            console.log('this.WeekDays_List = ' + this.WeekDays_List);
        this.TimeStart_timesp = new TimeSpan(null);
        this.TimeStart_timesp.Set_hhmmss(this.TimeStart);
        
        this.TimeEnd_timesp = new TimeSpan(null);
        this.TimeEnd_timesp.Set_hhmmss(this.TimeEnd);
        
    }
    public Generate_Attributes_TimesPan(){
        
     
        this.TimeStart_timesp = new TimeSpan(null);
        this.TimeStart_timesp.Set_hhmmss(this.TimeStart);
        
        this.TimeEnd_timesp = new TimeSpan(null);
        this.TimeEnd_timesp.Set_hhmmss(this.TimeEnd);
        
    }
    //Retorna un array binario con los dias en comun: 
    //a = 111110
    // b = 010101
    // res=111111
    public GetCommonDays(a: boolean[], b: boolean[]): boolean[] {
        for (var i = 0; i < a.length; i++) {
            b[i] = a[i] || b[i];
        }

        return b;
    }
    public Get_WeekDays_BinArray(): boolean[] {

        if (!this.WeekDays)
            this.WeekDays = 0;
        //if (!this.WeekDays_BinArray)
        this.WeekDays_BinArray = ResourceSchedulingBE.CreateBoolArray(this.WeekDays);
        return this.WeekDays_BinArray;
    }


    private getDayNames(): string[] {
        var days: string[] = [];
        if (!this.WeekDays_BinArray)
            this.WeekDays_BinArray = ResourceSchedulingBE.CreateBoolArray(this.WeekDays);

        for (let i: number = 0; i <= this.WeekDays_BinArray.length - 1; i++) {
            var dayName: string;
            if (this.WeekDays_BinArray[i]) {
                dayName = DayNamesIndex_Value_ES.find(d => d.index === i).name;
                days.push(dayName);
            }
        }
        return days;
    }

    /// Crea vector booleano y rellena hasta 7 con false en caso de no existir
    /// Resultado Valor
    //  index    6    5     4     3     2    1    0
    //          Sab   Vier   Jue  Mie   Mar  Lun    Dom
    /// 0000100	false,false,false,false,true,false,false = 4 --> Martes
    /// 0000101 false,false,false,false,true,false,true  = 5 --> Martes , Dom
    /// 0000110	false,false,false,false,true,true,false  = 6 --> Martes , Lunes
    /// 0000111	7
    /// 0001000	8
    /// 0001001	9
    /// 0001010	10
    static CreateBoolArray(weekdays: number): boolean[] {
        let stack = [];
        let stackInvertida: boolean[] = [];
        var weekdays_to_bin = Number(weekdays).toString(2);
        
        var weekdays_to_bin_Array = weekdays_to_bin.split('');
        
        let val: boolean;
        //Recorro el vector desde atras y los voy metiendo en la pila
        for (let i: number = weekdays_to_bin_Array.length - 1; i >= 0; i--) {
            //s = weekdays_to_bin_Array[i].ToString();
            val = weekdays_to_bin_Array[i] === '1' ? true : false;
            //bool val = Convert.Toboolean (Convert.ToInt16(weekdays_to_bin_Array[i]));
            stack.push(val);
        }


        //Completo la pila con con falses hasta llegar a 7 posiciones (i < 7 - weekdays_to_bin_Array.Length)
        //Es desir: Si weekdays_to_bin_Array tiene =  11 dado q weekdays fue 3 completo la pila con 11+00000,
        for (let i: number = 0; i < 7 - weekdays_to_bin_Array.length; i++) {
            stack.push(false);
        }

        //invierto stack asi me queda : 0000011 o false,false,false,false,false,true,true
        for (let i: number = stack.length - 1; i >= 0; i--) {
            stackInvertida.push(stack[i]);
        }

        return stackInvertida;

    }
    Get_ArrayOfTimes_TotalMinutes(): number[] {

        var arrayOfSeconds:number[]=[];
        let array = ResourceSchedulingBE.Get_ArrayOfTimes(new Date(), this.TimeStart_timesp, this.TimeEnd_timesp, this.Duration, this.Description);
        for (let i: number = 0; i <= array.length -1; i++) {
            arrayOfSeconds.push(array[i].Time.TotalMinutes);
        }
        return arrayOfSeconds;
    }

    Get_ArrayOfTimes(date: Date): TimespamView[] {
        return ResourceSchedulingBE.Get_ArrayOfTimes(date, this.TimeStart_timesp, this.TimeEnd_timesp, this.Duration, this.Description);
    }

    Get_ArrayOfTimes2(date: Date, chekWith: boolean): TimespamView[] {

        if (chekWith) {
            if (!this.Date_IsContained(date))
                return null;
        }
        return ResourceSchedulingBE.Get_ArrayOfTimes(date, this.TimeStart_timesp, this.TimeEnd_timesp, this.Duration, this.Description);
    }

    static Get_ArrayOfTimes(currentDate: Date, start: TimeSpan, end: TimeSpan, duration: number, name: string): TimespamView[] {
        var aux: TimeSpan = new TimeSpan();

       //console.log('start ' + start.HHMM + ' | End ' + end.HHMM) ;
        let wTimespamView: TimespamView;
        var times: TimespamView[] = [];
        wTimespamView = new TimespamView(currentDate);
        aux.Set_hhmmss(start.getHHMM());
        wTimespamView.Time = aux;
        wTimespamView.TimeString = wTimespamView.Time.getHHMM();
        
        times.push(wTimespamView);
        

        var t: TimeSpan = new TimeSpan();//=  Object.assign({}, start); 
        t.Set_hhmmss(start.getHHMM());
        let control: boolean = true;
        //  let count = 0;
        while (control) {

            //Para este algoritmo colaboro el cuero mrenaudo 
            
            //console.log('end TotalMinutes ' + end.TotalMinutes + ' | t TotalMinutes' + t.TotalMinutes) ;
            if ((end.TotalMinutes - t.TotalMinutes) >= duration) {
                //count = count +1;
                //console.log(end.TotalMinutes + '-' + t.TotalMinutes + ' = '+ (end.TotalMinutes - t.TotalMinutes));
                //  if(count==20)
                //  {
                //     control = false    ;

                //  }

                //alert((end.TotalMinutes - t.TotalMinutes) .toString());
                wTimespamView = new TimespamView(null);
                wTimespamView.Duration = duration;
                wTimespamView.Name = name;
                aux = new TimeSpan();
                t.addMinutes(duration);
                aux.Set_hhmmss(t.getHHMM());

                wTimespamView.Time = aux;
                wTimespamView.TimeString = aux.getHHMM();
                times.push(wTimespamView);
                //console.log('t.addMinutes(duration) = ' + t.TotalMinutes);
            }
            else { control = false; }
        }
         console.log('------------------------------------------------------------------------');
        return times;
    }

    /// <summary>
    /// Determina si el dia de la fecha [date] pertenece a la confuguracion [WeekDays] mediante operaciones logicas y binarias
    /// 
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    private Date_IsContained(date: Date): boolean {
        var weekDay = WeekDays_EN.EveryDay;
        let dayOfWeek = date.getDay();

        switch (dayOfWeek) {
            case DayOfWeek.Monday://Lunes
                {
                    weekDay = WeekDays_EN.Monday;
                    break;
                }
            case DayOfWeek.Tuesday://Martes
                {
                    weekDay = WeekDays_EN.Tuesday;
                    break;
                }
            case DayOfWeek.Wednesday://Miercoles
                {
                    weekDay = WeekDays_EN.Wednesday;
                    break;
                }
            case DayOfWeek.Thursday://Jueves
                {
                    weekDay = WeekDays_EN.Thursday;
                    break;
                }
            case DayOfWeek.Friday://Viernes
                {
                    weekDay = WeekDays_EN.Friday;
                    break;
                }
            case DayOfWeek.Saturday://Sabado
                {
                    weekDay = WeekDays_EN.Saturday;

                    break;
                }
            case DayOfWeek.Sunday://Domingo
                {
                    weekDay = WeekDays_EN.Sunday;
                    break;
                }
        }
        var bin1: boolean[] = ResourceSchedulingBE.CreateBoolArray(weekDay);
        return ResourceSchedulingBE.Math(bin1, this.WeekDays_BinArray);
    }


    /// <summary>
    /// 0000111
    /// 1000001 return True
    /// 
    /// 100000
    /// 000010 return False
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    static Math(a: boolean[], b: boolean[]): boolean {

        for (var i = 0; i < a.length; i++) {
            //console.log(i + '-->  a=' + a[i] + ' && b=' + b[i] );
            if (a[i]==true && b[i]==true)
            {
                return true;
            }
        }

        return false;

    }

    public HasDaysInCommon(weekDays_array: boolean[]):boolean{
   
           return ResourceSchedulingBE.Math(weekDays_array, this.WeekDays_BinArray);
    }

    static intersection_totalMinutes(resource1:ResourceSchedulingBE,resource2:ResourceSchedulingBE): number[]{

        var rangoTotal1 = resource1.Get_ArrayOfTimes_TotalMinutes();
        var rangoTotal2 = resource2.Get_ArrayOfTimes_TotalMinutes();
        console.log(resource1 .Description +' = ' + rangoTotal1);
        console.log(resource1.TimeStart_timesp.getHHMM() + ' -->' + resource1.TimeEnd_timesp.getHHMM());
        console.log(resource2 .Description +' = ' +rangoTotal2);
        console.log(resource1.TimeStart_timesp.getHHMM() + ' -->' + resource1.TimeEnd_timesp.getHHMM());
        
        var intersetResult = rangoTotal1.filter(item => rangoTotal2.includes(item));
        return  intersetResult;
    }

    public static Map(item:ResourceSchedulingBE): ResourceSchedulingBE {
        var x: ResourceSchedulingBE = new ResourceSchedulingBE();
        x.Duration = item.Duration;
        x.TimeStart = item.TimeStart;
        x.TimeEnd = item.TimeEnd;
        x.TimeStart_timesp = item.TimeStart_timesp;
        x.TimeEnd_timesp = item.TimeEnd_timesp;
        x.TimeStart_timesp = item.TimeStart_timesp;
        x.Description = item.Description;
        x.WeekDays_BinArray = item.WeekDays_BinArray;
        //alert('Map --> ' + item.WeekDays_BinArray);
        x.WeekDays_List = item.WeekDays_List;
        x.WeekDays = item.WeekDays;
        x.DateEnd = item.DateEnd;
        x.DateStart = item.DateStart;
        x.CreationUserId = item.CreationUserId;
        x.HealthInstitutionId = item.HealthInstitutionId;
        x.UpdateUserId = item.UpdateUserId;
        x.CreationUserId = item.CreationUserId;
        x.WeekOfMonth = item.WeekOfMonth;
        x.IdSheduler = item.IdSheduler;
        x.ResourceId = item.ResourceId;
        x.ResourceType = item.ResourceType;

        return x;
    }
}
