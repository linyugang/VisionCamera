using HalconDotNet;
using System;
using System.Threading;
using Yoga.Common;

namespace Yoga.Camera
{
    


    public abstract class CameraBase
    {
       protected HImage hPylonImage;

        #region 相机参数

        protected int cameraIndex;

        protected HTuple startTime;
        //相机参数
        /// <summary>
        /// 曝光初始值,单位us
        /// </summary>
        public virtual double ShotInitValue
        {
            get
            {
                return 3900;

            }
        }
        /// <summary>
        /// 增益初始值
        /// </summary>
        public virtual double GainInitValue
        {
            get
            {
                return gainMin;
            }
        }
        protected long shuterCur;                     //当前曝光值
        protected long shuterMin;                      //最小值
        protected long shuterMax;                      //最大值
        protected string shuterUnit;                       //单位

        protected double gainCur;             //当前增益值
        protected double gainMin;          //最小值
        protected double gainMax;        //最大值
        protected string gainUnit;         //单位 

        protected double triggerDelayAbsMax;
        protected double triggerDelayAbsMin;
        /// <summary>
        /// 外触发延时
        /// </summary>
        protected double triggerDelayAbs;

        protected double lineDebouncerTimeAbsMax;
        protected double lineDebouncerTimeAbsMin;
        /// <summary>
        /// 外触发防抖动
        /// </summary>
        protected double lineDebouncerTimeAbs;

        protected double outLineTime;
        /// <summary>
        /// 当前触发模式 连续触发=0 ;软触发=1
        /// </summary>
        protected int triggerMode;
        protected Fps fps = new Fps();
        public int CameraIndex
        {
            get
            {
                return cameraIndex;
            }
        }
        /// <summary>
        /// 当前曝光值单位us
        /// </summary>
        public abstract long ShuterCur { get; set; }                       //当前曝光值
        public long ShuterMin
        {
            get
            {
                return shuterMin;
            }
        }                      //最小值
        public long ShuterMax
        {
            get
            {
                return shuterMax;
            }
        }                      //最大值
        public string ShuterUnit
        {
            get
            {
                return shuterUnit;
            }
        }
        //单位

        public abstract double GainCur { get; set; }             //当前增益值
        public double GainMin
        {
            get
            {
                return gainMin;
            }

        }          //最小值
        public double GainMax
        {
            get
            {
                return gainMax;
            }
        }          //最大值
        public string GainUnit
        {
            get
            {
                return gainUnit;
            }
        }          //单位 
        #endregion
        private ImageAngle imageAngle = ImageAngle.角度0;
       
        /// <summary>
        /// 异常信息
        /// </summary>
        public string ErrMessage { get; protected set; }
        /// <summary>
        /// 是否建立采集连接
        /// </summary>
        public bool IsLink { get; protected set; }
        /// <summary>
        /// 是否在连续采集中
        /// </summary>
        public bool IsContinuousShot { get; protected set; }
        protected ManualResetEvent imageResetEvent = new ManualResetEvent(false);
        public bool IsExtTrigger { get; protected set; }
        private readonly object commandLock = new object();

        private Command command = Command.ExtTrigger;
        /// <summary>
        /// 当前采集图像附带命令信息-包含线程安全锁定
        /// </summary>
        public Command Command
        {
            protected get
            {
                lock (commandLock)
                {
                    return command;
                }
            }
            set
            {
                lock (commandLock)
                {
                    command = value;
                }
            }
        }
        public virtual double TriggerDelayAbsInit
        {
            get
            {
                return 10;
            }
        }
        /// <summary>
        /// 触发延时
        /// </summary>
        public virtual double TriggerDelayAbs
        {
            get
            {
                return triggerDelayAbs;
            }

            set
            {
                try
                {
                    double data = value;

                    //判断输入值是否在范围内
                    //若大于最大值则将曝光值设为最大值

                    if (data > TriggerDelayAbsMax)
                    {
                        data = TriggerDelayAbsMax;
                    }
                    //若小于最小值将曝光值设为最小值
                    else if (data < triggerDelayAbsMin)
                    {
                        data = triggerDelayAbsMin;
                    }

                    triggerDelayAbs = data;
                    if (IsExtTrigger)
                    {
                        SetExtTrigger();
                    }

                }
                catch (Exception ex)
                {
                    Util.WriteLog(this.GetType(), ex);
                    Util.Notify("相机触发延时设置异常");
                }
            }
        }
        /// <summary>
        /// 触发防抖默认值(us)
        /// </summary>
        public virtual double LineDebouncerTimeAbsInit
        {
            get
            {
                return 10;
            }
        }
       /// <summary>
       /// 触发防抖(us)
       /// </summary>
        public virtual double LineDebouncerTimeAbs
        {
            get
            {
                return lineDebouncerTimeAbs;
            }

            set
            {
                try
                {
                    double data = value;

                    //判断输入值是否在范围内
                    //若大于最大值则将触发防抖设为最大值

                    if (data > lineDebouncerTimeAbsMax)
                    {
                        data = lineDebouncerTimeAbsMax;
                    }
                    //若小于最小值将触发防抖设为最小值
                    else if (data < lineDebouncerTimeAbsMin)
                    {
                        data = lineDebouncerTimeAbsMin;
                    }

                    lineDebouncerTimeAbs = data;
                    if (IsExtTrigger)
                    {
                        SetExtTrigger();
                    }
                }
                catch (Exception ex)
                {
                    Util.WriteLog(this.GetType(), ex);
                    Util.Notify("相机触发防抖设置异常");
                }
            }
        }

