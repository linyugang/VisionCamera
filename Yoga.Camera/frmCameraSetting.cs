using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace Yoga.Camera
{
    public partial class frmCameraSetting : Form
    {
        private Camera.CameraBase camera1;
        bool settingLocck = false;
        public frmCameraSetting(Camera.CameraBase camera)
        {
            InitializeComponent();
            this.camera1 = camera;
        }

        private void frmCameraSetting_Load(object sender, EventArgs e)
        {
            settingLocck = true;
            try
            {
                if (camera1 is Camera.DirectShowCamera)
                {
                    trackBarShutter.Enabled = false;
                    UpDownShutter.Enabled = false;
                    btnResetShutter.Enabled = false;

                    trackBarGain.Enabled = false;
                    UpDownGain.Enabled = false;
                    btnResetGain.Enabled = false;

                    trackBarTriggerDelayAbs.Enabled = false;
                    UpDownTriggerDelayAbs.Enabled = false;
                    btnResetTriggerDelayAbs.Enabled = false;

                    trackBarLineDebouncerTimeAbs.Enabled = false;
                    UpDownLineDebouncerTimeAbs.Enabled = false;
                    btnResetLineDebouncerTimeAbs.Enabled = false;

                    trackBarOutLineTime.Enabled = false;
                    UpDownOutLineTime.Enabled = false;
                    btnResetOutLineTime.Enabled = false;

                }
                if (camera1.ShuterCur!=-1)
                {
                    //进度条值放大100倍
                    trackBarShutter.Minimum = (int)(camera1.ShuterMin);
                    trackBarShutter.Maximum = (int)(camera1.ShuterMax);

                    trackBarShutter.TickFrequency = (trackBarShutter.Maximum - trackBarShutter.Minimum) / 10;

                    //设置显示两位有效数字
                    UpDownShutter.DecimalPlaces = 0;
                    UpDownShutter.Increment = 100M;
                    UpDownShutter.Minimum = (int)(camera1.ShuterMin);
                    UpDownShutter.Maximum = (int)(camera1.ShuterMax);
                    UpDownShutter.Value = (decimal)(camera1.ShuterCur);
                }
                else
                {
                    trackBarShutter.Enabled = false;
                    UpDownShutter.Enabled = false;
                    btnResetShutter.Enabled = false;
                }

                if (camera1.GainCur!=-1)
                {
                    //进度条值与updown相同
                    trackBarGain.Minimum = (int)(camera1.GainMin);
                    trackBarGain.Maximum = (int)(camera1.GainMax);

                    UpDownGain.Minimum = (int)(camera1.GainMin);
                    UpDownGain.Maximum = (int)(camera1.GainMax);
                    UpDownGain.Value = (decimal)(camera1.GainCur);
                }
                else
                {
                    trackBarGain.Enabled = false;
                    UpDownGain.Enabled = false;
                    btnResetGain.Enabled = false;
                }
                if (camera1.TriggerDelayAbs!=-1)
                {
                    trackBarTriggerDelayAbs.Minimum = (int)camera1.TriggerDelayAbsMin;
                    trackBarTriggerDelayAbs.Maximum = (int)camera1.TriggerDelayAbsMax;
                    trackBarTriggerDelayAbs.TickFrequency = (trackBarTriggerDelayAbs.Maximum - trackBarTriggerDelayAbs.Minimum) / 10;

                    UpDownTriggerDelayAbs.Minimum = (int)camera1.TriggerDelayAbsMin;
                    UpDownTriggerDelayAbs.Maximum = (int)camera1.TriggerDelayAbsMax;
                    UpDownTriggerDelayAbs.Value = (decimal)camera1.TriggerDelayAbs;
                }
                else
                {
                    trackBarTriggerDelayAbs.Enabled = false;
                    UpDownTriggerDelayAbs.Enabled = false;
                    btnResetTriggerDelayAbs.Enabled = false;
                }

                if (camera1.LineDebouncerTimeAbs!=-1)
                {
                    trackBarLineDebouncerTimeAbs.Minimum = (int)camera1.LineDebouncerTimeAbsMin;
                    trackBarLineDebouncerTimeAbs.Maximum = (int)camera1.LineDebouncerTimeAbsMax;
                    trackBarLineDebouncerTimeAbs.TickFrequency = (trackBarLineDebouncerTimeAbs.Maximum - trackBarLineDebouncerTimeAbs.Minimum) / 10;

                    UpDownLineDebouncerTimeAbs.Minimum = (int)camera1.LineDebouncerTimeAbsMin;
                    UpDownLineDebouncerTimeAbs.Maximum = (int)camera1.LineDebouncerTimeAbsMax;
                    UpDownLineDebouncerTimeAbs.Value = (decimal)camera1.LineDebouncerTimeAbs;
                }
               else
                {
                    trackBarLineDebouncerTimeAbs.Enabled = false;
                    UpDownLineDebouncerTimeAbs.Enabled = false;
                    btnResetLineDebouncerTimeAbs.Enabled = false;
                }

                trackBarOutLineTime.Maximum = 10000;
                trackBarOutLineTime.Minimum = 0;
                trackBarOutLineTime.TickFrequency = (trackBarOutLineTime.Maximum - trackBarOutLineTime.Minimum) / 10;


                UpDownOutLineTime.Maximum = 10000;
                UpDownOutLineTime.Minimum = 0;
                UpDownOutLineTime.Value = (decimal)camera1.OutLineTime;

                ImageAngleComboBox.DataSource = System.Enum.GetNames(typeof(ImageAngle));
                ImageAngleComboBox.SelectedIndex = this.ImageAngleComboBox.FindString(camera1.ImageAngle.ToString());

                settingLocck = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("相机参数设置异常:" + ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void trackBarShutter_Scroll(object sender, EventArgs e)
        {
            //获取值
            UpDownShutter.Value = trackBarShutter.Value;
            UpDownShutter.Refresh();
        }



        private void trackBarGain_Scroll(object sender, EventArgs e)
        {
            UpDownGain.Value = trackBarGain.Value;
            UpDownGain.Refresh();
        }
        private void trackBarTriggerDelayAbs_Scroll(object sender, EventArgs e)
        {
            UpDownTriggerDelayAbs.Value = trackBarTriggerDelayAbs.Value;
            UpDownTriggerDelayAbs.Refresh();
        }

        private void trackBarLineDebouncerTimeAbs_Scroll(object sender, EventArgs e)
        {
            UpDownLineDebouncerTimeAbs.Value = trackBarLineDebouncerTimeAbs.Value;
            UpDownLineDebouncerTimeAbs.Refresh();
        }

        private void trackBarOutLineTime_Scroll(object sender, EventArgs e)
        {
            UpDownOutLineTime.Value = trackBarOutLineTime.Value;
            UpDownOutLineTime.Refresh();
        }


        private void UpDownShutter_ValueChanged(object sender, EventArgs e)
        {
            trackBarShutter.Value = (int)(UpDownShutter.Value);
            if (settingLocck)
            {
                return;
            }

            camera1.ShuterCur = (long)UpDownShutter.Value;
            //Debug.WriteLine(string.Format( "调整曝光值记录中 结果值{0}", camera1.ShuterCur));
        }

        private void UpDownGain_ValueChanged(object sender, EventArgs e)
        {
            trackBarGain.Value = (int)UpDownGain.Value;
            if (settingLocck)
            {
                return;
            }
            camera1.GainCur = (double)UpDownGain.Value;
            Debug.WriteLine("调整增益中");
        }
        private void UpDownTriggerDelayAbs_ValueChanged(object sender, EventArgs e)
        {
            trackBarTriggerDelayAbs.Value = (int)UpDownTriggerDelayAbs.Value;
            if (settingLocck)
            {
                return;
            }
            camera1.TriggerDelayAbs = (double)UpDownTriggerDelayAbs.Value;

        }

        private void UpDownLineDebouncerTimeAbs_ValueChanged(object sender, EventArgs e)
        {
            trackBarLineDebouncerTimeAbs.Value = (int)UpDownLineDebouncerTimeAbs.Value;
            if (settingLocck)
            {
                return;
            }
            camera1.LineDebouncerTimeAbs = (double)UpDownLineDebouncerTimeAbs.Value;
        }

        private void UpDownOutLineTime_ValueChanged(object sender, EventArgs e)
        {
            trackBarOutLineTime.Value = (int)UpDownOutLineTime.Value;
            if (settingLocck)
            {
                return;
            }
            camera1.OutLineTime = (double)UpDownOutLineTime.Value;
        }
        private void btnResetShutter_Click(object sender, EventArgs e)
        {
            UpDownShutter.Value = (decimal)(camera1.ShotInitValue);
            UpDownShutter.Refresh();
        }

        private void btnResetGain_Click(object sender, EventArgs e)
        {
            UpDownGain.Value = (decimal)camera1.GainInitValue;
            UpDownGain.Refresh();
        }



        private void btnResetTriggerDelayAbs_Click(object sender, EventArgs e)
        {
            UpDownTriggerDelayAbs.Value = (decimal)camera1.TriggerDelayAbsInit;
            UpDownTriggerDelayAbs.Refresh();
        }

        private void btnResetLineDebouncerTimeAbs_Click(object sender, EventArgs e)
        {
            UpDownLineDebouncerTimeAbs.Value = (decimal)camera1.LineDebouncerTimeAbsInit;
            UpDownLineDebouncerTimeAbs.Refresh();


        }
        private void btnResetOutLineTime_Click(object sender, EventArgs e)
        {
            UpDownOutLineTime.Value = 1000;
            UpDownOutLineTime.Refresh();
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void btnTestOut_Click(object sender, EventArgs e)
        {
            camera1.Output();
        }

        private void ImageAngleComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (settingLocck)
            {
                return;
            }
            camera1.ImageAngle = (ImageAngle)Enum.Parse(typeof(ImageAngle),
                    ImageAngleComboBox.SelectedItem.ToString(), false);
        }
    }
}
