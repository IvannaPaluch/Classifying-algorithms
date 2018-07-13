namespace PracaDyplomowa
{
    partial class Form1
    {
        /// <summary>
        /// Wymagana zmienna projektanta.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Wyczyść wszystkie używane zasoby.
        /// </summary>
        /// <param name="disposing">prawda, jeżeli zarządzane zasoby powinny zostać zlikwidowane; Fałsz w przeciwnym wypadku.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Kod generowany przez Projektanta formularzy systemu Windows

        /// <summary>
        /// Wymagana metoda obsługi projektanta — nie należy modyfikować 
        /// zawartość tej metody z edytorem kodu.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonChose = new System.Windows.Forms.Button();
            this.labelWay = new System.Windows.Forms.Label();
            this.openFileDialogWay = new System.Windows.Forms.OpenFileDialog();
            this.buttonGenerate = new System.Windows.Forms.Button();
            this.lable_reguly = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonChose
            // 
            this.buttonChose.Location = new System.Drawing.Point(572, 12);
            this.buttonChose.Name = "buttonChose";
            this.buttonChose.Size = new System.Drawing.Size(75, 23);
            this.buttonChose.TabIndex = 0;
            this.buttonChose.Text = "Wybierz";
            this.buttonChose.UseVisualStyleBackColor = true;
            this.buttonChose.Click += new System.EventHandler(this.buttonChose_Click);
            // 
            // labelWay
            // 
            this.labelWay.AutoSize = true;
            this.labelWay.Location = new System.Drawing.Point(32, 20);
            this.labelWay.Name = "labelWay";
            this.labelWay.Size = new System.Drawing.Size(0, 13);
            this.labelWay.TabIndex = 1;
            this.labelWay.Click += new System.EventHandler(this.labelWay_Click);
            // 
            // openFileDialogWay
            // 
            this.openFileDialogWay.FileName = "openFileDialog1";
            // 
            // buttonGenerate
            // 
            this.buttonGenerate.Location = new System.Drawing.Point(572, 41);
            this.buttonGenerate.Name = "buttonGenerate";
            this.buttonGenerate.Size = new System.Drawing.Size(75, 23);
            this.buttonGenerate.TabIndex = 2;
            this.buttonGenerate.Text = " Generuj";
            this.buttonGenerate.UseVisualStyleBackColor = true;
            this.buttonGenerate.Click += new System.EventHandler(this.buttonGenerate_Click);
            // 
            // lable_reguly
            // 
            this.lable_reguly.AutoSize = true;
            this.lable_reguly.Location = new System.Drawing.Point(32, 98);
            this.lable_reguly.Name = "lable_reguly";
            this.lable_reguly.Size = new System.Drawing.Size(0, 13);
            this.lable_reguly.TabIndex = 3;
            this.lable_reguly.Click += new System.EventHandler(this.lable_reguly_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(35, 57);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Otrzymane reguły:";
            this.label1.Visible = false;
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(572, 377);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(97, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "Zapisz do pliku";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(675, 412);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lable_reguly);
            this.Controls.Add(this.buttonGenerate);
            this.Controls.Add(this.labelWay);
            this.Controls.Add(this.buttonChose);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonChose;
        private System.Windows.Forms.Label labelWay;
        private System.Windows.Forms.OpenFileDialog openFileDialogWay;
        private System.Windows.Forms.Button buttonGenerate;
        private System.Windows.Forms.Label lable_reguly;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
    }
}

