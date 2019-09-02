using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Epiron.Gestion.Components;

using DevExpress.XtraBars;

using System.IO;
using System.Drawing.Text;
using ScrollUpDemo_w32;

namespace Epiron.Gestion.Front.CaseComment.UserControls
{
    public partial class UcSimpleCommentBase : UcBase
    {
        //public delegate void PopupHandler(object sender, EventArgs e);
        public Action<object, EventArgs> OnPopUpOpen = null;
        public Action<object, EventArgs> OnPopUpClose = null;

  
        protected bool _TagModuleEnable = false;
        protected int? _CtrlMaxWidth = null;
        protected bool _IsNewMessage = false;
        protected DevExpress.Utils.ToolTipController _ToolTipController = null;
        protected Color _DefaultBackcolor = Color.Transparent;
        private bool _IsPostAndComments = false;
        private int _ViewAutomatedTagged;
       

        public Color DefaultBckColor
        {
            get { return this._DefaultBackcolor; }
            set { this._DefaultBackcolor = value; }
        }

       

        public virtual int? CtrlMaxWidth
        {
            get { return this._CtrlMaxWidth; }
            set { this._CtrlMaxWidth = value; }
        }

        public Color BckgColor
        {
            set
            {
                pnlComment.BackColor = value;
                lblComment.BackColor = value;
            }
            get { return pnlComment.BackColor; }
        }

        public bool IsNewMessage
        {
            get { return this._IsNewMessage; }
        }

        public int ViewAutomatedTagged
        {
            set { this._ViewAutomatedTagged = value; }
        }

