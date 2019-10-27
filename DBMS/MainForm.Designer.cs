namespace DBMS
{
    partial class MainForm
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
            this.createDBButton = new System.Windows.Forms.Button();
            this.loadDBFromFileButton = new System.Windows.Forms.Button();
            this.loadFromPGButton = new System.Windows.Forms.Button();
            this.loadFromMongoButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // createDBButton
            // 
            this.createDBButton.Location = new System.Drawing.Point(12, 12);
            this.createDBButton.Name = "createDBButton";
            this.createDBButton.Size = new System.Drawing.Size(186, 51);
            this.createDBButton.TabIndex = 0;
            this.createDBButton.Text = "Create Database...";
            this.createDBButton.UseVisualStyleBackColor = true;
            this.createDBButton.Click += new System.EventHandler(this.createDBButton_Click);
            // 
            // loadDBFromFileButton
            // 
            this.loadDBFromFileButton.Location = new System.Drawing.Point(12, 69);
            this.loadDBFromFileButton.Name = "loadDBFromFileButton";
            this.loadDBFromFileButton.Size = new System.Drawing.Size(186, 51);
            this.loadDBFromFileButton.TabIndex = 1;
            this.loadDBFromFileButton.Text = "Load DB from file...";
            this.loadDBFromFileButton.UseVisualStyleBackColor = true;
            this.loadDBFromFileButton.Click += new System.EventHandler(this.loadDBFromFileButton_Click);
            // 
            // loadFromPGButton
            // 
            this.loadFromPGButton.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.loadFromPGButton.Location = new System.Drawing.Point(12, 126);
            this.loadFromPGButton.Name = "loadFromPGButton";
            this.loadFromPGButton.Size = new System.Drawing.Size(186, 51);
            this.loadFromPGButton.TabIndex = 2;
            this.loadFromPGButton.Text = "Load DB from PostrgeSQL";
            this.loadFromPGButton.UseVisualStyleBackColor = false;
            this.loadFromPGButton.Click += new System.EventHandler(this.loadFromPGButton_Click);
            // 
            // loadFromMongoButton
            // 
            this.loadFromMongoButton.BackColor = System.Drawing.Color.DarkSeaGreen;
            this.loadFromMongoButton.Location = new System.Drawing.Point(13, 183);
            this.loadFromMongoButton.Name = "loadFromMongoButton";
            this.loadFromMongoButton.Size = new System.Drawing.Size(186, 51);
            this.loadFromMongoButton.TabIndex = 3;
            this.loadFromMongoButton.Text = "Load DB from Mongo";
            this.loadFromMongoButton.UseVisualStyleBackColor = false;
            this.loadFromMongoButton.Click += new System.EventHandler(this.loadFromMongoButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(211, 246);
            this.Controls.Add(this.loadFromMongoButton);
            this.Controls.Add(this.loadFromPGButton);
            this.Controls.Add(this.loadDBFromFileButton);
            this.Controls.Add(this.createDBButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "MainForm";
            this.Text = "Open Database";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button createDBButton;
        private System.Windows.Forms.Button loadDBFromFileButton;
        private System.Windows.Forms.Button loadFromPGButton;
        private System.Windows.Forms.Button loadFromMongoButton;
    }
}

