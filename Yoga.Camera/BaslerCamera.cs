//using HalconDotNet;
//using System;
//using System.Runtime.InteropServices;
//using Basler.Pylon;
//using System.Diagnostics;
//using Yoga.Common;
//using System.Threading.Tasks;
//using System.Threading;

namespace Yoga.Camera
{
    //public class BaslerCamera : CameraBase
    //{
    //    #region 字段
    //    static Version Sfnc2_0_0 = new Version(2, 0, 0); // if  Sfnc2_0_0,说明是ＵＳＢ３的相机

    //    private Basler.Pylon.Camera g_camera = null;
    //    private PixelDataConverter g_converter = new PixelDataConverter();

    //    private int width = 0, height = 0;
    //    IntPtr m_latestFrameAddress;
    //    //private Stopwatch stopWatch = new Stopwatch();
    //    private bool ignoreImage = false;

    //    private object lockobj1 = new object();
    //    private object lockobj2 = new object();

    //    #endregion
    //    #region 属性设置
    //    public override double GainCur
    //    {
    //        get
    //        {
    //            return gainCur;
    //        }

    //        set
    //        {
    //            try
    //            {
    //                double gainValue = value;

    //                //判断输入值是否在增益值的范围内
    //                //若输入的值大于最大值则将增益值设置成最大值
    //                if (gainValue > GainMax)
    //                {
    //                    gainValue = GainMax;
    //                }

    //                //若输入的值小于最小值则将增益的值设置成最小值
    //                if (gainValue < GainMin)
    //                {
    //                    gainValue = GainMin;
    //                }
    //                // Some camera models may have auto functions enabled. To set the gain value to a specific value,
    //                // the Gain Auto function must be disabled first (if gain auto is available).
    //                g_camera.Parameters[PLCamera.GainAuto].TrySetValue(PLCamera.GainAuto.Off); // Set GainAuto to Off if it is writable.

    //                if (g_camera.GetSfncVersion() < Sfnc2_0_0)
    //                {
    //                    g_camera.Parameters[PLCamera.GainRaw].SetValue((long)gainValue);
    //                }
    //                else
    //                {
    //                    g_camera.Parameters[PLUsbCamera.Gain].SetValue((long)(gainValue * 8));
    //                }

    //                gainCur = gainValue;
    //            }
    //            catch (Exception ex)
    //            {
    //                Util.WriteLog(this.GetType(), ex);
    //                Util.Notify("相机增益设置异常");
    //            }
    //        }
    //    }

    //    public override long ShuterCur
    //    {
    //        get
    //        {
    //            return shuterCur;
    //        }

    //        set
    //        {
    //            try
    //            {
    //                long shutterValue = value;

    //                //获取当前相机的曝光值、最小值、最大值和单位

    //                //判断输入值是否在曝光时间的范围内
    //                //若大于最大值则将曝光值设为最大值

    //                long incr = g_camera.Parameters[PLCamera.ExposureTimeRaw].GetIncrement();
    //                if (shutterValue > ShuterMax)
    //                {
    //                    shutterValue = ShuterMax;
    //                }
    //                //若小于最小值将曝光值设为最小值
    //                else if (shutterValue < ShuterMin)
    //                {
    //                    shutterValue = ShuterMin;
    //                }
    //                else
    //                {
    //                    shutterValue = ShuterMin + (((shutterValue - ShuterMin) / incr) * incr);
    //                }

    //                // Some parameters have restrictions. You can use GetIncrement/GetMinimum/GetMaximum to make sure you set a valid value.                              
    //                // Or,here, we let pylon correct the value if needed.
    //                //long value = 30;
    //                //g_camera.Parameters[PLCamera.ExposureTimeRaw].SetValue(value, IntegerValueCorrection.Nearest);
    //                //g_camera.Parameters[PLCamera.ExposureTimeRaw].SetValue(value, IntegerValueCorrection.Nearest);


    //                // Some camera models may have auto functions enabled. To set the ExposureTime value to a specific value,
    //                // the ExposureAuto function must be disabled first (if ExposureAuto is available).
    //                g_camera.Parameters[PLCamera.ExposureAuto].TrySetValue(PLCamera.ExposureAuto.Off); // Set ExposureAuto to Off if it is writable.

