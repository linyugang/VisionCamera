using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MVGigE = MVAPI.MVGigE;
using MVPro = MVAPI.MVCamProptySheet;
using MVSTATUS = MVAPI.MVSTATUS_CODES;
using MVImage = MVAPI.MVImage;
using Yoga.Common;
using HalconDotNet;

namespace Yoga.Camera
{
   public class MicrovisionCamera : CameraBase
    {
        IntPtr m_hCam = IntPtr.Zero;

        MVAPI.MV_PixelFormatEnums m_PixelFormat;
        int m_nWidth;
        int m_nHeight;

        IntPtr m_hImage = IntPtr.Zero;

        MVAPI.MV_SNAPPROC StreamCBDelegate = null;


        private bool ignoreImage = false;

        public MicrovisionCamera(IntPtr m_hCam, int index)
        {
            this.m_hCam = m_hCam;
            this.cameraIndex = index;
        }

        int StreamCB(ref MVAPI.IMAGE_INFO pInfo, IntPtr UserVal)
        {
            MVGigE.MVInfo2Image(m_hCam, ref pInfo, m_hImage);
            try
            {
                if (ignoreImage)
                {
                    return 0;
                }

                //HTuple startTime;
                HOperatorSet.CountSeconds(out startTime);

                // Check if the image can be displayed.
                if (m_hImage!=IntPtr.Zero)
                {
                    // Reduce the number of displayed images to a reasonable amount if the camera is acquiring images very fast.
                    //if (!stopWatch.IsRunning || stopWatch.ElapsedMilliseconds > 33)
                    {
                        //stopWatch.Restart();

                        //if (hPylonImage != null && hPylonImage.IsInitialized())
                        //{
                        //    hPylonImage.Dispose();
                        //}
                        hPylonImage = new HImage();
                        if (m_PixelFormat == MVAPI.MV_PixelFormatEnums.PixelFormat_Mono8)
                        {
                            //Util.Notify(string.Format("相机{0}数据尺寸{1}", cameraIndex, grabResult.PayloadSize));
                            //allocate the m_stream_size amount of bytes in non-managed environment 

                            //转换为Halcon图像显示
                            hPylonImage.GenImage1("byte", m_nWidth, m_nHeight, m_hImage);


                        }
                        else if (m_PixelFormat == MVAPI.MV_PixelFormatEnums.PixelFormat_BayerRG8)
                        {

                            hPylonImage.GenImageInterleaved(m_hImage, "bgr",
                                     m_nWidth, m_nHeight, -1, "byte", m_nWidth, m_nHeight, 0, 0, -1, 0);

                        }
                        else
                        {
                            Util.Notify(string.Format("相机{0}编码格式不正确", cameraIndex));
                        }
                        TrigerImageEvent();
                    }
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
            return 0;
        }

        private void ImageCreat()
        {
            int w = 0, h = 0;

            MVSTATUS r = MVGigE.MVGetWidth(m_hCam, out w);
            if (r != MVSTATUS.MVST_SUCCESS)
            {
                Util.Notify("取得图像宽度失败");
                return;
            }

            r = MVGigE.MVGetHeight(m_hCam, out h);
            if (r != MVSTATUS.MVST_SUCCESS)
            {
                Util.Notify("取得图像高度失败");
                return;
            }
            r = MVGigE.MVGetPixelFormat(m_hCam, out m_PixelFormat);
            if (r != MVSTATUS.MVST_SUCCESS)
            {
                Util.Notify("取得图像颜色模式失败");
                return;
            }
            if (m_nWidth != w || m_nHeight != h)
            {
                m_nWidth = w;
                m_nHeight = h;

                if (m_hImage != IntPtr.Zero)
                {
                    MVAPI.MVImage.MVImageRelease(m_hImage);
                    m_hImage = IntPtr.Zero;
                }

                if (m_PixelFormat == MVAPI.MV_PixelFormatEnums.PixelFormat_Mono8)
                {
                    m_hImage = MVAPI.MVImage.MVImageCreate(w, h, 8);
                }
                else
                {
                    m_hImage = MVAPI.MVImage.MVImageCreate(w, h, 24);
                }
            }
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
                   MVGigE.MVSetGain(m_hCam, gainValue);

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
                    //else
                    //{
                    //    //shutterValue = ShuterMin + (((shutterValue - ShuterMin) / incr) * incr);
                    //}

                    MVGigE.MVSetExposureTime(m_hCam, shutterValue);
                    shuterCur = shutterValue;
                }
                catch (Exception ex)
                {
                    Util.WriteLog(this.GetType(), ex);
                    Util.Notify("相机曝光设置异常");
                }
            }
        }

