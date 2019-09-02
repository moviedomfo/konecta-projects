namespace ScrollUpDemo_w32
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pnlCaseComment = new System.Windows.Forms.Panel();
            this.tbComments = new System.Windows.Forms.TableLayoutPanel();
            this.btnSearchMore = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.pnlCaseComment.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlCaseComment
            // 
            this.pnlCaseComment.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(234)))), ((int)(((byte)(234)))));
            this.pnlCaseComment.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pnlCaseComment.Controls.Add(this.radioButton2);
            this.pnlCaseComment.Controls.Add(this.radioButton1);
            this.pnlCaseComment.Controls.Add(this.btnSearchMore);
            this.pnlCaseComment.Controls.Add(this.button1);
            this.pnlCaseComment.Controls.Add(this.tbComments);
            this.pnlCaseComment.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCaseComment.Location = new System.Drawing.Point(0, 0);
            this.pnlCaseComment.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pnlCaseComment.Name = "pnlCaseComment";
            this.pnlCaseComment.Size = new System.Drawing.Size(1213, 712);
            this.pnlCaseComment.TabIndex = 35;
            // 
            // tbComments
            // 
            this.tbComments.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbComments.AutoScroll = true;
            this.tbComments.BackColor = System.Drawing.Color.White;
            this.tbComments.ColumnCount = 1;
            this.tbComments.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tbComments.Location = new System.Drawing.Point(214, 62);
            this.tbComments.Margin = new System.Windows.Forms.Padding(4);
            this.tbComments.Name = "tbComments";
            this.tbComments.RowCount = 1;
            this.tbComments.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tbComments.Size = new System.Drawing.Size(984, 638);
            this.tbComments.TabIndex = 3;
            // 
            // btnSearchMore
            // 
            this.btnSearchMore.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearchMore.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(47)))), ((int)(((byte)(92)))));
            this.btnSearchMore.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSearchMore.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSearchMore.ForeColor = System.Drawing.Color.White;
            this.btnSearchMore.Location = new System.Drawing.Point(220, 20);
            this.btnSearchMore.Margin = new System.Windows.Forms.Padding(3, 11, 3, 11);
            this.btnSearchMore.Name = "btnSearchMore";
            this.btnSearchMore.Size = new System.Drawing.Size(978, 34);
            this.btnSearchMore.TabIndex = 0;
            this.btnSearchMore.Text = "MOSTRAR MAS";
            this.btnSearchMore.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnSearchMore.Click += new System.EventHandler(this.BtnSearchMore_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(20, 41);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(71, 36);
            this.button1.TabIndex = 4;
            this.button1.Text = "Limbiar";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Checked = true;
            this.radioButton1.Location = new System.Drawing.Point(41, 123);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(93, 21);
            this.radioButton1.TabIndex = 5;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Scroll Top";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(41, 166);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(108, 21);
            this.radioButton2.TabIndex = 6;
            this.radioButton2.Text = "Scroll button";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1213, 712);
            this.Controls.Add(this.pnlCaseComment);
            this.Name = "Form1";
            this.Text = "Form1";
            this.pnlCaseComment.ResumeLayout(false);
            this.pnlCaseComment.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlCaseComment;
        private System.Windows.Forms.TableLayoutPanel tbComments;
        private System.Windows.Forms.Label btnSearchMore;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton1;
    }
}