    //                g_camera.Parameters[PLCamera.ExposureMode].TrySetValue(PLCamera.ExposureMode.Timed); // Set ExposureMode to Timed if it is writable.


    //                if (g_camera.GetSfncVersion() < Sfnc2_0_0)
    //                {
    //                    g_camera.Parameters[PLCamera.ExposureTimeRaw].SetValue(shutterValue);
    //                }
    //                else
    //                {
    //                    g_camera.Parameters[PLUsbCamera.ExposureTime].SetValue((long)shutterValue);
    //                }

    //                shuterCur = shutterValue;
    //            }
    //            catch (Exception ex)
    //            {
    //                Util.WriteLog(this.GetType(), ex);
    //                Util.Notify("相机曝光设置异常");
    //            }
    //        }
    //    }


    //    #endregion
    //    public BaslerCamera(Basler.Pylon.Camera g_camera, int index)
    //    {
    //        this.g_camera = g_camera;
    //        this.cameraIndex = index;
    //    }
    //    public BaslerCamera()
    //    {
    //    }
    //    public override void Close()
    //    {
    //        try
    //        {
    //            IsLink = false;
    //            // Reset the stopwatch.
    //            //stopWatch.Reset();
    //            if (g_camera != null)
    //            {
    //                // Stop the grabbing.
    //                g_camera.StreamGrabber.Stop();
    //                // Close the connection to the camera device.
    //                g_camera.Close();
    //                g_camera.Dispose();
    //                g_camera = null;
    //            }

    //            if (m_latestFrameAddress != IntPtr.Zero)
    //            {
    //                Marshal.FreeHGlobal(m_latestFrameAddress);
    //                m_latestFrameAddress = IntPtr.Zero;
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            Util.WriteLog(this.GetType(), ex);
    //            Util.Notify("相机关闭异常");
    //        }
    //    }

    //    public override void ContinuousShot()
    //    {
    //        if (g_camera == null || g_camera.IsConnected == false)
    //        {
    //            return;
    //        }
    //        try
    //        {
    //            Command = Command.Video;
    //            if (g_camera.GetSfncVersion() < Sfnc2_0_0)
    //            {
    //                if (g_camera.Parameters[PLCamera.TriggerSelector].TrySetValue(PLCamera.TriggerSelector.AcquisitionStart))
    //                {
    //                    g_camera.Parameters[PLCamera.TriggerMode].TrySetValue(PLCamera.TriggerMode.Off);
    //                }
    //                if (g_camera.Parameters[PLCamera.TriggerSelector].TrySetValue(PLCamera.TriggerSelector.FrameStart))
    //                {
    //                    g_camera.Parameters[PLCamera.TriggerMode].TrySetValue(PLCamera.TriggerMode.Off);
    //                }
    //            }
    //            else // For SFNC 2.0 cameras, e.g. USB3 Vision cameras
    //            {
    //                if (g_camera.Parameters[PLCamera.TriggerSelector].TrySetValue(PLCamera.TriggerSelector.FrameBurstStart))
    //                {
    //                    g_camera.Parameters[PLCamera.TriggerMode].TrySetValue(PLCamera.TriggerMode.Off);
    //                }
    //                if (g_camera.Parameters[PLCamera.TriggerSelector].TrySetValue(PLCamera.TriggerSelector.FrameStart))
    //                {
    //                    g_camera.Parameters[PLCamera.TriggerMode].TrySetValue(PLCamera.TriggerMode.Off);
    //                }
    //            }
    //            IsContinuousShot = true;
    //        }
    //        catch (Exception ex)
    //        {
    //            Util.WriteLog(this.GetType(), ex);
    //            Util.Notify("相机连续采集开始异常");
    //        }
    //    }

