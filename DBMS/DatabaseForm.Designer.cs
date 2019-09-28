namespace DBMS
{
    partial class DatabaseForm
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
            this.dbTablesListBox = new System.Windows.Forms.ListBox();
            this.viewTableButton = new System.Windows.Forms.Button();
            this.deleteTableButton = new System.Windows.Forms.Button();
            this.addTableButton = new System.Windows.Forms.Button();
            this.saveDBButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // dbTablesListBox
            // 
            this.dbTablesListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dbTablesListBox.FormattingEnabled = true;
            this.dbTablesListBox.Location = new System.Drawing.Point(12, 12);
            this.dbTablesListBox.Name = "dbTablesListBox";
            this.dbTablesListBox.Size = new System.Drawing.Size(577, 420);
            this.dbTablesListBox.TabIndex = 0;
            this.dbTablesListBox.SelectedIndexChanged += new System.EventHandler(this.dbTablesListBox_SelectedIndexChanged);
            // 
            // viewTableButton
            // 
            this.viewTableButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.viewTableButton.Location = new System.Drawing.Point(595, 12);
            this.viewTableButton.Name = "viewTableButton";
            this.viewTableButton.Size = new System.Drawing.Size(193, 43);
            this.viewTableButton.TabIndex = 1;
            this.viewTableButton.Text = "View Table...";
            this.viewTableButton.UseVisualStyleBackColor = true;
            this.viewTableButton.Click += new System.EventHandler(this.viewTableButton_Click);
            // 
            // deleteTableButton
            // 
            this.deleteTableButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.deleteTableButton.Location = new System.Drawing.Point(595, 61);
            this.deleteTableButton.Name = "deleteTableButton";
            this.deleteTableButton.Size = new System.Drawing.Size(193, 43);
            this.deleteTableButton.TabIndex = 2;
            this.deleteTableButton.Text = "Delete Table";
            this.deleteTableButton.UseVisualStyleBackColor = true;
            this.deleteTableButton.Click += new System.EventHandler(this.deleteTableButton_Click);
            // 
            // addTableButton
            // 
            this.addTableButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.addTableButton.Location = new System.Drawing.Point(595, 110);
            this.addTableButton.Name = "addTableButton";
            this.addTableButton.Size = new System.Drawing.Size(193, 43);
            this.addTableButton.TabIndex = 3;
            this.addTableButton.Text = "Add Table...";
            this.addTableButton.UseVisualStyleBackColor = true;
            this.addTableButton.Click += new System.EventHandler(this.addTableButton_Click);
            // 
            // saveDBButton
            // 
            this.saveDBButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.saveDBButton.Location = new System.Drawing.Point(595, 389);
            this.saveDBButton.Name = "saveDBButton";
            this.saveDBButton.Size = new System.Drawing.Size(193, 43);
            this.saveDBButton.TabIndex = 4;
            this.saveDBButton.Text = "Save Database";
            this.saveDBButton.UseVisualStyleBackColor = true;
            this.saveDBButton.Click += new System.EventHandler(this.saveDBButton_Click);
            // 
            // DatabaseForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.saveDBButton);
            this.Controls.Add(this.addTableButton);
            this.Controls.Add(this.deleteTableButton);
            this.Controls.Add(this.viewTableButton);
            this.Controls.Add(this.dbTablesListBox);
            this.MinimumSize = new System.Drawing.Size(450, 260);
            this.Name = "DatabaseForm";
            this.Text = "Database";
            this.Load += new System.EventHandler(this.DatabaseForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox dbTablesListBox;
        private System.Windows.Forms.Button viewTableButton;
        private System.Windows.Forms.Button deleteTableButton;
        private System.Windows.Forms.Button addTableButton;
        private System.Windows.Forms.Button saveDBButton;
    }
}