namespace Epiron.Gestion.Front.CaseComment.UserControls
{
    partial class UcSimpleCommentUser
    {
        /// <summary> 
        /// Variable del diseñador requerida.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Limpiar los recursos que se estén utilizando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben eliminar; false en caso contrario, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de componentes

        /// <summary> 
        /// Método necesario para admitir el Diseñador. No se puede modificar 
        /// el contenido del método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.imgMd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgChannel)).BeginInit();
            
            this.pnlComment.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imgMark)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // lblDate
            // 
            this.lblDate.Appearance.ForeColor = System.Drawing.Color.Gray;
            // 
            // pnlComment
            // 
            this.pnlComment.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            // 
            // lblComment
            // 
            this.lblComment.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            // 
            // lblCmtInfo
            // 
            this.lblCmtInfo.Appearance.ForeColor = System.Drawing.Color.Gray;
            // 
            // barManager1
            // 
            this.barManager1.AllowCustomization = false;
            this.barManager1.AllowItemAnimatedHighlighting = false;
            this.barManager1.AllowMoveBarOnToolbar = false;
            this.barManager1.AllowQuickCustomization = false;
            this.barManager1.AllowShowToolbarsPopup = false;
            this.barManager1.CloseButtonAffectAllTabs = false;
            this.barManager1.DockingEnabled = false;
            this.barManager1.Form = this;
            this.barManager1.HideBarsWhenMerging = false;
            this.barManager1.MaxItemId = 0;
            // 
            // UcSimpleCommentUser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BckgColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.Name = "UcSimpleCommentUser";
            ((System.ComponentModel.ISupportInitialize)(this.imgMd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgChannel)).EndInit();
            
            this.pnlComment.ResumeLayout(false);
            this.pnlComment.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imgMark)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraBars.BarManager barManager1;
    }
}