    //    public override void ContinuousShotStop()
    //    {
    //        try
    //        {
    //            // Set an enum parameter.
    //            if (g_camera == null || g_camera.IsConnected == false)
    //            {
    //                return;
    //            }
    //            if (g_camera.GetSfncVersion() < Sfnc2_0_0)
    //            {
    //                if (g_camera.Parameters[PLCamera.TriggerSelector].TrySetValue(PLCamera.TriggerSelector.AcquisitionStart))
    //                {
    //                    if (g_camera.Parameters[PLCamera.TriggerSelector].TrySetValue(PLCamera.TriggerSelector.FrameStart))
    //                    {
    //                        g_camera.Parameters[PLCamera.TriggerSelector].TrySetValue(PLCamera.TriggerSelector.AcquisitionStart);
    //                        g_camera.Parameters[PLCamera.TriggerMode].TrySetValue(PLCamera.TriggerMode.Off);

    //                        g_camera.Parameters[PLCamera.TriggerSelector].TrySetValue(PLCamera.TriggerSelector.FrameStart);
    //                        g_camera.Parameters[PLCamera.TriggerMode].TrySetValue(PLCamera.TriggerMode.On);
    //                        g_camera.Parameters[PLCamera.TriggerSource].TrySetValue(PLCamera.TriggerSource.Software);
    //                    }
    //                    else
    //                    {
    //                        g_camera.Parameters[PLCamera.TriggerSelector].TrySetValue(PLCamera.TriggerSelector.AcquisitionStart);
    //                        g_camera.Parameters[PLCamera.TriggerMode].TrySetValue(PLCamera.TriggerMode.On);
    //                        g_camera.Parameters[PLCamera.TriggerSource].TrySetValue(PLCamera.TriggerSource.Software);
    //                    }
    //                }

    //            }
    //            else // For SFNC 2.0 cameras, e.g. USB3 Vision cameras
    //            {
    //                if (g_camera.Parameters[PLCamera.TriggerSelector].TrySetValue(PLCamera.TriggerSelector.FrameBurstStart))
    //                {
    //                    if (g_camera.Parameters[PLCamera.TriggerSelector].TrySetValue(PLCamera.TriggerSelector.FrameStart))
    //                    {
    //                        g_camera.Parameters[PLCamera.TriggerSelector].TrySetValue(PLCamera.TriggerSelector.FrameBurstStart);
    //                        g_camera.Parameters[PLCamera.TriggerMode].TrySetValue(PLCamera.TriggerMode.Off);

    //                        g_camera.Parameters[PLCamera.TriggerSelector].TrySetValue(PLCamera.TriggerSelector.FrameStart);
    //                        g_camera.Parameters[PLCamera.TriggerMode].TrySetValue(PLCamera.TriggerMode.On);
    //                        g_camera.Parameters[PLCamera.TriggerSource].TrySetValue(PLCamera.TriggerSource.Software);
    //                    }
    //                    else
    //                    {
    //                        g_camera.Parameters[PLCamera.TriggerSelector].TrySetValue(PLCamera.TriggerSelector.FrameBurstStart);
    //                        g_camera.Parameters[PLCamera.TriggerMode].TrySetValue(PLCamera.TriggerMode.On);
    //                        g_camera.Parameters[PLCamera.TriggerSource].TrySetValue(PLCamera.TriggerSource.Software);
    //                    }
    //                }
    //            }
    //            IsContinuousShot = false;
    //            IsExtTrigger = false;
    //            Task.Run(() =>
    //            {
    //                ignoreImage = true;
    //                Thread.Sleep(1000);
    //                ignoreImage = false;
    //            });
    //        }
    //        catch (Exception ex)
    //        {
    //            Util.WriteLog(this.GetType(), ex);
    //            Util.Notify("相机连续采集停止异常");
    //        }
    //    }
    //    public override void OneShot(Command command)
    //    {
    //        try
    //        {
    //            if (IsContinuousShot || IsExtTrigger)
    //            {
    //                ContinuousShotStop();
    //            }
    //            Command = command;
    //            // Execute the software trigger. Wait up to 1000 ms until the camera is ready for trigger.
    //            if (g_camera.WaitForFrameTriggerReady(1000, TimeoutHandling.ThrowException))
    //            {
    //                g_camera.ExecuteSoftwareTrigger();
    //            }
    //        }
    //        catch
    //        {
    //            IsLink = false;
    //            Util.Notify("相机软触发异常");
    //        }
    //    }
    //    public override bool Open()
    //    {
    //        try
    //        {
    //            //颜色格式设置,必须在相机开始采集之前设置
    //            string format = g_camera.Parameters[PLCamera.PixelFormat].GetValue();
    //            PLCamera.PixelFormatEnum formatEnmu = new PLCamera.PixelFormatEnum();
    //            //彩色相机格式转换为可以识别的bg8
    //            if (format == formatEnmu.YUV422Packed || format == formatEnmu.YUV422_YUYV_Packed ||
    //                format == formatEnmu.BayerBG12 || format == formatEnmu.BayerBG12Packed)
    //            {
    //                bool ischange = false;
    //                try
    //                {
    //                    g_camera.Parameters[PLCamera.PixelFormat].SetValue(formatEnmu.BayerBG8);
    //                    ischange = true;
    //                }
    //                catch
    //                {

