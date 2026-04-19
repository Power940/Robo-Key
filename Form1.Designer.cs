namespace RoboKey
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


        // stuff I added to make my life easier copying this stupid array into all the combo boxes
        object[] KeyOptions = new object[] { "LMouse", "MMouse", "RMouse", "Backspace", "Tab", "Enter", "LShift", "RShift", "LControl", "RControl", "LAlt", "RAlt", "CapsLock", "Escape", "Space", "PageUp", "PageDown", "End", "Home", "LeftArrow", "UpArrow", "RightArrow", "DownArrow", "PrintScreen", "Insert", "Delete", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "LeftWindows", "RightWindows", "Keypad0", "Keypad1", "Keypad2", "Keypad3", "Keypad4", "Keypad5", "Keypad6", "Keypad7", "Keypad8", "Keypad9", "Multiply", "Add", "Subtract", "Divide", "Decimal", "F1", "F2", "F3", "F4", "F5", "F6", "F7", "F8", "F9", "F10", "F11", "F12", "F13", "F14", "F15", "F16", "F17", "F18", "F19", "F20", "F21", "F22", "F23", "F24", "NumLock", "ScrollLock", "BrowserBack", "BrowserForward", "BrowserRefresh", "BrowserStop", "BrowserSearch", "BrowserFavorites", "BrowserStart", "VolumeMute", "VolumeDown", "VolumeUp", "NextTrack", "PreviousTrack", "Play/PauseMedia", "StartMail" };

        // add this at the bottom of InitializeComponent before it starts adding everything with an array of all the combo boxes
        // remove this line around he bottom of InitializeComponent to use the GUI editor again
        // dumb WinForms moment, but I'll take anything over making a GUI in python again
        private void AddKeyOptionsToComboBoxes(ComboBox[] comboBoxes)
        {
            foreach (ComboBox comboBox in comboBoxes)
            {
                comboBox.Items.AddRange(KeyOptions);
            }
        }


        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            CLI = new TextBox();
            CLI_Run = new Button();
            CLI_Run_Key = new ComboBox();
            HoldKeys = new Button();
            HoldKeys_Key = new ComboBox();
            AutoClick_Key = new ComboBox();
            AutoClick = new Button();
            AutoClick_MS = new NumericUpDown();
            LoggedKeys = new TextBox();
            AddLogged = new ComboBox();
            RemoveLogged = new ComboBox();
            AddLabel = new Label();
            RemoveLabel = new Label();
            AutoClick_MSLabel = new Label();
            CLI_Check = new Label();
            CLI_Help = new Button();
            CLI_Run_Toggle = new CheckBox();
            CLI_Stop = new Button();
            CLI_ClearKey = new Button();
            AutoClick_ClearKey = new Button();
            HoldKeys_ClearKey = new Button();
            ClearLogged = new Button();
            ((System.ComponentModel.ISupportInitialize)AutoClick_MS).BeginInit();
            SuspendLayout();
            // 
            // CLI
            // 
            CLI.AllowDrop = true;
            CLI.BackColor = SystemColors.ControlLight;
            CLI.BorderStyle = BorderStyle.FixedSingle;
            CLI.Cursor = Cursors.IBeam;
            CLI.Font = new Font("Segoe UI", 14F);
            CLI.Location = new Point(20, 490);
            CLI.Multiline = true;
            CLI.Name = "CLI";
            CLI.PlaceholderText = "Enter Commands Here";
            CLI.ScrollBars = ScrollBars.Both;
            CLI.Size = new Size(500, 200);
            CLI.TabIndex = 0;
            CLI.WordWrap = false;
            CLI.TextChanged += CLI_TextChanged;
            // 
            // CLI_Run
            // 
            CLI_Run.BackColor = SystemColors.ControlLight;
            CLI_Run.BackgroundImageLayout = ImageLayout.None;
            CLI_Run.Cursor = Cursors.Hand;
            CLI_Run.FlatStyle = FlatStyle.Flat;
            CLI_Run.Font = new Font("Segoe UI", 14F);
            CLI_Run.Location = new Point(530, 545);
            CLI_Run.Margin = new Padding(3, 3, 0, 3);
            CLI_Run.Name = "CLI_Run";
            CLI_Run.Size = new Size(280, 70);
            CLI_Run.TabIndex = 1;
            CLI_Run.Text = "Run Commands ()";
            CLI_Run.UseVisualStyleBackColor = false;
            CLI_Run.Click += CLI_Run_Click;
            // 
            // CLI_Run_Key
            // 
            CLI_Run_Key.BackColor = SystemColors.ControlLight;
            CLI_Run_Key.Cursor = Cursors.Hand;
            CLI_Run_Key.DropDownHeight = 200;
            CLI_Run_Key.FlatStyle = FlatStyle.Flat;
            CLI_Run_Key.Font = new Font("Segoe UI", 12F);
            CLI_Run_Key.FormattingEnabled = true;
            CLI_Run_Key.IntegralHeight = false;
            CLI_Run_Key.ItemHeight = 21;
            CLI_Run_Key.Location = new Point(810, 545);
            CLI_Run_Key.Margin = new Padding(0, 3, 3, 3);
            CLI_Run_Key.Name = "CLI_Run_Key";
            CLI_Run_Key.Size = new Size(220, 29);
            CLI_Run_Key.TabIndex = 2;
            CLI_Run_Key.SelectedIndexChanged += CLI_Run_Key_SelectedIndexChanged;
            // 
            // HoldKeys
            // 
            HoldKeys.BackColor = SystemColors.ControlLight;
            HoldKeys.BackgroundImageLayout = ImageLayout.None;
            HoldKeys.Cursor = Cursors.Hand;
            HoldKeys.FlatStyle = FlatStyle.Flat;
            HoldKeys.Font = new Font("Segoe UI", 14F);
            HoldKeys.Location = new Point(770, 40);
            HoldKeys.Margin = new Padding(3, 3, 0, 3);
            HoldKeys.Name = "HoldKeys";
            HoldKeys.Size = new Size(280, 70);
            HoldKeys.TabIndex = 3;
            HoldKeys.Text = "Hold Keys ()";
            HoldKeys.UseVisualStyleBackColor = false;
            HoldKeys.Click += HoldKeys_Click;
            // 
            // HoldKeys_Key
            // 
            HoldKeys_Key.BackColor = SystemColors.ControlLight;
            HoldKeys_Key.Cursor = Cursors.Hand;
            HoldKeys_Key.DropDownHeight = 200;
            HoldKeys_Key.FlatStyle = FlatStyle.Flat;
            HoldKeys_Key.Font = new Font("Segoe UI", 12F);
            HoldKeys_Key.FormattingEnabled = true;
            HoldKeys_Key.IntegralHeight = false;
            HoldKeys_Key.ItemHeight = 21;
            HoldKeys_Key.Location = new Point(1050, 40);
            HoldKeys_Key.Margin = new Padding(0, 3, 3, 3);
            HoldKeys_Key.Name = "HoldKeys_Key";
            HoldKeys_Key.Size = new Size(220, 29);
            HoldKeys_Key.TabIndex = 4;
            HoldKeys_Key.SelectedIndexChanged += HoldKeys_Key_SelectedIndexChanged;
            // 
            // AutoClick_Key
            // 
            AutoClick_Key.BackColor = SystemColors.ControlLight;
            AutoClick_Key.Cursor = Cursors.Hand;
            AutoClick_Key.DropDownHeight = 200;
            AutoClick_Key.FlatStyle = FlatStyle.Flat;
            AutoClick_Key.Font = new Font("Segoe UI", 12F);
            AutoClick_Key.FormattingEnabled = true;
            AutoClick_Key.IntegralHeight = false;
            AutoClick_Key.ItemHeight = 21;
            AutoClick_Key.Location = new Point(1050, 140);
            AutoClick_Key.Margin = new Padding(0, 3, 3, 3);
            AutoClick_Key.Name = "AutoClick_Key";
            AutoClick_Key.Size = new Size(220, 29);
            AutoClick_Key.TabIndex = 6;
            AutoClick_Key.SelectedIndexChanged += AutoClick_Key_SelectedIndexChanged;
            // 
            // AutoClick
            // 
            AutoClick.BackColor = SystemColors.ControlLight;
            AutoClick.BackgroundImageLayout = ImageLayout.None;
            AutoClick.Cursor = Cursors.Hand;
            AutoClick.FlatStyle = FlatStyle.Flat;
            AutoClick.Font = new Font("Segoe UI", 14F);
            AutoClick.Location = new Point(770, 140);
            AutoClick.Margin = new Padding(3, 3, 0, 3);
            AutoClick.Name = "AutoClick";
            AutoClick.Size = new Size(280, 70);
            AutoClick.TabIndex = 5;
            AutoClick.Text = "AutoClick Keys ()";
            AutoClick.UseVisualStyleBackColor = false;
            AutoClick.Click += AutoClick_Click;
            // 
            // AutoClick_MS
            // 
            AutoClick_MS.BackColor = SystemColors.ControlLight;
            AutoClick_MS.BorderStyle = BorderStyle.FixedSingle;
            AutoClick_MS.Font = new Font("Segoe UI", 14F);
            AutoClick_MS.Location = new Point(770, 240);
            AutoClick_MS.Maximum = new decimal(new int[] { 1000000000, 0, 0, 0 });
            AutoClick_MS.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            AutoClick_MS.Name = "AutoClick_MS";
            AutoClick_MS.Size = new Size(150, 32);
            AutoClick_MS.TabIndex = 8;
            AutoClick_MS.TextAlign = HorizontalAlignment.Right;
            AutoClick_MS.Value = new decimal(new int[] { 1, 0, 0, 0 });
            AutoClick_MS.ValueChanged += AutoClick_MS_ValueChanged;
            // 
            // LoggedKeys
            // 
            LoggedKeys.AcceptsReturn = true;
            LoggedKeys.AcceptsTab = true;
            LoggedKeys.BackColor = SystemColors.ControlLight;
            LoggedKeys.BorderStyle = BorderStyle.FixedSingle;
            LoggedKeys.Font = new Font("Segoe UI", 14F);
            LoggedKeys.Location = new Point(20, 12);
            LoggedKeys.Multiline = true;
            LoggedKeys.Name = "LoggedKeys";
            LoggedKeys.ReadOnly = true;
            LoggedKeys.ScrollBars = ScrollBars.Vertical;
            LoggedKeys.Size = new Size(350, 400);
            LoggedKeys.TabIndex = 9;
            // 
            // AddLogged
            // 
            AddLogged.BackColor = SystemColors.ControlLight;
            AddLogged.Cursor = Cursors.Hand;
            AddLogged.DropDownHeight = 200;
            AddLogged.FlatStyle = FlatStyle.Flat;
            AddLogged.Font = new Font("Segoe UI", 12F);
            AddLogged.FormattingEnabled = true;
            AddLogged.ImeMode = ImeMode.NoControl;
            AddLogged.IntegralHeight = false;
            AddLogged.ItemHeight = 21;
            AddLogged.Location = new Point(375, 40);
            AddLogged.Margin = new Padding(0, 3, 3, 3);
            AddLogged.Name = "AddLogged";
            AddLogged.Size = new Size(220, 29);
            AddLogged.TabIndex = 10;
            AddLogged.SelectedIndexChanged += AddLogged_SelectedIndexChanged;
            // 
            // RemoveLogged
            // 
            RemoveLogged.BackColor = SystemColors.ControlLight;
            RemoveLogged.Cursor = Cursors.Hand;
            RemoveLogged.DropDownHeight = 200;
            RemoveLogged.FlatStyle = FlatStyle.Flat;
            RemoveLogged.Font = new Font("Segoe UI", 12F);
            RemoveLogged.FormattingEnabled = true;
            RemoveLogged.IntegralHeight = false;
            RemoveLogged.ItemHeight = 21;
            RemoveLogged.Location = new Point(375, 140);
            RemoveLogged.Margin = new Padding(0, 3, 3, 3);
            RemoveLogged.Name = "RemoveLogged";
            RemoveLogged.Size = new Size(220, 29);
            RemoveLogged.TabIndex = 11;
            RemoveLogged.SelectedIndexChanged += RemoveLogged_SelectedIndexChanged;
            // 
            // AddLabel
            // 
            AddLabel.AutoSize = true;
            AddLabel.Font = new Font("Segoe UI", 14F);
            AddLabel.Location = new Point(601, 40);
            AddLabel.Name = "AddLabel";
            AddLabel.Size = new Size(81, 25);
            AddLabel.TabIndex = 12;
            AddLabel.Text = "Add Key";
            // 
            // RemoveLabel
            // 
            RemoveLabel.AutoSize = true;
            RemoveLabel.Font = new Font("Segoe UI", 14F);
            RemoveLabel.Location = new Point(601, 140);
            RemoveLabel.Name = "RemoveLabel";
            RemoveLabel.Size = new Size(113, 25);
            RemoveLabel.TabIndex = 13;
            RemoveLabel.Text = "Remove Key";
            // 
            // AutoClick_MSLabel
            // 
            AutoClick_MSLabel.AutoSize = true;
            AutoClick_MSLabel.Font = new Font("Segoe UI", 14F);
            AutoClick_MSLabel.Location = new Point(926, 242);
            AutoClick_MSLabel.Name = "AutoClick_MSLabel";
            AutoClick_MSLabel.Size = new Size(120, 25);
            AutoClick_MSLabel.TabIndex = 14;
            AutoClick_MSLabel.Text = "Click Interval";
            // 
            // CLI_Check
            // 
            CLI_Check.AutoSize = true;
            CLI_Check.Font = new Font("Segoe UI", 14F);
            CLI_Check.Location = new Point(530, 490);
            CLI_Check.Name = "CLI_Check";
            CLI_Check.Size = new Size(232, 25);
            CLI_Check.TabIndex = 15;
            CLI_Check.Text = "Not A Valid Command List";
            // 
            // CLI_Help
            // 
            CLI_Help.Anchor = AnchorStyles.Left;
            CLI_Help.BackColor = SystemColors.ControlLight;
            CLI_Help.BackgroundImageLayout = ImageLayout.None;
            CLI_Help.Cursor = Cursors.Hand;
            CLI_Help.FlatStyle = FlatStyle.Flat;
            CLI_Help.Font = new Font("Segoe UI", 8F);
            CLI_Help.Location = new Point(20, 459);
            CLI_Help.Name = "CLI_Help";
            CLI_Help.Size = new Size(100, 25);
            CLI_Help.TabIndex = 16;
            CLI_Help.Text = "Command Help";
            CLI_Help.UseVisualStyleBackColor = false;
            CLI_Help.Click += CLI_Help_Click;
            // 
            // CLI_Run_Toggle
            // 
            CLI_Run_Toggle.AutoSize = true;
            CLI_Run_Toggle.BackgroundImageLayout = ImageLayout.None;
            CLI_Run_Toggle.Checked = true;
            CLI_Run_Toggle.CheckState = CheckState.Checked;
            CLI_Run_Toggle.Cursor = Cursors.Hand;
            CLI_Run_Toggle.Font = new Font("Segoe UI", 14F);
            CLI_Run_Toggle.Location = new Point(530, 620);
            CLI_Run_Toggle.Name = "CLI_Run_Toggle";
            CLI_Run_Toggle.Size = new Size(279, 29);
            CLI_Run_Toggle.TabIndex = 17;
            CLI_Run_Toggle.Text = "Toggle Mode Run Commands";
            CLI_Run_Toggle.UseVisualStyleBackColor = true;
            CLI_Run_Toggle.CheckedChanged += CLI_Run_Toggle_CheckedChanged;
            // 
            // CLI_Stop
            // 
            CLI_Stop.BackColor = SystemColors.ControlLight;
            CLI_Stop.BackgroundImageLayout = ImageLayout.None;
            CLI_Stop.Cursor = Cursors.Hand;
            CLI_Stop.FlatStyle = FlatStyle.Flat;
            CLI_Stop.Font = new Font("Segoe UI", 10F);
            CLI_Stop.Location = new Point(530, 660);
            CLI_Stop.Name = "CLI_Stop";
            CLI_Stop.Size = new Size(179, 30);
            CLI_Stop.TabIndex = 18;
            CLI_Stop.Text = "Force Stop Commands";
            CLI_Stop.UseVisualStyleBackColor = false;
            CLI_Stop.Click += CLI_Stop_Click;
            // 
            // CLI_ClearKey
            // 
            CLI_ClearKey.BackColor = SystemColors.ControlLight;
            CLI_ClearKey.BackgroundImageLayout = ImageLayout.None;
            CLI_ClearKey.Cursor = Cursors.Hand;
            CLI_ClearKey.FlatStyle = FlatStyle.Flat;
            CLI_ClearKey.Font = new Font("Segoe UI", 10F);
            CLI_ClearKey.Location = new Point(810, 585);
            CLI_ClearKey.Name = "CLI_ClearKey";
            CLI_ClearKey.Size = new Size(110, 30);
            CLI_ClearKey.TabIndex = 19;
            CLI_ClearKey.Text = "Clear Keybind";
            CLI_ClearKey.UseVisualStyleBackColor = false;
            CLI_ClearKey.Click += CLI_ClearKey_Click;
            // 
            // AutoClick_ClearKey
            // 
            AutoClick_ClearKey.BackColor = SystemColors.ControlLight;
            AutoClick_ClearKey.BackgroundImageLayout = ImageLayout.None;
            AutoClick_ClearKey.Cursor = Cursors.Hand;
            AutoClick_ClearKey.FlatStyle = FlatStyle.Flat;
            AutoClick_ClearKey.Font = new Font("Segoe UI", 10F);
            AutoClick_ClearKey.Location = new Point(1050, 180);
            AutoClick_ClearKey.Name = "AutoClick_ClearKey";
            AutoClick_ClearKey.Size = new Size(110, 30);
            AutoClick_ClearKey.TabIndex = 20;
            AutoClick_ClearKey.Text = "Clear Keybind";
            AutoClick_ClearKey.UseVisualStyleBackColor = false;
            AutoClick_ClearKey.Click += AutoClick_ClearKey_Click;
            // 
            // HoldKeys_ClearKey
            // 
            HoldKeys_ClearKey.BackColor = SystemColors.ControlLight;
            HoldKeys_ClearKey.BackgroundImageLayout = ImageLayout.None;
            HoldKeys_ClearKey.Cursor = Cursors.Hand;
            HoldKeys_ClearKey.FlatStyle = FlatStyle.Flat;
            HoldKeys_ClearKey.Font = new Font("Segoe UI", 10F);
            HoldKeys_ClearKey.Location = new Point(1050, 80);
            HoldKeys_ClearKey.Name = "HoldKeys_ClearKey";
            HoldKeys_ClearKey.Size = new Size(110, 30);
            HoldKeys_ClearKey.TabIndex = 21;
            HoldKeys_ClearKey.Text = "Clear Keybind";
            HoldKeys_ClearKey.UseVisualStyleBackColor = false;
            HoldKeys_ClearKey.Click += HoldKeys_ClearKey_Click;
            // 
            // ClearLogged
            // 
            ClearLogged.BackColor = SystemColors.ControlLight;
            ClearLogged.BackgroundImageLayout = ImageLayout.None;
            ClearLogged.Cursor = Cursors.Hand;
            ClearLogged.FlatStyle = FlatStyle.Flat;
            ClearLogged.Font = new Font("Segoe UI", 10F);
            ClearLogged.Location = new Point(375, 240);
            ClearLogged.Name = "ClearLogged";
            ClearLogged.Size = new Size(145, 30);
            ClearLogged.TabIndex = 22;
            ClearLogged.Text = "Clear Logged Keys";
            ClearLogged.UseVisualStyleBackColor = false;
            ClearLogged.Click += ClearLogged_Click;

            AddKeyOptionsToComboBoxes([CLI_Run_Key, HoldKeys_Key, AutoClick_Key, AddLogged, RemoveLogged]);

            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ControlDark;
            ClientSize = new Size(1284, 711);
            Controls.Add(ClearLogged);
            Controls.Add(HoldKeys_ClearKey);
            Controls.Add(AutoClick_ClearKey);
            Controls.Add(CLI_ClearKey);
            Controls.Add(CLI_Stop);
            Controls.Add(CLI_Run_Toggle);
            Controls.Add(CLI_Help);
            Controls.Add(CLI_Check);
            Controls.Add(AutoClick_MSLabel);
            Controls.Add(RemoveLabel);
            Controls.Add(AddLabel);
            Controls.Add(RemoveLogged);
            Controls.Add(AddLogged);
            Controls.Add(LoggedKeys);
            Controls.Add(AutoClick_MS);
            Controls.Add(AutoClick_Key);
            Controls.Add(AutoClick);
            Controls.Add(HoldKeys_Key);
            Controls.Add(HoldKeys);
            Controls.Add(CLI_Run_Key);
            Controls.Add(CLI_Run);
            Controls.Add(CLI);
            Name = "Form1";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)AutoClick_MS).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        public TextBox CLI;
        public Button CLI_Run;
        public ComboBox CLI_Run_Key;
        public Button HoldKeys;
        public ComboBox HoldKeys_Key;
        public ComboBox AutoClick_Key;
        public Button AutoClick;
        public NumericUpDown AutoClick_MS;
        public TextBox LoggedKeys;
        public ComboBox AddLogged;
        public ComboBox RemoveLogged;
        public Label AddLabel;
        public Label RemoveLabel;
        public Label AutoClick_MSLabel;
        public Label CLI_Check;
        public Button CLI_Help;
        private CheckBox CLI_Run_Toggle;
        public Button CLI_Stop;
        public Button CLI_ClearKey;
        public Button AutoClick_ClearKey;
        public Button HoldKeys_ClearKey;
        public Button ClearLogged;
    }
}
