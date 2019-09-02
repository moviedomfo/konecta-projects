using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Epiron.Gestion.Components
{
    public partial class UcBase : UserControl
    {
        private bool _DeactivateControl = false;
        protected List<Guid> _BlockingGuidList = null;
       
        private bool _EnabledModule = false;
  

        public virtual bool DeactivateControl
        {
            get { return this._DeactivateControl; }
            set { this._DeactivateControl = value; }
        }

   
        public virtual bool EnabledModule
        {
            get { return this._EnabledModule; }
            set { this._EnabledModule = value; }
        }


        public UcBase()
        {
            InitializeComponent();           
        }

       


        public virtual void Delete()
        { }

     

        /// <summary>
        /// Guardar
        /// </summary>
        public virtual void Save()
        { }

        
        /// <summary>
        /// Validaciones
        /// </summary>
        /// <param name="pEntity"></param>
        public virtual bool Validation()
        {
            return true;
        }

     
        public virtual void Clear()
        { }

        /// <summary>
        /// Libera los recursos del formulario.
        /// (SE RECOMIENDA SU USO PARA EVITAR MEMORY LEAKS)
        /// </summary>
        public virtual void Destroy()
        { }

        /// <summary>
        /// SE RECOMIENDA SU USO PARA EVITAR MEMORY LEAKS
        /// </summary>
        private void ControlDispose()
        {
            foreach (Control ctrl in this.Controls)
            {
                try
                {
                    ctrl.Dispose();
                }
                catch { }
            }
        }

      
    }
}
