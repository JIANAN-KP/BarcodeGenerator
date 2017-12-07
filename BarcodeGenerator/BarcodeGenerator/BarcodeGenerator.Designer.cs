namespace BarcodeGenerator
{
    partial class BarcodeGenerator
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BarcodeGenerator));
            this.label1 = new System.Windows.Forms.Label();
            this.dtPicker = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnTaskFile = new System.Windows.Forms.Button();
            this.FolderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.label3 = new System.Windows.Forms.Label();
            this.tbSystemMsg = new System.Windows.Forms.RichTextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbAssay = new System.Windows.Forms.CheckBox();
            this.cbDaughter = new System.Windows.Forms.CheckBox();
            this.cbExtraction = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rbPrinter = new System.Windows.Forms.RadioButton();
            this.rbLabeler = new System.Windows.Forms.RadioButton();
            this.Label5 = new System.Windows.Forms.Label();
            this.cbTest = new System.Windows.Forms.CheckBox();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.label4 = new System.Windows.Forms.Label();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 106);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Date: ";
            // 
            // dtPicker
            // 
            this.dtPicker.Location = new System.Drawing.Point(168, 106);
            this.dtPicker.Name = "dtPicker";
            this.dtPicker.Size = new System.Drawing.Size(200, 22);
            this.dtPicker.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 140);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(126, 17);
            this.label2.TabIndex = 0;
            this.label2.Text = "Country Code(CC):";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(168, 140);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(200, 22);
            this.textBox1.TabIndex = 2;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(520, 454);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 29);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "&Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnTaskFile
            // 
            this.btnTaskFile.Location = new System.Drawing.Point(14, 171);
            this.btnTaskFile.Name = "btnTaskFile";
            this.btnTaskFile.Size = new System.Drawing.Size(581, 43);
            this.btnTaskFile.TabIndex = 4;
            this.btnTaskFile.Text = "Select &Folder (7900/Nexar) and Print Barcodes";
            this.btnTaskFile.UseVisualStyleBackColor = true;
            this.btnTaskFile.Click += new System.EventHandler(this.btnTaskFile_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 218);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(122, 17);
            this.label3.TabIndex = 5;
            this.label3.Text = "System Remarks: ";
            // 
            // tbSystemMsg
            // 
            this.tbSystemMsg.Location = new System.Drawing.Point(14, 284);
            this.tbSystemMsg.Name = "tbSystemMsg";
            this.tbSystemMsg.Size = new System.Drawing.Size(581, 164);
            this.tbSystemMsg.TabIndex = 6;
            this.tbSystemMsg.Text = "";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbAssay);
            this.groupBox1.Controls.Add(this.cbDaughter);
            this.groupBox1.Controls.Add(this.cbExtraction);
            this.groupBox1.Location = new System.Drawing.Point(396, 70);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 83);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Print Option";
            // 
            // cbAssay
            // 
            this.cbAssay.AutoSize = true;
            this.cbAssay.Location = new System.Drawing.Point(106, 21);
            this.cbAssay.Name = "cbAssay";
            this.cbAssay.Size = new System.Drawing.Size(68, 21);
            this.cbAssay.TabIndex = 2;
            this.cbAssay.Text = "Assay";
            this.cbAssay.UseVisualStyleBackColor = true;
            // 
            // cbDaughter
            // 
            this.cbDaughter.AutoSize = true;
            this.cbDaughter.Location = new System.Drawing.Point(8, 49);
            this.cbDaughter.Name = "cbDaughter";
            this.cbDaughter.Size = new System.Drawing.Size(89, 21);
            this.cbDaughter.TabIndex = 1;
            this.cbDaughter.Text = "Daughter";
            this.cbDaughter.UseVisualStyleBackColor = true;
            // 
            // cbExtraction
            // 
            this.cbExtraction.AutoSize = true;
            this.cbExtraction.Location = new System.Drawing.Point(8, 21);
            this.cbExtraction.Name = "cbExtraction";
            this.cbExtraction.Size = new System.Drawing.Size(92, 21);
            this.cbExtraction.TabIndex = 0;
            this.cbExtraction.Text = "Extraction";
            this.cbExtraction.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rbPrinter);
            this.groupBox2.Controls.Add(this.rbLabeler);
            this.groupBox2.Location = new System.Drawing.Point(396, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(200, 52);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Print Device";
            // 
            // rbPrinter
            // 
            this.rbPrinter.AutoSize = true;
            this.rbPrinter.Location = new System.Drawing.Point(91, 25);
            this.rbPrinter.Name = "rbPrinter";
            this.rbPrinter.Size = new System.Drawing.Size(71, 21);
            this.rbPrinter.TabIndex = 1;
            this.rbPrinter.TabStop = true;
            this.rbPrinter.Text = "Printer";
            this.rbPrinter.UseVisualStyleBackColor = true;
            this.rbPrinter.CheckedChanged += new System.EventHandler(this.rbPrinter_CheckedChanged);
            // 
            // rbLabeler
            // 
            this.rbLabeler.AutoSize = true;
            this.rbLabeler.Location = new System.Drawing.Point(7, 25);
            this.rbLabeler.Name = "rbLabeler";
            this.rbLabeler.Size = new System.Drawing.Size(77, 21);
            this.rbLabeler.TabIndex = 0;
            this.rbLabeler.TabStop = true;
            this.rbLabeler.Text = "Labeler";
            this.rbLabeler.UseVisualStyleBackColor = true;
            this.rbLabeler.CheckedChanged += new System.EventHandler(this.rbLabeler_CheckedChanged);
            // 
            // Label5
            // 
            this.Label5.AutoSize = true;
            this.Label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label5.Location = new System.Drawing.Point(9, 12);
            this.Label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Label5.Name = "Label5";
            this.Label5.Size = new System.Drawing.Size(253, 25);
            this.Label5.TabIndex = 12;
            this.Label5.Text = "Barcode Generator[V1.9]";
            // 
            // cbTest
            // 
            this.cbTest.AutoSize = true;
            this.cbTest.Location = new System.Drawing.Point(15, 48);
            this.cbTest.Name = "cbTest";
            this.cbTest.Size = new System.Drawing.Size(231, 21);
            this.cbTest.TabIndex = 13;
            this.cbTest.Text = "In Test Mode (Printing disabled)";
            this.cbTest.UseVisualStyleBackColor = true;
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(91, 245);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(504, 23);
            this.progressBar.TabIndex = 15;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 248);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(73, 17);
            this.label4.TabIndex = 16;
            this.label4.Text = "Progress: ";
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog";
            this.openFileDialog.Multiselect = true;
            // 
            // BarcodeGenerator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(609, 495);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.cbTest);
            this.Controls.Add(this.Label5);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.tbSystemMsg);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnTaskFile);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.dtPicker);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "BarcodeGenerator";
            this.Text = "BarcodeGenerator";
            this.Load += new System.EventHandler(this.BarcodeGenerator_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtPicker;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnTaskFile;
        private System.Windows.Forms.FolderBrowserDialog FolderBrowserDialog;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RichTextBox tbSystemMsg;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox cbDaughter;
        private System.Windows.Forms.CheckBox cbExtraction;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rbPrinter;
        private System.Windows.Forms.RadioButton rbLabeler;
        internal System.Windows.Forms.Label Label5;
        private System.Windows.Forms.CheckBox cbTest;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox cbAssay;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
    }
}