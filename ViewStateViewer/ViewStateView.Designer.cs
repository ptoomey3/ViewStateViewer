/* 
   Copyright (c) 2009, Neohapsis, Inc.
   All rights reserved.

 Implementation by Patrick Toomey

 Redistribution and use in source and binary forms, with or without modification, 
 are permitted provided that the following conditions are met: 

  - Redistributions of source code must retain the above copyright notice, this list 
    of conditions and the following disclaimer. 
  - Redistributions in binary form must reproduce the above copyright notice, this 
    list of conditions and the following disclaimer in the documentation and/or 
    other materials provided with the distribution. 
  - Neither the name of Neohapsis nor the names of its contributors may be used to 
    endorse or promote products derived from this software without specific prior 
    written permission. 

 THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND 
 ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED 
 WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE 
 DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR 
 ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES 
 (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; 
 LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON 
 ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT 
 (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS 
 SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/
using System.Windows.Forms;
namespace ViewState
{
    partial class ViewStateView : UserControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ViewStateView));
            this.viewStateEncodedTextBox = new System.Windows.Forms.RichTextBox();
            this.viewStateXMLEncodedTextBox = new System.Windows.Forms.RichTextBox();
            this.encodeButton = new System.Windows.Forms.Button();
            this.decodeButton = new System.Windows.Forms.Button();
            this.MACLabel = new System.Windows.Forms.Label();
            this.MACValueLabel = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.versionLabel = new System.Windows.Forms.Label();
            this.versionValueLabel = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.errorLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // viewStateEncodedTextBox
            // 
            this.viewStateEncodedTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.viewStateEncodedTextBox.Location = new System.Drawing.Point(2, 26);
            this.viewStateEncodedTextBox.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.viewStateEncodedTextBox.Name = "viewStateEncodedTextBox";
            this.viewStateEncodedTextBox.Size = new System.Drawing.Size(803, 150);
            this.viewStateEncodedTextBox.TabIndex = 0;
            this.viewStateEncodedTextBox.Text = "";
            // 
            // viewStateXMLEncodedTextBox
            // 
            this.viewStateXMLEncodedTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.viewStateXMLEncodedTextBox.Location = new System.Drawing.Point(0, 32);
            this.viewStateXMLEncodedTextBox.Name = "viewStateXMLEncodedTextBox";
            this.viewStateXMLEncodedTextBox.Size = new System.Drawing.Size(805, 144);
            this.viewStateXMLEncodedTextBox.TabIndex = 1;
            this.viewStateXMLEncodedTextBox.Text = "";
            this.viewStateXMLEncodedTextBox.WordWrap = false;
            this.viewStateXMLEncodedTextBox.TextChanged += new System.EventHandler(this.viewStateXMLEncodedTextBox_TextChanged);
            // 
            // encodeButton
            // 
            this.encodeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.encodeButton.Location = new System.Drawing.Point(725, 3);
            this.encodeButton.Name = "encodeButton";
            this.encodeButton.Size = new System.Drawing.Size(75, 23);
            this.encodeButton.TabIndex = 2;
            this.encodeButton.Text = "Encode";
            this.encodeButton.UseVisualStyleBackColor = true;
            this.encodeButton.Click += new System.EventHandler(this.encodeButtonClicked);
            // 
            // decodeButton
            // 
            this.decodeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.decodeButton.Location = new System.Drawing.Point(606, 3);
            this.decodeButton.Name = "decodeButton";
            this.decodeButton.Size = new System.Drawing.Size(75, 23);
            this.decodeButton.TabIndex = 3;
            this.decodeButton.Text = "Decode";
            this.decodeButton.UseVisualStyleBackColor = true;
            this.decodeButton.Click += new System.EventHandler(this.decodeButtonClicked);
            // 
            // MACLabel
            // 
            this.MACLabel.AutoSize = true;
            this.MACLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MACLabel.Location = new System.Drawing.Point(9, 8);
            this.MACLabel.Name = "MACLabel";
            this.MACLabel.Size = new System.Drawing.Size(33, 13);
            this.MACLabel.TabIndex = 4;
            this.MACLabel.Text = "MAC:";
            // 
            // MACValueLabel
            // 
            this.MACValueLabel.AutoSize = true;
            this.MACValueLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MACValueLabel.Location = new System.Drawing.Point(48, 8);
            this.MACValueLabel.Name = "MACValueLabel";
            this.MACValueLabel.Size = new System.Drawing.Size(0, 13);
            this.MACValueLabel.TabIndex = 5;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(572, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(28, 23);
            this.pictureBox1.TabIndex = 6;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox2.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(691, 3);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(28, 23);
            this.pictureBox2.TabIndex = 7;
            this.pictureBox2.TabStop = false;
            // 
            // versionLabel
            // 
            this.versionLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.versionLabel.AutoSize = true;
            this.versionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.versionLabel.Location = new System.Drawing.Point(433, 8);
            this.versionLabel.Name = "versionLabel";
            this.versionLabel.Size = new System.Drawing.Size(45, 13);
            this.versionLabel.TabIndex = 8;
            this.versionLabel.Text = "Version:";
            this.versionLabel.Click += new System.EventHandler(this.label1_Click);
            // 
            // versionValueLabel
            // 
            this.versionValueLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.versionValueLabel.AutoSize = true;
            this.versionValueLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.versionValueLabel.Location = new System.Drawing.Point(484, 8);
            this.versionValueLabel.Name = "versionValueLabel";
            this.versionValueLabel.Size = new System.Drawing.Size(0, 13);
            this.versionValueLabel.TabIndex = 9;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.viewStateEncodedTextBox);
            this.splitContainer1.Panel1.Controls.Add(this.errorLabel);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.MACLabel);
            this.splitContainer1.Panel2.Controls.Add(this.viewStateXMLEncodedTextBox);
            this.splitContainer1.Panel2.Controls.Add(this.versionValueLabel);
            this.splitContainer1.Panel2.Controls.Add(this.encodeButton);
            this.splitContainer1.Panel2.Controls.Add(this.versionLabel);
            this.splitContainer1.Panel2.Controls.Add(this.decodeButton);
            this.splitContainer1.Panel2.Controls.Add(this.pictureBox2);
            this.splitContainer1.Panel2.Controls.Add(this.MACValueLabel);
            this.splitContainer1.Panel2.Controls.Add(this.pictureBox1);
            this.splitContainer1.Size = new System.Drawing.Size(808, 359);
            this.splitContainer1.SplitterDistance = 179;
            this.splitContainer1.TabIndex = 12;
            // 
            // errorLabel
            // 
            this.errorLabel.AutoSize = true;
            this.errorLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.errorLabel.ForeColor = System.Drawing.Color.Red;
            this.errorLabel.Location = new System.Drawing.Point(16, 7);
            this.errorLabel.Name = "errorLabel";
            this.errorLabel.Size = new System.Drawing.Size(0, 13);
            this.errorLabel.TabIndex = 10;
            this.errorLabel.Click += new System.EventHandler(this.label1_Click_1);
            // 
            // ViewStateView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.splitContainer1);
            this.Name = "ViewStateView";
            this.Size = new System.Drawing.Size(808, 359);
            this.Load += new System.EventHandler(this.TextView_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public RichTextBox viewStateEncodedTextBox;
        public RichTextBox viewStateXMLEncodedTextBox;
        public Button encodeButton;
        public Button decodeButton;
        public Label MACLabel;
        public Label MACValueLabel;
        private PictureBox pictureBox1;
        private PictureBox pictureBox2;
        public Label versionLabel;
        public Label versionValueLabel;
        private SplitContainer splitContainer1;
        public Label errorLabel;

    }
}
