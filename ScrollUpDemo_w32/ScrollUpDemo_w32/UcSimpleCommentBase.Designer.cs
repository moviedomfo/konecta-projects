namespace Epiron.Gestion.Front.CaseComment.UserControls
{
    partial class UcSimpleCommentBase
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
         void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pnlComment = new System.Windows.Forms.Panel();
            this.lblComment = new System.Windows.Forms.TextBox();
            this.lblCmtInfo = new DevExpress.XtraEditors.LabelControl();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copiarUsuarioToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imgMark = new System.Windows.Forms.PictureBox();
            this.imgChannel = new System.Windows.Forms.PictureBox();
            this.BorderLeft = new System.Windows.Forms.Label();
            this.lblDate = new DevExpress.XtraEditors.LabelControl();
            this.imgMd = new System.Windows.Forms.PictureBox();
            this.webBrowserComment = new System.Windows.Forms.WebBrowser();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.lblCommentId = new DevExpress.XtraEditors.LabelControl();
            this.pnlComment.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imgMark)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgChannel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgMd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlComment
            // 
            this.pnlComment.BackColor = System.Drawing.Color.Transparent;
            this.pnlComment.Controls.Add(this.lblCommentId);
            this.pnlComment.Controls.Add(this.lblComment);
            this.pnlComment.Controls.Add(this.lblCmtInfo);
            this.pnlComment.Controls.Add(this.imgMark);
            this.pnlComment.Controls.Add(this.imgChannel);
            this.pnlComment.Controls.Add(this.BorderLeft);
            this.pnlComment.Controls.Add(this.lblDate);
            this.pnlComment.Controls.Add(this.imgMd);
            this.pnlComment.Controls.Add(this.webBrowserComment);
            this.pnlComment.Location = new System.Drawing.Point(0, 0);
            this.pnlComment.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnlComment.Name = "pnlComment";
            this.pnlComment.Padding = new System.Windows.Forms.Padding(0, 0, 4, 2);
            this.pnlComment.Size = new System.Drawing.Size(608, 86);
            this.pnlComment.TabIndex = 7;
            // 
            // lblComment
            // 
            this.lblComment.BackColor = System.Drawing.Color.White;
            this.lblComment.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lblComment.ForeColor = System.Drawing.Color.DimGray;
            this.lblComment.Location = new System.Drawing.Point(9, 46);
            this.lblComment.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.lblComment.Multiline = true;
            this.lblComment.Name = "lblComment";
            this.lblComment.ReadOnly = true;
            this.lblComment.Size = new System.Drawing.Size(591, 34);
            this.lblComment.TabIndex = 27;
            // 
            // lblCmtInfo
            // 
            this.lblCmtInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCmtInfo.Appearance.ForeColor = System.Drawing.Color.Gray;
            this.lblCmtInfo.ContextMenuStrip = this.contextMenuStrip1;
            this.lblCmtInfo.Location = new System.Drawing.Point(32, 26);
            this.lblCmtInfo.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.lblCmtInfo.Name = "lblCmtInfo";
            this.lblCmtInfo.Size = new System.Drawing.Size(63, 16);
            this.lblCmtInfo.TabIndex = 28;
            this.lblCmtInfo.Text = "Caso Nº xx";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copiarUsuarioToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.contextMenuStrip1.Size = new System.Drawing.Size(227, 28);
            // 
            // copiarUsuarioToolStripMenuItem
            // 
            this.copiarUsuarioToolStripMenuItem.Name = "copiarUsuarioToolStripMenuItem";
            this.copiarUsuarioToolStripMenuItem.Size = new System.Drawing.Size(226, 24);
            this.copiarUsuarioToolStripMenuItem.Text = "Copiar Datos del Caso";
            // 
            // imgMark
            // 
            this.imgMark.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.imgMark.BackColor = System.Drawing.Color.Transparent;
            this.imgMark.Location = new System.Drawing.Point(274, 0);
            this.imgMark.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.imgMark.Name = "imgMark";
            this.imgMark.Size = new System.Drawing.Size(42, 20);
            this.imgMark.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.imgMark.TabIndex = 26;
            this.imgMark.TabStop = false;
            // 
            // imgChannel
            // 
            this.imgChannel.Location = new System.Drawing.Point(55, 2);
            this.imgChannel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.imgChannel.Name = "imgChannel";
            this.imgChannel.Size = new System.Drawing.Size(21, 18);
            this.imgChannel.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.imgChannel.TabIndex = 12;
            this.imgChannel.TabStop = false;
            // 
            // BorderLeft
            // 
            this.BorderLeft.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.BorderLeft.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(47)))), ((int)(((byte)(92)))));
            this.BorderLeft.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BorderLeft.ForeColor = System.Drawing.Color.DimGray;
            this.BorderLeft.Location = new System.Drawing.Point(0, 0);
            this.BorderLeft.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.BorderLeft.Name = "BorderLeft";
            this.BorderLeft.Size = new System.Drawing.Size(4, 83);
            this.BorderLeft.TabIndex = 11;
            this.BorderLeft.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDate
            // 
            this.lblDate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDate.Appearance.ForeColor = System.Drawing.Color.Gray;
            this.lblDate.Location = new System.Drawing.Point(175, 2);
            this.lblDate.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(82, 16);
            this.lblDate.TabIndex = 6;
            this.lblDate.Text = "Hace 1 minuto";
            // 
            // imgMd
            // 
            this.imgMd.BackColor = System.Drawing.Color.Transparent;
            this.imgMd.Location = new System.Drawing.Point(55, 0);
            this.imgMd.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.imgMd.Name = "imgMd";
            this.imgMd.Size = new System.Drawing.Size(21, 18);
            this.imgMd.TabIndex = 4;
            this.imgMd.TabStop = false;
            this.imgMd.Click += new System.EventHandler(this.imgMd_Click);
            // 
            // webBrowserComment
            // 
            this.webBrowserComment.Location = new System.Drawing.Point(9, 48);
            this.webBrowserComment.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.webBrowserComment.MinimumSize = new System.Drawing.Size(26, 22);
            this.webBrowserComment.Name = "webBrowserComment";
            this.webBrowserComment.Size = new System.Drawing.Size(591, 34);
            this.webBrowserComment.TabIndex = 30;
            this.webBrowserComment.Visible = false;
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
            this.barManager1.OptionsLayout.AllowAddNewItems = false;
            this.barManager1.ShowFullMenusAfterDelay = false;
            this.barManager1.ShowScreenTipsInToolbars = false;
            this.barManager1.ShowShortcutInScreenTips = false;
            // 
            // lblCommentId
            // 
            this.lblCommentId.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCommentId.Appearance.ForeColor = System.Drawing.Color.Gray;
            this.lblCommentId.ContextMenuStrip = this.contextMenuStrip1;
            this.lblCommentId.Location = new System.Drawing.Point(12, 4);
            this.lblCommentId.Margin = new System.Windows.Forms.Padding(4);
            this.lblCommentId.Name = "lblCommentId";
            this.lblCommentId.Size = new System.Drawing.Size(66, 16);
            this.lblCommentId.TabIndex = 31;
            this.lblCommentId.Text = "CommentId";
            // 
            // UcSimpleCommentBase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.pnlComment);
            this.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.Name = "UcSimpleCommentBase";
            this.Size = new System.Drawing.Size(608, 87);
            this.SizeChanged += new System.EventHandler(this.pnlComent_SizeChanged);
            this.pnlComment.ResumeLayout(false);
            this.pnlComment.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.imgMark)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgChannel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgMd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        protected System.Windows.Forms.PictureBox imgMd;
        protected System.Windows.Forms.Label BorderLeft;
        protected System.Windows.Forms.PictureBox imgChannel;
        protected DevExpress.XtraEditors.LabelControl lblDate;
        
        protected System.Windows.Forms.Panel pnlComment;
        
        protected System.Windows.Forms.PictureBox imgMark;
        protected System.Windows.Forms.TextBox lblComment;
        protected DevExpress.XtraEditors.LabelControl lblCmtInfo;
        
        private System.Windows.Forms.ToolTip toolTip1;
        private DevExpress.XtraBars.BarManager barManager1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem copiarUsuarioToolStripMenuItem;
        private System.Windows.Forms.WebBrowser webBrowserComment;
        protected DevExpress.XtraEditors.LabelControl lblCommentId;
    }
}
