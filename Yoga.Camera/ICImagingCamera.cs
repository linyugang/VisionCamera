using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TIS.Imaging;
using TIS.Imaging.VCDHelpers;
using Yoga.Common;

namespace Yoga.Camera
{
    public class ICImagingCamera : CameraBase
    {
        ICImagingControl camera;
        private VCDAbsoluteValueProperty gainAbsoluteValue;
        private VCDAbsoluteValueProperty exposureAbsoluteValue;
        private VCDAbsoluteValueProperty triggerDelayTime;
        private VCDAbsoluteValueProperty triggerDebounceTime;
        private VCDSwitchProperty trigEnableSwitch;//触发使能开关
        private VCDButtonProperty softtrigger;

        private VCDSimpleProperty VCDProp;
        private bool ignoreImage = false;
        /// <summary>
        /// 曝光缩放比例 相机默认为s?转换为us
        /// </summary>
        private const long exposureSocle = 1000000;
        public ICImagingCamera(ICImagingControl camera, int index)
        {
            this.camera = camera;
            this.cameraIndex = index;

            VCDProp = VCDSimpleModule.GetSimplePropertyContainer(camera.VCDPropertyItems);
        }
        public override double GainCur
        {
            get
            {
                return gainCur;
            }

            set
            {
                try
                {
                    double gainValue = value;

                    //判断输入值是否在增益值的范围内
                    //若输入的值大于最大值则将增益值设置成最大值
                    if (gainValue > GainMax)
                    {
                        gainValue = GainMax;
                    }

                    //若输入的值小于最小值则将增益的值设置成最小值
                    if (gainValue < GainMin)
                    {
                        gainValue = GainMin;
                    }
                    GainAbsoluteValue.Value = gainValue;

                    gainCur = gainValue;
                }
                catch (Exception ex)
                {
                    Util.WriteLog(this.GetType(), ex);
                    Util.Notify("相机增益设置异常");
                }
            }
        }

        public override long ShuterCur
        {
            get
            {
                return shuterCur;
            }

            set
            {
                try
                {
                    long shutterValue = value;

                    if (shutterValue > ShuterMax)
                    {
                        shutterValue = ShuterMax;
                    }
                    //若小于最小值将曝光值设为最小值
                    else if (shutterValue < ShuterMin)
                    {
                        shutterValue = ShuterMin;
                    }

                    double dat = shutterValue / (exposureSocle * 1.0);
                    ExposureAbsoluteValue.Value = dat;

                    shuterCur = shutterValue;
                }
                catch (Exception ex)
                {
                    Util.WriteLog(this.GetType(), ex);
                    Util.Notify("相机曝光设置异常");
                }
            }
        }

        #region 相机驱动对应属性
        protected VCDAbsoluteValueProperty GainAbsoluteValue
        {
            get
            {

                if (gainAbsoluteValue == null)
                {
                    gainAbsoluteValue = (VCDAbsoluteValueProperty)camera.VCDPropertyItems.FindInterface(
                    VCDIDs.VCDID_Gain + ":" +
                    VCDIDs.VCDElement_Value + ":" +
                    VCDIDs.VCDInterface_AbsoluteValue);
                }
                return gainAbsoluteValue;
            }
        }

        protected VCDAbsoluteValueProperty ExposureAbsoluteValue
        {
            get
            {
                if (exposureAbsoluteValue == null)
                {
                    exposureAbsoluteValue = (VCDAbsoluteValueProperty)camera.VCDPropertyItems.FindInterface(
                                       VCDIDs.VCDID_Exposure + ":" +
                                       VCDIDs.VCDElement_Value + ":" +
                                       VCDIDs.VCDInterface_AbsoluteValue);
                }
                return exposureAbsoluteValue;
            }
        }

        protected VCDAbsoluteValueProperty TriggerDelayTime
        {
            get
            {
                if (triggerDelayTime == null)
                {
                    triggerDelayTime = (VCDAbsoluteValueProperty)camera.VCDPropertyItems.FindInterface(
                    VCDIDs.VCDID_TriggerMode + ":" +
                    VCDIDs.VCDElement_TriggerDelay + ":" +
                    VCDIDs.VCDInterface_AbsoluteValue);
                }
                return triggerDelayTime;
            }

        }

        protected VCDAbsoluteValueProperty TriggerDebounceTime
        {
            get
            {
                if (triggerDebounceTime == null)
                {
                    triggerDebounceTime = (VCDAbsoluteValueProperty)camera.VCDPropertyItems.FindInterface(
                    VCDIDs.VCDID_TriggerMode + ":" +
                    VCDIDs.VCDElement_TriggerDebounceTime + ":" +
                    VCDIDs.VCDInterface_AbsoluteValue);
                }
                return triggerDebounceTime;
            }

        }