        public double TriggerDelayAbsMax
        {
            get
            {
                return triggerDelayAbsMax;
            }
        }

        public double TriggerDelayAbsMin
        {
            get
            {
                return triggerDelayAbsMin;
            }
        }

        public double LineDebouncerTimeAbsMax
        {
            get
            {
                return lineDebouncerTimeAbsMax;
            }
        }

        public double LineDebouncerTimeAbsMin
        {
            get
            {
                return lineDebouncerTimeAbsMin;
            }
        }
        /// <summary>
        /// 外部信号输出时间
        /// </summary>
        public double OutLineTime
        {
            get
            {
                return outLineTime;
            }

            set
            {
                outLineTime = value;
            }
        }

        public ImageAngle ImageAngle
        {
            get
            {
                return imageAngle;
            }
            set
            {
                imageAngle = value;
            }
        }

        public HTuple StartTime
        {
            get
            {
                return startTime;
            }
        }

        public event EventHandler<ImageEventArgs> ImageEvent;
        public void TrigerImageEvent(object sender, ImageEventArgs e)
        {
            if (ImageEvent != null)
            {
                ImageEvent(sender, e);
            }
        }

        private HTuple timeOld;
        protected void TrigerImageEvent()
        {
            //帧率统计增加
            fps.IncreaseFrameNum();
            if (hPylonImage != null && hPylonImage.IsInitialized())
            {

                HImage imgTmp = RotateImage(hPylonImage);
                hPylonImage.Dispose();
                hPylonImage = imgTmp;
                imageResetEvent.Set();
                if (Command != Command.Grab)
                {
                    if (timeOld!=null)
                    {
                        double toolTime = (startTime - timeOld) * 1000.0;
                        if (Command!= Command.Video)
                        {
                            Util.Notify(string.Format("相机{0}收到图像,采集间隔{1:f2}ms", CameraIndex, toolTime));
                        }
                        
                    }
                    timeOld = startTime;
                    //Util.Notify("收到图像"+ startTime.D);
                    TrigerImageEvent(this, new ImageEventArgs(Command, hPylonImage, cameraIndex, new HTuple(startTime.D)));
                }
                //if (Command!= Command.Video)
                //{
                //    Command = Command.None;
                //}
            }
            else
            {
                Util.Notify(string.Format("相机{0}图像数据为空", cameraIndex));
            }
        }

        protected abstract void GetCameraSettingData();
        int fpsCount = 0;
        public virtual string GetCameraAcqInfo()
        {
            fpsCount++;
            if (fpsCount > 10 && IsContinuousShot)
            {
                fps.UpdateFps();
                fpsCount = 0;
            }
            string info = "";
            if (IsContinuousShot)
            {
                info = string.Format("FPS:{0:F1}|帧:{1}|",
                                      fps.GetFps(), fps.GetTotalFrameCount());
            }
            else
            {
                info = string.Format(" 帧:{0} |",
                                     fps.GetTotalFrameCount());
            }
            return info;
        }
        /// <summary>
        /// 打开相机
        /// </summary>
        /// <returns></returns>
        public abstract bool Open();
        /// <summary>
        /// 关闭设备
        /// </summary>
        public abstract void Close();
        /// <summary>
        /// 连续采集图像
        /// </summary>
        public abstract void ContinuousShot();
        /// <summary>
        /// 停止连续采集
        /// </summary>
        public abstract void ContinuousShotStop();
        /// <summary>
        /// 采集一张图像
        /// </summary>
        public abstract void OneShot(Command command);


        public virtual void ResetFps()
        {
            fps.Reset();
        }
        public virtual HImage GrabImage(int delayMs)
        {
            imageResetEvent.Reset();
            OneShot(Command.Grab);
            if (imageResetEvent.WaitOne(delayMs)&& hPylonImage.IsInitialized())
            {
                return hPylonImage;
            }
            else
            {
                throw new Exception("图像软触发采集超时");
            }
        }
        /// <summary>
        /// 设置为外触发模式
        /// </summary>
        public abstract void SetExtTrigger();

        public abstract void Output();

        protected HImage RotateImage(HImage img)
        {
            HImage img1=null;
            switch (ImageAngle)
            {
                case ImageAngle.角度0:
                    img1= img.CopyImage();
                    break;
                case ImageAngle.角度90:
                    img1 = img.RotateImage((double)90, "constant");
                    break;
                case ImageAngle.角度180:
                    img1 = img.RotateImage((double)180, "constant");
                    break;
                case ImageAngle.角度270:
                    img1 = img.RotateImage((double)270, "constant");
                    break;
                default:
                    break;
            }
            return img1;
        }
    }
}
