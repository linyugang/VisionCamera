namespace Yoga.Camera
{
    partial class frmCameraSetting
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
            this.trackBarShutter = new System.Windows.Forms.TrackBar();
            this.trackBarGain = new System.Windows.Forms.TrackBar();
            this.m_lbl_Shutter = new System.Windows.Forms.Label();
            this.m_lbl_Gain = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.ImageAngleComboBox = new System.Windows.Forms.ComboBox();
            this.btnTestOut = new System.Windows.Forms.Button();
            this.btnResetOutLineTime = new System.Windows.Forms.Button();
            this.UpDownOutLineTime = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.trackBarOutLineTime = new System.Windows.Forms.TrackBar();
            this.btnResetLineDebouncerTimeAbs = new System.Windows.Forms.Button();
            this.UpDownLineDebouncerTimeAbs = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.trackBarLineDebouncerTimeAbs = new System.Windows.Forms.TrackBar();
            this.btnResetTriggerDelayAbs = new System.Windows.Forms.Button();
            this.UpDownTriggerDelayAbs = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.trackBarTriggerDelayAbs = new System.Windows.Forms.TrackBar();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnResetGain = new System.Windows.Forms.Button();
            this.btnResetShutter = new System.Windows.Forms.Button();
            this.UpDownGain = new System.Windows.Forms.NumericUpDown();
            this.UpDownShutter = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarShutter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarGain)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.UpDownOutLineTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarOutLineTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.UpDownLineDebouncerTimeAbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarLineDebouncerTimeAbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.UpDownTriggerDelayAbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarTriggerDelayAbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.UpDownGain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.UpDownShutter)).BeginInit();
            this.SuspendLayout();
            // 
            // trackBarShutter
            // 
            this.trackBarShutter.Location = new System.Drawing.Point(194, 20);
            this.trackBarShutter.Name = "trackBarShutter";
            this.trackBarShutter.Size = new System.Drawing.Size(327, 45);
            this.trackBarShutter.TabIndex = 0;
            this.trackBarShutter.Scroll += new System.EventHandler(this.trackBarShutter_Scroll);
            // 
            // trackBarGain
            // 
            this.trackBarGain.Location = new System.Drawing.Point(194, 60);
            this.trackBarGain.Name = "trackBarGain";
            this.trackBarGain.Size = new System.Drawing.Size(327, 45);
            this.trackBarGain.TabIndex = 1;
            this.trackBarGain.Scroll += new System.EventHandler(this.trackBarGain_Scroll);
            // 
            // m_lbl_Shutter
            // 
            this.m_lbl_Shutter.AutoSize = true;
            this.m_lbl_Shutter.Location = new System.Drawing.Point(20, 27);
            this.m_lbl_Shutter.Name = "m_lbl_Shutter";
            this.m_lbl_Shutter.Size = new System.Drawing.Size(53, 12);
            this.m_lbl_Shutter.TabIndex = 13;
            this.m_lbl_Shutter.Text = "曝光时间";
            // 
            // m_lbl_Gain
            // 
            this.m_lbl_Gain.AutoSize = true;
            this.m_lbl_Gain.Location = new System.Drawing.Point(20, 66);
            this.m_lbl_Gain.Name = "m_lbl_Gain";
            this.m_lbl_Gain.Size = new System.Drawing.Size(29, 12);
            this.m_lbl_Gain.TabIndex = 15;
            this.m_lbl_Gain.Text = "增益";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.ImageAngleComboBox);
            this.groupBox3.Controls.Add(this.btnTestOut);
            this.groupBox3.Controls.Add(this.btnResetOutLineTime);
            this.groupBox3.Controls.Add(this.UpDownOutLineTime);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.trackBarOutLineTime);
            this.groupBox3.Controls.Add(this.btnResetLineDebouncerTimeAbs);
            this.groupBox3.Controls.Add(this.UpDownLineDebouncerTimeAbs);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.trackBarLineDebouncerTimeAbs);
            this.groupBox3.Controls.Add(this.btnResetTriggerDelayAbs);
            this.groupBox3.Controls.Add(this.UpDownTriggerDelayAbs);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.trackBarTriggerDelayAbs);
            this.groupBox3.Controls.Add(this.btnSave);
            this.groupBox3.Controls.Add(this.btnResetGain);
            this.groupBox3.Controls.Add(this.btnResetShutter);
            this.groupBox3.Controls.Add(this.UpDownGain);
            this.groupBox3.Controls.Add(this.UpDownShutter);
            this.groupBox3.Controls.Add(this.m_lbl_Gain);
            this.groupBox3.Controls.Add(this.trackBarGain);
            this.groupBox3.Controls.Add(this.m_lbl_Shutter);
            this.groupBox3.Controls.Add(this.trackBarShutter);
            this.groupBox3.Location = new System.Drawing.Point(12, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(585, 294);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "基本参数设置";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(20, 221);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 58;
            this.label4.Text = "图像角度";
            // 
            // ImageAngleComboBox
            // 
            this.ImageAngleComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ImageAngleComboBox.Location = new System.Drawing.Point(119, 218);
            this.ImageAngleComboBox.Name = "ImageAngleComboBox";
            this.ImageAngleComboBox.Size = new System.Drawing.Size(123, 20);
            this.ImageAngleComboBox.TabIndex = 57;
            this.ImageAngleComboBox.SelectedIndexChanged += new System.EventHandler(this.ImageAngleComboBox_SelectedIndexChanged);
            // 
            // btnTestOut
            // 
            this.btnTestOut.Location = new System.Drawing.Point(271, 265);
            this.btnTestOut.Name = "btnTestOut";
            this.btnTestOut.Size = new System.Drawing.Size(114, 23);
            this.btnTestOut.TabIndex = 56;
            this.btnTestOut.Text = "信号输出测试";
            this.btnTestOut.UseVisualStyleBackColor = true;
            this.btnTestOut.Click += new System.EventHandler(this.btnTestOut_Click);
            // 
            // btnResetOutLineTime
            // 
            this.btnResetOutLineTime.BackColor = System.Drawing.SystemColors.Control;
            this.btnResetOutLineTime.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnResetOutLineTime.Location = new System.Drawing.Point(522, 177);
            this.btnResetOutLineTime.Name = "btnResetOutLineTime";
            this.btnResetOutLineTime.Size = new System.Drawing.Size(57, 26);
            this.btnResetOutLineTime.TabIndex = 55;
            this.btnResetOutLineTime.Text = "重置";
            this.btnResetOutLineTime.UseVisualStyleBackColor = false;
            this.btnResetOutLineTime.Click += new System.EventHandler(this.btnResetOutLineTime_Click);
            // 
            // UpDownOutLineTime
            // 
            this.UpDownOutLineTime.Location = new System.Drawing.Point(119, 180);
            this.UpDownOutLineTime.Maximum = new decimal(new int[] {
            6,
            0,
            0,
            0});
            this.UpDownOutLineTime.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.UpDownOutLineTime.Name = "UpDownOutLineTime";
            this.UpDownOutLineTime.Size = new System.Drawing.Size(80, 21);
            this.UpDownOutLineTime.TabIndex = 54;
            this.UpDownOutLineTime.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.UpDownOutLineTime.ValueChanged += new System.EventHandler(this.UpDownOutLineTime_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 183);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 12);
            this.label3.TabIndex = 53;
            this.label3.Text = "输出延时(ms)";
            // 
            // trackBarOutLineTime
            // 
            this.trackBarOutLineTime.Location = new System.Drawing.Point(194, 180);
            this.trackBarOutLineTime.Name = "trackBarOutLineTime";
            this.trackBarOutLineTime.Size = new System.Drawing.Size(327, 45);
            this.trackBarOutLineTime.TabIndex = 52;
            this.trackBarOutLineTime.Scroll += new System.EventHandler(this.trackBarOutLineTime_Scroll);
            // 
            // btnResetLineDebouncerTimeAbs
            // 
            this.btnResetLineDebouncerTimeAbs.BackColor = System.Drawing.SystemColors.Control;
            this.btnResetLineDebouncerTimeAbs.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnResetLineDebouncerTimeAbs.Location = new System.Drawing.Point(522, 136);
            this.btnResetLineDebouncerTimeAbs.Name = "btnResetLineDebouncerTimeAbs";
            this.btnResetLineDebouncerTimeAbs.Size = new System.Drawing.Size(57, 26);
            this.btnResetLineDebouncerTimeAbs.TabIndex = 51;
            this.btnResetLineDebouncerTimeAbs.Text = "重置";
            this.btnResetLineDebouncerTimeAbs.UseVisualStyleBackColor = false;
            this.btnResetLineDebouncerTimeAbs.Click += new System.EventHandler(this.btnResetLineDebouncerTimeAbs_Click);
            // 
            // UpDownLineDebouncerTimeAbs
            // 
            this.UpDownLineDebouncerTimeAbs.Location = new System.Drawing.Point(119, 140);
            this.UpDownLineDebouncerTimeAbs.Maximum = new decimal(new int[] {
            6,
            0,
            0,
            0});
            this.UpDownLineDebouncerTimeAbs.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.UpDownLineDebouncerTimeAbs.Name = "UpDownLineDebouncerTimeAbs";
            this.UpDownLineDebouncerTimeAbs.Size = new System.Drawing.Size(80, 21);
            this.UpDownLineDebouncerTimeAbs.TabIndex = 50;
            this.UpDownLineDebouncerTimeAbs.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.UpDownLineDebouncerTimeAbs.ValueChanged += new System.EventHandler(this.UpDownLineDebouncerTimeAbs_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 144);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 49;
            this.label2.Text = "触发防抖(us)";
            // 
            // trackBarLineDebouncerTimeAbs
            // 
            this.trackBarLineDebouncerTimeAbs.Location = new System.Drawing.Point(194, 144);
            this.trackBarLineDebouncerTimeAbs.Name = "trackBarLineDebouncerTimeAbs";
            this.trackBarLineDebouncerTimeAbs.Size = new System.Drawing.Size(327, 45);
            this.trackBarLineDebouncerTimeAbs.TabIndex = 48;
            this.trackBarLineDebouncerTimeAbs.Scroll += new System.EventHandler(this.trackBarLineDebouncerTimeAbs_Scroll);
            // 
            // btnResetTriggerDelayAbs
            // 
            this.btnResetTriggerDelayAbs.BackColor = System.Drawing.SystemColors.Control;
            this.btnResetTriggerDelayAbs.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnResetTriggerDelayAbs.Location = new System.Drawing.Point(522, 95);
            this.btnResetTriggerDelayAbs.Name = "btnResetTriggerDelayAbs";
            this.btnResetTriggerDelayAbs.Size = new System.Drawing.Size(57, 26);
            this.btnResetTriggerDelayAbs.TabIndex = 47;
            this.btnResetTriggerDelayAbs.Text = "重置";
            this.btnResetTriggerDelayAbs.UseVisualStyleBackColor = false;
            this.btnResetTriggerDelayAbs.Click += new System.EventHandler(this.btnResetTriggerDelayAbs_Click);
            // 
            // UpDownTriggerDelayAbs
            // 
            this.UpDownTriggerDelayAbs.Location = new System.Drawing.Point(119, 100);
            this.UpDownTriggerDelayAbs.Maximum = new decimal(new int[] {
            6,
            0,
            0,
            0});
            this.UpDownTriggerDelayAbs.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.UpDownTriggerDelayAbs.Name = "UpDownTriggerDelayAbs";
            this.UpDownTriggerDelayAbs.Size = new System.Drawing.Size(80, 21);
            this.UpDownTriggerDelayAbs.TabIndex = 46;
            this.UpDownTriggerDelayAbs.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.UpDownTriggerDelayAbs.ValueChanged += new System.EventHandler(this.UpDownTriggerDelayAbs_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 105);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 45;
            this.label1.Text = "触发延时(us)";
            // 
            // trackBarTriggerDelayAbs
            // 
            this.trackBarTriggerDelayAbs.Location = new System.Drawing.Point(194, 103);
            this.trackBarTriggerDelayAbs.Name = "trackBarTriggerDelayAbs";
            this.trackBarTriggerDelayAbs.Size = new System.Drawing.Size(327, 45);
            this.trackBarTriggerDelayAbs.TabIndex = 44;
            this.trackBarTriggerDelayAbs.Scroll += new System.EventHandler(this.trackBarTriggerDelayAbs_Scroll);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(492, 265);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 43;
            this.btnSave.Text = "保存";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnResetGain
            // 
            this.btnResetGain.BackColor = System.Drawing.SystemColors.Control;
            this.btnResetGain.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnResetGain.Location = new System.Drawing.Point(522, 54);
            this.btnResetGain.Name = "btnResetGain";
            this.btnResetGain.Size = new System.Drawing.Size(57, 26);
            this.btnResetGain.TabIndex = 42;
            this.btnResetGain.Text = "重置";
            this.btnResetGain.UseVisualStyleBackColor = false;
            this.btnResetGain.Click += new System.EventHandler(this.btnResetGain_Click);
            // 
            // btnResetShutter
            // 
            this.btnResetShutter.BackColor = System.Drawing.SystemColors.Control;
            this.btnResetShutter.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnResetShutter.Location = new System.Drawing.Point(522, 13);
            this.btnResetShutter.Name = "btnResetShutter";
            this.btnResetShutter.Size = new System.Drawing.Size(57, 26);
            this.btnResetShutter.TabIndex = 41;
            this.btnResetShutter.Text = "重置";
            this.btnResetShutter.UseVisualStyleBackColor = false;
            this.btnResetShutter.Click += new System.EventHandler(this.btnResetShutter_Click);
            // 
            // UpDownGain
            // 
            this.UpDownGain.Location = new System.Drawing.Point(119, 60);
            this.UpDownGain.Maximum = new decimal(new int[] {
            6,
            0,
            0,
            0});
            this.UpDownGain.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.UpDownGain.Name = "UpDownGain";
            this.UpDownGain.Size = new System.Drawing.Size(80, 21);
            this.UpDownGain.TabIndex = 33;
            this.UpDownGain.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.UpDownGain.ValueChanged += new System.EventHandler(this.UpDownGain_ValueChanged);
            // 
            // UpDownShutter
            // 
            this.UpDownShutter.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.UpDownShutter.Location = new System.Drawing.Point(119, 20);
            this.UpDownShutter.Maximum = new decimal(new int[] {
            6,
            0,
            0,
            0});
            this.UpDownShutter.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.UpDownShutter.Name = "UpDownShutter";
            this.UpDownShutter.Size = new System.Drawing.Size(80, 21);
            this.UpDownShutter.TabIndex = 32;
            this.UpDownShutter.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.UpDownShutter.ValueChanged += new System.EventHandler(this.UpDownShutter_ValueChanged);
            // 
            // frmCameraSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(637, 328);
            this.Controls.Add(this.groupBox3);
            this.Name = "frmCameraSetting";
            this.Text = "相机参数设定";
            this.Load += new System.EventHandler(this.frmCameraSetting_Load);
            ((System.ComponentModel.ISupportInitialize)(this.trackBarShutter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarGain)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.UpDownOutLineTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarOutLineTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.UpDownLineDebouncerTimeAbs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarLineDebouncerTimeAbs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.UpDownTriggerDelayAbs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarTriggerDelayAbs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.UpDownGain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.UpDownShutter)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TrackBar trackBarShutter;
        private System.Windows.Forms.TrackBar trackBarGain;
        private System.Windows.Forms.Label m_lbl_Shutter;
        private System.Windows.Forms.Label m_lbl_Gain;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.NumericUpDown UpDownShutter;
        private System.Windows.Forms.NumericUpDown UpDownGain;
        private System.Windows.Forms.Button btnResetGain;
        private System.Windows.Forms.Button btnResetShutter;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnResetTriggerDelayAbs;
        private System.Windows.Forms.NumericUpDown UpDownTriggerDelayAbs;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TrackBar trackBarTriggerDelayAbs;
        private System.Windows.Forms.Button btnResetLineDebouncerTimeAbs;
        private System.Windows.Forms.NumericUpDown UpDownLineDebouncerTimeAbs;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TrackBar trackBarLineDebouncerTimeAbs;
        private System.Windows.Forms.Button btnResetOutLineTime;
        private System.Windows.Forms.NumericUpDown UpDownOutLineTime;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TrackBar trackBarOutLineTime;
        private System.Windows.Forms.Button btnTestOut;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox ImageAngleComboBox;
    }
}