        public UcSimpleCommentBase()
        {
            InitializeComponent();

            //Modo de diseño
            if (System.ComponentModel.LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;

            this.Disposed += (s, e) =>
            {
                if (this._ToolTipController != null)
                {
                    this._ToolTipController.Dispose();
                    this._ToolTipController = null;
                }

              
            };

            


            //Cargamos la Fuente para Emojis
            //PrivateFontCollection private_fonts = new PrivateFontCollection();
            //string wResourceFontPath = "\\Resources\\Fonts\\";
            //string wResource = "seguiemj.ttf";
            //private_fonts = Helper.LoadFont(Application.StartupPath + wResourceFontPath + wResource);
            //int wFontSize = 14;
            //if (private_fonts != null)
            //{
            //    elementHostWPF.Font = new Font(private_fonts.Families[0], wFontSize);
            //}
        }

        #region << -Eventos- >>
        /// <summary>
        /// Acomoda el alto del control segun el comentario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pnlComent_SizeChanged(object sender, EventArgs e)
        {
            this.Resized();
        }

        /// <summary>
        /// Acomoda el alto del control segun el comentario
        /// </summary>
        public void Resized()
        {
           

            
        }

        /// <summary>
        /// Acomoda el alto del control segun el comentario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void btnExpand_Click(object sender, EventArgs e)
        {
           
        }

        #endregion

        #region << -Métodos- >>
        /// <summary>
        /// Popula el control
        /// </summary>
        /// <param name="pEntity"></param>
        /// <returns></returns>
        public  void Populate(CaseCommentBE pEntity)
        {

            lblCmtInfo.Text = string.Format("UserChannelId: {0} ", pEntity.UserChannelId.ToString());
            this.lblComment.Text = pEntity.CaseCommentText;
            lblCommentId.Text = pEntity.CaseCommentId.ToString();


            //Me fijo si el mensaje es de hoy            
            TimeSpan wTs = DateTime.Now - pEntity.CaseCommentCreated;
            if (wTs.Days == 0) //Es de hoy
            {
                //Tiempo transcurrido desde su creación
                lblDate.Text = Helper.TimeElapsedToString(wTs.Seconds);

                lblDate.Text = string.Concat(lblDate.Text, "/ Caso: ", pEntity.CaseId.ToString());
            }
            else //Mensaje viejo
            {
                lblDate.Text = pEntity.CreatedRowOrigenLog.ToString();
                lblDate.Text = string.Concat(lblDate.Text, "/ Caso: ", pEntity.CaseId.ToString());
            }
            lblDate.ToolTip = pEntity.CaseCommentCreated.ToString();
        }

        void imgChannel_Click(object sender, EventArgs e)
        {
         
            //Common.Helper.ShowInfoBox("URL copiada al portapapeles");
        }

     

        /// <summary>
        /// Setea el tiempo de creación
        /// </summary>
        /// <param name="pTimeInSeconds"></param>
        public void UpdateDate(DateTime pCurrentDate)
        {
            //int wDiff = lblDate.Width;
            //TimeSpan wTs = pCurrentDate - this._Comment.CreationDateLog;

            //if (wTs.Days > 0)
            //    return;

            //lblDate.Text = BackEnd.Common.Helper.TimeElapsedToString((int)wTs.TotalSeconds);
            //lblDate.Text = string.Concat(lblDate.Text, "/ Caso: ", this._Comment.CaseId.ToString());

            ////Actualizo la posición de los controles, si el ancho de la fecha crecio
            //wDiff = wDiff - lblDate.Width;

            //imgMark.Location = new Point(lblDate.Location.X + lblDate.Width + 3, ucTagComment.Location.Y);
            //ucTagComment.Location = new Point(imgMark.Location.X + imgMark.Width + 3, ucTagComment.Location.Y);
            //if (wDiff > 0)
            //    ucTagComment.Width -= Math.Abs(wDiff);
            //else
            //    ucTagComment.Width += Math.Abs(wDiff);
        }

        /// <summary>
        /// Le agrega al comentario la marca de nuevo
        /// </summary>
        public void MarkAsNew()
        {
            this._IsNewMessage = true;
            //imgMark.Image = Properties.Resources.NewLabel_48x48;
        }

        /// <summary>
        /// Le agrega al comentario la marca de no enviado
        /// </summary>
        public void MarkAsNotSended()
        {
      
            toolTip1.SetToolTip(imgMark, "El mensaje no fue enviado");
        }

        /// <summary>
        /// Limpia las marcas
        /// </summary>
        public void CleanMark()
        {
            this._IsNewMessage = false;
            imgMark.Image = null;
            toolTip1.SetToolTip(imgMark, "");
        }

        /// <summary>
        /// Busca patrones 0x en un mensaje.
        /// </summary>
        private string FindAndReplace0x(string pMessage)
        {
            try
            {
                List<Int32> wIdx = new List<int>();
                string wHex;
                string wUtf16 = "";

                string s = pMessage;

                string a = "0";
                int l = s.Length;
                int c = 0;

                for (int i = 0; i < l; i++)
                {
                    if (s[i] == a[0])
                    {
                        c = i + 1;
                        if (c == l)
                            break;
                        if (s[c].ToString() == "x")
                            wIdx.Add(i);
                    }
                }

                foreach (int w in wIdx)
                {
                    //if (w != 0 && w == wIdx.Last())
                    //    break;

                    Char isSix = s.Substring(w, 3).Last();

                    if (Char.IsDigit(isSix))
                        wHex = s.Substring(w, 6);
                    else
                    {
                        if (w == wIdx.Last())
                            wHex = s.Substring(w, 6);
                        else
                            wHex = s.Substring(w, 12);
                    }


                    if (!string.IsNullOrEmpty(wUtf16))
                        pMessage = pMessage.Replace(wHex, wUtf16);
                    else
                        pMessage = pMessage.Replace(wHex, string.Empty);
                }
            }
            catch
            {
                return pMessage;
            }

            return pMessage;
        }




        #endregion

        private void lblCmtInfo_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject(lblCmtInfo.Text);
        }

        private void imgMd_Click(object sender, EventArgs e)
        {
            if (!_IsPostAndComments)
                return;

            try
            {
                //UcFilterParams wUcFilterParams = new UcFilterParams();
                //wUcFilterParams.AllTree = true;
                //wUcFilterParams.PostId = this._Comment.ElementId.Value;
                //wUcFilterParams.SCInternalCode = this._Comment.SCInternalCode;

                //UcPostAndComment wUcPostAndComment = new UcPostAndComment();
                //wUcPostAndComment.Populate(wUcFilterParams);

                PopupControlContainer popup = new PopupControlContainer();
                
                
                popup.BringToFront();
                popup.ShowPopup(barManager1, this.PointToScreen(this.imgMd.Location));
            }
            catch (Exception ex)
            {
                //Helper.ShowErrorMessage(ex);
            }
        }
    }
}