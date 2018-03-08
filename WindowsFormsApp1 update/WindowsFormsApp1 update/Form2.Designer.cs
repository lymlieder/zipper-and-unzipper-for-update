namespace WindowsFormsApp1_update
{
    partial class Form2
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
            this.detailShow = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // detailShow
            // 
            this.detailShow.Location = new System.Drawing.Point(12, 12);
            this.detailShow.Multiline = true;
            this.detailShow.Name = "detailShow";
            this.detailShow.ReadOnly = true;
            this.detailShow.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.detailShow.Size = new System.Drawing.Size(528, 350);
            this.detailShow.TabIndex = 0;
            this.detailShow.TextChanged += new System.EventHandler(this.detailShow_TextChanged);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(552, 374);
            this.Controls.Add(this.detailShow);
            this.Name = "Form2";
            this.Text = "Form2";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.TextBox detailShow;
    }
}