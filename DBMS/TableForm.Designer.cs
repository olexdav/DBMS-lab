namespace DBMS
{
    partial class TableForm
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
            this.rowsDataGridView = new System.Windows.Forms.DataGridView();
            this.editRowButton = new System.Windows.Forms.Button();
            this.deleteRowButton = new System.Windows.Forms.Button();
            this.addRowButton = new System.Windows.Forms.Button();
            this.searchButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.rowsDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // rowsDataGridView
            // 
            this.rowsDataGridView.AllowUserToAddRows = false;
            this.rowsDataGridView.AllowUserToDeleteRows = false;
            this.rowsDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rowsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.rowsDataGridView.Location = new System.Drawing.Point(13, 13);
            this.rowsDataGridView.Name = "rowsDataGridView";
            this.rowsDataGridView.ReadOnly = true;
            this.rowsDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.rowsDataGridView.Size = new System.Drawing.Size(587, 425);
            this.rowsDataGridView.TabIndex = 0;
            this.rowsDataGridView.ColumnAdded += new System.Windows.Forms.DataGridViewColumnEventHandler(this.rowsDataGridView_ColumnAdded);
            this.rowsDataGridView.SelectionChanged += new System.EventHandler(this.rowsDataGridView_SelectionChanged);
            // 
            // editRowButton
            // 
            this.editRowButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.editRowButton.Location = new System.Drawing.Point(615, 13);
            this.editRowButton.Name = "editRowButton";
            this.editRowButton.Size = new System.Drawing.Size(173, 40);
            this.editRowButton.TabIndex = 1;
            this.editRowButton.Text = "Edit Row...";
            this.editRowButton.UseVisualStyleBackColor = true;
            this.editRowButton.Click += new System.EventHandler(this.editRowButton_Click);
            // 
            // deleteRowButton
            // 
            this.deleteRowButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.deleteRowButton.Location = new System.Drawing.Point(615, 59);
            this.deleteRowButton.Name = "deleteRowButton";
            this.deleteRowButton.Size = new System.Drawing.Size(173, 40);
            this.deleteRowButton.TabIndex = 2;
            this.deleteRowButton.Text = "Delete Row...";
            this.deleteRowButton.UseVisualStyleBackColor = true;
            this.deleteRowButton.Click += new System.EventHandler(this.deleteRowButton_Click);
            // 
            // addRowButton
            // 
            this.addRowButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.addRowButton.Location = new System.Drawing.Point(615, 106);
            this.addRowButton.Name = "addRowButton";
            this.addRowButton.Size = new System.Drawing.Size(173, 40);
            this.addRowButton.TabIndex = 3;
            this.addRowButton.Text = "Add Row...";
            this.addRowButton.UseVisualStyleBackColor = true;
            this.addRowButton.Click += new System.EventHandler(this.addRowButton_Click);
            // 
            // searchButton
            // 
            this.searchButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.searchButton.Location = new System.Drawing.Point(615, 398);
            this.searchButton.Name = "searchButton";
            this.searchButton.Size = new System.Drawing.Size(173, 40);
            this.searchButton.TabIndex = 6;
            this.searchButton.Text = "Search...";
            this.searchButton.UseVisualStyleBackColor = true;
            this.searchButton.Click += new System.EventHandler(this.SearchButton_Click);
            // 
            // TableForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.searchButton);
            this.Controls.Add(this.addRowButton);
            this.Controls.Add(this.deleteRowButton);
            this.Controls.Add(this.editRowButton);
            this.Controls.Add(this.rowsDataGridView);
            this.MinimumSize = new System.Drawing.Size(450, 260);
            this.Name = "TableForm";
            this.Text = "TableForm";
            ((System.ComponentModel.ISupportInitialize)(this.rowsDataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView rowsDataGridView;
        private System.Windows.Forms.Button editRowButton;
        private System.Windows.Forms.Button deleteRowButton;
        private System.Windows.Forms.Button addRowButton;
        private System.Windows.Forms.Button searchButton;
    }
}