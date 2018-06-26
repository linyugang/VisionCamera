using HalconDotNet;
using MVSDK;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Yoga.Common;
using CameraHandle = System.Int32;

namespace Yoga.Camera
{
    /// <summary>
    /// 迈徳威视相机
    /// </summary>
    public class MindCamera : CameraBase
    {
        IntPtr m_Grabber;
        CameraHandle m_hCamera;
        private bool ignoreImage = false;
        //private Stopwatch stopWatch = new Stopwatch();
        protected pfnCameraGrabberFrameListener m_FrameListener;
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
                    // Some camera models may have auto functions enabled. To set the gain value to a specific value,
                    // the Gain Auto function must be disabled first (if gain auto is available).
                    //g_camera.Parameters[PLCamera.GainAuto].TrySetValue(PLCamera.GainAuto.Off); // Set GainAuto to Off if it is writable.

                    MvApi.CameraSetAnalogGain(m_hCamera, (int)gainValue);

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

                    //获取当前相机的曝光值、最小值、最大值和单位

                    //判断输入值是否在曝光时间的范围内
                    //若大于最大值则将曝光值设为最大值


                    if (shutterValue > ShuterMax)
                    {
                        shutterValue = ShuterMax;
                    }
                    //若小于最小值将曝光值设为最小值
                    else if (shutterValue < ShuterMin)
                    {
                        shutterValue = ShuterMin;
                    }

