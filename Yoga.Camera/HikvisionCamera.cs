using DeviceSource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yoga.Common;
using System.Diagnostics;
using MvCamCtrl.NET;
using System.Threading;
using HalconDotNet;

namespace Yoga.Camera
{
    public class HikvisionCamera : CameraBase
    {
        #region 字段
        private bool ignoreImage = false;

        CameraOperator m_pOperator;
        MyCamera.cbOutputdelegate ImageCallback;
        private object lockobj1 = new object();
        private object lockobj2 = new object();
        #endregion
        #region 属性设置
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

                    m_pOperator.SetEnumValue("GainAuto", 0);
                    int nRet;
                    nRet = m_pOperator.SetFloatValue("Gain", (float)gainValue);

                    if (nRet != CameraOperator.CO_OK)
                    {
                        throw new Exception("设置曝光时间失败！");
                    }
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

                    m_pOperator.SetEnumValue("ExposureAuto", 0);
                    int nRet;
                    nRet = m_pOperator.SetFloatValue("ExposureTime", shutterValue);
                    if (nRet != CameraOperator.CO_OK)
                    {
                        throw new Exception("设置曝光时间失败！");
                    }
                    shuterCur = shutterValue;
                }
                catch (Exception ex)
                {
                    Util.WriteLog(this.GetType(), ex);
                    Util.Notify("相机曝光设置异常");
                }
            }
        }


        #endregion
        public HikvisionCamera(CameraOperator m_pOperator, int index)
        {
            this.m_pOperator = m_pOperator;
            this.cameraIndex = index;
        }
        public override void Close()
        {
            try
            {
                IsLink = false;
                if (m_pOperator != null)
                {
                    //停止采集
                    m_pOperator.StopGrabbing();
                    //关闭设备
                    m_pOperator.Close();



                    m_pOperator = null;
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
            if (m_pOperator == null || IsLink == false)
            {
                return;
            }
            try
            {
                Command = Command.Video;
                m_pOperator.SetEnumValue("TriggerMode", 0);
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
                if (m_pOperator == null)
                {
                    return;
                }
                // m_pOperator.SetEnumValue("AcquisitionMode", 2);
                m_pOperator.SetEnumValue("TriggerMode", 1);
                //触发源选择:0 - Line0;
                //           1 - Line1;
                //           2 - Line2;
                //           3 - Line3;
                //           4 - Counter;
                //           7 - Software;
                m_pOperator.SetEnumValue("TriggerSource", 7);
                IsContinuousShot = false;
                IsExtTrigger = false;
                ignoreImage = true;
                Task.Run(() =>
                {
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
                Command =command;
                int nRet;

                //触发命令
                nRet = m_pOperator.CommandExecute("TriggerSoftware");
                if (CameraOperator.CO_OK != nRet)
                {
                    Util.Notify("相机软触发异常");
                }
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
                int nRet;


                //Util.Notify("相机开始打开");

                uint pixelFormat = 0;
                nRet = m_pOperator.GetEnumValue("PixelFormat", ref pixelFormat);
                if (MyCamera.MV_OK != nRet)
                {
                    throw new Exception("图像格式获取错误");
                }
                MyCamera.MvGvspPixelType imgType = (MyCamera.MvGvspPixelType)pixelFormat;
                if (imgType == MyCamera.MvGvspPixelType.PixelType_Gvsp_YUV411_Packed ||
                   imgType == MyCamera.MvGvspPixelType.PixelType_Gvsp_YUV422_Packed ||
                   imgType == MyCamera.MvGvspPixelType.PixelType_Gvsp_YUV422_YUYV_Packed ||
                   imgType == MyCamera.MvGvspPixelType.PixelType_Gvsp_YUV444_Packed)
                {
                    int result = m_pOperator.SetEnumValue("PixelFormat", (uint)MyCamera.MvGvspPixelType.PixelType_Gvsp_RGB8_Packed);
                    if (MyCamera.MV_OK != result)
                    {
                        throw new Exception("图像格式设置错误");
                    }
                }

                ////设置采集连续模式
                //nRet = m_pOperator.SetEnumValue("AcquisitionMode", 2);
                //if (MyCamera.MV_OK != nRet)
                //{
                //    throw new Exception("采集模式设置失败");

                //}

                //nRet = m_pOperator.SetEnumValue("TriggerMode", 0);
                //if (MyCamera.MV_OK != nRet)
                //{
                //    throw new Exception("触发模式设置失败");

                //}
                ImageCallback = new MyCamera.cbOutputdelegate(SaveImage);
                nRet = m_pOperator.RegisterImageCallBack(ImageCallback, IntPtr.Zero);
                if (MyCamera.MV_OK != nRet)
                {
                    throw new Exception("回调函数注册失败");

                }


                ContinuousShotStop();
                //开始采集
                nRet = m_pOperator.StartGrabbing();
                if (MyCamera.MV_OK != nRet)
                {
                    throw new Exception("开始采集失败");

                }

               

                // Reset the stopwatch used to reduce the amount of displayed images. The camera may acquire images faster than the images can be displayed
                //stopWatch.Reset();

                GetCameraSettingData();

                IsLink = true;

                //Thread.Sleep(500);
                //nRet = m_pOperator.SetEnumValue("TriggerMode", 1);
                //if (MyCamera.MV_OK != nRet)
                //{
                //    throw new Exception("触发模式设置失败");

                //}
                //Thread.Sleep(500);
                //nRet = m_pOperator.SetEnumValue("TriggerSource", 0);

                //if (MyCamera.MV_OK != nRet)
                //{
                //    throw new Exception("触发源设置失败");

                //}

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
                Util.Notify("相机打开出现异常:" + ex.Message);
                throw ex;
            }
            return true;
        }

        private void SaveImage(IntPtr pData, ref MyCamera.MV_FRAME_OUT_INFO pFrameInfo, IntPtr pUser)
        {
            try
            {
                if (ignoreImage)
                {
                    return;
                }

                //HTuple startTime;
                HOperatorSet.CountSeconds(out startTime);

                // Reduce the number of displayed images to a reasonable amount if the camera is acquiring images very fast.
                //if (!stopWatch.IsRunning || stopWatch.ElapsedMilliseconds > 10)
                {
                    //stopWatch.Restart();

                    //if (hPylonImage != null && hPylonImage.IsInitialized())
                    //{
                    //    hPylonImage.Dispose();
                    //}
                    hPylonImage = new HImage();
                    if (pFrameInfo.enPixelType == MyCamera.MvGvspPixelType.PixelType_Gvsp_Mono8)
                    {
                        hPylonImage.GenImage1("byte", pFrameInfo.nWidth, pFrameInfo.nHeight, pData);
                    }
                    else if (pFrameInfo.enPixelType == MyCamera.MvGvspPixelType.PixelType_Gvsp_RGB8_Packed)
                    {
                        hPylonImage.GenImageInterleaved(pData,
                        "rgb",
                         pFrameInfo.nWidth, pFrameInfo.nHeight,
                        -1, "byte",
                        pFrameInfo.nWidth, pFrameInfo.nHeight,
                        0, 0, -1, 0);
                    }
                    else
                    {
                        Util.Notify(string.Format("相机{0}编码格式不正确,当前格式{1}", cameraIndex, pFrameInfo.enPixelType));
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

        protected override void GetCameraSettingData()
        {
            try
            {
                //long max, min, cur;

                float fGain = 0;
                m_pOperator.GetFloatValue("Gain", ref fGain);

                gainMin = 0;
                gainMax = 17;
                gainCur = fGain;
                gainUnit = "db";

                shuterUnit = "us";

                float fExposure = 0;
                m_pOperator.GetFloatValue("ExposureTime", ref fExposure);
                shuterMin = 20;
                shuterMax = 1000000;
                shuterCur = (long)fExposure;


                float fTriggerDelay = 0;
                m_pOperator.GetFloatValue("TriggerDelay", ref fTriggerDelay);
                triggerDelayAbsMin = 0;
                triggerDelayAbsMax = 1000;
                triggerDelayAbs = fTriggerDelay;

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

        public override void SetExtTrigger()
        {
            try
            {
                //int ret = 1;
                // ret= m_pOperator.SetEnumValue("TriggerSelector", 0);//0 ：AcquisitionStart
                //// if (ret==0)
                // {
                //     ret = m_pOperator.SetEnumValue("TriggerSelector", 3);//3：FrameStart
                //     if (ret==0)
                //     {
                //         m_pOperator.SetEnumValue("TriggerSelector", 0);//0 ：AcquisitionStart
                //         m_pOperator.SetEnumValue("TriggerMode", 0);//0：Off 

                //         m_pOperator.SetEnumValue("TriggerSelector", 3);//3：FrameStart
                //         m_pOperator.SetEnumValue("TriggerMode", 1);//1：On

                //         //触发源选择:0 - Line0;
                //         //           1 - Line1;
                //         //           2 - Line2;
                //         //           3 - Line3;
                //         //           4 - Counter;
                //         //           7 - Software;
                //         m_pOperator.SetEnumValue("TriggerSource", 1);

                //     }
                //     else
                //     {
                //       //  m_pOperator.SetEnumValue("TriggerSelector", 0);//0 ：AcquisitionStart

                //         m_pOperator.SetEnumValue("TriggerMode", 1);//1：On
                //         m_pOperator.SetEnumValue("TriggerSource", 1);
                //        // m_pOperator.StartGrabbing();


                //     }
                // }
                m_pOperator.SetEnumValue("TriggerMode", 1);
                //触发源选择:0 - Line0;
                //           1 - Line1;
                //           2 - Line2;
                //           3 - Line3;
                //           4 - Counter;
                //           7 - Software;
                m_pOperator.SetEnumValue("TriggerSource", 0);

                //m_pOperator.SetEnumValue("TriggerMode", 1);
                //m_pOperator.SetEnumValue("TriggerSource", 1);


                m_pOperator.SetEnumValue("ExposureAuto", 0);

                m_pOperator.SetFloatValue("TriggerDelay", (float)triggerDelayAbs);
                m_pOperator.SetIntValue("LineDebouncerTime", (uint)LineDebouncerTimeAbs);
                Command = Command.ExtTrigger;
                IsExtTrigger = true;
            }
            catch (Exception ex)
            {
                Util.WriteLog(this.GetType(), ex);
                Util.Notify("相机外触发设置异常");
            }
        }
        /// <summary>
        /// 信号输出,已经在task中运行
        /// </summary>
        public override void Output()
        {
            if (IOSerial.Instance.Rs232Param.Use == false)
            {
                Task.Run(() =>
                {
                    lock (lockobj1)
                    {
                        int nRet;
                        nRet = m_pOperator.SetEnumValue("LineSelector", 1);
                        if (nRet != CameraOperator.CO_OK)
                        {
                            Util.Notify("LineSelector异常");
                        }
                        nRet = m_pOperator.SetEnumValue("LineMode", 8);
                        if (nRet != CameraOperator.CO_OK)
                        {
                            Util.Notify("LineMode异常");
                        }
                        nRet = m_pOperator.SetBoolValue("LineInverter", true);
                        if (nRet != CameraOperator.CO_OK)
                        {
                            Util.Notify("LineInverter异常");
                        }
                        Thread.Sleep((int)outLineTime);
                        nRet = m_pOperator.SetBoolValue("LineInverter", false);
                        if (nRet != CameraOperator.CO_OK)
                        {
                            Util.Notify("LineInverter异常");
                        }
                        // g_camera.Parameters[PLCamera.LineSelector].TrySetValue(PLCamera.LineSelector.Line1);
                        //m_pOperator.SetEnumValue("LineSelector", 1);
                        //g_camera.Parameters[PLCamera.LineSource].TrySetValue(PLCamera.LineSource.UserOutput);
                        //g_camera.Parameters[PLCamera.UserOutputValue].TrySetValue(true);
                        //Thread.Sleep((int)outLineTime);
                        //g_camera.Parameters[PLCamera.UserOutputValue].TrySetValue(false);
                        //g_camera.Parameters[PLCamera.timer].TrySetValue(PLCamera.LineSource.Timer1Active.);
                        Util.Notify("海康相机报警输出完成");
                    }
                });
            }

            else
            {
                Task.Run(() =>
                {
                    lock (lockobj2)
                    {
                        try
                        {
                            IOSerial.Instance.WriteDataToSerial("#sw1n");
                            IOSerial.Instance.WriteDataToSerial("#sw2n");
                            IOSerial.Instance.WriteDataToSerial("#sw3n");
                            Thread.Sleep((int)outLineTime);
                            IOSerial.Instance.WriteDataToSerial("#sw1f");
                            IOSerial.Instance.WriteDataToSerial("#sw2f");
                            IOSerial.Instance.WriteDataToSerial("#sw3f");

                            Util.Notify("串口io报警输出完成");
                        }
                        catch (Exception ex)
                        {
                            Util.WriteLog(this.GetType(), ex);
                            Util.Notify("串口io通信失败");
                        }
                    }
                });
            }
        }
    }
}
