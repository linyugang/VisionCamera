using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Yoga.Common;

namespace Yoga.Camera
{
   public class DirectShowCamera : CameraBase
    {
        HFramegrabber framegrabber;
        AutoResetEvent threadRunSignal = new AutoResetEvent(false);

        //private bool ignoreImage = false;
        Thread runThread ;
        public DirectShowCamera(HFramegrabber framegrabber, int index)
        {
            this.framegrabber = framegrabber;
            this.cameraIndex = index;
        }
        /// <summary>
        /// 图像采集线程对应方法
        /// </summary>
        public void Run()
        {
            while (IsLink)
            {

                threadRunSignal.WaitOne();

                Util.Notify("开始连续采集图像");
                if (IsLink)
                {
                    while (IsContinuousShot)
                    {
                        GetImage();
                        if (hPylonImage!=null&& hPylonImage.IsInitialized())
                        {
                            TrigerImageEvent();
                        }
                    }

                }
            }
        }
        int reTryCount = 0;
        public override HImage GrabImage(int delayMs)
        {
            if (framegrabber == null || framegrabber.IsInitialized() == false)
            {
                Util.Notify("图像采集设备打开异常");
                return null;
            }
            GetImage();
            if (hPylonImage==null|| hPylonImage.IsInitialized()==false)
            {
                reTryCount++;
                if (reTryCount < 3)
                {
                    GrabImage(1);
                }
            }
            else
            {
                //帧率统计增加
                fps.IncreaseFrameNum();
            }  
            reTryCount = 0;
            return hPylonImage;
        }

        private void GetImage()
        {
            try
            {
                hPylonImage = null;
                HOperatorSet.CountSeconds(out startTime);
                hPylonImage = framegrabber.GrabImage();
                
            }
            catch(Exception ex)
            {
                Util.WriteLog(this.GetType(), ex);
                Util.Notify("图像采集发生异常,usb设备图像采集失败" );
            }
        }

        public override double GainCur
        {
            get
            {
                return -1;
            }

            set
            {
                ;
            }
        }



        public override long ShuterCur
        {
            get
            {
                return -1;
            }

            set
            {
                ;
            }
        }

        public override void Close()
        {
            try
            {
                IsLink = false;
                threadRunSignal.Set();
                // Reset the stopwatch.
                //stopWatch.Reset();
                if (framegrabber != null)
                {
                    framegrabber.Dispose();
                    framegrabber = null;
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
            if (framegrabber == null || framegrabber.IsInitialized() == false)
            {
                return;
            }
            try
            {
                Command = Command.Video;
                
                IsContinuousShot = true;
                threadRunSignal.Set();
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
                if (framegrabber == null || framegrabber.IsInitialized() == false)
                {
                    return;
                }

                IsContinuousShot = false;
                IsExtTrigger = false;
                //Task.Run(() =>
                //{
                //    ignoreImage = true;
                //    Thread.Sleep(1000);
                //    ignoreImage = false;
                //});
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
                // Execute the software trigger. Wait up to 1000 ms until the camera is ready for trigger.
                threadRunSignal.Set();
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
             
              
                //ContinuousShotStop();//设置为软触发模式


                // Reset the stopwatch used to reduce the amount of displayed images. The camera may acquire images faster than the images can be displayed
                //stopWatch.Reset();

                GetCameraSettingData();
                //usb相机第一次采集图像缓慢,采集一张图像不使用来提速
                GetImage();

                IsLink = true;
                runThread = new Thread(new ThreadStart(Run));
                runThread.IsBackground = true;
                runThread.Start();
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

        }

        public override void SetExtTrigger()
        {

        }

        protected override void GetCameraSettingData()
        {
            try
            {
                //long max, min, cur;
                //gainMin = g_camera.Parameters[PLCamera.GainRaw].GetMinimum();
                //gainMax = g_camera.Parameters[PLCamera.GainRaw].GetMaximum();
                //gainCur = g_camera.Parameters[PLCamera.GainRaw].GetValue();
                gainUnit = "";

                shuterUnit = "us";

                //shuterMin = g_camera.Parameters[PLCamera.ExposureTimeRaw].GetMinimum();
                //shuterMax = g_camera.Parameters[PLCamera.ExposureTimeRaw].GetMaximum();
                //shuterCur = g_camera.Parameters[PLCamera.ExposureTimeRaw].GetValue();

                //triggerDelayAbsMin = g_camera.Parameters[PLCamera.TriggerDelayAbs].GetMinimum();
                //triggerDelayAbsMax = g_camera.Parameters[PLCamera.TriggerDelayAbs].GetMaximum();
                triggerDelayAbs = -1;

                lineDebouncerTimeAbsMin = 0;
                lineDebouncerTimeAbsMax = 5000;
                lineDebouncerTimeAbs =-1;

            }
            catch (Exception ex)
            {
                Util.WriteLog(this.GetType(), ex);
                Util.Notify("相机设置信息获取异常");
            }
        }
    }
}
