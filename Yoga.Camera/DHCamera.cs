//using GxIAPINET;
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
   //public class DHCamera : CameraBase
   // {
   //     private bool ignoreImage = false;
   //     private IGXDevice IGXDevice1 = null;
   //     /// <summary>
   //     /// Factory对像
   //     /// </summary>
   //     private IGXFactory IGXFactory1 = null;
   //     /// <summary>
   //     /// 流对像
   //     /// </summary>
   //     private IGXStream IGXStream1 = null;
   //     /// <summary>
   //     /// 远端设备属性控制器对像
   //     /// </summary>
   //     private IGXFeatureControl IGXFeatureControl = null;
   //     public override double GainCur
   //     {
   //         get
   //         {
   //             return gainCur;
   //         }

   //         set
   //         {
   //             try
   //             {
   //                 double gainValue = value;

   //                 //判断输入值是否在增益值的范围内
   //                 //若输入的值大于最大值则将增益值设置成最大值
   //                 if (gainValue > GainMax)
   //                 {
   //                     gainValue = GainMax;
   //                 }

   //                 //若输入的值小于最小值则将增益的值设置成最小值
   //                 if (gainValue < GainMin)
   //                 {
   //                     gainValue = GainMin;
   //                 }

   //                 IGXFeatureControl.GetFloatFeature("Gain").SetValue(gainValue);
   //                 gainCur = gainValue;
   //             }
   //             catch (Exception ex)
   //             {
   //                 Util.WriteLog(this.GetType(), ex);
   //                 Util.Notify("相机增益设置异常");
   //             }
   //         }
   //     }

   //     public override long ShuterCur
   //     {
   //         get
   //         {
   //             return shuterCur;
   //         }

   //         set
   //         {
   //             try
   //             {
   //                 long shutterValue = value;

   //                 //获取当前相机的曝光值、最小值、最大值和单位

   //                 //判断输入值是否在曝光时间的范围内
   //                 //若大于最大值则将曝光值设为最大值

   //                 if (shutterValue > ShuterMax)
   //                 {
   //                     shutterValue = ShuterMax;
   //                 }
   //                 //若小于最小值将曝光值设为最小值
   //                 else if (shutterValue < ShuterMin)
   //                 {
   //                     shutterValue = ShuterMin;
   //                 }

   //                 IGXFeatureControl.GetFloatFeature("ExposureTime").SetValue(shutterValue);
   //                 shuterCur = shutterValue;
   //             }
   //             catch (Exception ex)
   //             {
   //                 Util.WriteLog(this.GetType(), ex);
   //                 Util.Notify("相机曝光设置异常");
   //             }
   //         }
   //     }
   //     public DHCamera(IGXFactory IGXFactory1, IGXDevice IGXDevice1)
   //     {
   //         this.IGXFactory1 = IGXFactory1;
   //         this.IGXDevice1 = IGXDevice1;
   //         IGXFeatureControl = IGXDevice1.GetRemoteFeatureControl();
   //     }
   //     public override void Close()
   //     {
   //         try
   //         {
   //             ContinuousShotStop();
   //         }
   //         catch (Exception )
   //         {
   //             //LogHelper.WriteLog(typeof(DHCamera), "相机停止采集异常" + ex.ToString());
   //         }

   //         try
   //         {
   //             //停止流通道、注销采集回调和关闭流
   //             if (null != IGXStream1)
   //             {
   //                 IGXStream1.StopGrab();
   //                 IGXStream1.UnregisterCaptureCallback();
   //                 IGXStream1.Close();
   //                 IGXStream1 = null;
   //             }
   //         }
   //         catch (Exception )
   //         {
   //             //LogHelper.WriteLog(typeof(DHCamera), "相机停止采集异常" + ex.ToString());
   //         }

   //         try
   //         {
   //             //关闭设备
   //             if (null != IGXDevice1)
   //             {
   //                 IGXDevice1.Close();
   //                 IGXDevice1 = null;
   //             }
   //         }
   //         catch (Exception )
   //         {
   //             //LogHelper.WriteLog(typeof(DHCamera), "相机停止采集异常" + ex.ToString());
   //         }

   //         try
   //         {
   //             //反初始化
   //             if (null != IGXFactory1)
   //             {
   //                 IGXFactory1.Uninit();
   //             }
   //         }
   //         catch (Exception )
   //         {
   //             //LogHelper.WriteLog(typeof(DHCamera), "相机初始化异常" + ex.ToString());
   //         }
   //     }

   //     public override void ContinuousShot()
   //     {
   //         try
   //         {
   //             Command = Command.Video;
   //             #region 停止采集
   //             //发送停采命令
   //             if (null != IGXFeatureControl)
   //             {
   //                 IGXFeatureControl.GetCommandFeature("AcquisitionStop").Execute();
   //             }
   //             //关闭采集流通道
   //             if (null != IGXStream1)
   //             {
   //                 IGXStream1.StopGrab();
   //                 //注销采集回调函数
   //                 IGXStream1.UnregisterCaptureCallback();
   //             }
   //             #endregion
   //             #region 修改触发模式
   //             if (null != IGXFeatureControl)
   //             {
   //                 //设置触发模式为关
   //                 IGXFeatureControl.GetEnumFeature("TriggerMode").SetValue("Off");
   //             }
   //             #endregion
   //             #region 打开采集
   //             //开启采集流通道
   //             if (null != IGXStream1)
   //             {
   //                 //RegisterCaptureCallback第一个参数属于用户自定参数(类型必须为引用
   //                 //类型)，若用户想用这个参数可以在委托函数中进行使用
   //                 IGXStream1.RegisterCaptureCallback(this, CaptureCallbackPro);
   //                 IGXStream1.StartGrab();
   //             }
   //             //发送开采命令
   //             if (null != IGXFeatureControl)
   //             {
   //                 IGXFeatureControl.GetCommandFeature("AcquisitionStart").Execute();
   //             }
   //             #endregion
   //             IsContinuousShot = true;
   //         }
   //         catch (Exception ex)
   //         {
   //             Util.WriteLog(this.GetType(), ex);
   //             Util.Notify("相机连续采集开始异常");
   //         }
   //     }

   //     public override void ContinuousShotStop()
   //     {
   //         try
   //         {
   //             #region 停止采集
   //             //发送停采命令
   //             if (null != IGXFeatureControl)
   //             {
   //                 IGXFeatureControl.GetCommandFeature("AcquisitionStop").Execute();
   //             }
   //             //关闭采集流通道
   //             if (null != IGXStream1)
   //             {
   //                 IGXStream1.StopGrab();
   //                 //注销采集回调函数
   //                 IGXStream1.UnregisterCaptureCallback();
   //             }
   //             #endregion

   //             #region 修改触发模式
   //             if (null != IGXFeatureControl)
   //             {
   //                 //设置触发模式为开
   //                 IGXFeatureControl.GetEnumFeature("TriggerMode").SetValue("On");
   //             }
   //             #endregion
   //             #region 打开采集
   //             //开启采集流通道
   //             if (null != IGXStream1)
   //             {
   //                 //RegisterCaptureCallback第一个参数属于用户自定参数(类型必须为引用
   //                 //类型)，若用户想用这个参数可以在委托函数中进行使用
   //                 IGXStream1.RegisterCaptureCallback(this, CaptureCallbackPro);
   //                 IGXStream1.StartGrab();
   //             }
   //             //发送开采命令
   //             if (null != IGXFeatureControl)
   //             {
   //                 IGXFeatureControl.GetCommandFeature("AcquisitionStart").Execute();
   //             }
   //             #endregion
   //             IsContinuousShot = false;
   //             IsExtTrigger = false;
   //             ignoreImage = true;
   //             Task.Run(() =>
   //             {
   //                 Thread.Sleep(1000);
   //                 ignoreImage = false;
   //             });
   //         }
   //         catch (Exception ex)
   //         {
   //             Util.WriteLog(this.GetType(), ex);
   //             Util.Notify("相机连续采集停止异常");
   //         }
   //     }

   //     public override void OneShot(Command command)
   //     {
   //         try
   //         {
   //             if (IsContinuousShot || IsExtTrigger)
   //             {
   //                 ContinuousShotStop();
   //             }
   //             Command = command;
   //             //发送软触发命令
   //             if (null != IGXFeatureControl)
   //             {
   //                 IGXFeatureControl.GetCommandFeature("TriggerSoftware").Execute();
   //             }
   //             else
   //             {
   //                 Util.Notify("相机软触发异常,设备未连接");
   //             }
   //         }
   //         catch
   //         {
   //             IsLink = false;
   //             Util.Notify("相机软触发异常");
   //         }
   //     }
   //     private void CaptureCallbackPro(object objUserParam, IFrameData objIFrameData)
   //     {
   //         try
   //         {
   //             if (ignoreImage)
   //             {
   //                 return;
   //             }
   //             HOperatorSet.CountSeconds(out startTime);
   //             {

   //                 hPylonImage = new HImage();

   //                 IntPtr ImageData = objIFrameData.GetBuffer();
   //                 if (objIFrameData.GetPixelFormat()== GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_MONO8)
   //                 {
   //                     hPylonImage.GenImage1("byte",(int) objIFrameData.GetWidth(), (int)objIFrameData.GetHeight(), objIFrameData.GetBuffer());
   //                 }
   //                 else if (objIFrameData.GetPixelFormat() == GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_RGB8_PLANAR)
   //                 {
   //                     hPylonImage.GenImageInterleaved(objIFrameData.GetBuffer(),
   //                     "rgb",
   //                     (int)objIFrameData.GetWidth(), (int)objIFrameData.GetHeight(),
   //                     -1, "byte",
   //                      (int)objIFrameData.GetWidth(), (int)objIFrameData.GetHeight(),
   //                     0, 0, -1, 0);
   //                 }
   //                 else
   //                 {
   //                     Util.Notify(string.Format("相机{0}编码格式不正确,当前格式{1}", cameraIndex, objIFrameData.GetPixelFormat()));
   //                 }
   //                 TrigerImageEvent();

   //             }

   //         }
   //         catch (System.ArgumentException ex)
   //         {
   //             Util.WriteLog(this.GetType(), ex);
   //             Util.Notify(string.Format("相机{0}图像数据包丢失", cameraIndex));
   //         }
   //         catch (Exception ex)
   //         {
   //             Util.WriteLog(this.GetType(), ex);
   //             Util.Notify(string.Format("相机{0}图像数据返回出现异常", cameraIndex));
   //         }
   //     }
   //     public override bool Open()
   //     {
   //         try
   //         {
   //             //打开流
   //             if (null != IGXDevice1)
   //             {
   //                 IGXStream1 = IGXDevice1.OpenStream(0);
   //             }

   //             //初始化相机参数
   //             if (null != IGXFeatureControl)
   //             {
   //                 //设置采集模式连续采集------------------------------
   //                 IGXFeatureControl.GetEnumFeature("AcquisitionMode").SetValue("Continuous");
   //                 //设置触发模式为关
   //                 IGXFeatureControl.GetEnumFeature("TriggerMode").SetValue("On");
   //             }
   //             //开启采集流通道
   //             if (null != IGXStream1)
   //             {
   //                 //RegisterCaptureCallback第一个参数属于用户自定参数(类型必须为引用
   //                 //类型)，若用户想用这个参数可以在委托函数中进行使用
   //                 IGXStream1.RegisterCaptureCallback(this, CaptureCallbackPro);
   //                 IGXStream1.StartGrab();
   //             }

   //             //发送开采命令
   //             if (null != IGXFeatureControl)
   //             {
   //                 IGXFeatureControl.GetCommandFeature("AcquisitionStart").Execute();
   //             }

   //             ContinuousShotStop();

   //             // Reset the stopwatch used to reduce the amount of displayed images. The camera may acquire images faster than the images can be displayed
   //             //stopWatch.Reset();

   //             GetCameraSettingData();

   //             IsLink = true;


   //         }
   //         catch (Exception ex)
   //         {
   //             Util.WriteLog(this.GetType(), ex);
   //             Util.Notify("相机打开出现异常:" + ex.Message);
   //             throw ex;
   //         }
   //         return true;
   //     }

   //     public override void Output()
   //     {
   //         throw new NotImplementedException();
   //     }

   //     public override void SetExtTrigger()
   //     {
   //         throw new NotImplementedException();
   //     }

   //     protected override void GetCameraSettingData()
   //     {
   //         try
   //         {
   //             //long max, min, cur;

   //             gainCur = IGXFeatureControl.GetFloatFeature("Gain").GetValue();
   //             gainMin = IGXFeatureControl.GetFloatFeature("Gain").GetMin();
   //             gainMax = IGXFeatureControl.GetFloatFeature("Gain").GetMax();

   //             gainUnit = "db";


   //             shuterCur = (long)IGXFeatureControl.GetFloatFeature("ExposureTime").GetValue();
   //             shuterMin = (long)IGXFeatureControl.GetFloatFeature("ExposureTime").GetMin();
   //             shuterMax = (long)IGXFeatureControl.GetFloatFeature("ExposureTime").GetMax();
   //             shuterUnit = IGXFeatureControl.GetFloatFeature("ExposureTime").GetUnit();


   //             //float fTriggerDelay = 0;
   //             //m_pOperator.GetFloatValue("TriggerDelay", ref fTriggerDelay);
   //             //triggerDelayAbsMin = 0;
   //             //triggerDelayAbsMax = 1000;
   //             //triggerDelayAbs = fTriggerDelay;

   //             lineDebouncerTimeAbsMin = 0;
   //             lineDebouncerTimeAbsMax = 5000;
   //             lineDebouncerTimeAbs = 0;

   //         }
   //         catch (Exception ex)
   //         {
   //             Util.WriteLog(this.GetType(), ex);
   //             Util.Notify("相机设置信息获取异常");
   //         }
   //     }
   // }
}
