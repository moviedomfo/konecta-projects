using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Epiron.Gestion.Components;
using ScrollUpDemo_w32;

namespace Epiron.Gestion.Front.CaseComment.UserControls.Base
{
    public partial class UcFilterBase : UcBase
    {
        protected int _ConversationId = 0;
        protected bool _TagModuleEnable = false;

        public UcFilterBase()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Devuelve el nombre del tipo de filtro
        /// </summary>
        /// <returns></returns>
        public virtual string FilterTypeName()
        {
            return string.Empty;
        }

     
        
        /// <summary>
        /// Incia el filtro
        /// </summary>
        /// <param name="pEntity"></param>
        /// <returns></returns>
        public void Populate(CaseCommentBE pEntity)
        {
            

            try
            {
                
            }
            catch (Exception ex)
            {
                throw ex;
            }

            
        }

        /// <summary>
        /// Lleva a cabo el evento focus sobre un filtro.
        /// </summary>
        public virtual void FocusFilter()
        { }
    }
}
