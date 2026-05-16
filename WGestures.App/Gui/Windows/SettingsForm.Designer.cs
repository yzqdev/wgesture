using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;
using WGestures.App.Gui.Windows.Controls;
using WGestures.App.Properties;
using WGestures.Common.OsSpecific.Windows;

namespace WGestures.App.Gui.Windows
{
    partial class SettingsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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

            if (disposing)
            {
                Icon.Dispose();
                Icon = null;

                //循环引用会造成内存泄漏
                 settingFormController = null;
                imglistAppIcons = null;
                dummyImgLstForLstViewHeightFix = null;


                if (_versionChecker != null)
                {
                    _versionChecker.Dispose();
                    _versionChecker = null;
                }
                //Resources.ResourceManager.ReleaseAllResources();

                //GC.Collect();

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
            components = new System.ComponentModel.Container();
            tabControl = new System.Windows.Forms.TabControl();
            tabPage_general = new System.Windows.Forms.TabPage();
            groupBox2 = new System.Windows.Forms.GroupBox();
            check_preferCursorWindow = new System.Windows.Forms.CheckBox();
            settingsFormControllerBindingSource = new System.Windows.Forms.BindingSource(components);
            check_enable8DirGesture = new System.Windows.Forms.CheckBox();
            label3 = new System.Windows.Forms.Label();
            lineLabel2 = new WGestures.App.Gui.Windows.Controls.LineLabel();
            full = new System.Windows.Forms.CheckBox();
            num_pathTrackerInitialStayTimeoutMillis = new WGestures.App.Gui.Windows.Controls.InstantNumericUpDown();
            check_pathTrackerInitialStayTimeout = new System.Windows.Forms.CheckBox();
            label9 = new System.Windows.Forms.Label();
            colorMiddle = new WGestures.App.Gui.Windows.Controls.ColorButton();
            colorBtn_recogonized = new WGestures.App.Gui.Windows.Controls.ColorButton();
            colorBtn_x = new WGestures.App.Gui.Windows.Controls.ColorButton();
            colorBtn_unrecogonized = new WGestures.App.Gui.Windows.Controls.ColorButton();
            numPathTrackerStayTimeoutMillis = new WGestures.App.Gui.Windows.Controls.InstantNumericUpDown();
            numPathTrackerInitialValidMove = new WGestures.App.Gui.Windows.Controls.InstantNumericUpDown();
            checkBox1 = new System.Windows.Forms.CheckBox();
            check_gestBtn_X = new System.Windows.Forms.CheckBox();
            checkGestureView_fadeOut = new System.Windows.Forms.CheckBox();
            check_gestBtn_Middle = new System.Windows.Forms.CheckBox();
            checkGestureViewShowCommandName = new System.Windows.Forms.CheckBox();
            checkPathTrackerStayTimeout = new System.Windows.Forms.CheckBox();
            check_gestBtn_Right = new System.Windows.Forms.CheckBox();
            checkGestureViewShowPath = new System.Windows.Forms.CheckBox();
            label6 = new System.Windows.Forms.Label();
            label5 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            groupBox1 = new System.Windows.Forms.GroupBox();
            shortcutRec_pause = new WGestures.App.Gui.Windows.Controls.ShortcutRecordButton();
            flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
            label4 = new System.Windows.Forms.Label();
            lb_Version = new System.Windows.Forms.Label();
            check_autoStart = new System.Windows.Forms.CheckBox();
            btn_checkUpdateNow = new System.Windows.Forms.Button();
            check_autoCheckUpdate = new System.Windows.Forms.CheckBox();
            lb_pause_shortcut = new System.Windows.Forms.Label();
            label14 = new System.Windows.Forms.Label();
            tabPage2 = new System.Windows.Forms.TabPage();
            flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            panel1 = new System.Windows.Forms.Panel();
            checkInheritGlobal = new System.Windows.Forms.CheckBox();
            flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            check_gesturingDisabled = new System.Windows.Forms.CheckBox();
            pictureSelectedApp = new System.Windows.Forms.PictureBox();
            label15 = new System.Windows.Forms.Label();
            label7 = new System.Windows.Forms.Label();
            listGestureIntents = new WGestures.App.Gui.Windows.Controls.AlwaysSelectedListView();
            colGestureName = new System.Windows.Forms.ColumnHeader();
            colGestureDirs = new System.Windows.Forms.ColumnHeader();
            operation = new System.Windows.Forms.ColumnHeader();
            dummyImgLstForLstViewHeightFix = new System.Windows.Forms.ImageList(components);
            panel_intentListOperations = new System.Windows.Forms.Panel();
            btn_RemoveGesture = new WGestures.App.Gui.Windows.Controls.MetroButton();
            btn_modifyGesture = new WGestures.App.Gui.Windows.Controls.MetroButton();
            btnAddGesture = new WGestures.App.Gui.Windows.Controls.MetroButton();
            group_Command = new System.Windows.Forms.GroupBox();
            flowLayoutPanel6 = new System.Windows.Forms.FlowLayoutPanel();
            panel3 = new System.Windows.Forms.Panel();
            label8 = new System.Windows.Forms.Label();
            combo_CommandTypes = new System.Windows.Forms.ComboBox();
            check_executeOnMouseWheeling = new System.Windows.Forms.CheckBox();
            lineLabel1 = new WGestures.App.Gui.Windows.Controls.LineLabel();
            panel_commandView = new System.Windows.Forms.Panel();
            btnEditApp = new WGestures.App.Gui.Windows.Controls.MetroButton();
            btnAppRemove = new WGestures.App.Gui.Windows.Controls.MetroButton();
            btnAddApp = new WGestures.App.Gui.Windows.Controls.MetroButton();
            listApps = new WGestures.App.Gui.Windows.Controls.AlwaysSelectedListView();
            colListAppDummy = new System.Windows.Forms.ColumnHeader();
            imglistAppIcons = new System.Windows.Forms.ImageList(components);
            tab_hotCorners = new System.Windows.Forms.TabPage();
            panel_hotcornerSettings = new System.Windows.Forms.Panel();
            radio_edge_0 = new System.Windows.Forms.RadioButton();
            radio_edge_2 = new System.Windows.Forms.RadioButton();
            radio_edge_1 = new System.Windows.Forms.RadioButton();
            radio_edge_3 = new System.Windows.Forms.RadioButton();
            radio_corner_1 = new System.Windows.Forms.RadioButton();
            label11 = new System.Windows.Forms.Label();
            panel2 = new System.Windows.Forms.Panel();
            label13 = new System.Windows.Forms.Label();
            combo_hotcornerCmdTypes = new System.Windows.Forms.ComboBox();
            panel_cornorCmdView = new System.Windows.Forms.Panel();
            radio_corner_0 = new System.Windows.Forms.RadioButton();
            radio_corner_2 = new System.Windows.Forms.RadioButton();
            radio_corner_3 = new System.Windows.Forms.RadioButton();
            check_enableRubEdge = new System.Windows.Forms.CheckBox();
            check_enableHotCorners = new System.Windows.Forms.CheckBox();
            tabPage1 = new System.Windows.Forms.TabPage();
            tb_updateLog = new System.Windows.Forms.TextBox();
            flowLayoutPanel5 = new System.Windows.Forms.FlowLayoutPanel();
            linkLabel1 = new System.Windows.Forms.LinkLabel();
            linkLabel2 = new System.Windows.Forms.LinkLabel();
            picture_logo = new System.Windows.Forms.PictureBox();
            flowLayoutPanel4 = new System.Windows.Forms.FlowLayoutPanel();
            lb_info = new System.Windows.Forms.Label();
            tip = new System.Windows.Forms.ToolTip(components);
            pic_menuBtn = new System.Windows.Forms.PictureBox();
            ctx_gesturesMenu = new System.Windows.Forms.ContextMenuStrip(components);
            menuItem_import = new System.Windows.Forms.ToolStripMenuItem();
            menuItem_export = new System.Windows.Forms.ToolStripMenuItem();
            menuItem_resetGestures = new System.Windows.Forms.ToolStripMenuItem();
            errorProvider = new System.Windows.Forms.ErrorProvider(components);
            tabControl.SuspendLayout();
            tabPage_general.SuspendLayout();
            groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) settingsFormControllerBindingSource).BeginInit();
            ((System.ComponentModel.ISupportInitialize) num_pathTrackerInitialStayTimeoutMillis).BeginInit();
            ((System.ComponentModel.ISupportInitialize) numPathTrackerStayTimeoutMillis).BeginInit();
            ((System.ComponentModel.ISupportInitialize) numPathTrackerInitialValidMove).BeginInit();
            groupBox1.SuspendLayout();
            flowLayoutPanel3.SuspendLayout();
            tabPage2.SuspendLayout();
            flowLayoutPanel2.SuspendLayout();
            panel1.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) pictureSelectedApp).BeginInit();
            panel_intentListOperations.SuspendLayout();
            group_Command.SuspendLayout();
            flowLayoutPanel6.SuspendLayout();
            panel3.SuspendLayout();
            tab_hotCorners.SuspendLayout();
            panel_hotcornerSettings.SuspendLayout();
            panel2.SuspendLayout();
            tabPage1.SuspendLayout();
            flowLayoutPanel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) picture_logo).BeginInit();
            flowLayoutPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) pic_menuBtn).BeginInit();
            ctx_gesturesMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) errorProvider).BeginInit();
            SuspendLayout();
            // 
            // tabControl
            // 
            tabControl.Controls.Add(tabPage_general);
            tabControl.Controls.Add(tabPage2);
            tabControl.Controls.Add(tab_hotCorners);
            tabControl.Controls.Add(tabPage1);
            tabControl.HotTrack = true;
            tabControl.ItemSize = new System.Drawing.Size(250, 28);
            tabControl.Location = new System.Drawing.Point(12, 12);
            tabControl.Margin = new System.Windows.Forms.Padding(10);
            tabControl.Multiline = true;
            tabControl.Name = "tabControl";
            tabControl.Padding = new System.Drawing.Point(20, 3);
            tabControl.SelectedIndex = 0;
            tabControl.Size = new System.Drawing.Size(698, 630);
            tabControl.TabIndex = 0;
            tabControl.SelectedIndexChanged += tabControl_SelectedIndexChanged;
            // 
            // tabPage_general
            // 
            tabPage_general.BackColor = System.Drawing.Color.White;
            tabPage_general.Controls.Add(groupBox2);
            tabPage_general.Controls.Add(groupBox1);
            tabPage_general.Location = new System.Drawing.Point(4, 32);
            tabPage_general.Margin = new System.Windows.Forms.Padding(0);
            tabPage_general.Name = "tabPage_general";
            tabPage_general.Size = new System.Drawing.Size(690, 594);
            tabPage_general.TabIndex = 0;
            tabPage_general.Tag = "general";
            tabPage_general.Text = "选 项";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(check_preferCursorWindow);
            groupBox2.Controls.Add(check_enable8DirGesture);
            groupBox2.Controls.Add(label3);
            groupBox2.Controls.Add(lineLabel2);
            groupBox2.Controls.Add(full);
            groupBox2.Controls.Add(num_pathTrackerInitialStayTimeoutMillis);
            groupBox2.Controls.Add(check_pathTrackerInitialStayTimeout);
            groupBox2.Controls.Add(label9);
            groupBox2.Controls.Add(colorMiddle);
            groupBox2.Controls.Add(colorBtn_recogonized);
            groupBox2.Controls.Add(colorBtn_x);
            groupBox2.Controls.Add(colorBtn_unrecogonized);
            groupBox2.Controls.Add(numPathTrackerStayTimeoutMillis);
            groupBox2.Controls.Add(numPathTrackerInitialValidMove);
            groupBox2.Controls.Add(checkBox1);
            groupBox2.Controls.Add(check_gestBtn_X);
            groupBox2.Controls.Add(checkGestureView_fadeOut);
            groupBox2.Controls.Add(check_gestBtn_Middle);
            groupBox2.Controls.Add(checkGestureViewShowCommandName);
            groupBox2.Controls.Add(checkPathTrackerStayTimeout);
            groupBox2.Controls.Add(check_gestBtn_Right);
            groupBox2.Controls.Add(checkGestureViewShowPath);
            groupBox2.Controls.Add(label6);
            groupBox2.Controls.Add(label5);
            groupBox2.Controls.Add(label1);
            groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            groupBox2.Location = new System.Drawing.Point(18, 214);
            groupBox2.Margin = new System.Windows.Forms.Padding(2);
            groupBox2.Name = "groupBox2";
            groupBox2.Padding = new System.Windows.Forms.Padding(2);
            groupBox2.Size = new System.Drawing.Size(658, 340);
            groupBox2.TabIndex = 4;
            groupBox2.TabStop = false;
            groupBox2.Text = "参数";
            // 
            // check_preferCursorWindow
            // 
            check_preferCursorWindow.AutoSize = true;
            check_preferCursorWindow.DataBindings.Add(new System.Windows.Forms.Binding("Checked", settingsFormControllerBindingSource, "PathTrackerPreferCursorWindow", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            check_preferCursorWindow.Location = new System.Drawing.Point(375, 142);
            check_preferCursorWindow.Margin = new System.Windows.Forms.Padding(5);
            check_preferCursorWindow.Name = "check_preferCursorWindow";
            check_preferCursorWindow.Size = new System.Drawing.Size(171, 21);
            check_preferCursorWindow.TabIndex = 18;
            check_preferCursorWindow.Text = "总是作用于指针下方的窗口";
            tip.SetToolTip(check_preferCursorWindow, "使手势总是作用于鼠标指针下方窗口，而不是当前活动程序");
            check_preferCursorWindow.UseVisualStyleBackColor = true;
            // 
            // settingsFormControllerBindingSource
            // 
            settingsFormControllerBindingSource.DataSource = typeof(WGestures.App.Gui.Windows.SettingsFormController);
            // 
            // check_enable8DirGesture
            // 
            check_enable8DirGesture.AutoSize = true;
            check_enable8DirGesture.DataBindings.Add(new System.Windows.Forms.Binding("Checked", settingsFormControllerBindingSource, "GestureParserEnable8DirGesture", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            check_enable8DirGesture.Location = new System.Drawing.Point(375, 105);
            check_enable8DirGesture.Margin = new System.Windows.Forms.Padding(5);
            check_enable8DirGesture.Name = "check_enable8DirGesture";
            check_enable8DirGesture.Size = new System.Drawing.Size(123, 21);
            check_enable8DirGesture.TabIndex = 17;
            check_enable8DirGesture.Text = "允许使用斜线手势";
            tip.SetToolTip(check_enable8DirGesture, "是否允许使用”↖↙↗↘“手势");
            check_enable8DirGesture.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            label3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            label3.Location = new System.Drawing.Point(366, 40);
            label3.Margin = new System.Windows.Forms.Padding(5);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(47, 17);
            label3.TabIndex = 16;
            label3.Text = "手势键:";
            label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            tip.SetToolTip(label3, "允许哪个鼠标按钮触发手势？");
            // 
            // lineLabel2
            // 
            lineLabel2.BackColor = System.Drawing.Color.Transparent;
            lineLabel2.CausesValidation = false;
            lineLabel2.ForeColor = System.Drawing.Color.Gainsboro;
            lineLabel2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            lineLabel2.Location = new System.Drawing.Point(334, 38);
            lineLabel2.Margin = new System.Windows.Forms.Padding(5);
            lineLabel2.Name = "lineLabel2";
            lineLabel2.Size = new System.Drawing.Size(22, 136);
            lineLabel2.TabIndex = 13;
            // 
            // full
            // 
            full.AutoSize = true;
            full.DataBindings.Add(new System.Windows.Forms.Binding("Checked", settingsFormControllerBindingSource, "PathTrackerDisableInFullScreen", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            full.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            full.Location = new System.Drawing.Point(105, 146);
            full.Margin = new System.Windows.Forms.Padding(5);
            full.Name = "full";
            full.Size = new System.Drawing.Size(135, 21);
            full.TabIndex = 12;
            full.Text = "全屏时自动禁用手势";
            full.UseVisualStyleBackColor = true;
            // 
            // num_pathTrackerInitialStayTimeoutMillis
            // 
            num_pathTrackerInitialStayTimeoutMillis.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            num_pathTrackerInitialStayTimeoutMillis.DataBindings.Add(new System.Windows.Forms.Binding("Value", settingsFormControllerBindingSource, "PathTrackerInitalStayTimeoutMillis", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            num_pathTrackerInitialStayTimeoutMillis.DataBindings.Add(new System.Windows.Forms.Binding("Enabled", settingsFormControllerBindingSource, "PathTrackerInitialStayTimeout", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            num_pathTrackerInitialStayTimeoutMillis.Increment = new decimal(new int[] {10, 0, 0, 0});
            num_pathTrackerInitialStayTimeoutMillis.Location = new System.Drawing.Point(261, 71);
            num_pathTrackerInitialStayTimeoutMillis.Margin = new System.Windows.Forms.Padding(5);
            num_pathTrackerInitialStayTimeoutMillis.Maximum = new decimal(new int[] {2000, 0, 0, 0});
            num_pathTrackerInitialStayTimeoutMillis.Minimum = new decimal(new int[] {20, 0, 0, 0});
            num_pathTrackerInitialStayTimeoutMillis.Name = "num_pathTrackerInitialStayTimeoutMillis";
            num_pathTrackerInitialStayTimeoutMillis.Size = new System.Drawing.Size(62, 23);
            num_pathTrackerInitialStayTimeoutMillis.TabIndex = 11;
            tip.SetToolTip(num_pathTrackerInitialStayTimeoutMillis, "若按下右键后超过此时间未移动，则执行正常右键拖拽操作");
            num_pathTrackerInitialStayTimeoutMillis.Value = new decimal(new int[] {150, 0, 0, 0});
            // 
            // check_pathTrackerInitialStayTimeout
            // 
            check_pathTrackerInitialStayTimeout.AutoSize = true;
            check_pathTrackerInitialStayTimeout.DataBindings.Add(new System.Windows.Forms.Binding("Checked", settingsFormControllerBindingSource, "PathTrackerInitialStayTimeout", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            check_pathTrackerInitialStayTimeout.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            check_pathTrackerInitialStayTimeout.Location = new System.Drawing.Point(105, 71);
            check_pathTrackerInitialStayTimeout.Margin = new System.Windows.Forms.Padding(5);
            check_pathTrackerInitialStayTimeout.Name = "check_pathTrackerInitialStayTimeout";
            check_pathTrackerInitialStayTimeout.Size = new System.Drawing.Size(111, 21);
            check_pathTrackerInitialStayTimeout.TabIndex = 10;
            check_pathTrackerInitialStayTimeout.Text = "起始超时 (毫秒)";
            tip.SetToolTip(check_pathTrackerInitialStayTimeout, "若按下右键后超过此时间未移动，则执行正常右键拖拽操作");
            check_pathTrackerInitialStayTimeout.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            label9.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            label9.Location = new System.Drawing.Point(101, 240);
            label9.Margin = new System.Windows.Forms.Padding(5);
            label9.Name = "label9";
            label9.Size = new System.Drawing.Size(59, 17);
            label9.TabIndex = 9;
            label9.Text = "轨迹风格:";
            // 
            // colorMiddle
            // 
            colorMiddle.BackColor = System.Drawing.Color.White;
            colorMiddle.DataBindings.Add(new System.Windows.Forms.Binding("Color", settingsFormControllerBindingSource, "GestureViewMiddleBtnMainColor", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            colorMiddle.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int) ((byte) 224)), ((int) ((byte) 224)), ((int) ((byte) 224)));
            colorMiddle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            colorMiddle.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            colorMiddle.Location = new System.Drawing.Point(190, 272);
            colorMiddle.Margin = new System.Windows.Forms.Padding(5);
            colorMiddle.Name = "colorMiddle";
            colorMiddle.Size = new System.Drawing.Size(75, 44);
            colorMiddle.TabIndex = 8;
            colorMiddle.Text = "中键";
            colorMiddle.UseVisualStyleBackColor = false;
            // 
            // colorBtn_recogonized
            // 
            colorBtn_recogonized.BackColor = System.Drawing.Color.White;
            colorBtn_recogonized.DataBindings.Add(new System.Windows.Forms.Binding("Color", settingsFormControllerBindingSource, "GestureViewMainPathColor", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            colorBtn_recogonized.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int) ((byte) 224)), ((int) ((byte) 224)), ((int) ((byte) 224)));
            colorBtn_recogonized.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            colorBtn_recogonized.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            colorBtn_recogonized.Location = new System.Drawing.Point(105, 272);
            colorBtn_recogonized.Margin = new System.Windows.Forms.Padding(5);
            colorBtn_recogonized.Name = "colorBtn_recogonized";
            colorBtn_recogonized.Size = new System.Drawing.Size(75, 44);
            colorBtn_recogonized.TabIndex = 8;
            colorBtn_recogonized.Text = "右键";
            tip.SetToolTip(colorBtn_recogonized, "手势被识别时，轨迹的颜色");
            colorBtn_recogonized.UseVisualStyleBackColor = false;
            // 
            // colorBtn_x
            // 
            colorBtn_x.BackColor = System.Drawing.Color.White;
            colorBtn_x.DataBindings.Add(new System.Windows.Forms.Binding("Color", settingsFormControllerBindingSource, "GestureVieXBtnMainColor", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            colorBtn_x.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int) ((byte) 224)), ((int) ((byte) 224)), ((int) ((byte) 224)));
            colorBtn_x.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            colorBtn_x.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            colorBtn_x.Location = new System.Drawing.Point(275, 272);
            colorBtn_x.Margin = new System.Windows.Forms.Padding(5);
            colorBtn_x.Name = "colorBtn_x";
            colorBtn_x.Size = new System.Drawing.Size(75, 44);
            colorBtn_x.TabIndex = 8;
            colorBtn_x.Text = "X键";
            colorBtn_x.UseVisualStyleBackColor = false;
            // 
            // colorBtn_unrecogonized
            // 
            colorBtn_unrecogonized.BackColor = System.Drawing.Color.White;
            colorBtn_unrecogonized.DataBindings.Add(new System.Windows.Forms.Binding("Color", settingsFormControllerBindingSource, "GestureViewAlternativePathColor", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            colorBtn_unrecogonized.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int) ((byte) 224)), ((int) ((byte) 224)), ((int) ((byte) 224)));
            colorBtn_unrecogonized.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            colorBtn_unrecogonized.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            colorBtn_unrecogonized.Location = new System.Drawing.Point(368, 272);
            colorBtn_unrecogonized.Margin = new System.Windows.Forms.Padding(5);
            colorBtn_unrecogonized.Name = "colorBtn_unrecogonized";
            colorBtn_unrecogonized.Size = new System.Drawing.Size(75, 44);
            colorBtn_unrecogonized.TabIndex = 8;
            colorBtn_unrecogonized.Text = "未识别";
            tip.SetToolTip(colorBtn_unrecogonized, "手势未被识别时，轨迹的颜色");
            colorBtn_unrecogonized.UseVisualStyleBackColor = false;
            // 
            // numPathTrackerStayTimeoutMillis
            // 
            numPathTrackerStayTimeoutMillis.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            numPathTrackerStayTimeoutMillis.DataBindings.Add(new System.Windows.Forms.Binding("Value", settingsFormControllerBindingSource, "PathTrackerStayTimeoutMillis", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            numPathTrackerStayTimeoutMillis.DataBindings.Add(new System.Windows.Forms.Binding("Enabled", settingsFormControllerBindingSource, "PathTrackerStayTimeout", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            numPathTrackerStayTimeoutMillis.Increment = new decimal(new int[] {10, 0, 0, 0});
            numPathTrackerStayTimeoutMillis.Location = new System.Drawing.Point(261, 109);
            numPathTrackerStayTimeoutMillis.Margin = new System.Windows.Forms.Padding(5);
            numPathTrackerStayTimeoutMillis.Maximum = new decimal(new int[] {10000, 0, 0, 0});
            numPathTrackerStayTimeoutMillis.Minimum = new decimal(new int[] {50, 0, 0, 0});
            numPathTrackerStayTimeoutMillis.Name = "numPathTrackerStayTimeoutMillis";
            numPathTrackerStayTimeoutMillis.Size = new System.Drawing.Size(62, 23);
            numPathTrackerStayTimeoutMillis.TabIndex = 7;
            tip.SetToolTip(numPathTrackerStayTimeoutMillis, "若鼠标停止移动超过此时间，已画出的手势将被取消");
            numPathTrackerStayTimeoutMillis.Value = new decimal(new int[] {50, 0, 0, 0});
            // 
            // numPathTrackerInitialValidMove
            // 
            numPathTrackerInitialValidMove.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            numPathTrackerInitialValidMove.DataBindings.Add(new System.Windows.Forms.Binding("Value", settingsFormControllerBindingSource, "PathTrackerInitialValidMove", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            numPathTrackerInitialValidMove.Location = new System.Drawing.Point(261, 38);
            numPathTrackerInitialValidMove.Margin = new System.Windows.Forms.Padding(5);
            numPathTrackerInitialValidMove.Name = "numPathTrackerInitialValidMove";
            numPathTrackerInitialValidMove.Size = new System.Drawing.Size(62, 23);
            numPathTrackerInitialValidMove.TabIndex = 7;
            tip.SetToolTip(numPathTrackerInitialValidMove, "只有移动超过此距离，才开始识别手势");
            numPathTrackerInitialValidMove.Value = new decimal(new int[] {10, 0, 0, 0});
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.DataBindings.Add(new System.Windows.Forms.Binding("Checked", settingsFormControllerBindingSource, "PathTrackerEnableWinKeyGesturing", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            checkBox1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            checkBox1.Location = new System.Drawing.Point(375, 69);
            checkBox1.Margin = new System.Windows.Forms.Padding(5);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new System.Drawing.Size(212, 21);
            checkBox1.TabIndex = 1;
            checkBox1.Tag = "12";
            checkBox1.Text = "启用Windows键触发 (等价于右键)";
            checkBox1.UseVisualStyleBackColor = true;
            // 
            // check_gestBtn_X
            // 
            check_gestBtn_X.AutoSize = true;
            check_gestBtn_X.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            check_gestBtn_X.Location = new System.Drawing.Point(582, 39);
            check_gestBtn_X.Margin = new System.Windows.Forms.Padding(5);
            check_gestBtn_X.Name = "check_gestBtn_X";
            check_gestBtn_X.Size = new System.Drawing.Size(47, 21);
            check_gestBtn_X.TabIndex = 1;
            check_gestBtn_X.Tag = "12";
            check_gestBtn_X.Text = "X键";
            check_gestBtn_X.UseVisualStyleBackColor = true;
            check_gestBtn_X.CheckedChanged += check_gestBtns_checkedChanged;
            // 
            // checkGestureView_fadeOut
            // 
            checkGestureView_fadeOut.AutoSize = true;
            checkGestureView_fadeOut.DataBindings.Add(new System.Windows.Forms.Binding("Checked", settingsFormControllerBindingSource, "GestureViewFadeOut", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            checkGestureView_fadeOut.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            checkGestureView_fadeOut.Location = new System.Drawing.Point(338, 202);
            checkGestureView_fadeOut.Margin = new System.Windows.Forms.Padding(5);
            checkGestureView_fadeOut.Name = "checkGestureView_fadeOut";
            checkGestureView_fadeOut.Size = new System.Drawing.Size(87, 21);
            checkGestureView_fadeOut.TabIndex = 1;
            checkGestureView_fadeOut.Text = "执行后淡出";
            tip.SetToolTip(checkGestureView_fadeOut, "手势执行后图形逐渐消失(而非突然消失)");
            checkGestureView_fadeOut.UseVisualStyleBackColor = true;
            // 
            // check_gestBtn_Middle
            // 
            check_gestBtn_Middle.AutoSize = true;
            check_gestBtn_Middle.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            check_gestBtn_Middle.Location = new System.Drawing.Point(509, 39);
            check_gestBtn_Middle.Margin = new System.Windows.Forms.Padding(5);
            check_gestBtn_Middle.Name = "check_gestBtn_Middle";
            check_gestBtn_Middle.Size = new System.Drawing.Size(51, 21);
            check_gestBtn_Middle.TabIndex = 1;
            check_gestBtn_Middle.Tag = "2";
            check_gestBtn_Middle.Text = "中键";
            check_gestBtn_Middle.UseVisualStyleBackColor = true;
            check_gestBtn_Middle.CheckedChanged += check_gestBtns_checkedChanged;
            // 
            // checkGestureViewShowCommandName
            // 
            checkGestureViewShowCommandName.AutoSize = true;
            checkGestureViewShowCommandName.DataBindings.Add(new System.Windows.Forms.Binding("Checked", settingsFormControllerBindingSource, "GestureViewShowCommandName", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            checkGestureViewShowCommandName.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            checkGestureViewShowCommandName.Location = new System.Drawing.Point(205, 202);
            checkGestureViewShowCommandName.Margin = new System.Windows.Forms.Padding(5);
            checkGestureViewShowCommandName.Name = "checkGestureViewShowCommandName";
            checkGestureViewShowCommandName.Size = new System.Drawing.Size(75, 21);
            checkGestureViewShowCommandName.TabIndex = 1;
            checkGestureViewShowCommandName.Text = "手势名称";
            checkGestureViewShowCommandName.UseVisualStyleBackColor = true;
            // 
            // checkPathTrackerStayTimeout
            // 
            checkPathTrackerStayTimeout.AutoSize = true;
            checkPathTrackerStayTimeout.DataBindings.Add(new System.Windows.Forms.Binding("Checked", settingsFormControllerBindingSource, "PathTrackerStayTimeout", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            checkPathTrackerStayTimeout.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            checkPathTrackerStayTimeout.Location = new System.Drawing.Point(105, 109);
            checkPathTrackerStayTimeout.Margin = new System.Windows.Forms.Padding(5);
            checkPathTrackerStayTimeout.Name = "checkPathTrackerStayTimeout";
            checkPathTrackerStayTimeout.Size = new System.Drawing.Size(111, 21);
            checkPathTrackerStayTimeout.TabIndex = 0;
            checkPathTrackerStayTimeout.Text = "停留超时 (毫秒)";
            tip.SetToolTip(checkPathTrackerStayTimeout, "若鼠标停止移动超过此时间，已画出的手势将被取消");
            checkPathTrackerStayTimeout.UseVisualStyleBackColor = true;
            // 
            // check_gestBtn_Right
            // 
            check_gestBtn_Right.AutoSize = true;
            check_gestBtn_Right.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            check_gestBtn_Right.Location = new System.Drawing.Point(435, 39);
            check_gestBtn_Right.Margin = new System.Windows.Forms.Padding(5);
            check_gestBtn_Right.Name = "check_gestBtn_Right";
            check_gestBtn_Right.Size = new System.Drawing.Size(51, 21);
            check_gestBtn_Right.TabIndex = 0;
            check_gestBtn_Right.Tag = "1";
            check_gestBtn_Right.Text = "右键";
            check_gestBtn_Right.UseVisualStyleBackColor = true;
            check_gestBtn_Right.CheckedChanged += check_gestBtns_checkedChanged;
            // 
            // checkGestureViewShowPath
            // 
            checkGestureViewShowPath.AutoSize = true;
            checkGestureViewShowPath.DataBindings.Add(new System.Windows.Forms.Binding("Checked", settingsFormControllerBindingSource, "GestureViewShowPath", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            checkGestureViewShowPath.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            checkGestureViewShowPath.Location = new System.Drawing.Point(105, 202);
            checkGestureViewShowPath.Margin = new System.Windows.Forms.Padding(5);
            checkGestureViewShowPath.Name = "checkGestureViewShowPath";
            checkGestureViewShowPath.Size = new System.Drawing.Size(51, 21);
            checkGestureViewShowPath.TabIndex = 0;
            checkGestureViewShowPath.Text = "轨迹";
            checkGestureViewShowPath.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            label6.Location = new System.Drawing.Point(101, 40);
            label6.Margin = new System.Windows.Forms.Padding(5);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(112, 17);
            label6.TabIndex = 2;
            label6.Text = "起始移动距离(像素)";
            label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            tip.SetToolTip(label6, "只有移动超过此距离，才开始识别手势");
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            label5.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            label5.Location = new System.Drawing.Point(32, 40);
            label5.Margin = new System.Windows.Forms.Padding(5);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(47, 17);
            label5.TabIndex = 2;
            label5.Text = "有效性:";
            label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            label1.Location = new System.Drawing.Point(32, 205);
            label1.Margin = new System.Windows.Forms.Padding(5);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(47, 17);
            label1.TabIndex = 2;
            label1.Text = "显   示:";
            label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(shortcutRec_pause);
            groupBox1.Controls.Add(flowLayoutPanel3);
            groupBox1.Controls.Add(check_autoStart);
            groupBox1.Controls.Add(btn_checkUpdateNow);
            groupBox1.Controls.Add(check_autoCheckUpdate);
            groupBox1.Controls.Add(lb_pause_shortcut);
            groupBox1.Controls.Add(label14);
            groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            groupBox1.Location = new System.Drawing.Point(18, 25);
            groupBox1.Margin = new System.Windows.Forms.Padding(2);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new System.Windows.Forms.Padding(2);
            groupBox1.Size = new System.Drawing.Size(658, 159);
            groupBox1.TabIndex = 3;
            groupBox1.TabStop = false;
            groupBox1.Text = "通用";
            // 
            // shortcutRec_pause
            // 
            shortcutRec_pause.Location = new System.Drawing.Point(179, 109);
            shortcutRec_pause.Margin = new System.Windows.Forms.Padding(5);
            shortcutRec_pause.Name = "shortcutRec_pause";
            shortcutRec_pause.Size = new System.Drawing.Size(110, 29);
            shortcutRec_pause.TabIndex = 6;
            shortcutRec_pause.Text = "录入快捷键";
            shortcutRec_pause.UseVisualStyleBackColor = true;
            shortcutRec_pause.EndRecord += shortcutRec_pause_EndRecord;
            // 
            // flowLayoutPanel3
            // 
            flowLayoutPanel3.Controls.Add(label4);
            flowLayoutPanel3.Controls.Add(lb_Version);
            flowLayoutPanel3.Location = new System.Drawing.Point(290, 68);
            flowLayoutPanel3.Margin = new System.Windows.Forms.Padding(5);
            flowLayoutPanel3.Name = "flowLayoutPanel3";
            flowLayoutPanel3.Size = new System.Drawing.Size(348, 29);
            flowLayoutPanel3.TabIndex = 5;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            label4.ForeColor = System.Drawing.Color.Gray;
            label4.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            label4.Location = new System.Drawing.Point(2, 8);
            label4.Margin = new System.Windows.Forms.Padding(2, 8, 2, 2);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(59, 17);
            label4.TabIndex = 2;
            label4.Text = "当前版本:";
            // 
            // lb_Version
            // 
            lb_Version.AutoSize = true;
            lb_Version.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            lb_Version.ForeColor = System.Drawing.Color.Gray;
            lb_Version.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            lb_Version.Location = new System.Drawing.Point(65, 8);
            lb_Version.Margin = new System.Windows.Forms.Padding(2, 8, 2, 2);
            lb_Version.Name = "lb_Version";
            lb_Version.Size = new System.Drawing.Size(50, 17);
            lb_Version.TabIndex = 3;
            lb_Version.Text = "version";
            // 
            // check_autoStart
            // 
            check_autoStart.AutoSize = true;
            check_autoStart.DataBindings.Add(new System.Windows.Forms.Binding("Checked", settingsFormControllerBindingSource, "AutoStart", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            check_autoStart.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            check_autoStart.Location = new System.Drawing.Point(38, 38);
            check_autoStart.Margin = new System.Windows.Forms.Padding(5);
            check_autoStart.Name = "check_autoStart";
            check_autoStart.Size = new System.Drawing.Size(99, 21);
            check_autoStart.TabIndex = 0;
            check_autoStart.Text = "开机自动运行";
            check_autoStart.UseVisualStyleBackColor = true;
            // 
            // btn_checkUpdateNow
            // 
            btn_checkUpdateNow.BackColor = System.Drawing.SystemColors.Control;
            btn_checkUpdateNow.FlatStyle = System.Windows.Forms.FlatStyle.System;
            btn_checkUpdateNow.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            btn_checkUpdateNow.Location = new System.Drawing.Point(179, 71);
            btn_checkUpdateNow.Margin = new System.Windows.Forms.Padding(5);
            btn_checkUpdateNow.Name = "btn_checkUpdateNow";
            btn_checkUpdateNow.Size = new System.Drawing.Size(92, 29);
            btn_checkUpdateNow.TabIndex = 2;
            btn_checkUpdateNow.Text = "立即检查";
            btn_checkUpdateNow.UseVisualStyleBackColor = false;
            btn_checkUpdateNow.Click += btn_checkUpdateNow_Click;
            // 
            // check_autoCheckUpdate
            // 
            check_autoCheckUpdate.AutoSize = true;
            check_autoCheckUpdate.DataBindings.Add(new System.Windows.Forms.Binding("Checked", settingsFormControllerBindingSource, "AutoCheckForUpdate", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            check_autoCheckUpdate.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            check_autoCheckUpdate.Location = new System.Drawing.Point(38, 75);
            check_autoCheckUpdate.Margin = new System.Windows.Forms.Padding(5);
            check_autoCheckUpdate.Name = "check_autoCheckUpdate";
            check_autoCheckUpdate.Size = new System.Drawing.Size(99, 21);
            check_autoCheckUpdate.TabIndex = 1;
            check_autoCheckUpdate.Text = "自动检查更新";
            check_autoCheckUpdate.UseVisualStyleBackColor = true;
            // 
            // lb_pause_shortcut
            // 
            lb_pause_shortcut.AutoSize = true;
            lb_pause_shortcut.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            lb_pause_shortcut.Location = new System.Drawing.Point(299, 112);
            lb_pause_shortcut.Margin = new System.Windows.Forms.Padding(5);
            lb_pause_shortcut.Name = "lb_pause_shortcut";
            lb_pause_shortcut.Size = new System.Drawing.Size(20, 17);
            lb_pause_shortcut.TabIndex = 2;
            lb_pause_shortcut.Text = "无";
            lb_pause_shortcut.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            label14.Location = new System.Drawing.Point(34, 112);
            label14.Margin = new System.Windows.Forms.Padding(5);
            label14.Name = "label14";
            label14.Size = new System.Drawing.Size(100, 17);
            label14.TabIndex = 2;
            label14.Text = "暂停/继续快捷键:";
            label14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tabPage2
            // 
            tabPage2.BackColor = System.Drawing.Color.White;
            tabPage2.Controls.Add(flowLayoutPanel2);
            tabPage2.Controls.Add(btnEditApp);
            tabPage2.Controls.Add(btnAppRemove);
            tabPage2.Controls.Add(btnAddApp);
            tabPage2.Controls.Add(listApps);
            tabPage2.Location = new System.Drawing.Point(4, 32);
            tabPage2.Margin = new System.Windows.Forms.Padding(0);
            tabPage2.Name = "tabPage2";
            tabPage2.Size = new System.Drawing.Size(690, 594);
            tabPage2.TabIndex = 1;
            tabPage2.Tag = "gestures";
            tabPage2.Text = "手 势";
            // 
            // flowLayoutPanel2
            // 
            flowLayoutPanel2.AutoSize = true;
            flowLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            flowLayoutPanel2.Controls.Add(panel1);
            flowLayoutPanel2.Controls.Add(listGestureIntents);
            flowLayoutPanel2.Controls.Add(panel_intentListOperations);
            flowLayoutPanel2.Controls.Add(group_Command);
            flowLayoutPanel2.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            flowLayoutPanel2.Location = new System.Drawing.Point(238, 22);
            flowLayoutPanel2.Margin = new System.Windows.Forms.Padding(2);
            flowLayoutPanel2.Name = "flowLayoutPanel2";
            flowLayoutPanel2.Size = new System.Drawing.Size(439, 549);
            flowLayoutPanel2.TabIndex = 6;
            // 
            // panel1
            // 
            panel1.AutoSize = true;
            panel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            panel1.Controls.Add(checkInheritGlobal);
            panel1.Controls.Add(flowLayoutPanel1);
            panel1.Location = new System.Drawing.Point(0, 0);
            panel1.Margin = new System.Windows.Forms.Padding(0);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(417, 25);
            panel1.TabIndex = 12;
            // 
            // checkInheritGlobal
            // 
            checkInheritGlobal.AutoSize = true;
            checkInheritGlobal.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            checkInheritGlobal.Location = new System.Drawing.Point(318, 2);
            checkInheritGlobal.Margin = new System.Windows.Forms.Padding(0);
            checkInheritGlobal.Name = "checkInheritGlobal";
            checkInheritGlobal.Size = new System.Drawing.Size(99, 21);
            checkInheritGlobal.TabIndex = 3;
            checkInheritGlobal.Text = "继承全局手势";
            checkInheritGlobal.UseVisualStyleBackColor = true;
            checkInheritGlobal.CheckedChanged += checkInheritGlobal_CheckedChanged;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.AutoSize = true;
            flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            flowLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            flowLayoutPanel1.Controls.Add(check_gesturingDisabled);
            flowLayoutPanel1.Controls.Add(pictureSelectedApp);
            flowLayoutPanel1.Controls.Add(label15);
            flowLayoutPanel1.Controls.Add(label7);
            flowLayoutPanel1.ForeColor = System.Drawing.SystemColors.ControlDark;
            flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new System.Drawing.Size(241, 25);
            flowLayoutPanel1.TabIndex = 2;
            // 
            // check_gesturingDisabled
            // 
            check_gesturingDisabled.AutoSize = true;
            check_gesturingDisabled.ForeColor = System.Drawing.Color.Black;
            check_gesturingDisabled.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            check_gesturingDisabled.Location = new System.Drawing.Point(12, 2);
            check_gesturingDisabled.Margin = new System.Windows.Forms.Padding(12, 2, 0, 2);
            check_gesturingDisabled.Name = "check_gesturingDisabled";
            check_gesturingDisabled.Size = new System.Drawing.Size(63, 21);
            check_gesturingDisabled.TabIndex = 5;
            check_gesturingDisabled.Text = "不要在";
            tip.SetToolTip(check_gesturingDisabled, "在该程序上禁用手势（等同于双击应用程序条目）");
            check_gesturingDisabled.UseVisualStyleBackColor = true;
            check_gesturingDisabled.CheckedChanged += check_gesturingEnabled_CheckedChanged;
            // 
            // pictureSelectedApp
            // 
            pictureSelectedApp.Anchor = ((System.Windows.Forms.AnchorStyles) (System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right));
            pictureSelectedApp.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            pictureSelectedApp.Location = new System.Drawing.Point(75, 0);
            pictureSelectedApp.Margin = new System.Windows.Forms.Padding(0, 0, 0, 2);
            pictureSelectedApp.Name = "pictureSelectedApp";
            pictureSelectedApp.Size = new System.Drawing.Size(22, 22);
            pictureSelectedApp.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            pictureSelectedApp.TabIndex = 2;
            pictureSelectedApp.TabStop = false;
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.BackColor = System.Drawing.Color.White;
            label15.ForeColor = System.Drawing.Color.Black;
            label15.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            label15.Location = new System.Drawing.Point(97, 2);
            label15.Margin = new System.Windows.Forms.Padding(0, 2, 0, 2);
            label15.Name = "label15";
            label15.Size = new System.Drawing.Size(92, 17);
            label15.TabIndex = 3;
            label15.Text = "上使用任何手势";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.BackColor = System.Drawing.Color.White;
            label7.Font = new System.Drawing.Font("微软雅黑", 9F);
            label7.ForeColor = System.Drawing.SystemColors.ControlText;
            label7.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            label7.Location = new System.Drawing.Point(189, 2);
            label7.Margin = new System.Windows.Forms.Padding(0, 2, 0, 2);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(52, 17);
            label7.TabIndex = 3;
            label7.Text = "(黑名单)";
            // 
            // listGestureIntents
            // 
            listGestureIntents.AllowDrop = true;
            listGestureIntents.AllowItemDrag = true;
            listGestureIntents.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            listGestureIntents.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {colGestureName, colGestureDirs, operation});
            listGestureIntents.Font = new System.Drawing.Font("微软雅黑", 8.25F);
            listGestureIntents.FullRowSelect = true;
            listGestureIntents.GridLines = true;
            listGestureIntents.InsertionLineColor = System.Drawing.Color.DeepSkyBlue;
            listGestureIntents.LabelEdit = true;
            listGestureIntents.Location = new System.Drawing.Point(12, 27);
            listGestureIntents.Margin = new System.Windows.Forms.Padding(12, 2, 2, 2);
            listGestureIntents.MultiSelect = false;
            listGestureIntents.Name = "listGestureIntents";
            listGestureIntents.Size = new System.Drawing.Size(424, 204);
            listGestureIntents.SmallImageList = dummyImgLstForLstViewHeightFix;
            listGestureIntents.TabIndex = 1;
            listGestureIntents.TileSize = new System.Drawing.Size(255, 84);
            listGestureIntents.UseCompatibleStateImageBehavior = false;
            listGestureIntents.View = System.Windows.Forms.View.Details;
            listGestureIntents.AfterLabelEdit += listGestureIntents_AfterLabelEdit;
            listGestureIntents.ItemSelectionChanged += listGestureIntents_ItemSelectionChanged;
            listGestureIntents.DoubleClick += listGestureIntents_DoubleClick;
            listGestureIntents.MouseEnter += listGestureIntents_MouseEnter;
            listGestureIntents.MouseHover += listGestureIntents_MouseHover;
            // 
            // colGestureName
            // 
            colGestureName.Name = "colGestureName";
            colGestureName.Text = "名称";
            colGestureName.Width = 10;
            // 
            // colGestureDirs
            // 
            colGestureDirs.Name = "colGestureDirs";
            colGestureDirs.Text = "助记符";
            colGestureDirs.Width = 10;
            // 
            // operation
            // 
            operation.Name = "operation";
            operation.Text = "操作";
            // 
            // dummyImgLstForLstViewHeightFix
            // 
            dummyImgLstForLstViewHeightFix.ColorDepth = System.Windows.Forms.ColorDepth.Depth4Bit;
            dummyImgLstForLstViewHeightFix.ImageSize = new System.Drawing.Size(1, 24);
            dummyImgLstForLstViewHeightFix.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // panel_intentListOperations
            // 
            panel_intentListOperations.Controls.Add(btn_RemoveGesture);
            panel_intentListOperations.Controls.Add(btn_modifyGesture);
            panel_intentListOperations.Controls.Add(btnAddGesture);
            panel_intentListOperations.Location = new System.Drawing.Point(12, 235);
            panel_intentListOperations.Margin = new System.Windows.Forms.Padding(12, 2, 0, 0);
            panel_intentListOperations.Name = "panel_intentListOperations";
            panel_intentListOperations.Size = new System.Drawing.Size(425, 28);
            panel_intentListOperations.TabIndex = 9;
            // 
            // btn_RemoveGesture
            // 
            btn_RemoveGesture.Enabled = false;
            btn_RemoveGesture.Font = new System.Drawing.Font("Verdana", 10.125F, System.Drawing.FontStyle.Bold);
            btn_RemoveGesture.ForeColor = System.Drawing.Color.FromArgb(((int) ((byte) 64)), ((int) ((byte) 64)), ((int) ((byte) 64)));
            btn_RemoveGesture.Image = global::WGestures.App.Properties.Resources.remove;
            btn_RemoveGesture.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            btn_RemoveGesture.Location = new System.Drawing.Point(35, 0);
            btn_RemoveGesture.Margin = new System.Windows.Forms.Padding(0);
            btn_RemoveGesture.Name = "btn_RemoveGesture";
            btn_RemoveGesture.Size = new System.Drawing.Size(38, 25);
            btn_RemoveGesture.TabIndex = 8;
            tip.SetToolTip(btn_RemoveGesture, "删除选中的项目");
            btn_RemoveGesture.UseVisualStyleBackColor = true;
            btn_RemoveGesture.Click += btnRemoveGesture_Click;
            // 
            // btn_modifyGesture
            // 
            btn_modifyGesture.Enabled = false;
            btn_modifyGesture.Font = new System.Drawing.Font("Verdana", 10.125F, System.Drawing.FontStyle.Bold);
            btn_modifyGesture.ForeColor = System.Drawing.Color.FromArgb(((int) ((byte) 64)), ((int) ((byte) 64)), ((int) ((byte) 64)));
            btn_modifyGesture.Image = global::WGestures.App.Properties.Resources.Edit;
            btn_modifyGesture.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            btn_modifyGesture.Location = new System.Drawing.Point(386, 0);
            btn_modifyGesture.Margin = new System.Windows.Forms.Padding(0, 2, 0, 2);
            btn_modifyGesture.Name = "btn_modifyGesture";
            btn_modifyGesture.Size = new System.Drawing.Size(38, 25);
            btn_modifyGesture.TabIndex = 8;
            btn_modifyGesture.UseVisualStyleBackColor = true;
            btn_modifyGesture.Click += btn_modifyGesture_Click;
            // 
            // btnAddGesture
            // 
            btnAddGesture.Font = new System.Drawing.Font("Verdana", 10.125F, System.Drawing.FontStyle.Bold);
            btnAddGesture.ForeColor = System.Drawing.Color.FromArgb(((int) ((byte) 64)), ((int) ((byte) 64)), ((int) ((byte) 64)));
            btnAddGesture.Image = global::WGestures.App.Properties.Resources.add;
            btnAddGesture.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            btnAddGesture.Location = new System.Drawing.Point(0, 0);
            btnAddGesture.Margin = new System.Windows.Forms.Padding(0);
            btnAddGesture.Name = "btnAddGesture";
            btnAddGesture.Size = new System.Drawing.Size(38, 25);
            btnAddGesture.TabIndex = 8;
            tip.SetToolTip(btnAddGesture, "添加手势");
            btnAddGesture.UseVisualStyleBackColor = true;
            btnAddGesture.Click += btnAddGesture_Click;
            // 
            // group_Command
            // 
            group_Command.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            group_Command.Controls.Add(flowLayoutPanel6);
            group_Command.Enabled = false;
            group_Command.Location = new System.Drawing.Point(12, 275);
            group_Command.Margin = new System.Windows.Forms.Padding(12, 12, 2, 2);
            group_Command.Name = "group_Command";
            group_Command.Padding = new System.Windows.Forms.Padding(2);
            group_Command.Size = new System.Drawing.Size(425, 272);
            group_Command.TabIndex = 10;
            group_Command.TabStop = false;
            group_Command.Text = "手势参数";
            // 
            // flowLayoutPanel6
            // 
            flowLayoutPanel6.Controls.Add(panel3);
            flowLayoutPanel6.Controls.Add(check_executeOnMouseWheeling);
            flowLayoutPanel6.Controls.Add(lineLabel1);
            flowLayoutPanel6.Controls.Add(panel_commandView);
            flowLayoutPanel6.Location = new System.Drawing.Point(5, 22);
            flowLayoutPanel6.Margin = new System.Windows.Forms.Padding(2);
            flowLayoutPanel6.Name = "flowLayoutPanel6";
            flowLayoutPanel6.Size = new System.Drawing.Size(415, 242);
            flowLayoutPanel6.TabIndex = 4;
            // 
            // panel3
            // 
            panel3.Controls.Add(label8);
            panel3.Controls.Add(combo_CommandTypes);
            panel3.Location = new System.Drawing.Point(0, 0);
            panel3.Margin = new System.Windows.Forms.Padding(0);
            panel3.Name = "panel3";
            panel3.Size = new System.Drawing.Size(415, 42);
            panel3.TabIndex = 2;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            label8.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            label8.Location = new System.Drawing.Point(2, 9);
            label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            label8.Name = "label8";
            label8.Size = new System.Drawing.Size(56, 17);
            label8.TabIndex = 1;
            label8.Text = "执行操作";
            // 
            // combo_CommandTypes
            // 
            combo_CommandTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            combo_CommandTypes.FlatStyle = System.Windows.Forms.FlatStyle.System;
            combo_CommandTypes.Font = new System.Drawing.Font("微软雅黑", 9F);
            combo_CommandTypes.FormattingEnabled = true;
            combo_CommandTypes.ItemHeight = 17;
            combo_CommandTypes.Location = new System.Drawing.Point(82, 5);
            combo_CommandTypes.Margin = new System.Windows.Forms.Padding(2);
            combo_CommandTypes.Name = "combo_CommandTypes";
            combo_CommandTypes.Size = new System.Drawing.Size(324, 25);
            combo_CommandTypes.TabIndex = 0;
            tip.SetToolTip(combo_CommandTypes, "手势触发后要执行的操作");
            combo_CommandTypes.SelectedIndexChanged += combo_CommandTypes_SelectedIndexChanged;
            // 
            // check_executeOnMouseWheeling
            // 
            check_executeOnMouseWheeling.AutoSize = true;
            check_executeOnMouseWheeling.FlatStyle = System.Windows.Forms.FlatStyle.System;
            check_executeOnMouseWheeling.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            check_executeOnMouseWheeling.Location = new System.Drawing.Point(2, 44);
            check_executeOnMouseWheeling.Margin = new System.Windows.Forms.Padding(2, 2, 2, 0);
            check_executeOnMouseWheeling.Name = "check_executeOnMouseWheeling";
            check_executeOnMouseWheeling.Size = new System.Drawing.Size(153, 22);
            check_executeOnMouseWheeling.TabIndex = 3;
            check_executeOnMouseWheeling.Text = "修饰键触发时立即执行";
            check_executeOnMouseWheeling.UseVisualStyleBackColor = true;
            check_executeOnMouseWheeling.Visible = false;
            check_executeOnMouseWheeling.CheckedChanged += check_executeOnMouseWheeling_CheckedChanged;
            // 
            // lineLabel1
            // 
            lineLabel1.ForeColor = System.Drawing.Color.Gainsboro;
            lineLabel1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            lineLabel1.Location = new System.Drawing.Point(2, 66);
            lineLabel1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            lineLabel1.Name = "lineLabel1";
            lineLabel1.Size = new System.Drawing.Size(412, 8);
            lineLabel1.TabIndex = 3;
            // 
            // panel_commandView
            // 
            panel_commandView.AutoScroll = true;
            panel_commandView.BackColor = System.Drawing.Color.Transparent;
            panel_commandView.Location = new System.Drawing.Point(0, 74);
            panel_commandView.Margin = new System.Windows.Forms.Padding(0);
            panel_commandView.Name = "panel_commandView";
            panel_commandView.Size = new System.Drawing.Size(415, 160);
            panel_commandView.TabIndex = 2;
            tip.SetToolTip(panel_commandView, "操作的额外参数");
            // 
            // btnEditApp
            // 
            btnEditApp.Font = new System.Drawing.Font("Verdana", 10.125F, System.Drawing.FontStyle.Bold);
            btnEditApp.ForeColor = System.Drawing.Color.FromArgb(((int) ((byte) 64)), ((int) ((byte) 64)), ((int) ((byte) 64)));
            btnEditApp.Image = global::WGestures.App.Properties.Resources.Edit;
            btnEditApp.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            btnEditApp.Location = new System.Drawing.Point(190, 554);
            btnEditApp.Margin = new System.Windows.Forms.Padding(0, 2, 0, 2);
            btnEditApp.Name = "btnEditApp";
            btnEditApp.Size = new System.Drawing.Size(38, 25);
            btnEditApp.TabIndex = 8;
            tip.SetToolTip(btnEditApp, "修改选中项目的名称或路径");
            btnEditApp.UseVisualStyleBackColor = true;
            btnEditApp.Click += btnEditApp_Click;
            // 
            // btnAppRemove
            // 
            btnAppRemove.Font = new System.Drawing.Font("Verdana", 10.125F, System.Drawing.FontStyle.Bold);
            btnAppRemove.ForeColor = System.Drawing.Color.FromArgb(((int) ((byte) 64)), ((int) ((byte) 64)), ((int) ((byte) 64)));
            btnAppRemove.Image = global::WGestures.App.Properties.Resources.remove;
            btnAppRemove.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            btnAppRemove.Location = new System.Drawing.Point(52, 554);
            btnAppRemove.Margin = new System.Windows.Forms.Padding(0, 2, 0, 2);
            btnAppRemove.Name = "btnAppRemove";
            btnAppRemove.Size = new System.Drawing.Size(38, 25);
            btnAppRemove.TabIndex = 8;
            tip.SetToolTip(btnAppRemove, "删除选中的项目");
            btnAppRemove.UseVisualStyleBackColor = true;
            btnAppRemove.Click += btnAppRemove_Click;
            // 
            // btnAddApp
            // 
            btnAddApp.Font = new System.Drawing.Font("Verdana", 10.125F, System.Drawing.FontStyle.Bold);
            btnAddApp.ForeColor = System.Drawing.Color.FromArgb(((int) ((byte) 64)), ((int) ((byte) 64)), ((int) ((byte) 64)));
            btnAddApp.Image = global::WGestures.App.Properties.Resources.add;
            btnAddApp.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            btnAddApp.Location = new System.Drawing.Point(18, 554);
            btnAddApp.Margin = new System.Windows.Forms.Padding(0, 2, 0, 2);
            btnAddApp.Name = "btnAddApp";
            btnAddApp.Size = new System.Drawing.Size(38, 25);
            btnAddApp.TabIndex = 8;
            tip.SetToolTip(btnAddApp, "添加应用程序");
            btnAddApp.UseVisualStyleBackColor = true;
            btnAddApp.Click += btnAddApp_Click;
            // 
            // listApps
            // 
            listApps.Alignment = System.Windows.Forms.ListViewAlignment.Left;
            listApps.AllowDrop = true;
            listApps.AllowItemDrag = true;
            listApps.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            listApps.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {colListAppDummy});
            listApps.Font = new System.Drawing.Font("微软雅黑", 9F);
            listApps.FullRowSelect = true;
            listApps.GridLines = true;
            listApps.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            listApps.InsertionLineColor = System.Drawing.Color.DeepSkyBlue;
            listApps.LabelWrap = false;
            listApps.Location = new System.Drawing.Point(18, 25);
            listApps.Margin = new System.Windows.Forms.Padding(2);
            listApps.MultiSelect = false;
            listApps.Name = "listApps";
            listApps.Size = new System.Drawing.Size(210, 524);
            listApps.SmallImageList = imglistAppIcons;
            listApps.TabIndex = 0;
            listApps.TileSize = new System.Drawing.Size(160, 42);
            listApps.UseCompatibleStateImageBehavior = false;
            listApps.View = System.Windows.Forms.View.Details;
            listApps.ItemDragDrop += listApps_ItemDragDrop;
            listApps.ItemDragging += listApps_ItemDragging;
            listApps.ItemSelectionChanged += listApps_ItemSelectionChanged;
            listApps.DragOver += listApps_DragOver;
            listApps.DoubleClick += listApps_DoubleClick;
            // 
            // colListAppDummy
            // 
            colListAppDummy.Name = "colListAppDummy";
            colListAppDummy.Width = 64;
            // 
            // imglistAppIcons
            // 
            imglistAppIcons.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            imglistAppIcons.ImageSize = new System.Drawing.Size(32, 32);
            imglistAppIcons.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // tab_hotCorners
            // 
            tab_hotCorners.Controls.Add(panel_hotcornerSettings);
            tab_hotCorners.Controls.Add(check_enableRubEdge);
            tab_hotCorners.Controls.Add(check_enableHotCorners);
            tab_hotCorners.Location = new System.Drawing.Point(4, 32);
            tab_hotCorners.Margin = new System.Windows.Forms.Padding(2);
            tab_hotCorners.Name = "tab_hotCorners";
            tab_hotCorners.Size = new System.Drawing.Size(690, 594);
            tab_hotCorners.TabIndex = 3;
            tab_hotCorners.Tag = "corners";
            tab_hotCorners.Text = "触发角 & 摩擦边";
            tab_hotCorners.UseVisualStyleBackColor = true;
            // 
            // panel_hotcornerSettings
            // 
            panel_hotcornerSettings.Controls.Add(radio_edge_0);
            panel_hotcornerSettings.Controls.Add(radio_edge_2);
            panel_hotcornerSettings.Controls.Add(radio_edge_1);
            panel_hotcornerSettings.Controls.Add(radio_edge_3);
            panel_hotcornerSettings.Controls.Add(radio_corner_1);
            panel_hotcornerSettings.Controls.Add(label11);
            panel_hotcornerSettings.Controls.Add(panel2);
            panel_hotcornerSettings.Controls.Add(combo_hotcornerCmdTypes);
            panel_hotcornerSettings.Controls.Add(panel_cornorCmdView);
            panel_hotcornerSettings.Controls.Add(radio_corner_0);
            panel_hotcornerSettings.Controls.Add(radio_corner_2);
            panel_hotcornerSettings.Controls.Add(radio_corner_3);
            panel_hotcornerSettings.Location = new System.Drawing.Point(18, 58);
            panel_hotcornerSettings.Margin = new System.Windows.Forms.Padding(2);
            panel_hotcornerSettings.Name = "panel_hotcornerSettings";
            panel_hotcornerSettings.Size = new System.Drawing.Size(652, 522);
            panel_hotcornerSettings.TabIndex = 10;
            // 
            // radio_edge_0
            // 
            radio_edge_0.Appearance = System.Windows.Forms.Appearance.Button;
            radio_edge_0.BackColor = System.Drawing.Color.WhiteSmoke;
            radio_edge_0.FlatAppearance.BorderColor = System.Drawing.Color.LightGray;
            radio_edge_0.FlatAppearance.CheckedBackColor = System.Drawing.Color.PaleTurquoise;
            radio_edge_0.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            radio_edge_0.Font = new System.Drawing.Font("微软雅黑", 8.25F);
            radio_edge_0.Location = new System.Drawing.Point(106, 120);
            radio_edge_0.Margin = new System.Windows.Forms.Padding(2);
            radio_edge_0.Name = "radio_edge_0";
            radio_edge_0.Size = new System.Drawing.Size(130, 32);
            radio_edge_0.TabIndex = 7;
            radio_edge_0.TabStop = true;
            radio_edge_0.Tag = "4";
            radio_edge_0.Text = "?";
            radio_edge_0.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            tip.SetToolTip(radio_edge_0, "屏幕左边缘");
            radio_edge_0.UseVisualStyleBackColor = false;
            radio_edge_0.CheckedChanged += radio_corner_1_CheckedChanged;
            // 
            // radio_edge_2
            // 
            radio_edge_2.Appearance = System.Windows.Forms.Appearance.Button;
            radio_edge_2.BackColor = System.Drawing.Color.WhiteSmoke;
            radio_edge_2.FlatAppearance.BorderColor = System.Drawing.Color.LightGray;
            radio_edge_2.FlatAppearance.CheckedBackColor = System.Drawing.Color.PaleTurquoise;
            radio_edge_2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            radio_edge_2.Font = new System.Drawing.Font("微软雅黑", 8.25F);
            radio_edge_2.Location = new System.Drawing.Point(418, 120);
            radio_edge_2.Margin = new System.Windows.Forms.Padding(2);
            radio_edge_2.Name = "radio_edge_2";
            radio_edge_2.Size = new System.Drawing.Size(130, 32);
            radio_edge_2.TabIndex = 7;
            radio_edge_2.TabStop = true;
            radio_edge_2.Tag = "6";
            radio_edge_2.Text = "?";
            radio_edge_2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            tip.SetToolTip(radio_edge_2, "屏幕右边缘");
            radio_edge_2.UseVisualStyleBackColor = false;
            radio_edge_2.CheckedChanged += radio_corner_1_CheckedChanged;
            // 
            // radio_edge_1
            // 
            radio_edge_1.Appearance = System.Windows.Forms.Appearance.Button;
            radio_edge_1.BackColor = System.Drawing.Color.WhiteSmoke;
            radio_edge_1.FlatAppearance.BorderColor = System.Drawing.Color.LightGray;
            radio_edge_1.FlatAppearance.CheckedBackColor = System.Drawing.Color.PaleTurquoise;
            radio_edge_1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            radio_edge_1.Font = new System.Drawing.Font("微软雅黑", 8.25F);
            radio_edge_1.Location = new System.Drawing.Point(265, 14);
            radio_edge_1.Margin = new System.Windows.Forms.Padding(2);
            radio_edge_1.Name = "radio_edge_1";
            radio_edge_1.Size = new System.Drawing.Size(130, 32);
            radio_edge_1.TabIndex = 7;
            radio_edge_1.TabStop = true;
            radio_edge_1.Tag = "5";
            radio_edge_1.Text = "?";
            radio_edge_1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            tip.SetToolTip(radio_edge_1, "屏幕上边缘");
            radio_edge_1.UseVisualStyleBackColor = false;
            radio_edge_1.CheckedChanged += radio_corner_1_CheckedChanged;
            // 
            // radio_edge_3
            // 
            radio_edge_3.Appearance = System.Windows.Forms.Appearance.Button;
            radio_edge_3.BackColor = System.Drawing.Color.WhiteSmoke;
            radio_edge_3.FlatAppearance.BorderColor = System.Drawing.Color.LightGray;
            radio_edge_3.FlatAppearance.CheckedBackColor = System.Drawing.Color.PaleTurquoise;
            radio_edge_3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            radio_edge_3.Font = new System.Drawing.Font("微软雅黑", 8.25F);
            radio_edge_3.Location = new System.Drawing.Point(265, 225);
            radio_edge_3.Margin = new System.Windows.Forms.Padding(2);
            radio_edge_3.Name = "radio_edge_3";
            radio_edge_3.Size = new System.Drawing.Size(130, 32);
            radio_edge_3.TabIndex = 7;
            radio_edge_3.TabStop = true;
            radio_edge_3.Tag = "7";
            radio_edge_3.Text = "?";
            radio_edge_3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            tip.SetToolTip(radio_edge_3, "屏幕下边缘");
            radio_edge_3.UseVisualStyleBackColor = false;
            radio_edge_3.CheckedChanged += radio_corner_1_CheckedChanged;
            // 
            // radio_corner_1
            // 
            radio_corner_1.Appearance = System.Windows.Forms.Appearance.Button;
            radio_corner_1.BackColor = System.Drawing.Color.WhiteSmoke;
            radio_corner_1.FlatAppearance.BorderColor = System.Drawing.Color.LightGray;
            radio_corner_1.FlatAppearance.CheckedBackColor = System.Drawing.Color.PaleTurquoise;
            radio_corner_1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            radio_corner_1.Font = new System.Drawing.Font("微软雅黑", 8.25F);
            radio_corner_1.Location = new System.Drawing.Point(36, 30);
            radio_corner_1.Margin = new System.Windows.Forms.Padding(2);
            radio_corner_1.Name = "radio_corner_1";
            radio_corner_1.Size = new System.Drawing.Size(130, 32);
            radio_corner_1.TabIndex = 7;
            radio_corner_1.TabStop = true;
            radio_corner_1.Tag = "1";
            radio_corner_1.Text = "?";
            radio_corner_1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            tip.SetToolTip(radio_corner_1, "屏幕左上角");
            radio_corner_1.UseVisualStyleBackColor = false;
            radio_corner_1.CheckedChanged += radio_corner_1_CheckedChanged;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            label11.Location = new System.Drawing.Point(84, 299);
            label11.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            label11.Name = "label11";
            label11.Size = new System.Drawing.Size(56, 17);
            label11.TabIndex = 9;
            label11.Text = "执行操作";
            label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel2
            // 
            panel2.BackColor = System.Drawing.Color.AliceBlue;
            panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            panel2.Controls.Add(label13);
            panel2.ForeColor = System.Drawing.Color.AliceBlue;
            panel2.Location = new System.Drawing.Point(171, 30);
            panel2.Margin = new System.Windows.Forms.Padding(2);
            panel2.Name = "panel2";
            panel2.Size = new System.Drawing.Size(312, 211);
            panel2.TabIndex = 1;
            // 
            // label13
            // 
            label13.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right));
            label13.AutoSize = true;
            label13.Font = new System.Drawing.Font("微软雅黑", 14.25F);
            label13.ForeColor = System.Drawing.Color.Silver;
            label13.Location = new System.Drawing.Point(125, 89);
            label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label13.Name = "label13";
            label13.Size = new System.Drawing.Size(56, 25);
            label13.TabIndex = 0;
            label13.Text = "屏 幕";
            label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // combo_hotcornerCmdTypes
            // 
            combo_hotcornerCmdTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            combo_hotcornerCmdTypes.FlatStyle = System.Windows.Forms.FlatStyle.System;
            combo_hotcornerCmdTypes.FormattingEnabled = true;
            combo_hotcornerCmdTypes.Location = new System.Drawing.Point(159, 295);
            combo_hotcornerCmdTypes.Margin = new System.Windows.Forms.Padding(2);
            combo_hotcornerCmdTypes.Name = "combo_hotcornerCmdTypes";
            combo_hotcornerCmdTypes.Size = new System.Drawing.Size(152, 25);
            combo_hotcornerCmdTypes.TabIndex = 8;
            combo_hotcornerCmdTypes.SelectedIndexChanged += combo_hotcornerCmdTypes_SelectedIndexChanged;
            combo_hotcornerCmdTypes.SelectedValueChanged += combo_hotcornerCmdTypes_SelectedValueChanged;
            // 
            // panel_cornorCmdView
            // 
            panel_cornorCmdView.BackColor = System.Drawing.Color.Transparent;
            panel_cornorCmdView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            panel_cornorCmdView.Location = new System.Drawing.Point(88, 334);
            panel_cornorCmdView.Margin = new System.Windows.Forms.Padding(2);
            panel_cornorCmdView.Name = "panel_cornorCmdView";
            panel_cornorCmdView.Size = new System.Drawing.Size(490, 176);
            panel_cornorCmdView.TabIndex = 6;
            // 
            // radio_corner_0
            // 
            radio_corner_0.Appearance = System.Windows.Forms.Appearance.Button;
            radio_corner_0.BackColor = System.Drawing.Color.WhiteSmoke;
            radio_corner_0.FlatAppearance.BorderColor = System.Drawing.Color.LightGray;
            radio_corner_0.FlatAppearance.CheckedBackColor = System.Drawing.Color.PaleTurquoise;
            radio_corner_0.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            radio_corner_0.Font = new System.Drawing.Font("微软雅黑", 8.25F);
            radio_corner_0.Location = new System.Drawing.Point(36, 209);
            radio_corner_0.Margin = new System.Windows.Forms.Padding(2);
            radio_corner_0.Name = "radio_corner_0";
            radio_corner_0.Size = new System.Drawing.Size(130, 32);
            radio_corner_0.TabIndex = 7;
            radio_corner_0.TabStop = true;
            radio_corner_0.Tag = "0";
            radio_corner_0.Text = "?";
            radio_corner_0.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            tip.SetToolTip(radio_corner_0, "屏幕左下角");
            radio_corner_0.UseVisualStyleBackColor = false;
            radio_corner_0.CheckedChanged += radio_corner_1_CheckedChanged;
            // 
            // radio_corner_2
            // 
            radio_corner_2.Appearance = System.Windows.Forms.Appearance.Button;
            radio_corner_2.BackColor = System.Drawing.Color.WhiteSmoke;
            radio_corner_2.FlatAppearance.BorderColor = System.Drawing.Color.LightGray;
            radio_corner_2.FlatAppearance.CheckedBackColor = System.Drawing.Color.PaleTurquoise;
            radio_corner_2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            radio_corner_2.Font = new System.Drawing.Font("微软雅黑", 8.25F);
            radio_corner_2.Location = new System.Drawing.Point(489, 30);
            radio_corner_2.Margin = new System.Windows.Forms.Padding(2);
            radio_corner_2.Name = "radio_corner_2";
            radio_corner_2.Size = new System.Drawing.Size(130, 32);
            radio_corner_2.TabIndex = 7;
            radio_corner_2.TabStop = true;
            radio_corner_2.Tag = "2";
            radio_corner_2.Text = "?";
            radio_corner_2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            tip.SetToolTip(radio_corner_2, "屏幕右上角");
            radio_corner_2.UseVisualStyleBackColor = false;
            radio_corner_2.CheckedChanged += radio_corner_1_CheckedChanged;
            // 
            // radio_corner_3
            // 
            radio_corner_3.Appearance = System.Windows.Forms.Appearance.Button;
            radio_corner_3.BackColor = System.Drawing.Color.WhiteSmoke;
            radio_corner_3.FlatAppearance.BorderColor = System.Drawing.Color.LightGray;
            radio_corner_3.FlatAppearance.CheckedBackColor = System.Drawing.Color.PaleTurquoise;
            radio_corner_3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            radio_corner_3.Font = new System.Drawing.Font("微软雅黑", 8.25F);
            radio_corner_3.Location = new System.Drawing.Point(489, 209);
            radio_corner_3.Margin = new System.Windows.Forms.Padding(2);
            radio_corner_3.Name = "radio_corner_3";
            radio_corner_3.Size = new System.Drawing.Size(130, 32);
            radio_corner_3.TabIndex = 7;
            radio_corner_3.TabStop = true;
            radio_corner_3.Tag = "3";
            radio_corner_3.Text = "?";
            radio_corner_3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            tip.SetToolTip(radio_corner_3, "屏幕右下角");
            radio_corner_3.UseVisualStyleBackColor = false;
            radio_corner_3.CheckedChanged += radio_corner_1_CheckedChanged;
            // 
            // check_enableRubEdge
            // 
            check_enableRubEdge.AutoSize = true;
            check_enableRubEdge.DataBindings.Add(new System.Windows.Forms.Binding("Checked", settingsFormControllerBindingSource, "GestureParserEnableRubEdges", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            check_enableRubEdge.FlatStyle = System.Windows.Forms.FlatStyle.System;
            check_enableRubEdge.Location = new System.Drawing.Point(139, 25);
            check_enableRubEdge.Margin = new System.Windows.Forms.Padding(2);
            check_enableRubEdge.Name = "check_enableRubEdge";
            check_enableRubEdge.Size = new System.Drawing.Size(93, 22);
            check_enableRubEdge.TabIndex = 0;
            check_enableRubEdge.Text = "启用摩擦边";
            check_enableRubEdge.UseVisualStyleBackColor = true;
            // 
            // check_enableHotCorners
            // 
            check_enableHotCorners.AutoSize = true;
            check_enableHotCorners.DataBindings.Add(new System.Windows.Forms.Binding("Checked", settingsFormControllerBindingSource, "GestureParserEnableHotCorners", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            check_enableHotCorners.FlatStyle = System.Windows.Forms.FlatStyle.System;
            check_enableHotCorners.Location = new System.Drawing.Point(18, 25);
            check_enableHotCorners.Margin = new System.Windows.Forms.Padding(2);
            check_enableHotCorners.Name = "check_enableHotCorners";
            check_enableHotCorners.Size = new System.Drawing.Size(93, 22);
            check_enableHotCorners.TabIndex = 0;
            check_enableHotCorners.Text = "启用触发角";
            check_enableHotCorners.UseVisualStyleBackColor = true;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(tb_updateLog);
            tabPage1.Controls.Add(flowLayoutPanel5);
            tabPage1.Controls.Add(picture_logo);
            tabPage1.Location = new System.Drawing.Point(4, 32);
            tabPage1.Margin = new System.Windows.Forms.Padding(2);
            tabPage1.Name = "tabPage1";
            tabPage1.Size = new System.Drawing.Size(690, 594);
            tabPage1.TabIndex = 2;
            tabPage1.Tag = "about";
            tabPage1.Text = "关 于";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // tb_updateLog
            // 
            tb_updateLog.Font = new System.Drawing.Font("微软雅黑", 9F);
            tb_updateLog.Location = new System.Drawing.Point(159, 28);
            tb_updateLog.Margin = new System.Windows.Forms.Padding(2);
            tb_updateLog.Multiline = true;
            tb_updateLog.Name = "tb_updateLog";
            tb_updateLog.ReadOnly = true;
            tb_updateLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            tb_updateLog.Size = new System.Drawing.Size(514, 192);
            tb_updateLog.TabIndex = 3;
            // 
            // flowLayoutPanel5
            // 
            flowLayoutPanel5.AutoSize = true;
            flowLayoutPanel5.Controls.Add(linkLabel1);
            flowLayoutPanel5.Controls.Add(linkLabel2);
            flowLayoutPanel5.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            flowLayoutPanel5.Location = new System.Drawing.Point(12, 158);
            flowLayoutPanel5.Margin = new System.Windows.Forms.Padding(2);
            flowLayoutPanel5.Name = "flowLayoutPanel5";
            flowLayoutPanel5.Size = new System.Drawing.Size(138, 102);
            flowLayoutPanel5.TabIndex = 2;
            // 
            // linkLabel1
            // 
            linkLabel1.AutoSize = true;
            linkLabel1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            linkLabel1.LinkColor = System.Drawing.Color.DodgerBlue;
            linkLabel1.Location = new System.Drawing.Point(2, 0);
            linkLabel1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 10);
            linkLabel1.Name = "linkLabel1";
            linkLabel1.Size = new System.Drawing.Size(56, 17);
            linkLabel1.TabIndex = 1;
            linkLabel1.TabStop = true;
            linkLabel1.Text = "项目主页";
            linkLabel1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            linkLabel1.LinkClicked += linkLabel1_LinkClicked;
            // 
            // linkLabel2
            // 
            linkLabel2.AutoSize = true;
            linkLabel2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            linkLabel2.LinkColor = System.Drawing.Color.DodgerBlue;
            linkLabel2.Location = new System.Drawing.Point(2, 27);
            linkLabel2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 10);
            linkLabel2.Name = "linkLabel2";
            linkLabel2.Size = new System.Drawing.Size(56, 17);
            linkLabel2.TabIndex = 1;
            linkLabel2.TabStop = true;
            linkLabel2.Text = "作者邮箱";
            linkLabel2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            linkLabel2.LinkClicked += linkLabel2_LinkClicked;
            // 
            // picture_logo
            // 
            picture_logo.Image = global::WGestures.App.Properties.Resources._128;
            picture_logo.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            picture_logo.Location = new System.Drawing.Point(12, 28);
            picture_logo.Margin = new System.Windows.Forms.Padding(2);
            picture_logo.Name = "picture_logo";
            picture_logo.Size = new System.Drawing.Size(132, 122);
            picture_logo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            picture_logo.TabIndex = 0;
            picture_logo.TabStop = false;
            // 
            // flowLayoutPanel4
            // 
            flowLayoutPanel4.Controls.Add(lb_info);
            flowLayoutPanel4.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            flowLayoutPanel4.Location = new System.Drawing.Point(12, 652);
            flowLayoutPanel4.Margin = new System.Windows.Forms.Padding(0, 15, 0, 12);
            flowLayoutPanel4.Name = "flowLayoutPanel4";
            flowLayoutPanel4.Size = new System.Drawing.Size(698, 20);
            flowLayoutPanel4.TabIndex = 10;
            // 
            // lb_info
            // 
            lb_info.AutoSize = true;
            lb_info.ForeColor = System.Drawing.Color.Gray;
            lb_info.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            lb_info.Location = new System.Drawing.Point(539, 0);
            lb_info.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            lb_info.Name = "lb_info";
            lb_info.Size = new System.Drawing.Size(157, 17);
            lb_info.TabIndex = 6;
            lb_info.Text = "*改动将自动保存并立即生效";
            // 
            // tip
            // 
            tip.AutomaticDelay = 80000;
            tip.AutoPopDelay = 144640;
            tip.InitialDelay = 120;
            tip.ReshowDelay = 2892;
            // 
            // pic_menuBtn
            // 
            pic_menuBtn.BackColor = System.Drawing.Color.Transparent;
            pic_menuBtn.Image = global::WGestures.App.Properties.Resources.menuBtn;
            pic_menuBtn.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            pic_menuBtn.Location = new System.Drawing.Point(678, 12);
            pic_menuBtn.Margin = new System.Windows.Forms.Padding(2);
            pic_menuBtn.Name = "pic_menuBtn";
            pic_menuBtn.Size = new System.Drawing.Size(30, 30);
            pic_menuBtn.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            pic_menuBtn.TabIndex = 11;
            pic_menuBtn.TabStop = false;
            tip.SetToolTip(pic_menuBtn, "菜单");
            pic_menuBtn.Click += pic_menuBtn_Click;
            pic_menuBtn.MouseDown += pic_menuBtn_MouseDown;
            // 
            // ctx_gesturesMenu
            // 
            ctx_gesturesMenu.ImageScalingSize = new System.Drawing.Size(24, 24);
            ctx_gesturesMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {menuItem_import, menuItem_export, menuItem_resetGestures});
            ctx_gesturesMenu.Name = "contextMenuStrip1";
            ctx_gesturesMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            ctx_gesturesMenu.Size = new System.Drawing.Size(134, 70);
            ctx_gesturesMenu.Closed += ctx_gesturesMenu_Closed;
            // 
            // menuItem_import
            // 
            menuItem_import.Name = "menuItem_import";
            menuItem_import.Size = new System.Drawing.Size(133, 22);
            menuItem_import.Text = "导入...";
            menuItem_import.Click += menuItem_imxport_Click;
            // 
            // menuItem_export
            // 
            menuItem_export.Name = "menuItem_export";
            menuItem_export.Size = new System.Drawing.Size(133, 22);
            menuItem_export.Text = "导出...";
            menuItem_export.Click += menuItem_export_Click;
            // 
            // menuItem_resetGestures
            // 
            menuItem_resetGestures.Name = "menuItem_resetGestures";
            menuItem_resetGestures.Size = new System.Drawing.Size(133, 22);
            menuItem_resetGestures.Text = "恢复默认...";
            menuItem_resetGestures.Click += menuItem_resetGestures_Click;
            // 
            // errorProvider
            // 
            errorProvider.BlinkRate = 300;
            errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            errorProvider.ContainerControl = this;
            // 
            // SettingsForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            AutoSize = true;
            AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            BackColor = System.Drawing.Color.WhiteSmoke;
            ClientSize = new System.Drawing.Size(722, 659);
            Controls.Add(pic_menuBtn);
            Controls.Add(flowLayoutPanel4);
            Controls.Add(tabControl);
            Font = new System.Drawing.Font("微软雅黑", 9F);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            Margin = new System.Windows.Forms.Padding(2);
            MaximizeBox = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "WGestures设置";
            FormClosing += SettingsForm_FormClosing;
            Shown += SettingsForm_Shown;
            tabControl.ResumeLayout(false);
            tabPage_general.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) settingsFormControllerBindingSource).EndInit();
            ((System.ComponentModel.ISupportInitialize) num_pathTrackerInitialStayTimeoutMillis).EndInit();
            ((System.ComponentModel.ISupportInitialize) numPathTrackerStayTimeoutMillis).EndInit();
            ((System.ComponentModel.ISupportInitialize) numPathTrackerInitialValidMove).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            flowLayoutPanel3.ResumeLayout(false);
            flowLayoutPanel3.PerformLayout();
            tabPage2.ResumeLayout(false);
            tabPage2.PerformLayout();
            flowLayoutPanel2.ResumeLayout(false);
            flowLayoutPanel2.PerformLayout();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            flowLayoutPanel1.ResumeLayout(false);
            flowLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) pictureSelectedApp).EndInit();
            panel_intentListOperations.ResumeLayout(false);
            group_Command.ResumeLayout(false);
            flowLayoutPanel6.ResumeLayout(false);
            flowLayoutPanel6.PerformLayout();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            tab_hotCorners.ResumeLayout(false);
            tab_hotCorners.PerformLayout();
            panel_hotcornerSettings.ResumeLayout(false);
            panel_hotcornerSettings.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            tabPage1.ResumeLayout(false);
            tabPage1.PerformLayout();
            flowLayoutPanel5.ResumeLayout(false);
            flowLayoutPanel5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) picture_logo).EndInit();
            flowLayoutPanel4.ResumeLayout(false);
            flowLayoutPanel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) pic_menuBtn).EndInit();
            ctx_gesturesMenu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize) errorProvider).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private TabControl tabControl;
        private TabPage tabPage_general;
        private TabPage tabPage2;
        private CheckBox check_autoStart;
        private TabPage tabPage1;
        private CheckBox check_autoCheckUpdate;
        private Button btn_checkUpdateNow;
        private GroupBox groupBox2;
        private GroupBox groupBox1;
        private CheckBox checkGestureViewShowPath;
        private Label label1;
        private CheckBox checkGestureViewShowCommandName;
        private Label label4;
        private Label label5;
        private Label label6;
        private InstantNumericUpDown numPathTrackerInitialValidMove;
        private CheckBox checkPathTrackerStayTimeout;
        private InstantNumericUpDown numPathTrackerStayTimeoutMillis;
        private AlwaysSelectedListView listApps;
        private Label lb_Version;
        private ImageList imglistAppIcons;
        private FlowLayoutPanel flowLayoutPanel1;
        private AlwaysSelectedListView listGestureIntents;
        private ColumnHeader colGestureName;
        private ColumnHeader colGestureDirs;
        private CheckBox check_gesturingDisabled;
        private ColumnHeader colListAppDummy;
        private PictureBox pictureSelectedApp;
        private FlowLayoutPanel flowLayoutPanel2;
        private Label label7;
        private MetroButton btnAddApp;
        private MetroButton btnAppRemove;
        private MetroButton btnEditApp;
        private Panel panel_intentListOperations;
        private MetroButton btn_RemoveGesture;
        private MetroButton btnAddGesture;
        private GroupBox group_Command;
        private Label label8;
        private ComboBox combo_CommandTypes;
        private Panel panel_commandView;
        private Windows.Controls.ColorButton colorBtn_unrecogonized;
        private Windows.Controls.ColorButton colorBtn_recogonized;
        private Label label9;
        private ColumnHeader operation;
        private ImageList dummyImgLstForLstViewHeightFix;
        private ToolTip tip;
        private FlowLayoutPanel flowLayoutPanel4;
        private Label lb_info;
        private PictureBox picture_logo;
        private CheckBox checkGestureView_fadeOut;
        private MetroButton btn_modifyGesture;
        private FlowLayoutPanel flowLayoutPanel3;
        private ErrorProvider errorProvider;
        private TextBox tb_updateLog;
        private FlowLayoutPanel flowLayoutPanel5;
        private LinkLabel linkLabel1;
        private LinkLabel linkLabel2;
        private LineLabel lineLabel1;
        private FlowLayoutPanel flowLayoutPanel6;
        private Panel panel3;
        private CheckBox check_executeOnMouseWheeling;
        private InstantNumericUpDown num_pathTrackerInitialStayTimeoutMillis;
        private CheckBox check_pathTrackerInitialStayTimeout;
        private PictureBox pic_menuBtn;
        private ContextMenuStrip ctx_gesturesMenu;
        private ToolStripMenuItem menuItem_import;
        private ToolStripMenuItem menuItem_export;
        private CheckBox full;
        private BindingSource settingsFormControllerBindingSource;
        private ColorButton colorMiddle;
        private Panel panel1;
        private CheckBox checkInheritGlobal;
        private LineLabel lineLabel2;
        private Label label3;
        private TabPage tab_hotCorners;
        private CheckBox check_enableHotCorners;
        private Panel panel2;
        private CheckBox check_enable8DirGesture;
        private CheckBox check_preferCursorWindow;
        private ToolStripMenuItem menuItem_resetGestures;
        private Panel panel_cornorCmdView;
        private RadioButton radio_corner_2;
        private RadioButton radio_corner_3;
        private RadioButton radio_corner_0;
        private RadioButton radio_corner_1;
        private ComboBox combo_hotcornerCmdTypes;
        private Label label11;
        private Panel panel_hotcornerSettings;
        private RadioButton radio_edge_3;
        private RadioButton radio_edge_0;
        private RadioButton radio_edge_1;
        private RadioButton radio_edge_2;
        private CheckBox check_enableRubEdge;
        private Label label13;
        private Label label14;
        private ShortcutRecordButton shortcutRec_pause;
        private Label lb_pause_shortcut;
        private Label label15;
        private CheckBox check_gestBtn_X;
        private CheckBox check_gestBtn_Middle;
        private CheckBox check_gestBtn_Right;
        private ColorButton colorBtn_x;
        private CheckBox checkBox1;
    }
}