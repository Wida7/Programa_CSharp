namespace Procesar_XML
{
    partial class btnCargar
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(btnCargar));
            this.button1 = new System.Windows.Forms.Button();
            this.pBarProceso = new System.Windows.Forms.ProgressBar();
            this.NombreArchivoLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(131, 54);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(345, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Cargar archivos";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // pBarProceso
            // 
            this.pBarProceso.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.pBarProceso.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.pBarProceso.Location = new System.Drawing.Point(69, 117);
            this.pBarProceso.Name = "pBarProceso";
            this.pBarProceso.Size = new System.Drawing.Size(474, 23);
            this.pBarProceso.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.pBarProceso.TabIndex = 1;
            // 
            // NombreArchivoLabel
            // 
            this.NombreArchivoLabel.Font = new System.Drawing.Font("Berlin Sans FB", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NombreArchivoLabel.Location = new System.Drawing.Point(12, 174);
            this.NombreArchivoLabel.Name = "NombreArchivoLabel";
            this.NombreArchivoLabel.Size = new System.Drawing.Size(586, 103);
            this.NombreArchivoLabel.TabIndex = 2;
            this.NombreArchivoLabel.Text = "\r\n\r\n\r\n\r\n\r\n\"Un sueño no se hace realidad por arte de magia, necesita sudor, determ" +
    "inación y trabajo duro\"  -Colin Powell";
            // 
            // btnCargar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.ClientSize = new System.Drawing.Size(610, 286);
            this.Controls.Add(this.NombreArchivoLabel);
            this.Controls.Add(this.pBarProceso);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "btnCargar";
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Text = "Procesar XML";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ProgressBar pBarProceso;
        private System.Windows.Forms.Label NombreArchivoLabel;
    }
}

