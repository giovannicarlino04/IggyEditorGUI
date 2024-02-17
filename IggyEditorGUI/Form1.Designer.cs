namespace IggyEditorGUI
{
    partial class Form1
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
            menuStrip1 = new MenuStrip();
            filleToolStripMenuItem = new ToolStripMenuItem();
            openToolStripMenuItem = new ToolStripMenuItem();
            saveToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            extractSubFilesToolStripMenuItem = new ToolStripMenuItem();
            iggyInfo = new RichTextBox();
            label1 = new Label();
            label2 = new Label();
            consoleLog = new RichTextBox();
            label3 = new Label();
            tbHeight = new TextBox();
            tbWidth = new TextBox();
            label4 = new Label();
            label5 = new Label();
            saveiggyToolStripMenuItem = new ToolStripMenuItem();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { filleToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(898, 24);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // filleToolStripMenuItem
            // 
            filleToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { openToolStripMenuItem, saveiggyToolStripMenuItem, saveToolStripMenuItem, toolStripSeparator1, extractSubFilesToolStripMenuItem });
            filleToolStripMenuItem.Name = "filleToolStripMenuItem";
            filleToolStripMenuItem.Size = new Size(37, 20);
            filleToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            openToolStripMenuItem.Name = "openToolStripMenuItem";
            openToolStripMenuItem.Size = new Size(201, 22);
            openToolStripMenuItem.Text = "Open";
            openToolStripMenuItem.Click += openToolStripMenuItem_Click;
            // 
            // saveToolStripMenuItem
            // 
            saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            saveToolStripMenuItem.Size = new Size(201, 22);
            saveToolStripMenuItem.Text = "Save (Separate Sections)";
            saveToolStripMenuItem.Click += saveToolStripMenuItem_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(198, 6);
            // 
            // extractSubFilesToolStripMenuItem
            // 
            extractSubFilesToolStripMenuItem.Name = "extractSubFilesToolStripMenuItem";
            extractSubFilesToolStripMenuItem.Size = new Size(201, 22);
            extractSubFilesToolStripMenuItem.Text = "Extract Sub Files";
            extractSubFilesToolStripMenuItem.Click += extractSubFilesToolStripMenuItem_Click;
            // 
            // iggyInfo
            // 
            iggyInfo.Location = new Point(12, 42);
            iggyInfo.Name = "iggyInfo";
            iggyInfo.ReadOnly = true;
            iggyInfo.Size = new Size(237, 207);
            iggyInfo.TabIndex = 1;
            iggyInfo.Text = "";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 24);
            label1.Name = "label1";
            label1.Size = new Size(75, 15);
            label1.TabIndex = 2;
            label1.Text = "Iggy File Info";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(649, 24);
            label2.Name = "label2";
            label2.Size = new Size(73, 15);
            label2.TabIndex = 4;
            label2.Text = "Console Log";
            // 
            // consoleLog
            // 
            consoleLog.Location = new Point(649, 42);
            consoleLog.Name = "consoleLog";
            consoleLog.ReadOnly = true;
            consoleLog.Size = new Size(237, 207);
            consoleLog.TabIndex = 3;
            consoleLog.Text = "";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(354, 282);
            label3.Name = "label3";
            label3.Size = new Size(123, 15);
            label3.TabIndex = 5;
            label3.Text = "EXPERIMENTAL STUFF";
            // 
            // tbHeight
            // 
            tbHeight.Location = new Point(12, 332);
            tbHeight.Name = "tbHeight";
            tbHeight.Size = new Size(100, 23);
            tbHeight.TabIndex = 6;
            // 
            // tbWidth
            // 
            tbWidth.Location = new Point(118, 332);
            tbWidth.Name = "tbWidth";
            tbWidth.Size = new Size(100, 23);
            tbWidth.TabIndex = 6;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(12, 314);
            label4.Name = "label4";
            label4.Size = new Size(43, 15);
            label4.TabIndex = 7;
            label4.Text = "Height";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(118, 314);
            label5.Name = "label5";
            label5.Size = new Size(39, 15);
            label5.TabIndex = 7;
            label5.Text = "Width";
            // 
            // saveiggyToolStripMenuItem
            // 
            saveiggyToolStripMenuItem.Name = "saveiggyToolStripMenuItem";
            saveiggyToolStripMenuItem.Size = new Size(201, 22);
            saveiggyToolStripMenuItem.Text = "Save (.iggy)";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(898, 479);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(tbWidth);
            Controls.Add(tbHeight);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(consoleLog);
            Controls.Add(label1);
            Controls.Add(iggyInfo);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "Form1";
            Text = "IggyEditor";
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem filleToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem;
        private ToolStripMenuItem saveToolStripMenuItem;
        private RichTextBox iggyInfo;
        private Label label1;
        private Label label2;
        private RichTextBox consoleLog;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem extractSubFilesToolStripMenuItem;
        private Label label3;
        private TextBox tbHeight;
        private TextBox tbWidth;
        private Label label4;
        private Label label5;
        private ToolStripMenuItem saveiggyToolStripMenuItem;
    }
}
