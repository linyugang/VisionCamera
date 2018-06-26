using HalconDotNet;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Yoga.Camera
{
    [Serializable]
    public class ImageEventArgs : EventArgs, IDisposable
    {
        public readonly Command Command;
        public readonly HImage CameraImage;
        public readonly HTuple StartTime;

        int? cameraIndex;
        int? settingIndex;
        bool _disposed;
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;
            if (disposing)
            {
                if (CameraImage != null && CameraImage.IsInitialized())
                {
                    CameraImage.Dispose();
                }
                _disposed = true;
            }
            cameraIndex = null;
            settingIndex = null;

        }

        ~ImageEventArgs()
        {
            this.Dispose(false);
        }
        /// <summary>
        /// 工具编号,默认为1,只能设置一次
        /// </summary>
        public int SettingIndex
        {
            get
            {
                if (settingIndex == null)
                {
                    return 1;
                }
                return settingIndex.Value;
            }

            set
            {
                if (settingIndex == null)
                {
                    settingIndex = value;
                }
            }
        }
        /// <summary>
        /// 相机编号,默认为1,只能设置一次
        /// </summary>
        public int CameraIndex
        {
            get
            {
                if (cameraIndex == null)
                {
                    return 1;
                }
                return cameraIndex.Value;
            }

            set
            {
                if (cameraIndex == null)
                {
                    cameraIndex = value;
                }
            }
        }
        public ImageEventArgs Clone()
        {
            using (Stream objectStream = new MemoryStream())
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(objectStream, this);

                BinaryFormatter b = new BinaryFormatter();
                object obj = b.Deserialize(objectStream);
                return obj as ImageEventArgs;
            }
        }
        public ImageEventArgs(Command command, HImage cameraImage, int cameraIndex, HTuple startTime)
        {
            Command = command;
            CameraImage = cameraImage;
            CameraIndex = cameraIndex;
            StartTime = startTime;
        }
        public ImageEventArgs(Command command, HImage cameraImage, int cameraIndex, int settingIndex, HTuple startTime)
        {
            Command = command;
            CameraImage = cameraImage;
            CameraIndex = cameraIndex;
            SettingIndex = SettingIndex;
            StartTime = startTime;
        }
    }
}