    //                }
    //                if (ischange == false)
    //                {
    //                    g_camera.Parameters[PLCamera.PixelFormat].SetValue(formatEnmu.BayerGB8);
    //                }
    //            }
    //            //g_allCameras = CameraFinder.Enumerate();

    //            //foreach (ICameraInfo cameraInfo in g_allCameras)
    //            //{                               
    //            ////   camera.CameraInfo[CameraInfoKey.ModelName]

    //            //}

    //            //if (g_allCameras.Count > 0)
    //            //{
    //            //g_camera = new Basler.Pylon.Camera(g_allCameras[0]);


    //            //if (true == g_camera.IsOpen)
    //            //    g_camera.Close();

    //            //g_camera.Open();
    //            //string id = g_camera.Parameters[PLCamera.DeviceUserID].GetValue();

    //            // Enumeration values are plain strings.
    //            // Set pixel format is set to Mono8
    //            //g_camera.Parameters[PLCamera.PixelFormat].SetValue(PLCamera.PixelFormat.Mono8);

    //            // On some cameras, the offsets are read-only. If they are writable, set the offsets to min.
    //            g_camera.Parameters[PLCamera.OffsetX].TrySetToMinimum();
    //            g_camera.Parameters[PLCamera.OffsetY].TrySetToMinimum();

    //            // Some parameters have restrictions. You can use GetIncrement/GetMinimum/GetMaximum to make sure you set a valid value.                   
    //            g_camera.Parameters[PLCamera.Width].SetValue(g_camera.Parameters[PLCamera.Width].GetMaximum());
    //            g_camera.Parameters[PLCamera.Height].SetValue(g_camera.Parameters[PLCamera.Height].GetMaximum());

    //            //// Here, we let pylon correct the value if needed.
    //            //g_camera.Parameters[PLCamera.Width].SetValue(202, IntegerValueCorrection.Nearest);
    //            //g_camera.Parameters[PLCamera.Height].SetValue(101, IntegerValueCorrection.Nearest);

    //            // Set a handler for processing the images.
    //            g_camera.StreamGrabber.ImageGrabbed += OnImageGrabbed;

    //            ContinuousShotStop();//设置为软触发模式
    //                                 // Start the grabbing of images until grabbing is stopped.
    //            g_camera.Parameters[PLCamera.AcquisitionMode].SetValue(PLCamera.AcquisitionMode.Continuous);
    //            //g_camera.Parameters[PLCamera.PacketSize].SetValue(8000);
    //            g_camera.StreamGrabber.Start(GrabStrategy.OneByOne, GrabLoop.ProvidedByStreamGrabber);

    //            // Reset the stopwatch used to reduce the amount of displayed images. The camera may acquire images faster than the images can be displayed
    //            //stopWatch.Reset();

    //            GetCameraSettingData();

    //            IsLink = true;
    //            //}
    //            //else
    //            //{
    //            //    Util.Notify("无相机连接");
    //            //    return false;
    //            //}
    //        }
    //        catch (Exception ex)
    //        {
    //            Util.WriteLog(this.GetType(), ex);
    //            Util.Notify("相机打开出现异常");

    //            throw ex;
    //        }
    //        return true;
    //    }

