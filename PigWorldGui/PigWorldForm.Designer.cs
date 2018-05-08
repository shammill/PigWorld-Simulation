namespace PigWorldNamespace {

    partial class PigWorldForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            this.buttonPanel = new System.Windows.Forms.Panel();
            this.slowerLabel = new System.Windows.Forms.Label();
            this.fasterLabel = new System.Windows.Forms.Label();
            this.speedTrackBar = new System.Windows.Forms.TrackBar();
            this.enableRealAudioCheckBox = new System.Windows.Forms.CheckBox();
            this.removeAllButton = new System.Windows.Forms.Button();
            this.removeWallsButton = new System.Windows.Forms.Button();
            this.stepButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.startButton = new System.Windows.Forms.Button();
            this.demoNumber = new System.Windows.Forms.NumericUpDown();
            this.showDebugInfoCheckBox = new System.Windows.Forms.CheckBox();
            this.setupDemoButton = new System.Windows.Forms.Button();
            this.quitButton = new System.Windows.Forms.Button();
            this.currentTypeOfObjectPanel = new System.Windows.Forms.Panel();
            this.boyPigRadioButton = new System.Windows.Forms.RadioButton();
            this.objectAddGroupBox = new System.Windows.Forms.GroupBox();
            this.wolfRadioButton = new System.Windows.Forms.RadioButton();
            this.treeRadioButton = new System.Windows.Forms.RadioButton();
            this.pigFoodRadioButton = new System.Windows.Forms.RadioButton();
            this.girlPigRadioButton = new System.Windows.Forms.RadioButton();
            this.Timer = new System.Windows.Forms.Timer(this.components);
            this.buttonPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.speedTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.demoNumber)).BeginInit();
            this.objectAddGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonPanel
            // 
            this.buttonPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.buttonPanel.Controls.Add(this.slowerLabel);
            this.buttonPanel.Controls.Add(this.fasterLabel);
            this.buttonPanel.Controls.Add(this.speedTrackBar);
            this.buttonPanel.Controls.Add(this.enableRealAudioCheckBox);
            this.buttonPanel.Controls.Add(this.removeAllButton);
            this.buttonPanel.Controls.Add(this.removeWallsButton);
            this.buttonPanel.Controls.Add(this.stepButton);
            this.buttonPanel.Controls.Add(this.stopButton);
            this.buttonPanel.Controls.Add(this.startButton);
            this.buttonPanel.Controls.Add(this.demoNumber);
            this.buttonPanel.Controls.Add(this.showDebugInfoCheckBox);
            this.buttonPanel.Controls.Add(this.setupDemoButton);
            this.buttonPanel.Controls.Add(this.quitButton);
            this.buttonPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonPanel.Location = new System.Drawing.Point(562, 0);
            this.buttonPanel.Name = "buttonPanel";
            this.buttonPanel.Size = new System.Drawing.Size(122, 564);
            this.buttonPanel.TabIndex = 2;
            // 
            // slowerLabel
            // 
            this.slowerLabel.AutoSize = true;
            this.slowerLabel.Location = new System.Drawing.Point(22, 483);
            this.slowerLabel.Name = "slowerLabel";
            this.slowerLabel.Size = new System.Drawing.Size(39, 13);
            this.slowerLabel.TabIndex = 22;
            this.slowerLabel.Text = "Slower";
            // 
            // fasterLabel
            // 
            this.fasterLabel.AutoSize = true;
            this.fasterLabel.Location = new System.Drawing.Point(25, 328);
            this.fasterLabel.Name = "fasterLabel";
            this.fasterLabel.Size = new System.Drawing.Size(36, 13);
            this.fasterLabel.TabIndex = 21;
            this.fasterLabel.Text = "Faster";
            // 
            // speedTrackBar
            // 
            this.speedTrackBar.Location = new System.Drawing.Point(64, 319);
            this.speedTrackBar.Name = "speedTrackBar";
            this.speedTrackBar.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.speedTrackBar.Size = new System.Drawing.Size(42, 186);
            this.speedTrackBar.TabIndex = 20;
            this.speedTrackBar.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.speedTrackBar.Scroll += new System.EventHandler(this.speedTrackBar_Scroll);
            // 
            // enableRealAudioCheckBox
            // 
            this.enableRealAudioCheckBox.AutoSize = true;
            this.enableRealAudioCheckBox.Location = new System.Drawing.Point(8, 511);
            this.enableRealAudioCheckBox.Name = "enableRealAudioCheckBox";
            this.enableRealAudioCheckBox.Size = new System.Drawing.Size(114, 17);
            this.enableRealAudioCheckBox.TabIndex = 19;
            this.enableRealAudioCheckBox.Text = "Enable Real Audio";
            this.enableRealAudioCheckBox.UseVisualStyleBackColor = true;
            this.enableRealAudioCheckBox.CheckedChanged += new System.EventHandler(this.enableRealAudioCheckBox_CheckedChanged);
            // 
            // removeAllButton
            // 
            this.removeAllButton.Location = new System.Drawing.Point(15, 230);
            this.removeAllButton.Name = "removeAllButton";
            this.removeAllButton.Size = new System.Drawing.Size(91, 29);
            this.removeAllButton.TabIndex = 18;
            this.removeAllButton.Text = "Remove All";
            this.removeAllButton.UseVisualStyleBackColor = true;
            this.removeAllButton.Click += new System.EventHandler(this.removeAllButton_Click);
            // 
            // removeWallsButton
            // 
            this.removeWallsButton.Location = new System.Drawing.Point(15, 195);
            this.removeWallsButton.Name = "removeWallsButton";
            this.removeWallsButton.Size = new System.Drawing.Size(91, 29);
            this.removeWallsButton.TabIndex = 17;
            this.removeWallsButton.Text = "Remove Walls";
            this.removeWallsButton.UseVisualStyleBackColor = true;
            this.removeWallsButton.Click += new System.EventHandler(this.removeWallsButton_Click);
            // 
            // stepButton
            // 
            this.stepButton.Location = new System.Drawing.Point(15, 139);
            this.stepButton.Name = "stepButton";
            this.stepButton.Size = new System.Drawing.Size(91, 29);
            this.stepButton.TabIndex = 16;
            this.stepButton.Text = "Step";
            this.stepButton.UseVisualStyleBackColor = true;
            this.stepButton.Click += new System.EventHandler(this.stepButton_Click);
            // 
            // stopButton
            // 
            this.stopButton.Location = new System.Drawing.Point(15, 104);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(91, 29);
            this.stopButton.TabIndex = 15;
            this.stopButton.Text = "Stop";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(15, 69);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(91, 29);
            this.startButton.TabIndex = 14;
            this.startButton.Text = "Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // demoNumber
            // 
            this.demoNumber.Location = new System.Drawing.Point(81, 21);
            this.demoNumber.Maximum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.demoNumber.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.demoNumber.Name = "demoNumber";
            this.demoNumber.Size = new System.Drawing.Size(34, 20);
            this.demoNumber.TabIndex = 13;
            this.demoNumber.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // showDebugInfoCheckBox
            // 
            this.showDebugInfoCheckBox.AutoSize = true;
            this.showDebugInfoCheckBox.Location = new System.Drawing.Point(8, 534);
            this.showDebugInfoCheckBox.Name = "showDebugInfoCheckBox";
            this.showDebugInfoCheckBox.Size = new System.Drawing.Size(109, 17);
            this.showDebugInfoCheckBox.TabIndex = 9;
            this.showDebugInfoCheckBox.Text = "Show Debug Info";
            this.showDebugInfoCheckBox.UseVisualStyleBackColor = true;
            this.showDebugInfoCheckBox.CheckedChanged += new System.EventHandler(this.showDebugInfoCheckBox_CheckedChanged);
            // 
            // setupDemoButton
            // 
            this.setupDemoButton.Location = new System.Drawing.Point(3, 16);
            this.setupDemoButton.Name = "setupDemoButton";
            this.setupDemoButton.Size = new System.Drawing.Size(74, 28);
            this.setupDemoButton.TabIndex = 7;
            this.setupDemoButton.Text = "Setup Demo";
            this.setupDemoButton.UseVisualStyleBackColor = true;
            this.setupDemoButton.Click += new System.EventHandler(this.setupDemoButton_Click);
            // 
            // quitButton
            // 
            this.quitButton.Location = new System.Drawing.Point(15, 285);
            this.quitButton.Name = "quitButton";
            this.quitButton.Size = new System.Drawing.Size(91, 28);
            this.quitButton.TabIndex = 2;
            this.quitButton.Text = "Quit";
            this.quitButton.UseVisualStyleBackColor = true;
            this.quitButton.Click += new System.EventHandler(this.quitButton_Click);
            // 
            // currentTypeOfObjectPanel
            // 
            this.currentTypeOfObjectPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.currentTypeOfObjectPanel.Location = new System.Drawing.Point(0, 507);
            this.currentTypeOfObjectPanel.Name = "currentTypeOfObjectPanel";
            this.currentTypeOfObjectPanel.Size = new System.Drawing.Size(562, 57);
            this.currentTypeOfObjectPanel.TabIndex = 1;
            // 
            // boyPigRadioButton
            // 
            this.boyPigRadioButton.AutoSize = true;
            this.boyPigRadioButton.Checked = true;
            this.boyPigRadioButton.Location = new System.Drawing.Point(25, 17);
            this.boyPigRadioButton.Name = "boyPigRadioButton";
            this.boyPigRadioButton.Size = new System.Drawing.Size(58, 17);
            this.boyPigRadioButton.TabIndex = 0;
            this.boyPigRadioButton.TabStop = true;
            this.boyPigRadioButton.Text = "BoyPig";
            this.boyPigRadioButton.UseVisualStyleBackColor = true;
            this.boyPigRadioButton.CheckedChanged += new System.EventHandler(this.boyPigRadioButton_CheckedChanged);
            // 
            // objectAddGroupBox
            // 
            this.objectAddGroupBox.Controls.Add(this.wolfRadioButton);
            this.objectAddGroupBox.Controls.Add(this.treeRadioButton);
            this.objectAddGroupBox.Controls.Add(this.pigFoodRadioButton);
            this.objectAddGroupBox.Controls.Add(this.girlPigRadioButton);
            this.objectAddGroupBox.Controls.Add(this.boyPigRadioButton);
            this.objectAddGroupBox.Location = new System.Drawing.Point(12, 512);
            this.objectAddGroupBox.Name = "objectAddGroupBox";
            this.objectAddGroupBox.Size = new System.Drawing.Size(397, 40);
            this.objectAddGroupBox.TabIndex = 3;
            this.objectAddGroupBox.TabStop = false;
            this.objectAddGroupBox.Text = " Current type of object to add";
            // 
            // wolfRadioButton
            // 
            this.wolfRadioButton.AutoSize = true;
            this.wolfRadioButton.Location = new System.Drawing.Point(337, 17);
            this.wolfRadioButton.Name = "wolfRadioButton";
            this.wolfRadioButton.Size = new System.Drawing.Size(47, 17);
            this.wolfRadioButton.TabIndex = 4;
            this.wolfRadioButton.Text = "Wolf";
            this.wolfRadioButton.UseVisualStyleBackColor = true;
            this.wolfRadioButton.CheckedChanged += new System.EventHandler(this.wolfRadioButton_CheckedChanged);
            // 
            // treeRadioButton
            // 
            this.treeRadioButton.AutoSize = true;
            this.treeRadioButton.Location = new System.Drawing.Point(274, 17);
            this.treeRadioButton.Name = "treeRadioButton";
            this.treeRadioButton.Size = new System.Drawing.Size(47, 17);
            this.treeRadioButton.TabIndex = 3;
            this.treeRadioButton.Text = "Tree";
            this.treeRadioButton.UseVisualStyleBackColor = true;
            this.treeRadioButton.CheckedChanged += new System.EventHandler(this.treeRadioButton_CheckedChanged);
            // 
            // pigFoodRadioButton
            // 
            this.pigFoodRadioButton.AutoSize = true;
            this.pigFoodRadioButton.Location = new System.Drawing.Point(187, 17);
            this.pigFoodRadioButton.Name = "pigFoodRadioButton";
            this.pigFoodRadioButton.Size = new System.Drawing.Size(64, 17);
            this.pigFoodRadioButton.TabIndex = 2;
            this.pigFoodRadioButton.Text = "PigFood";
            this.pigFoodRadioButton.UseVisualStyleBackColor = true;
            this.pigFoodRadioButton.CheckedChanged += new System.EventHandler(this.pigFoodRadioButton_CheckedChanged);
            // 
            // girlPigRadioButton
            // 
            this.girlPigRadioButton.AutoSize = true;
            this.girlPigRadioButton.Location = new System.Drawing.Point(107, 17);
            this.girlPigRadioButton.Name = "girlPigRadioButton";
            this.girlPigRadioButton.Size = new System.Drawing.Size(55, 17);
            this.girlPigRadioButton.TabIndex = 1;
            this.girlPigRadioButton.Text = "GirlPig";
            this.girlPigRadioButton.UseVisualStyleBackColor = true;
            this.girlPigRadioButton.CheckedChanged += new System.EventHandler(this.girlPigRadioButton_CheckedChanged);
            // 
            // Timer
            // 
            this.Timer.Interval = 1000;
            this.Timer.Tick += new System.EventHandler(this.Timer_Tick);
            // 
            // PigWorldForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 564);
            this.Controls.Add(this.objectAddGroupBox);
            this.Controls.Add(this.currentTypeOfObjectPanel);
            this.Controls.Add(this.buttonPanel);
            this.Name = "PigWorldForm";
            this.Text = "Pig World";
            this.buttonPanel.ResumeLayout(false);
            this.buttonPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.speedTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.demoNumber)).EndInit();
            this.objectAddGroupBox.ResumeLayout(false);
            this.objectAddGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel buttonPanel;
        private System.Windows.Forms.Panel currentTypeOfObjectPanel;
        private System.Windows.Forms.Button quitButton;
        private System.Windows.Forms.Button setupDemoButton;
        private System.Windows.Forms.CheckBox showDebugInfoCheckBox;
        private System.Windows.Forms.NumericUpDown demoNumber;
        private System.Windows.Forms.Label slowerLabel;
        private System.Windows.Forms.Label fasterLabel;
        private System.Windows.Forms.TrackBar speedTrackBar;
        private System.Windows.Forms.CheckBox enableRealAudioCheckBox;
        private System.Windows.Forms.Button removeAllButton;
        private System.Windows.Forms.Button removeWallsButton;
        private System.Windows.Forms.Button stepButton;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.RadioButton boyPigRadioButton;
        private System.Windows.Forms.GroupBox objectAddGroupBox;
        private System.Windows.Forms.RadioButton treeRadioButton;
        private System.Windows.Forms.RadioButton pigFoodRadioButton;
        private System.Windows.Forms.RadioButton girlPigRadioButton;
        private System.Windows.Forms.RadioButton wolfRadioButton;
        private System.Windows.Forms.Timer Timer;
    }
}

