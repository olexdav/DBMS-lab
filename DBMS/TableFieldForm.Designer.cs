namespace DBMS
{
    partial class TableFieldForm
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
            this.tableFieldsListBox = new System.Windows.Forms.ListBox();
            this.tableFieldsLabel = new System.Windows.Forms.Label();
            this.addFieldPanel = new System.Windows.Forms.Panel();
            this.fieldNameTextBox = new System.Windows.Forms.TextBox();
            this.fieldTypeComboBox = new System.Windows.Forms.ComboBox();
            this.addFieldButton = new System.Windows.Forms.Button();
            this.deleteFieldButton = new System.Windows.Forms.Button();
            this.fieldNameLabel = new System.Windows.Forms.Label();
            this.fieldTypeLabel = new System.Windows.Forms.Label();
            this.addFieldPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableFieldsListBox
            // 
            this.tableFieldsListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableFieldsListBox.FormattingEnabled = true;
            this.tableFieldsListBox.Location = new System.Drawing.Point(12, 29);
            this.tableFieldsListBox.Name = "tableFieldsListBox";
            this.tableFieldsListBox.Size = new System.Drawing.Size(244, 147);
            this.tableFieldsListBox.TabIndex = 0;
            // 
            // tableFieldsLabel
            // 
            this.tableFieldsLabel.AutoSize = true;
            this.tableFieldsLabel.Location = new System.Drawing.Point(13, 13);
            this.tableFieldsLabel.Name = "tableFieldsLabel";
            this.tableFieldsLabel.Size = new System.Drawing.Size(64, 13);
            this.tableFieldsLabel.TabIndex = 1;
            this.tableFieldsLabel.Text = "Table fields:";
            // 
            // addFieldPanel
            // 
            this.addFieldPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.addFieldPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.addFieldPanel.Controls.Add(this.fieldTypeLabel);
            this.addFieldPanel.Controls.Add(this.fieldNameLabel);
            this.addFieldPanel.Controls.Add(this.addFieldButton);
            this.addFieldPanel.Controls.Add(this.fieldTypeComboBox);
            this.addFieldPanel.Controls.Add(this.fieldNameTextBox);
            this.addFieldPanel.Location = new System.Drawing.Point(262, 29);
            this.addFieldPanel.Name = "addFieldPanel";
            this.addFieldPanel.Size = new System.Drawing.Size(211, 101);
            this.addFieldPanel.TabIndex = 2;
            // 
            // fieldNameTextBox
            // 
            this.fieldNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fieldNameTextBox.Location = new System.Drawing.Point(71, 4);
            this.fieldNameTextBox.Name = "fieldNameTextBox";
            this.fieldNameTextBox.Size = new System.Drawing.Size(133, 20);
            this.fieldNameTextBox.TabIndex = 0;
            // 
            // fieldTypeComboBox
            // 
            this.fieldTypeComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fieldTypeComboBox.FormattingEnabled = true;
            this.fieldTypeComboBox.Location = new System.Drawing.Point(71, 27);
            this.fieldTypeComboBox.Name = "fieldTypeComboBox";
            this.fieldTypeComboBox.Size = new System.Drawing.Size(133, 21);
            this.fieldTypeComboBox.TabIndex = 1;
            // 
            // addFieldButton
            // 
            this.addFieldButton.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.addFieldButton.Location = new System.Drawing.Point(7, 54);
            this.addFieldButton.Name = "addFieldButton";
            this.addFieldButton.Size = new System.Drawing.Size(197, 40);
            this.addFieldButton.TabIndex = 2;
            this.addFieldButton.Text = "Add Field";
            this.addFieldButton.UseVisualStyleBackColor = true;
            // 
            // deleteFieldButton
            // 
            this.deleteFieldButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.deleteFieldButton.Location = new System.Drawing.Point(270, 136);
            this.deleteFieldButton.Name = "deleteFieldButton";
            this.deleteFieldButton.Size = new System.Drawing.Size(197, 40);
            this.deleteFieldButton.TabIndex = 3;
            this.deleteFieldButton.Text = "Delete Field";
            this.deleteFieldButton.UseVisualStyleBackColor = true;
            // 
            // fieldNameLabel
            // 
            this.fieldNameLabel.AutoSize = true;
            this.fieldNameLabel.Location = new System.Drawing.Point(4, 7);
            this.fieldNameLabel.Name = "fieldNameLabel";
            this.fieldNameLabel.Size = new System.Drawing.Size(61, 13);
            this.fieldNameLabel.TabIndex = 3;
            this.fieldNameLabel.Text = "Field name:";
            // 
            // fieldTypeLabel
            // 
            this.fieldTypeLabel.AutoSize = true;
            this.fieldTypeLabel.Location = new System.Drawing.Point(4, 30);
            this.fieldTypeLabel.Name = "fieldTypeLabel";
            this.fieldTypeLabel.Size = new System.Drawing.Size(55, 13);
            this.fieldTypeLabel.TabIndex = 4;
            this.fieldTypeLabel.Text = "Field type:";
            // 
            // TableFieldForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 188);
            this.Controls.Add(this.deleteFieldButton);
            this.Controls.Add(this.addFieldPanel);
            this.Controls.Add(this.tableFieldsLabel);
            this.Controls.Add(this.tableFieldsListBox);
            this.Name = "TableFieldForm";
            this.Text = "TableFieldForm";
            this.addFieldPanel.ResumeLayout(false);
            this.addFieldPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox tableFieldsListBox;
        private System.Windows.Forms.Label tableFieldsLabel;
        private System.Windows.Forms.Panel addFieldPanel;
        private System.Windows.Forms.Label fieldTypeLabel;
        private System.Windows.Forms.Label fieldNameLabel;
        private System.Windows.Forms.Button addFieldButton;
        private System.Windows.Forms.ComboBox fieldTypeComboBox;
        private System.Windows.Forms.TextBox fieldNameTextBox;
        private System.Windows.Forms.Button deleteFieldButton;
    }
}