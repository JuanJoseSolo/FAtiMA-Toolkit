﻿
namespace IntegratedAuthoringToolWF.IEP
{
    partial class QuickAddBeliefs
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QuickAddBeliefs));
            this.inputBox = new System.Windows.Forms.GroupBox();
            this.descriptionText = new System.Windows.Forms.RichTextBox();
            this.processInput = new System.Windows.Forms.Button();
            this.inputBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // inputBox
            // 
            this.inputBox.Controls.Add(this.descriptionText);
            this.inputBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.inputBox.Location = new System.Drawing.Point(13, 13);
            this.inputBox.Margin = new System.Windows.Forms.Padding(4);
            this.inputBox.Name = "inputBox";
            this.inputBox.Padding = new System.Windows.Forms.Padding(4);
            this.inputBox.Size = new System.Drawing.Size(373, 113);
            this.inputBox.TabIndex = 3;
            this.inputBox.TabStop = false;
            this.inputBox.Text = "Input (write beliefs here)";
            // 
            // descriptionText
            // 
            this.descriptionText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.descriptionText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.descriptionText.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.descriptionText.ForeColor = System.Drawing.SystemColors.ControlText;
            this.descriptionText.Location = new System.Drawing.Point(24, 32);
            this.descriptionText.Margin = new System.Windows.Forms.Padding(20);
            this.descriptionText.Name = "descriptionText";
            this.descriptionText.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.descriptionText.Size = new System.Drawing.Size(325, 79);
            this.descriptionText.TabIndex = 0;
            this.descriptionText.Text = "John is a student";
            // 
            // processInput
            // 
            this.processInput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.processInput.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.processInput.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.processInput.Image = global::IntegratedAuthoringToolWF.Properties.Resources.right_arrow2;
            this.processInput.Location = new System.Drawing.Point(110, 132);
            this.processInput.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.processInput.Name = "processInput";
            this.processInput.Size = new System.Drawing.Size(174, 38);
            this.processInput.TabIndex = 4;
            this.processInput.Text = "    Process Input";
            this.processInput.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.processInput.UseVisualStyleBackColor = true;
            this.processInput.Click += new System.EventHandler(this.processInput_Click);
            // 
            // QuickAddBeliefs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(405, 189);
            this.Controls.Add(this.processInput);
            this.Controls.Add(this.inputBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "QuickAddBeliefs";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Quick Belief Generator Wizard";
            this.Load += new System.EventHandler(this.QuickAddBeliefs_Load);
            this.inputBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox inputBox;
        private System.Windows.Forms.RichTextBox descriptionText;
        private System.Windows.Forms.Button processInput;
    }
}