    //    private void OnImageGrabbed(object sender, ImageGrabbedEventArgs e)
    //    {
    //        try
    //        {
    //            if (ignoreImage)
    //            {
    //                return;
    //            }


    //            HOperatorSet.CountSeconds(out startTime);
    //            // Acquire the image from the camera. Only show the latest image. The camera may acquire images faster than the images can be displayed.

    //            // Get the grab result.
    //            IGrabResult grabResult = e.GrabResult;

    //            // Check if the image can be displayed.
    //            if (grabResult.IsValid)
    //            {
    //                // Reduce the number of displayed images to a reasonable amount if the camera is acquiring images very fast.
    //                //if (!stopWatch.IsRunning || stopWatch.ElapsedMilliseconds > 33)
    //                {
    //                    //stopWatch.Restart();
    //                    width = grabResult.Width;
    //                    height = grabResult.Height;
    //                    //if (hPylonImage != null && hPylonImage.IsInitialized())
    //                    //{
    //                    //    hPylonImage.Dispose();
    //                    //}
    //                    hPylonImage = new HImage();
    //                    if (grabResult.PixelTypeValue == PixelType.Mono8)
    //                    {
    //                        if (grabResult.GrabSucceeded == false)
    //                        {
    //                            Util.Notify(string.Format("相机{0}数据损坏,采集失败", cameraIndex));
    //                            return;
    //                        }
    //                        if (grabResult.PayloadSize == 0)
    //                        {
    //                            Util.Notify(string.Format("相机{0}数据损坏,图像包大小为0", cameraIndex));
    //                            return;
    //                        }


    //                        //Util.Notify(string.Format("相机{0}数据尺寸{1}", cameraIndex, grabResult.PayloadSize));
    //                        //allocate the m_stream_size amount of bytes in non-managed environment 
    //                        if (m_latestFrameAddress == IntPtr.Zero)
    //                        {
    //                            m_latestFrameAddress = Marshal.AllocHGlobal((Int32)grabResult.PayloadSize);
    //                        }
    //                        g_converter.OutputPixelFormat = PixelType.Mono8;
    //                        g_converter.Convert(m_latestFrameAddress, grabResult.PayloadSize, grabResult);

    //                        //转换为Halcon图像显示
    //                        hPylonImage.GenImage1("byte", grabResult.Width, grabResult.Height, m_latestFrameAddress);


    //                    }
    //                    else if (grabResult.PixelTypeValue == PixelType.BayerBG8 || grabResult.PixelTypeValue == PixelType.BayerGB8)
    //                    {
    //                        //allocate the m_stream_size amount of bytes in non-managed environment 
    //                        if (m_latestFrameAddress == IntPtr.Zero)
    //                        {
    //                            m_latestFrameAddress = Marshal.AllocHGlobal((Int32)(3 * g_camera.Parameters[PLCamera.PayloadSize].GetValue()));
    //                        }
    //                        g_converter.OutputPixelFormat = PixelType.BGR8packed;
    //                        g_converter.Convert(m_latestFrameAddress, 3 * grabResult.PayloadSize, grabResult);
    //                        hPylonImage.GenImageInterleaved(m_latestFrameAddress, "bgr",
    //                                 grabResult.Width, grabResult.Height, -1, "byte", grabResult.Width, grabResult.Height, 0, 0, -1, 0);

    //                    }
    //                    else
    //                    {
    //                        Util.Notify(string.Format("相机{0}编码格式不正确", cameraIndex));
    //                    }
    //                    TrigerImageEvent();
    //                }
    //            }
    //        }
    //        catch (System.ArgumentException ex)
    //        {
    //            Util.WriteLog(this.GetType(), ex);
    //            Util.Notify(string.Format("相机{0}图像数据包丢失", cameraIndex));
    //        }
    //        catch (Exception ex)
    //        {
    //            Util.WriteLog(this.GetType(), ex);
    //            Util.Notify(string.Format("相机{0}图像数据返回出现异常", cameraIndex));
    //        }
    //        finally
    //        {
    //            // Dispose the grab result if needed for returning it to the grab loop.
    //            e.DisposeGrabResultIfClone();
    //        }
    //    }


