using Epiron.Gestion.Front.CaseComment.UserControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScrollUpDemo_w32
{
    public partial class Form1 : Form
    {
        CaseCommentList currentCaseCommenList;
        int lastItemId = 0;
        public Form1()
        {
            InitializeComponent();
            currentCaseCommenList = new CaseCommentList();
        }

        private void BtnSearchMore_Click(object sender, EventArgs e)
        {
            try
            {
                //this.ShowLoadingPanel();
                if(currentCaseCommenList.Count==0)
                    LoadComments(true, false);
                else
                    this.LoadComments(false, false);
                //Foco en el botón "Mostrar más"
                //tbComments.ScrollControlIntoView(this.btnSearchMore);


                //verr private void CommentViewOrderAscendant()
            }
            catch (Exception fex)
            {
                MessageBox.Show(fex.Message);
            }
        }
        private void Button1_Click(object sender, EventArgs e)
        {
            this.currentCaseCommenList.Clear();
            tbComments.Controls.Clear();
            lastItemId = 0;

        }
       
        private void LoadComments(bool pRefresh, bool pRefreshTimeLine)
        {  
           

           
            var list = CaseCommenDAC.search(lastItemId);
          
            list.ForEach(item =>
            {
                setControl(item);
            });
            //posiciono el scroll
            tbComments.ScrollControlIntoView(tbComments.Controls[tbComments.Controls.Count-1]);
            //Agrego el nuevo set de datos
            currentCaseCommenList.AddRange(list.ToArray());
            //Busco el lastItemId mas grande obtenido o el ultimo ya que esta ordenado asc por CaseCommentId
            lastItemId = currentCaseCommenList.Last().CaseCommentId.Value;
            //Foco en el primer control
            if(radioButton1.Checked)
                Helper.TableLayoutPanel_ScrollToTop(tbComments);  
            else
                Helper.TableLayoutPanel_ScrollToBotton(tbComments);
        }

        void setControl(CaseCommentBE item)
        {
            UcSimpleCommentUser ctrl = new UcSimpleCommentUser();
            ctrl.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
            ctrl.Populate(item);
            AddCommentControl(ctrl);
            
        }
        /// <summary>
        /// Cargo cada contrl siempre por ensima para que el mas viejo quede en la parte superior del TaableLayout
        /// </summary>
        /// <param name="pControl"></param>
        /// <returns></returns>
        private void AddCommentControl(Control ctrl)
        {
            //Agrego una row
            tbComments.RowCount++;
            //Corro todos para abajo
            foreach (Control c in tbComments.Controls)
            {
                tbComments.SetRow(c, tbComments.GetRow(c) + 1);
            }
            tbComments.Controls.Add(ctrl, 0, 0);
        }

        
    }
}
