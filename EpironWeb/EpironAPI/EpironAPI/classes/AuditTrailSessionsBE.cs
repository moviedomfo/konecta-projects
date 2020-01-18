using Fwk.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpironAPI.classes
{
    public class AuditTrailSessionsBE
    {
        public string Par_Event_Tag { get; set; }
        public Guid Par_RequestSessionGuid { get; set; }
        public Guid Par_ApplicationInstanceGuid { get; set; }
        public string Par_AutenticationTypeTag { get; set; }
        public Guid Par_DomainGUID { get; set; }
        public Guid Par_MenuGUID { get; set; }
        public string Par_ListOfEntityGuids { get; set; }
        public string Par_PersonDocument { get; set; }

    }

    public class EntityRegisterUserPermissionBE : Entity
    {
        public int EntityRegisterId { get; set; }
        public string EntityRegisterName { get; set; }
        public Guid EntityRegisterGUID { get; set; }
        public string EntityRegisterPk { get; set; }
        public Guid EntityRegisterElementGUID { get; set; }
        public Guid EntityRegisterParentGUID { get; set; }
        public bool Edicion { get; set; }
        public bool Eliminacion { get; set; }
        public bool Seleccion { get; set; }
    }


    public class EntityBE : Entity
    {
        public int EntiyId { get; set; }
        public string EntityName { get; set; }
        public Guid EntityGUID { get; set; }
    }

}