    //    protected override void GetCameraSettingData()
    //    {
    //        try
    //        {
    //            //long max, min, cur;
    //            gainMin = g_camera.Parameters[PLCamera.GainRaw].GetMinimum();
    //            gainMax = g_camera.Parameters[PLCamera.GainRaw].GetMaximum();
    //            gainCur = g_camera.Parameters[PLCamera.GainRaw].GetValue();
    //            gainUnit = "";

    //            shuterUnit = "us";

    //            shuterMin = g_camera.Parameters[PLCamera.ExposureTimeRaw].GetMinimum();
    //            shuterMax = g_camera.Parameters[PLCamera.ExposureTimeRaw].GetMaximum();
    //            shuterCur = g_camera.Parameters[PLCamera.ExposureTimeRaw].GetValue();

    //            triggerDelayAbsMin = g_camera.Parameters[PLCamera.TriggerDelayAbs].GetMinimum();
    //            triggerDelayAbsMax = g_camera.Parameters[PLCamera.TriggerDelayAbs].GetMaximum();
    //            triggerDelayAbs = g_camera.Parameters[PLCamera.TriggerDelayAbs].GetValue();

    //            lineDebouncerTimeAbsMin = 0;
    //            lineDebouncerTimeAbsMax = 5000;
    //            lineDebouncerTimeAbs = 0;

    //        }
    //        catch (Exception ex)
    //        {
    //            Util.WriteLog(this.GetType(), ex);
    //            Util.Notify("相机设置信息获取异常");
    //        }
    //    }

    //    public override void SetExtTrigger()
    //    {
    //        try
    //        {
    //            if (g_camera.GetSfncVersion() < Sfnc2_0_0)
    //            {
    //                if (g_camera.Parameters[PLCamera.TriggerSelector].TrySetValue(PLCamera.TriggerSelector.AcquisitionStart))
    //                {
    //                    if (g_camera.Parameters[PLCamera.TriggerSelector].TrySetValue(PLCamera.TriggerSelector.FrameStart))
    //                    {
    //                        g_camera.Parameters[PLCamera.TriggerSelector].SetValue(PLCamera.TriggerSelector.AcquisitionStart);
    //                        g_camera.Parameters[PLCamera.TriggerMode].SetValue(PLCamera.TriggerMode.Off);

    //                        g_camera.Parameters[PLCamera.TriggerSelector].SetValue(PLCamera.TriggerSelector.FrameStart);
    //                        g_camera.Parameters[PLCamera.TriggerMode].SetValue(PLCamera.TriggerMode.On);
    //                        g_camera.Parameters[PLCamera.TriggerSource].SetValue(PLCamera.TriggerSource.Line1);
    //                    }
    //                    else
    //                    {
    //                        g_camera.Parameters[PLCamera.TriggerSelector].SetValue(PLCamera.TriggerSelector.AcquisitionStart);
    //                        g_camera.Parameters[PLCamera.TriggerMode].SetValue(PLCamera.TriggerMode.On);
    //                        g_camera.Parameters[PLCamera.TriggerSource].SetValue(PLCamera.TriggerSource.Line1);
    //                    }
    //                }

    //                //Sets the trigger delay time in microseconds.
    //                g_camera.Parameters[PLCamera.TriggerDelayAbs].SetValue(triggerDelayAbs);

    //                //Sets the absolute value of the selected line debouncer time in microseconds
    //                g_camera.Parameters[PLCamera.LineSelector].SetValue(PLCamera.LineSelector.Line1);
    //                g_camera.Parameters[PLCamera.LineMode].SetValue(PLCamera.LineMode.Input);
    //                g_camera.Parameters[PLCamera.LineDebouncerTimeAbs].SetValue(LineDebouncerTimeAbs);