        public override void Close()
        {
            try
            {
                IsLink = false;
                // Reset the stopwatch.
                //stopWatch.Reset();
                MVGigE.MVSetTriggerMode(m_hCam, MVAPI.TriggerModeEnums.TriggerMode_Off);
                MVGigE.MVCloseCam(m_hCam);
                m_hCam = IntPtr.Zero;
                MVGigE.MVTerminateLib();
                MVAPI.MVImage.MVImageRelease(m_hImage);
            }
            catch (Exception ex)
            {
                Util.WriteLog(this.GetType(), ex);
                Util.Notify("相机关闭异常");
            }
        }

        public override void ContinuousShot()
        {
            if (m_hCam == IntPtr.Zero)
            {
                return;
            }
            try
            {
                Command = Command.Video;
                MVGigE.MVSetTriggerMode(m_hCam, MVAPI.TriggerModeEnums.TriggerMode_On);
                MVGigE.MVStartGrab(m_hCam, StreamCBDelegate, IntPtr.Zero);
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
                if (m_hCam==IntPtr.Zero)
                {
                    return;
                }
                MVGigE.MVStopGrab(m_hCam);
                IsContinuousShot = false;
                IsExtTrigger = false;
                Task.Run(() =>
                {
                    ignoreImage = true;
                    System.Threading.Thread.Sleep(1000);
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
                MVGigE.MVSetTriggerMode(m_hCam, MVAPI.TriggerModeEnums.TriggerMode_Off);
                MVGigE.MVStartGrab(m_hCam, StreamCBDelegate, IntPtr.Zero);
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

                ImageCreat();

                

                ContinuousShotStop();//设置为软触发模式

                MVGigE.MVSetStrobeSource(m_hCam, MVAPI.LineSourceEnums.LineSource_ExposureActive);


                StreamCBDelegate += new MVAPI.MV_SNAPPROC(StreamCB);
                MVGigE.MVStartGrab(m_hCam, StreamCBDelegate, IntPtr.Zero);
                GetCameraSettingData();

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

        public override void Output()
        {
            throw new NotImplementedException();
        }

        public override void SetExtTrigger()
        {
            throw new NotImplementedException();
        }

        protected override void GetCameraSettingData()
        {

            try
            {
                //long max, min, cur;

                MVGigE.MVGetGainRange(m_hCam, out gainMin, out gainMax);
                MVGigE.MVGetGain(m_hCam, out gainCur);
                gainUnit = "";

                shuterUnit = "us";


                double pExpMin;
                double pExpMax;
                MVGigE.MVGetExposureTimeRange(m_hCam, out pExpMin, out pExpMax);

                double pExposuretime;
                MVGigE.MVGetExposureTime(m_hCam, out pExposuretime);
                shuterMin = (long)pExpMin;
                shuterMax = (long)pExpMax;
                shuterCur = (long)pExposuretime;

                uint pMin;
                uint pMax;
                MVGigE.MVGetTriggerDelayRange(m_hCam, out pMin, out pMax);
                triggerDelayAbsMin = pMin;
                triggerDelayAbsMax = pMax;

                uint pDelay_us;
                MVGigE.MVGetTriggerDelay(m_hCam, out pDelay_us);
                triggerDelayAbs = pDelay_us;

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