        protected VCDSwitchProperty TrigEnableSwitch
        {
            get
            {
                if (trigEnableSwitch == null)
                {
                    trigEnableSwitch = (VCDSwitchProperty)camera.VCDPropertyItems.FindInterface(
                        VCDIDs.VCDID_TriggerMode + ":" +
                        VCDIDs.VCDElement_Value + ":" +
                        VCDIDs.VCDInterface_Switch);

                }
                return trigEnableSwitch;
            }
        }

        protected VCDButtonProperty Softtrigger
        {
            get
            {
                if (softtrigger == null)
                {
                    softtrigger = (VCDButtonProperty)camera.VCDPropertyItems.FindInterface(
                        VCDIDs.VCDID_TriggerMode + ":" +
                        VCDIDs.VCDElement_SoftwareTrigger + ":" +
                        VCDIDs.VCDInterface_Button);

                }
                return softtrigger;
            }

        }
        #endregion

        public override void Close()
        {
            try
            {
                IsLink = false;
                // Reset the stopwatch.
                //stopWatch.Reset();
                if (camera != null && camera.DeviceValid)
                {
                    camera.LiveStop();
                    camera.Dispose();
                }
            }
            catch (Exception ex)
            {
                Util.WriteLog(this.GetType(), ex);
                Util.Notify("相机关闭异常");
            }

        }

        public override void ContinuousShot()
        {
            if (camera == null || camera.DeviceValid == false)
            {
                return;
            }
            try
            {
                Command = Command.Video;
                TrigEnableSwitch.Switch = false;
                IsContinuousShot = true;
            }
            catch (Exception ex)
            {
                Util.WriteLog(this.GetType(), ex);
                Util.Notify("相机连续采集开始异常");
            }
        }

        public override void ContinuousShotStop()
        {
            try
            {
                // Set an enum parameter.
                if (camera == null || camera.DeviceValid == false)
                {
                    return;
                }
                TrigEnableSwitch.Switch = true;

                IsContinuousShot = false;
                IsExtTrigger = false;
                //OneShot(Command.Test);
                Task.Run(() =>
                {
                    ignoreImage = true;
                    Thread.Sleep(1000);
                    ignoreImage = false;
                });
            }
            catch (Exception ex)
            {
                Util.WriteLog(this.GetType(), ex);
                Util.Notify("相机连续采集停止异常");
            }
        }

        public override void OneShot(Command command)
        {
            try
            {
                if (IsContinuousShot || IsExtTrigger)
                {
                    ContinuousShotStop();
                }
                Command = command;
                Softtrigger.Push();
            }
            catch
            {
                IsLink = false;
                Util.Notify("相机软触发异常");
            }
        }

        public override bool Open()
        {
            try
            {

                string name = camera.Device.Substring(0, 3);
                if (name == "DMK")  //DMK为黑白相机标识 DFK则为彩色相机 当前未测试
                {
                    camera.MemoryCurrentGrabberColorformat = ICImagingControlColorformats.ICY8;
                }
                else
                {
                    camera.MemoryCurrentGrabberColorformat = ICImagingControlColorformats.ICRGB32;
                }

                ContinuousShotStop();//设置为软触发模式
                //设置帧率为最大帧率模式
                camera.DeviceFrameRate = camera.DeviceFrameRates.Max();
                camera.LiveDisplay = false;
                camera.LiveCaptureContinuous = true; // ImageAvailable


                camera.ImageAvailable += new System.EventHandler<TIS.Imaging.ICImagingControl.ImageAvailableEventArgs>(TIS_ImageAvailable);
                GetCameraSettingData();
                camera.LiveStart();


                IsLink = true;
                //}
                //else
                //{
                //    Util.Notify("无相机连接");
                //    return false;
                //}
            }
            catch (Exception ex)
            {
                Util.WriteLog(this.GetType(), ex);
                Util.Notify("相机打开出现异常");
                throw ex;
            }
            return true;


        }