    //            }
    //            else // For SFNC 2.0 cameras, e.g. USB3 Vision cameras
    //            {
    //                if (g_camera.Parameters[PLCamera.TriggerSelector].TrySetValue(PLCamera.TriggerSelector.FrameBurstStart))
    //                {
    //                    if (g_camera.Parameters[PLCamera.TriggerSelector].TrySetValue(PLCamera.TriggerSelector.FrameStart))
    //                    {
    //                        g_camera.Parameters[PLCamera.TriggerSelector].TrySetValue(PLCamera.TriggerSelector.FrameBurstStart);
    //                        g_camera.Parameters[PLCamera.TriggerMode].TrySetValue(PLCamera.TriggerMode.Off);

    //                        g_camera.Parameters[PLCamera.TriggerSelector].TrySetValue(PLCamera.TriggerSelector.FrameStart);
    //                        g_camera.Parameters[PLCamera.TriggerMode].TrySetValue(PLCamera.TriggerMode.On);
    //                        g_camera.Parameters[PLCamera.TriggerSource].TrySetValue(PLCamera.TriggerSource.Line1);
    //                    }
    //                    else
    //                    {
    //                        g_camera.Parameters[PLCamera.TriggerSelector].TrySetValue(PLCamera.TriggerSelector.FrameBurstStart);
    //                        g_camera.Parameters[PLCamera.TriggerMode].TrySetValue(PLCamera.TriggerMode.On);
    //                        g_camera.Parameters[PLCamera.TriggerSource].TrySetValue(PLCamera.TriggerSource.Line1);
    //                    }
    //                }

    //                //Sets the trigger delay time in microseconds.//float
    //                g_camera.Parameters[PLCamera.TriggerDelay].SetValue(triggerDelayAbs);

    //                //Sets the absolute value of the selected line debouncer time in microseconds
    //                g_camera.Parameters[PLCamera.LineSelector].TrySetValue(PLCamera.LineSelector.Line1);
    //                g_camera.Parameters[PLCamera.LineMode].TrySetValue(PLCamera.LineMode.Input);
    //                g_camera.Parameters[PLCamera.LineDebouncerTime].SetValue(LineDebouncerTimeAbs);

    //            }
    //            Command = Command.Test;
    //            IsExtTrigger = true;
    //        }
    //        catch (Exception ex)
    //        {
    //            Util.WriteLog(this.GetType(), ex);
    //            Util.Notify("相机外触发设置异常");
    //        }
    //    }
    //    /// <summary>
    //    /// 信号输出,已经在task中运行
    //    /// </summary>
    //    public override void Output()
    //    {
    //        if (IOSerial.Instance.Rs232Param.Use == false)
    //        {
    //            Task.Run(() =>
    //            {
    //                lock (lockobj1)
    //                {
    //                    g_camera.Parameters[PLCamera.LineSelector].TrySetValue(PLCamera.LineSelector.Line1);
    //                    g_camera.Parameters[PLCamera.LineSource].TrySetValue(PLCamera.LineSource.UserOutput);
    //                    g_camera.Parameters[PLCamera.UserOutputValue].TrySetValue(true);
    //                    Thread.Sleep((int)outLineTime);
    //                    g_camera.Parameters[PLCamera.UserOutputValue].TrySetValue(false);
    //                    //g_camera.Parameters[PLCamera.timer].TrySetValue(PLCamera.LineSource.Timer1Active.);
    //                    Util.Notify("相机报警输出完成");
    //                }
    //            });
    //        }

    //        else
    //        {
    //            Task.Run(() =>
    //            {
    //                lock (lockobj2)
    //                {
    //                    try
    //                    {
    //                        IOSerial.Instance.WriteDataToSerial("#sw1n");
    //                        IOSerial.Instance.WriteDataToSerial("#sw2n");
    //                        IOSerial.Instance.WriteDataToSerial("#sw3n");
    //                        Thread.Sleep((int)outLineTime);
    //                        IOSerial.Instance.WriteDataToSerial("#sw1f");
    //                        IOSerial.Instance.WriteDataToSerial("#sw2f");
    //                        IOSerial.Instance.WriteDataToSerial("#sw3f");

    //                        Util.Notify("串口io报警输出完成");
    //                    }
    //                    catch (Exception ex)
    //                    {
    //                        Util.WriteLog(this.GetType(), ex);
    //                        Util.Notify("串口io通信失败");
    //                    }
    //                }
    //            });
    //        }
    //    }
    //}
}
