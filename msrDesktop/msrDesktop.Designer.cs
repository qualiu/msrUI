
namespace msrDesktop
{
    partial class msrDesktop
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(msrDesktop));
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.labelExamples = new System.Windows.Forms.Label();
            this.numericUpDownExampleCount = new System.Windows.Forms.NumericUpDown();
            this.checkBoxShowExamples = new System.Windows.Forms.CheckBox();
            this.textBoxForbidArgNamePattern = new System.Windows.Forms.TextBox();
            this.checkBoxForbidArgNamePattern = new System.Windows.Forms.CheckBox();
            this.textBoxHideArgNamePattern = new System.Windows.Forms.TextBox();
            this.checkBoxHideArgNamePattern = new System.Windows.Forms.CheckBox();
            this.buttonValidateArgs = new System.Windows.Forms.Button();
            this.labelUsage = new System.Windows.Forms.Label();
            this.checkBoxUseLongArgName = new System.Windows.Forms.CheckBox();
            this.richTextBoxInfo = new System.Windows.Forms.RichTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownExampleCount)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer
            // 
            this.splitContainer.Cursor = System.Windows.Forms.Cursors.HSplit;
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 0);
            this.splitContainer.Margin = new System.Windows.Forms.Padding(4);
            this.splitContainer.Name = "splitContainer";
            this.splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.labelExamples);
            this.splitContainer.Panel1.Controls.Add(this.numericUpDownExampleCount);
            this.splitContainer.Panel1.Controls.Add(this.checkBoxShowExamples);
            this.splitContainer.Panel1.Controls.Add(this.textBoxForbidArgNamePattern);
            this.splitContainer.Panel1.Controls.Add(this.checkBoxForbidArgNamePattern);
            this.splitContainer.Panel1.Controls.Add(this.textBoxHideArgNamePattern);
            this.splitContainer.Panel1.Controls.Add(this.checkBoxHideArgNamePattern);
            this.splitContainer.Panel1.Controls.Add(this.buttonValidateArgs);
            this.splitContainer.Panel1.Controls.Add(this.labelUsage);
            this.splitContainer.Panel1.Controls.Add(this.checkBoxUseLongArgName);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.richTextBoxInfo);
            this.splitContainer.Size = new System.Drawing.Size(1805, 1075);
            this.splitContainer.SplitterDistance = 886;
            this.splitContainer.SplitterWidth = 5;
            this.splitContainer.TabIndex = 0;
            // 
            // labelExamples
            // 
            this.labelExamples.AutoSize = true;
            this.labelExamples.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.labelExamples.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.labelExamples.ForeColor = System.Drawing.Color.Blue;
            this.labelExamples.Location = new System.Drawing.Point(1638, 8);
            this.labelExamples.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelExamples.Name = "labelExamples";
            this.labelExamples.Size = new System.Drawing.Size(103, 30);
            this.labelExamples.TabIndex = 9;
            this.labelExamples.Text = "Examples";
            this.labelExamples.Visible = false;
            // 
            // numericUpDownExampleCount
            // 
            this.numericUpDownExampleCount.Location = new System.Drawing.Point(458, 11);
            this.numericUpDownExampleCount.Maximum = new decimal(new int[] {
            9,
            0,
            0,
            0});
            this.numericUpDownExampleCount.Name = "numericUpDownExampleCount";
            this.numericUpDownExampleCount.Size = new System.Drawing.Size(56, 37);
            this.numericUpDownExampleCount.TabIndex = 8;
            this.numericUpDownExampleCount.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // checkBoxShowExamples
            // 
            this.checkBoxShowExamples.AutoSize = true;
            this.checkBoxShowExamples.Checked = true;
            this.checkBoxShowExamples.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxShowExamples.Location = new System.Drawing.Point(257, 9);
            this.checkBoxShowExamples.Name = "checkBoxShowExamples";
            this.checkBoxShowExamples.Size = new System.Drawing.Size(194, 34);
            this.checkBoxShowExamples.TabIndex = 7;
            this.checkBoxShowExamples.Text = "Show examples:";
            this.checkBoxShowExamples.UseVisualStyleBackColor = true;
            this.checkBoxShowExamples.CheckedChanged += new System.EventHandler(this.checkBoxShowExamples_CheckedChanged);
            // 
            // textBoxForbidArgNamePattern
            // 
            this.textBoxForbidArgNamePattern.Location = new System.Drawing.Point(928, 8);
            this.textBoxForbidArgNamePattern.Name = "textBoxForbidArgNamePattern";
            this.textBoxForbidArgNamePattern.Size = new System.Drawing.Size(179, 37);
            this.textBoxForbidArgNamePattern.TabIndex = 6;
            this.textBoxForbidArgNamePattern.Text = "-R -X --force";
            this.textBoxForbidArgNamePattern.TextChanged += new System.EventHandler(this.textBoxForbidArgNamePattern_TextChanged);
            // 
            // checkBoxForbidArgNamePattern
            // 
            this.checkBoxForbidArgNamePattern.AutoSize = true;
            this.checkBoxForbidArgNamePattern.Location = new System.Drawing.Point(826, 11);
            this.checkBoxForbidArgNamePattern.Name = "checkBoxForbidArgNamePattern";
            this.checkBoxForbidArgNamePattern.Size = new System.Drawing.Size(107, 34);
            this.checkBoxForbidArgNamePattern.TabIndex = 5;
            this.checkBoxForbidArgNamePattern.Tag = "";
            this.checkBoxForbidArgNamePattern.Text = "Forbid:";
            this.checkBoxForbidArgNamePattern.UseVisualStyleBackColor = true;
            this.checkBoxForbidArgNamePattern.CheckedChanged += new System.EventHandler(this.checkBoxForbidArgNamePattern_CheckedChanged);
            // 
            // textBoxHideArgNamePattern
            // 
            this.textBoxHideArgNamePattern.Location = new System.Drawing.Point(630, 10);
            this.textBoxHideArgNamePattern.Name = "textBoxHideArgNamePattern";
            this.textBoxHideArgNamePattern.Size = new System.Drawing.Size(180, 37);
            this.textBoxHideArgNamePattern.TabIndex = 4;
            this.textBoxHideArgNamePattern.Tag = "";
            this.textBoxHideArgNamePattern.Text = "-z -h --verbose";
            this.textBoxHideArgNamePattern.TextChanged += new System.EventHandler(this.textBoxHideArgNamePattern_TextChanged);
            // 
            // checkBoxHideArgNamePattern
            // 
            this.checkBoxHideArgNamePattern.AutoSize = true;
            this.checkBoxHideArgNamePattern.Checked = true;
            this.checkBoxHideArgNamePattern.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxHideArgNamePattern.Location = new System.Drawing.Point(544, 11);
            this.checkBoxHideArgNamePattern.Name = "checkBoxHideArgNamePattern";
            this.checkBoxHideArgNamePattern.Size = new System.Drawing.Size(90, 34);
            this.checkBoxHideArgNamePattern.TabIndex = 3;
            this.checkBoxHideArgNamePattern.Tag = "";
            this.checkBoxHideArgNamePattern.Text = "Hide:";
            this.checkBoxHideArgNamePattern.UseVisualStyleBackColor = true;
            this.checkBoxHideArgNamePattern.CheckedChanged += new System.EventHandler(this.checkBoxHideArgNamePattern_CheckedChanged);
            // 
            // buttonValidateArgs
            // 
            this.buttonValidateArgs.Location = new System.Drawing.Point(1135, 6);
            this.buttonValidateArgs.Margin = new System.Windows.Forms.Padding(4);
            this.buttonValidateArgs.Name = "buttonValidateArgs";
            this.buttonValidateArgs.Size = new System.Drawing.Size(165, 41);
            this.buttonValidateArgs.TabIndex = 1;
            this.buttonValidateArgs.Text = "&Validate args.";
            this.buttonValidateArgs.UseVisualStyleBackColor = true;
            this.buttonValidateArgs.Click += new System.EventHandler(this.ButtonValidateArgs_Click);
            // 
            // labelUsage
            // 
            this.labelUsage.AutoSize = true;
            this.labelUsage.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.labelUsage.ForeColor = System.Drawing.Color.Blue;
            this.labelUsage.Location = new System.Drawing.Point(1536, 9);
            this.labelUsage.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelUsage.Name = "labelUsage";
            this.labelUsage.Size = new System.Drawing.Size(94, 30);
            this.labelUsage.TabIndex = 2;
            this.labelUsage.Text = "TipLabel";
            this.labelUsage.Visible = false;
            // 
            // checkBoxUseLongArgName
            // 
            this.checkBoxUseLongArgName.AutoSize = true;
            this.checkBoxUseLongArgName.Location = new System.Drawing.Point(14, 9);
            this.checkBoxUseLongArgName.Margin = new System.Windows.Forms.Padding(4);
            this.checkBoxUseLongArgName.Name = "checkBoxUseLongArgName";
            this.checkBoxUseLongArgName.Size = new System.Drawing.Size(236, 34);
            this.checkBoxUseLongArgName.TabIndex = 0;
            this.checkBoxUseLongArgName.Tag = "Use long names like --ignore-case instead of -i.";
            this.checkBoxUseLongArgName.Text = "Use long arg names.";
            this.checkBoxUseLongArgName.UseVisualStyleBackColor = true;
            this.checkBoxUseLongArgName.CheckedChanged += new System.EventHandler(this.CheckBoxUseLongArgName_CheckedChanged);
            // 
            // richTextBoxInfo
            // 
            this.richTextBoxInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxInfo.Location = new System.Drawing.Point(0, 0);
            this.richTextBoxInfo.Margin = new System.Windows.Forms.Padding(4);
            this.richTextBoxInfo.Name = "richTextBoxInfo";
            this.richTextBoxInfo.Size = new System.Drawing.Size(1805, 184);
            this.richTextBoxInfo.TabIndex = 0;
            this.richTextBoxInfo.Text = "";
            // 
            // msrDesktop
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 30F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1805, 1075);
            this.Controls.Add(this.splitContainer);
            this.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "msrDesktop";
            this.Text = "Desktop for msr on 64+32 bit Windows + MinGW + Cygwin + Ubuntu + CentOS + Fedora";
            this.Load += new System.EventHandler(this.MsrDesktop_Load);
            this.SizeChanged += new System.EventHandler(this.MsrDesktop_SizeChanged);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel1.PerformLayout();
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownExampleCount)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.RichTextBox richTextBoxInfo;
        private System.Windows.Forms.CheckBox checkBoxUseLongArgName;
        private System.Windows.Forms.Label labelUsage;
        private System.Windows.Forms.Button buttonValidateArgs;
        private System.Windows.Forms.TextBox textBoxForbidArgNamePattern;
        private System.Windows.Forms.CheckBox checkBoxForbidArgNamePattern;
        private System.Windows.Forms.TextBox textBoxHideArgNamePattern;
        private System.Windows.Forms.CheckBox checkBoxHideArgNamePattern;
        private System.Windows.Forms.Label labelExamples;
        private System.Windows.Forms.NumericUpDown numericUpDownExampleCount;
        private System.Windows.Forms.CheckBox checkBoxShowExamples;
    }
}