                    MvApi.CameraSetExposureTime(m_hCamera, shutterValue);
                    shuterCur = shutterValue;
                }
                catch (Exception ex)
                {
                    Util.WriteLog(this.GetType(), ex);
                    Util.Notify("相机曝光设置异常");
                }
            }
        }


        public MindCamera(CameraHandle m_hCamera, IntPtr m_Grabber, int index)
        {
            this.m_hCamera = m_hCamera;
            this.m_Grabber = m_Grabber;
            this.cameraIndex = index;

        }

        private int CameraGrabberFrameListener(IntPtr Grabber, int Phase, IntPtr pFrameBuffer, ref tSdkFrameHead pFrameHead, IntPtr Context)
        {
            if (Phase == 0)
            {
                // RAW数据处理，pFrameBuffer=Raw数据
            }
            else if (Phase == 1)
            {
                // 截图前处理，pFrameBuffer=RGB数据
            }
            else if (Phase == 2)
            {
                // 显示前处理，pFrameBuffer=RGB数据

                try
                {
                    if (ignoreImage)
                    {
                        return 1;
                    }
                    //HTuple startTime;
                    HOperatorSet.CountSeconds(out startTime);
                    // Reduce the number of displayed images to a reasonable amount if the camera is acquiring images very fast.
                    //if (!stopWatch.IsRunning || stopWatch.ElapsedMilliseconds > 33)
                    {
                        //stopWatch.Restart();

                        //if (hPylonImage != null && hPylonImage.IsInitialized())
                        //{
                        //    hPylonImage.Dispose();
                        //}
                        hPylonImage = new HImage();
                        emImageFormat currentFormat = (emImageFormat)pFrameHead.uiMediaType;
                        if (currentFormat == emImageFormat.CAMERA_MEDIA_TYPE_MONO8)
                        {
                            hPylonImage.GenImage1("byte", pFrameHead.iWidth, pFrameHead.iHeight, pFrameBuffer);
                        }
                        else if (currentFormat == emImageFormat.CAMERA_MEDIA_TYPE_RGB8)
                        {
                            hPylonImage.GenImageInterleaved(pFrameBuffer,
                            "rgb",
                              pFrameHead.iWidth, pFrameHead.iHeight,
                            -1, "byte",
                            pFrameHead.iWidth, pFrameHead.iHeight,
                            0, 0, -1, 0);
                        }
                        else
                        {
                            Util.Notify(string.Format("相机{0}编码格式不正确,当前格式{1}", cameraIndex, pFrameHead.uiMediaType));
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

            return 1;
        }

        public override void Close()
        {

            try
            {
                IsLink = false;

                // Reset the stopwatch.
                //stopWatch.Reset();
                if (m_Grabber != IntPtr.Zero)
                {
                    MvApi.CameraGrabber_Destroy(m_Grabber);
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
            if (m_Grabber == IntPtr.Zero)
            {
                return;
            }
            try
            {
                Command = Command.Video;
                MvApi.CameraSetTriggerMode(m_hCamera, (int)emSdkSnapMode.CONTINUATION);
                CameraSdkStatus suatus = MvApi.CameraGrabber_StartLive(m_Grabber);
                if (suatus != CameraSdkStatus.CAMERA_STATUS_SUCCESS)
                {
                    Util.Notify("相机连续采集开始异常");
                }
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
                if (m_Grabber == IntPtr.Zero)
                {
                    return;
                }
                CameraSdkStatus status;
                CameraSdkStatus st = MvApi.CameraSetExtTrigSignalType(m_hCamera, 1);//上升沿触发
                status = MvApi.CameraSetTriggerMode(m_hCamera, (int)emSdkSnapMode.SOFT_TRIGGER);
                IsContinuousShot = false;
                IsExtTrigger = false;
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
                CameraSdkStatus status;
                //status = MvApi.CameraSetTriggerMode(m_hCamera, (int)emSdkSnapMode.SOFT_TRIGGER);
                status = MvApi.CameraSoftTrigger(m_hCamera);
            }
            catch
            {
                IsLink = false;
                Util.Notify("相机软触发异常");
            }
        }
        public enum emCameraGPIOMode
        {
            IOMODE_TRIG_INPUT = 0,		//触发输入
            IOMODE_STROBE_OUTPUT,		//闪光灯输出
            IOMODE_GP_INPUT,			//通用型输入
            IOMODE_GP_OUTPUT,			//通用型输出
            IOMODE_PWM_OUTPUT,			//PWM型输出
        }
        public override bool Open()
        {
            try
            {
                m_FrameListener = new pfnCameraGrabberFrameListener(CameraGrabberFrameListener);
                //图像镜像设置
                MvApi.CameraSetMirror(m_hCamera, 0, 0);
                MvApi.CameraSetMirror(m_hCamera, 1, 0);
                MvApi.CameraGrabber_SetFrameListener(m_Grabber, m_FrameListener, IntPtr.Zero);
                ContinuousShotStop();
                GetCameraSettingData();
                MvApi.CameraGrabber_StartLive(m_Grabber);
                MvApi.CameraSetOutPutIOMode(m_hCamera, 0, (int)emCameraGPIOMode.IOMODE_GP_OUTPUT);
                MvApi.CameraSetIOState(m_hCamera, 0, 1);
                // Reset the stopwatch used to reduce the amount of displayed images. The camera may acquire images faster than the images can be displayed
                //stopWatch.Reset();
                IsLink = true;
            }
            catch (Exception ex)
            {
                Util.WriteLog(this.GetType(), ex);
                Util.Notify("相机打开出现异常");
                throw ex;
            }
            return true;
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

                    MvApi.CameraSetIOState(m_hCamera, 0, 0);
                    Thread.Sleep((int)outLineTime);
                    MvApi.CameraSetIOState(m_hCamera, 0, 1);
                    //g_camera.Parameters[PLCamera.timer].TrySetValue(PLCamera.LineSource.Timer1Active.);
                    Util.Notify("相机报警输出完成");
                    inOutPut = false;
                }
            });
        }

        public override void SetExtTrigger()
        {
            try
            {

                CameraSdkStatus st= MvApi.CameraSetExtTrigSignalType(m_hCamera, 1);//下降沿触发
                st=MvApi.CameraSetTriggerMode(m_hCamera, (int)emSdkSnapMode.EXTERNAL_TRIGGER);


                st = MvApi.CameraSetTriggerDelayTime(m_hCamera, (uint)triggerDelayAbs);

                //m_pOperator.SetEnumValue("ExposureAuto", 0);

                //m_pOperator.SetFloatValue("TriggerDelay", (float)triggerDelayAbs);

                st = MvApi.CameraSetExtTrigJitterTime(m_hCamera, (uint)LineDebouncerTimeAbs);
                //m_pOperator.SetIntValue("LineDebouncerTime", (uint)LineDebouncerTimeAbs);
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
                tSdkCameraCapbility cap;
                MvApi.CameraGetCapability(m_hCamera, out cap);

                if (cap.sIspCapacity.bMonoSensor == 1)
                {
                    MvApi.CameraSetIspOutFormat(m_hCamera, (uint)emImageFormat.CAMERA_MEDIA_TYPE_MONO8);
                }
                else
                {

                    MvApi.CameraSetIspOutFormat(m_hCamera, (uint)emImageFormat.CAMERA_MEDIA_TYPE_RGB8);
                }
                //long max, min, cur;
                gainMin = cap.sExposeDesc.uiAnalogGainMin;
                gainMax = cap.sExposeDesc.uiAnalogGainMax;
                int piAnalogGain = 0;
                MvApi.CameraGetAnalogGain(m_hCamera, ref piAnalogGain);

                gainCur = piAnalogGain;
                gainUnit = "";

                shuterUnit = "us";

                shuterMin = cap.sExposeDesc.uiExposeTimeMin;
                shuterMax = cap.sExposeDesc.uiExposeTimeMax;
                double pfLineTime = 0;
                MvApi.CameraGetExposureLineTime(m_hCamera, ref pfLineTime);

                shuterCur = (long)pfLineTime;


                triggerDelayAbsMin = 0;
                triggerDelayAbsMax = 1000000;
                uint puDelayTimeUs = 0;
                MvApi.CameraGetTriggerDelayTime(m_hCamera, ref puDelayTimeUs);

                triggerDelayAbs = puDelayTimeUs;

                lineDebouncerTimeAbsMin = 0;
                lineDebouncerTimeAbsMax = 5000;
                lineDebouncerTimeAbs = 0;

            }
            catch (Exception ex)
            {
                Util.WriteLog(this.GetType(), ex);
                Util.Notify("相机设置信息获取异常");
            }
        }
    }
}
