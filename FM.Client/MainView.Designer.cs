namespace FMShell
{
    partial class MainView
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
            this.components = new System.ComponentModel.Container();
            this.updateTimer = new System.Windows.Forms.Timer(this.components);
            this.cefWebBrowser1 = new FM.Lib.Controls.CefWebBrowser();
            this.SuspendLayout();
            // 
            // updateTimer
            // 
            this.updateTimer.Interval = 3000;
            this.updateTimer.Tick += new System.EventHandler(this.updateTimer_Tick);
            // 
            // cefWebBrowser1
            // 
            this.cefWebBrowser1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.cefWebBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cefWebBrowser1.Location = new System.Drawing.Point(0, 0);
            this.cefWebBrowser1.Name = "cefWebBrowser1";
            this.cefWebBrowser1.Size = new System.Drawing.Size(1066, 750);
            this.cefWebBrowser1.StartUrl = "http://192.168.2.160:8080/";
            this.cefWebBrowser1.TabIndex = 0;
            this.cefWebBrowser1.Text = "cefWebBrowser1";
            // 
            // MainView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Info;
            this.ClientSize = new System.Drawing.Size(1066, 750);
            this.Controls.Add(this.cefWebBrowser1);
            this.Name = "MainView";
            this.Text = "MainView";
            this.ResumeLayout(false);

        }

        #endregion

        private FM.Lib.Controls.CefWebBrowser cefWebBrowser1;
        private System.Windows.Forms.Timer updateTimer;
    }
}