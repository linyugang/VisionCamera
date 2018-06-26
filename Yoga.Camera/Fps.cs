using HalconDotNet;
using System;

namespace Yoga.Camera
{
    public class Fps
    {
        ulong frameCount = 0;                           // 从上次计算完毕开始累积的帧数
        HTuple beginTime = 0.0;                         // 第一帧之前的一帧的时间（初始为0）
        HTuple endTime = 0.0;                         //最后一帧的时间

  
        double fps = 0.0;                         //通过帧数与时间间隔之比得出的帧率(帧/秒)
        double currentFps = 0.0;                         //当前的帧率,可能是预测得到的（帧/秒）
        ulong totalFrameCount = 0;                           //累积的帧数
        //TimeWatch objTime = new TimeWatch();            // 计时器
        object m_objLock = new object();

        /// <summary>
        /// 构造函数
        /// </summary>
        public Fps()
        {
            //重置所有参数
            Reset();
        }


        /// <summary>
        /// 获取最近一次的帧率
        /// </summary>
        /// <returns>当前帧图像</returns>
        public double GetFps()
        {
            lock (m_objLock)
            {
                //返回当前的帧率
                return currentFps;
            }
        }

        /// <summary>
        /// 获取累积的总帧数
        /// </summary>
        /// <returns>当前帧图像</returns>
        public ulong GetTotalFrameCount()
        {
            lock (m_objLock)
            {
                return totalFrameCount;
            }
        }

        /// <summary>
        /// 增加帧数
        /// </summary>
        public void IncreaseFrameNum()
        {
            lock (m_objLock)
            {
                //累积帧数
                totalFrameCount++;

                //增加帧数
                frameCount++;

                //更新时间间隔
                HOperatorSet.CountSeconds(out endTime);
                //endTime = objTime.ElapsedTime();
            }
        }

        /// <summary>
        /// 更新帧率
        /// 如果该函数被调用的频率超过了帧频率，则帧率会降为零
        /// </summary>
        public void UpdateFps()
        {
            lock (m_objLock)
            {
                //计算时间间隔
                double dInterval =( endTime - beginTime)*1000.0;

                //时间间隔大于零（有新帧）
                if (dInterval > 0)
                {
                    fps = 1000.0 * frameCount / dInterval;
                    frameCount = 0;              //累积帧数清零
                    beginTime = endTime;      //更新起始时间

                    currentFps = fps;
                }
                else if (dInterval == 0) //时间间隔等于零（无新帧）
                {
                    //如果上次的帧率非零，则调整帧率
                    if (currentFps != 0)
                    {
                       

                        //从上一帧到现在的经历的时间（毫秒）
                        HTuple nowTime;
                        HOperatorSet.CountSeconds(out nowTime);

                        //从上一帧到现在的经历的时间（毫秒）
                        double dCurrentInterval = (nowTime - beginTime)*1000.0;

                        //根据当前帧率计算更新帧率的时间阈值
                        double dPeriod = 1000.0 / currentFps;   //上次的帧周期(毫秒)
                        const double RATIO = 1.5;                      //超过帧周期的多少倍，帧率才更新
                        double dThresh = RATIO * dPeriod;          //多长时间没有来帧，帧率就更新

                        //如果超过2秒没有来帧，则帧率降为零。
                        const double ZERO_FPS_INTERVAL = 2000;
                        if (dCurrentInterval > ZERO_FPS_INTERVAL)
                        {
                            currentFps = 0;
                        }
                        //如果在2秒之内已经超过1.5倍的帧周期没有来帧，则降低帧率
                        else if (dCurrentInterval > dThresh)
                        {
                            currentFps = fps / (dCurrentInterval / (1000.0 / fps));
                        }
                        else { }
                    }
                    else { }
                }
                else { }
            }

        }

        /// <summary>
        /// 将计时器恢复为初始状态
        /// </summary>
        public void Reset()
        {
            frameCount = 0;
            beginTime = 0.0;
            endTime = 0.0;
            totalFrameCount = 0;
            fps = 0.0;
            currentFps = 0.0;
            HOperatorSet.CountSeconds(out beginTime);
            //objTime.Start();          //重启计时器
        }
    }
}