        private void TIS_ImageAvailable(object sender, ICImagingControl.ImageAvailableEventArgs e)
        {
            try
            {
                //Util.Notify(string.Format("相机{0}收到图像", cameraIndex));
                if (ignoreImage)
                {
                    return;
                }

                //HTuple startTime;
                HOperatorSet.CountSeconds(out startTime);
                // Acquire the image from the camera. Only show the latest image. The camera may acquire images faster than the images can be displayed.

                ImageBuffer ImgBuffer = e.ImageBuffer;
                // Reduce the number of displayed images to a reasonable amount if the camera is acquiring images very fast.
                //if (!stopWatch.IsRunning || stopWatch.ElapsedMilliseconds > 33)
                {
                    //stopWatch.Restart();
                    //if (hPylonImage != null && hPylonImage.IsInitialized())
                    //{
                    //    hPylonImage.Dispose();
                    //}
                    hPylonImage = new HImage();

                    if (ImgBuffer.GetIntPtr() == IntPtr.Zero)
                    {
                        Util.Notify(string.Format("相机{0}数据损坏,采集失败", cameraIndex));
                        return;
                    }

                    if (camera.MemoryCurrentGrabberColorformat == ICImagingControlColorformats.ICY8)
                    {



                        hPylonImage.GenImage1("byte", ImgBuffer.Size.Width, ImgBuffer.Size.Height, ImgBuffer.GetImageDataPtr());
                        HImage imgTmp = hPylonImage.MirrorImage("row");
                        hPylonImage.Dispose();
                        hPylonImage = imgTmp;

                    }
                    else if (camera.MemoryCurrentGrabberColorformat == ICImagingControlColorformats.ICRGB32)
                    {
                        //allocate the m_stream_size amount of bytes in non-managed environment 
                        hPylonImage.GenImageInterleaved(ImgBuffer.GetImageDataPtr(), "rgb",
                                 ImgBuffer.Size.Width, ImgBuffer.Size.Height, -1, "byte", ImgBuffer.Size.Width, ImgBuffer.Size.Height, 0, 0, -1, 0);

                    }
                    else
                    {
                        Util.Notify(string.Format("相机{0}编码格式不正确", cameraIndex));
                    }
                    TrigerImageEvent();
                }
            }
            catch (System.ArgumentException ex)
            {
                Util.WriteLog(this.GetType(), ex);
                Util.Notify(string.Format("相机{0}图像数据包丢失", cameraIndex));
            }
            catch (Exception ex)
            {
                Util.WriteLog(this.GetType(), ex);
                Util.Notify(string.Format("相机{0}图像数据返回出现异常", cameraIndex));
            }
        }
        private object lockObj1 = new object();

        private bool inOutPut = false;
        public override void Output()
        {

            if (inOutPut)
            {
                Util.Notify("报警输出中当前输出忽略");
                return;
            }
            //
            Task.Run(() =>
            {
                lock (lockObj1)
                {
                    inOutPut = true;

                    VCDProp.RangeValue[VCDIDs.VCDElement_GPIOOut] = 1;
                    // Now write it into the video capture device.
                    VCDProp.OnePush(VCDIDs.VCDElement_GPIOWrite);
                    Thread.Sleep((int)outLineTime);
                    VCDProp.RangeValue[VCDIDs.VCDElement_GPIOOut] = 0;
                    // Now write it into the video capture device.
                    VCDProp.OnePush(VCDIDs.VCDElement_GPIOWrite);
                    Util.Notify("相机报警输出完成");
                    inOutPut = false;
                }
            });
        }

        public override void SetExtTrigger()
        {
            try
            {
                if (IsContinuousShot || IsExtTrigger)
                {
                    ContinuousShotStop();
                }
                TriggerDelayTime.Value = TriggerDelayAbs;
                TriggerDebounceTime.Value = LineDebouncerTimeAbs;
                Command = Command.ExtTrigger;
                IsExtTrigger = true;
            }
            catch (Exception ex)
            {
                Util.WriteLog(this.GetType(), ex);
                Util.Notify("相机外触发设置异常");
            }
        }

        protected override void GetCameraSettingData()
        {
            try
            {
                //关闭自动曝光
                VCDSwitchProperty exposureAuto = (VCDSwitchProperty)camera.VCDPropertyItems.FindInterface(
                    VCDIDs.VCDID_Exposure + ":" +
                    VCDIDs.VCDElement_Auto + ":" +
                    VCDIDs.VCDInterface_Switch);
                exposureAuto.Switch = false;

                //关闭自动增益
                VCDSwitchProperty gainSwith = (VCDSwitchProperty)camera.VCDPropertyItems.FindInterface(
                    VCDIDs.VCDID_Gain + ":" +
                    VCDIDs.VCDElement_Auto + ":" +
                    VCDIDs.VCDInterface_Switch);
                gainSwith.Switch = false;


                //long max, min, cur;
                gainMin = GainAbsoluteValue.RangeMin;
                gainMax = GainAbsoluteValue.RangeMax;
                gainCur = GainAbsoluteValue.Value;
                gainUnit = "";

                shuterUnit = "us";


                shuterMin = (long)ExposureAbsoluteValue.RangeMin * exposureSocle;
                shuterMax = (long)ExposureAbsoluteValue.RangeMax * exposureSocle;
                shuterCur = (long)ExposureAbsoluteValue.Value * exposureSocle;


                triggerDelayAbsMin = TriggerDelayTime.RangeMin;
                triggerDelayAbsMax = TriggerDelayTime.RangeMax;
                triggerDelayAbs = TriggerDelayTime.Value;

                lineDebouncerTimeAbsMin = TriggerDebounceTime.RangeMin;
                lineDebouncerTimeAbsMax = TriggerDebounceTime.RangeMax;
                lineDebouncerTimeAbs = TriggerDebounceTime.Value;

            }
            catch (Exception ex)
            {
                Util.WriteLog(this.GetType(), ex);
                Util.Notify("相机设置信息获取异常");
            }
        }
    }
}
