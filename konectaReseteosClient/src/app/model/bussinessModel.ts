export class Empleado
{
     public Emp_id :number;
    public ApeNom :string;
    public Nombre: string;
    public Aplellido:  string;
    public Cuenta:  string;
    public Cargo :  string;
    public Subarea :  string;
    public DNI :string;

    
    public Telefono :  string;
    public Direccion :  string;
    public FechaNacimiento :  string;
    public CiudadNatal :  string;
 
    public Email: string;
    //public Dom_id: number;
    public WindowsUser:  string;

    public WindosUserList :WindosUser[];



  }
 
  export class WindosUser
  {
    public dom_id :number;
    public Emp_Id : number;
    public Dominio: string;
    public WindowsUser:  string;
    
  }
  export class retriveEmpleadosReseteosReq
  {
      
   
      public userName : string;
      public domain: string;
      public dni:  string;
      public  userCAIS :boolean;
      
      
  }
 

   export class userResetPasswordReq{
    public WindowsUser :string;
    public password :string;
    public DomainName :string;
    public dom_id :number;
    
    //Id del empleado al que se le resetea el UW
    public Emp_Id :number;
    
    //Id del usuario logueado
    public ResetUserName :string;
    public ResetUserId :number;
    //
    public ticket  : string;
    //con el nombre de la PC usada
    public host :string;
    public ResetUserCAIS :boolean;
   
  }

  export class userChangePasswordReq{

//    public windowsUser :string;
    public newPassword :string;
    public domainName :string;
    public dom_id :number;
    
    //Id del empleado al que se le resetea el UW
    public emp_Id :number;
    
    //Id del usuario logueado
    public userName :string;
    public userId :number;
    
    
    //con el nombre de la PC usada
    public host :string;
    
   
  }
  export class userForgotPasswordVerifyReq{
    public dni :string;
    public code :string;

  }

  export class forgotPassword_requetsReq{
    
        
        //Id del usuario logueado
        public dni :string;
        //con el nombre de la PC usada
        public host :string;
      }


   export class userUnlockReq{
    public WindowsUser :string;
    public DomainName :string;
    public dom_id :number;
    
    //Id del empleado al que se le resetea el UW
    public Emp_Id :number;
    
    //Id del usuario logueado
    public ResetUserName :string;
    
    public ResetUserId :number;
    public ticket :string;
    //con el nombre de la PC usada
    public host :string;
    public ResetUserCAIS :boolean;
   
  }
  export class Domain{
    public Domain :string;
    public DomainId :number;
  }

  export class ApiServerInfo
    {
      public   HostName :string;
      public   SQLServerMeucci :string;
      public    SQLServerSeguridad :string;
      public   Ip :string;
      public  url_api:string;
    }

