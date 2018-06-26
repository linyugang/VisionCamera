//using Basler.Pylon;
using DeviceSource;
using MvCamCtrl.NET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using Yoga.Common;
using MVSDK;
using CameraHandle = System.Int32;
using MVAPI;

using MVSTATUS = MVAPI.MVSTATUS_CODES;
using HalconDotNet;
//using GxIAPINET;

namespace Yoga.Camera
{
    [Flags]
    public enum CameraFlag
    {
        Basler = 1,
        Hikvision = 2,
        Mindvision = 4,
        Microvision = 8,
        ICImaging = 16,
        DirectShow = 32,
        DaHeng = 64,
        Gige = 128
    }
    public static class CameraManger
    {
        static Dictionary<int, CameraBase> cameraDic;

        public static event EventHandler<EventArgs> CameraInitFinishEvent;


        /// <summary>
        /// 相机类型标记
        /// </summary>
        public static CameraFlag CameraFlag = CameraFlag.Basler;

        public static string DirectShowIndex = "1";
        public static string DirectShowColorSpace = "gray";
        public static string DirectShowCameraType = "yuv (1600x1200)";
        public static Dictionary<int, CameraBase> CameraDic
        {
            get
            {
                if (cameraDic == null)
                {
                    cameraDic = new Dictionary<int, CameraBase>();
                }
                return cameraDic;
            }

            set
            {
                cameraDic = value;
            }
        }
        /// <summary>
        /// 获取gige相机的属性信息
        /// </summary>
        /// <param name="dat"></param>
        /// <param name="valType"></param>
        /// <returns></returns>
        private static string GetGigeValue(string dat, string valType)
        {
            int userNameIndex = dat.IndexOf(valType);
            if (userNameIndex == -1)
            {
                return null;
            }
            int count = valType.Count();
            string userNameTr = dat.Substring(userNameIndex + count);
            int sub = userNameTr.IndexOf("|");
            if (sub == -1)
            {
                sub = userNameTr.Length;
            }
            string name = userNameTr.Substring(0, sub);
            string ttt = name.Trim();
            return ttt;
        }
        public static bool Open()
        {
            try
            {
                int cameraCount = 0;
                if ((CameraFlag & CameraFlag.Basler) == CameraFlag.Basler)
                {
                    //#region basler相机遍历查询

                    //List<Basler.Pylon.ICameraInfo> g_allCameras = Basler.Pylon.CameraFinder.Enumerate();

                    //if (g_allCameras.Count > 0)
                    //{
                    //    for (int i = 0; i < g_allCameras.Count; i++)
                    //    {
                    //        Basler.Pylon.Camera g_camera = new Basler.Pylon.Camera(g_allCameras[i]);
                    //        if (true == g_camera.IsOpen)
                    //            g_camera.Close();

                    //        g_camera.Open();
                    //        string id = g_camera.Parameters[Basler.Pylon.PLCamera.DeviceUserID].GetValue();
                    //        int id1;
                    //        if (int.TryParse(id, out id1))
                    //        {
                    //            if (CameraDic.Keys.Contains(id1) == false)
                    //            {
                    //                CameraDic.Add(id1, new BaslerCamera(g_camera, id1));
                    //                cameraCount++;
                    //            }
                    //        }
                    //        else
                    //        {
                    //            Util.Notify("相机ID未设置");
                    //        }
                    //    }
                    //}

                    //#endregion
                }
                if ((CameraFlag & CameraFlag.Hikvision) == CameraFlag.Hikvision)
                {
                    #region 海康相机遍历枚举
                    MyCamera.MV_CC_DEVICE_INFO_LIST m_pDeviceList = new MyCamera.MV_CC_DEVICE_INFO_LIST();
                    int nRet;


                    nRet = CameraOperator.EnumDevices(MyCamera.MV_GIGE_DEVICE | MyCamera.MV_USB_DEVICE, ref m_pDeviceList);
                    if (0 == nRet && m_pDeviceList.nDeviceNum > 0)
                    {
                        for (int i = 0; i < m_pDeviceList.nDeviceNum; i++)
                        {


                            MyCamera.MV_CC_DEVICE_INFO device =
                        (MyCamera.MV_CC_DEVICE_INFO)Marshal.PtrToStructure(m_pDeviceList.pDeviceInfo[i],
                                                                  typeof(MyCamera.MV_CC_DEVICE_INFO));

                            if (device.nTLayerType == MyCamera.MV_GIGE_DEVICE)
                            {
                                IntPtr buffer = Marshal.UnsafeAddrOfPinnedArrayElement(device.SpecialInfo.stGigEInfo, 0);
                                MyCamera.MV_GIGE_DEVICE_INFO gigeInfo = (MyCamera.MV_GIGE_DEVICE_INFO)Marshal.PtrToStructure(buffer, typeof(MyCamera.MV_GIGE_DEVICE_INFO));

                                int key;

                                if (int.TryParse(gigeInfo.chUserDefinedName, out key))
                                {
                                    if (CameraDic.Keys.Contains(key) == false)
                                    {
                                        CameraOperator m_pOperator = new CameraOperator();
                                        //打开设备
                                        nRet = m_pOperator.Open(ref device);
                                        //Util.Notify("打开相机");
                                        if (MyCamera.MV_OK == nRet)
                                        {
                                            CameraDic.Add(key, new HikvisionCamera(m_pOperator, key));
                                            cameraCount++;
                                        }
                                    }
                                }
                                else
                                {
                                    Util.Notify("相机ID未设置");
                                }
                            }
                            else if (device.nTLayerType == MyCamera.MV_USB_DEVICE)
                            {
                                IntPtr buffer = Marshal.UnsafeAddrOfPinnedArrayElement(device.SpecialInfo.stUsb3VInfo, 0);
                                MyCamera.MV_USB3_DEVICE_INFO usbInfo = (MyCamera.MV_USB3_DEVICE_INFO)Marshal.PtrToStructure(buffer, typeof(MyCamera.MV_USB3_DEVICE_INFO));

                                int key;

                                if (int.TryParse(usbInfo.chUserDefinedName, out key))
                                {
                                    if (CameraDic.Keys.Contains(key) == false)
                                    {
                                        CameraOperator m_pOperator = new CameraOperator();
                                        //打开设备
                                        nRet = m_pOperator.Open(ref device);
                                        if (MyCamera.MV_OK == nRet)
                                        {
                                            CameraDic.Add(key, new HikvisionCamera(m_pOperator, key));
                                            cameraCount++;
                                        }
                                    }
                                    else
                                    {
                                        Util.Notify("相机ID未设置");
                                    }
                                }
                            }
                        }
                    }

                    #endregion
                }
                if ((CameraFlag & CameraFlag.Mindvision) == CameraFlag.Mindvision)
                {
                    #region 迈徳威视相机遍历枚举

                    CameraSdkStatus status;
                    tSdkCameraDevInfo[] tCameraDevInfoList;

                    status = MvApi.CameraEnumerateDevice(out tCameraDevInfoList);
                    if (status == CameraSdkStatus.CAMERA_STATUS_SUCCESS)
                    {
                        if (tCameraDevInfoList != null && tCameraDevInfoList.Count() > 0)//此时iCameraCounts返回了实际连接的相机个数。如果大于1，则初始化第一个相机
                        {
                            for (int i = 0; i < tCameraDevInfoList.Count(); i++)
                            {
                                IntPtr m_Grabber;
                                status = MvApi.CameraGrabber_Create(out m_Grabber, ref tCameraDevInfoList[i]);
                                if (status == CameraSdkStatus.CAMERA_STATUS_SUCCESS)
                                {
                                    CameraHandle m_hCamera = 0;             // 句柄
                                    MvApi.CameraGrabber_GetCameraHandle(m_Grabber, out m_hCamera);
                                    byte[] pName = new byte[255];
                                    MvApi.CameraGetFriendlyName(m_hCamera, pName);
                                    string str = System.Text.Encoding.ASCII.GetString(pName).Substring(0, 1);

                                    int key = -1;

                                    if (int.TryParse(str, out key) && CameraDic.Keys.Contains(key) == false)
                                    {
                                        CameraDic.Add(key, new MindCamera(m_hCamera, m_Grabber, key));
                                        cameraCount++;
                                    }
                                    //else
                                    //{
                                    //    Util.Notify("相机ID未设置");
                                    //}
                                }
                            }
                        }
                    }

                    #endregion
                }
                if ((CameraFlag & CameraFlag.Microvision) == CameraFlag.Microvision)
                {
                    #region 维视图像相机相机遍历枚举

                    //个数枚举
                    MVGigE.MVInitLib();
                    int CamNum = 0;
                    MVSTATUS r = MVGigE.MVGetNumOfCameras(out CamNum);
                    for (int i = 0; i < CamNum; i++)
                    {
                        IntPtr m_hCam;
                        byte index = (byte)i;
                        r = MVGigE.MVOpenCamByIndex(index, out m_hCam);
                        if (m_hCam != IntPtr.Zero)
                        {
                            MVCamInfo pCamInfo;
                            MVGigE.MVGetCameraInfo(index, out pCamInfo);

                            string str = pCamInfo.mUserDefinedName;

                            int key = -1;

                            if (int.TryParse(str, out key) && CameraDic.Keys.Contains(key) == false)
                            {
                                CameraDic.Add(key, new MicrovisionCamera(m_hCam, key));
                                cameraCount++;
                            }
                            else
                            {
                                Util.Notify("相机ID未设置");
                            }

                        }
                    }

                    #endregion
                }
                if ((CameraFlag & CameraFlag.ICImaging) == CameraFlag.ICImaging)
                {
                    #region 映美精相机遍历查询
                    TIS.Imaging.ICImagingControl cameraFind = new TIS.Imaging.ICImagingControl();

                    foreach (TIS.Imaging.Device Item in cameraFind.Devices)
                    {
                        string cameraName = Item.Name;
                        int index1 = cameraName.IndexOf("[");
                        if (index1 > -1)
                        {
                            string dat = cameraName.Substring(index1);
                            string dat1 = dat.Replace("[", "").Replace("]", "");
                            int id1;
                            if (int.TryParse(dat1, out id1))
                            {
                                if (CameraDic.Keys.Contains(id1) == false)
                                {
                                    TIS.Imaging.ICImagingControl cameraTmp = new TIS.Imaging.ICImagingControl();
                                    cameraTmp.Device = Item.Name;
                                    bool x = cameraTmp.DeviceValid;
                                    CameraDic.Add(id1, new ICImagingCamera(cameraTmp, id1));
                                    cameraCount++;
                                }
                            }
                        }

                    }

                    #endregion
                }
                if ((CameraFlag & CameraFlag.DirectShow) == CameraFlag.DirectShow)
                {
                    #region DirectShow相机 当前只支持一个相机
                    HFramegrabber framegrabber = new HFramegrabber();
                    //HTuple Information = null;
                    //HTuple ValueList = null;
                    //HOperatorSet.InfoFramegrabber("DirectShow", "info_boards",out Information,out ValueList);
                    framegrabber.OpenFramegrabber("DirectShow", 1, 1, 0, 0, 0, 0, "default", 8, DirectShowColorSpace,
            -1, "false", DirectShowCameraType, DirectShowIndex, 0, -1);
                    CameraDic.Add(1, new DirectShowCamera(framegrabber, 1));
                    #endregion
                }
                if ((CameraFlag & CameraFlag.DaHeng) == CameraFlag.DaHeng)
                {
                    //#region 大恒相机
                    //List<IGXDeviceInfo> listGXDeviceInfo = new List<IGXDeviceInfo>();
                    //IGXFactory IGXFactory1 = IGXFactory.GetInstance();
                    //IGXFactory1.Init();
                    //IGXFactory1.UpdateDeviceList(200, listGXDeviceInfo);
                    //foreach (var item in listGXDeviceInfo)
                    //{
                    //    string name = item.GetUserID();
                    //    int key;

                    //    if (int.TryParse(name, out key))
                    //    {
                    //        if (CameraDic.Keys.Contains(key) == false)
                    //        {
                    //            IGXDevice IGXDevice1 = null;
                    //            IGXDevice1 = IGXFactory1.OpenDeviceBySN(item.GetSN(), GX_ACCESS_MODE.GX_ACCESS_EXCLUSIVE);
                    //            CameraDic.Add(key, new DHCamera(IGXFactory1, IGXDevice1));

                    //        }
                    //    }

                    //}

                    //#endregion
                }

                if ((CameraFlag & CameraFlag.Gige) == CameraFlag.Gige)
                {
                    //#region Gige相机遍历查询

                    HTuple Information = null;
                    HTuple ValueList = null;
                    HOperatorSet.InfoFramegrabber("GigEVision", "info_boards", out Information, out ValueList);
                    for (int i = 0; i < ValueList.Length; i++)
                    {
                        string valueTmp = ValueList[i];
                        string name = GetGigeValue(valueTmp, "user_name:");

                        string device = GetGigeValue(valueTmp, "device:");
                        if (name != null && device != null)
                        {
                            int id1;
                            if (int.TryParse(name, out id1))
                            {
                                if (CameraDic.Keys.Contains(id1) == false)
                                {
                                    HFramegrabber grabber = new HFramegrabber();
                                    grabber.OpenFramegrabber("GigEVision", 0, 0, 0, 0, 0, 0, "default", -1,
                                    "default", -1, "false", "default", device, 0, -1);
                                    CameraDic.Add(id1, new GigeCamera(grabber, id1));
                                    cameraCount++;
                                }
                            }
                        }
                    }
                    //List<ICameraInfo> g_allCameras = CameraFinder.Enumerate();

                    //if (g_allCameras.Count > 0)
                    //{
                    //    for (int i = 0; i < g_allCameras.Count; i++)
                    //    {
                    //        Basler.Pylon.Camera g_camera = new Basler.Pylon.Camera(g_allCameras[i]);
                    //        if (true == g_camera.IsOpen)
                    //            g_camera.Close();

                    //        g_camera.Open();
                    //        string id = g_camera.Parameters[PLCamera.DeviceUserID].GetValue();
                    //        int id1;
                    //        if (int.TryParse(id, out id1))
                    //        {
                    //            if (CameraDic.Keys.Contains(id1) == false)
                    //            {
                    //                CameraDic.Add(id1, new BaslerCamera(g_camera, id1));
                    //                cameraCount++;
                    //            }
                    //        }
                    //        else
                    //        {
                    //            Util.Notify("相机ID未设置");
                    //        }
                    //    }
                    //}

                    //#endregion
                }
                //打开相机
                foreach (var item in CameraDic.Values)
                {
                    item.Open();
                    Thread.Sleep(30);
                }

                if (CameraInitFinishEvent != null)
                {
                    CameraInitFinishEvent(null, null);
                }
                //else
                //{
                //    Util.Notify("无相机连接");
                //    return false;
                //}
            }
            catch (Exception ex)
            {
                Util.WriteLog(typeof(CameraManger), ex);
                Util.Notify("相机打开出现异常");
                Close();
                return false;
            }
            return true;
        }

        public static void Close()
        {
            foreach (var item in CameraDic.Values)
            {
                try
                {
                    item.Close();
                }
                catch (Exception ex)
                {
                    Util.WriteLog(typeof(CameraManger), ex);
                    Util.Notify("相机关闭出现异常");
                }
            }
            CameraDic = new Dictionary<int, CameraBase>();
        }
    }
}
