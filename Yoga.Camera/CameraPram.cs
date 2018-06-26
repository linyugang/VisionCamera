using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yoga.Camera
{

    public enum ImageAngle
    {
        角度0,
        角度90,
        角度180,
        角度270
    }

    [Serializable]
    public class CameraPram
    {
        /// <summary>
        /// 曝光时间
        /// </summary>
        private long shutter = 3500;
        /// <summary>
        /// 增益
        /// </summary>
        private double gain = 0;
        /// <summary>
        /// 外触发延时
        /// </summary>
        private double triggerDelayAbs = 10;
        /// <summary>
        /// 外触发防抖动
        /// </summary>
        private double lineDebouncerTimeAbs = 10;
        /// <summary>
        /// 输出信号输出时间
        /// </summary>
        private double outLineTime = 1000;

        private ImageAngle imageAngle = ImageAngle.角度0;

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
        public long Shutter
        {
            get
            {
                return shutter;
            }

            set
            {
                shutter = value;
            }
        }

        public double Gain
        {
            get
            {
                return gain;
            }

            set
            {
                gain = value;
            }
        }

        public double TriggerDelayAbs
        {
            get
            {
                return triggerDelayAbs;
            }

            set
            {
                triggerDelayAbs = value;
            }
        }

        public double LineDebouncerTimeAbs
        {
            get
            {
                return lineDebouncerTimeAbs;
            }

            set
            {
                lineDebouncerTimeAbs = value;
            }
        }

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
    }
}
