using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yoga.Common.Basic;

namespace Yoga.Camera
{
   public class IOSerial
    {
        private static IOSerial instance;

        private SerialPort com = new SerialPort();


        private CommunicationParam rs232Param;
        public CommunicationParam Rs232Param
        {
            get
            {
                if (rs232Param == null)
                {
                    rs232Param = new CommunicationParam();
                }
                return rs232Param;
            }

            set
            {
                rs232Param = value;
            }
        }
        public static IOSerial Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new IOSerial();
                }
                return instance;
            }
        }

        /// <summary>
        /// 串口初始化
        /// </summary>
        public void InitSerial()
        {
            try
            {
                if (com.IsOpen)
                {
                    Close();
                }

                com.PortName = Rs232Param.ComName;
                com.BaudRate = Convert.ToInt32(Rs232Param.BaudRate);
                com.Parity = (Parity)Convert.ToInt32(Rs232Param.Parity);
                com.DataBits = Convert.ToInt32(Rs232Param.DataBits);
                com.StopBits = (StopBits)Convert.ToInt32(Rs232Param.StopBits);
                //com.NewLine = "\r\n";
                //com.NewLine = "\r";
                //com.DataReceived += new SerialDataReceivedEventHandler(this.OnDataReceived);

                com.Open();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("串口打开失败," + ex.Message);
            }
        }

        public void Close()
        {
            if (com.IsOpen)
                com.Close();
        }

        /// <summary>
        /// 写入数据到串口
        /// </summary>
        /// <param name="str">待写入字符串-无校验</param>
        /// <returns></returns>
        public bool WriteDataToSerial(string str)
        {
            bool writeFlag = false;
            try
            {
                if (!com.IsOpen)
                {
                    InitSerial();
                    //com.Open();
                }
                com.Write(str);
                writeFlag = true;
            }
            catch
            {
                throw new ApplicationException("串口设置异常");
            }
            return writeFlag;
        }
    }
}
