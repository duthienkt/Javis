namespace Javis
{
    partial class Javis
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Javis));
            this.conversationBox = new System.Windows.Forms.RichTextBox();
            this.messBox = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // conversationBox
            // 
            this.conversationBox.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.conversationBox.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.conversationBox.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.conversationBox.Location = new System.Drawing.Point(12, 12);
            this.conversationBox.Name = "conversationBox";
            this.conversationBox.ReadOnly = true;
            this.conversationBox.Size = new System.Drawing.Size(592, 539);
            this.conversationBox.TabIndex = 0;
            this.conversationBox.Text = "";
            this.conversationBox.TextChanged += new System.EventHandler(this.conversationBox_TextChanged);
            // 
            // messBox
            // 
            this.messBox.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.messBox.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.messBox.Location = new System.Drawing.Point(12, 557);
            this.messBox.Name = "messBox";
            this.messBox.Size = new System.Drawing.Size(592, 92);
            this.messBox.TabIndex = 1;
            this.messBox.Text = "";
            this.messBox.TextChanged += new System.EventHandler(this.messBox_TextChanged);
            this.messBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.messBox_KeyDown);
            this.messBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.messBox_KeyPress);
            // 
            // Javis
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(616, 661);
            this.Controls.Add(this.messBox);
            this.Controls.Add(this.conversationBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Javis";
            this.Opacity = 0.9D;
            this.Text = "Javis";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox conversationBox;
        private System.Windows.Forms.RichTextBox messBox;
    }
}

