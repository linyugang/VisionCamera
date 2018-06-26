/*
 * 工业相机C# SDK开发例程
 * 更多资料，请参考SDK开发手册
 */

//BIG5 TRANS ALLOWED

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;

//相机句柄，SDK支持同时打开多个相机，用该句柄区分多相机
using CameraHandle = System.Int32;


namespace MVSDK
{

    //SDK接口的返回值，定义如下
    public enum CameraSdkStatus
    {
        CAMERA_STATUS_SUCCESS = 0,   // 操作成功
        CAMERA_STATUS_FAILED = -1,   // 操作失败
        CAMERA_STATUS_intERNAL_ERROR = -2,   // 内部错误
        CAMERA_STATUS_UNKNOW = -3,   // 未知错误
        CAMERA_STATUS_NOT_SUPPORTED = -4,   // 不支持该功能
        CAMERA_STATUS_NOT_INITIALIZED = -5,   // 初始化未完成
        CAMERA_STATUS_PARAMETER_INVALID = -6,   // 参数无效
        CAMERA_STATUS_PARAMETER_OUT_OF_BOUND = -7,   // 参数越界
        CAMERA_STATUS_UNENABLED = -8,   // 未使能
        CAMERA_STATUS_USER_CANCEL = -9,   // 用户手动取消了，比如roi面板点击取消，返回
        CAMERA_STATUS_PATH_NOT_FOUND = -10,  // 注册表中没有找到对应的路径
        CAMERA_STATUS_SIZE_DISMATCH = -11,  // 获得图像数据长度和定义的尺寸不匹配
        CAMERA_STATUS_TIME_OUT = -12,  // 超时错误
        CAMERA_STATUS_IO_ERROR = -13,  // 硬件IO错误
        CAMERA_STATUS_COMM_ERROR = -14,  // 通讯错误
        CAMERA_STATUS_BUS_ERROR = -15,  // 总线错误
        CAMERA_STATUS_NO_DEVICE_FOUND = -16,  // 没有发现设备
        CAMERA_STATUS_NO_LOGIC_DEVICE_FOUND = -17,  // 未找到逻辑设备
        CAMERA_STATUS_DEVICE_IS_OPENED = -18,  // 设备已经打开
        CAMERA_STATUS_DEVICE_IS_CLOSED = -19,  // 设备已经关闭
        CAMERA_STATUS_DEVICE_VEDIO_CLOSED = -20,  // 没有打开设备视频，调用录像相关的函数时，如果相机视频没有打开，则回返回该错误。
        CAMERA_STATUS_NO_MEMORY = -21,  // 没有足够系统内存
        CAMERA_STATUS_FILE_CREATE_FAILED = -22,  // 创建文件失败
        CAMERA_STATUS_FILE_INVALID = -23,  // 文件格式无效
        CAMERA_STATUS_WRITE_PROTECTED = -24,  // 写保护，不可写
        CAMERA_STATUS_GRAB_FAILED = -25,  // 数据采集失败
        CAMERA_STATUS_LOST_DATA = -26,  // 数据丢失，不完整
        CAMERA_STATUS_EOF_ERROR = -27,  // 未接收到帧结束符
        CAMERA_STATUS_BUSY = -28,  // 正忙(上一次操作还在进行中)，此次操作不能进行
        CAMERA_STATUS_WAIT = -29,  // 需要等待(进行操作的条件不成立)，可以再次尝试
        CAMERA_STATUS_IN_PROCESS = -30,  // 正在进行，已经被操作过
        CAMERA_STATUS_IIC_ERROR = -31,  // IIC传输错误
        CAMERA_STATUS_SPI_ERROR = -32,  // SPI传输错误
        CAMERA_STATUS_USB_CONTROL_ERROR = -33,  // USB控制传输错误
        CAMERA_STATUS_USB_BULK_ERROR = -34,  // USB BULK传输错误
        CAMERA_STATUS_SOCKET_INIT_ERROR = -35,  // 网络传输套件初始化失败
        CAMERA_STATUS_GIGE_FILTER_INIT_ERROR = -36,  // 网络相机内核过滤驱动初始化失败，请检查是否正确安装了驱动，或者重新安装。
        CAMERA_STATUS_NET_SEND_ERROR = -37,  // 网络数据发送错误
        CAMERA_STATUS_DEVICE_LOST = -38,  // 与网络相机失去连接，心跳检测超时
        CAMERA_STATUS_DATA_RECV_LESS = -39,  // 接收到的字节数比请求的少 
        CAMERA_STATUS_FUNCTION_LOAD_FAILED = -40,  // 从文件中加载程序失败
        CAMERA_STATUS_CRITICAL_FILE_LOST = -41,  // 程序运行所必须的文件丢失。
        CAMERA_STATUS_SENSOR_ID_DISMATCH = -42,  // 固件和程序不匹配，原因是下载了错误的固件。
        CAMERA_STATUS_OUT_OF_RANGE = -43,  // 参数超出有效范围。   
        CAMERA_STATUS_REGISTRY_ERROR = -44,  // 安装程序注册错误。请重新安装程序，或者运行安装目录Setup/Installer.exe
        CAMERA_STATUS_ACCESS_DENY = -45,  // 禁止访问。指定相机已经被其他程序占用时，再申请访问该相机，会返回该状态。(一个相机不能被多个程序同时访问) 
        //AIA的标准兼容的错误码
        CAMERA_AIA_PACKET_RESEND = 0x0100, //该帧需要重传
        CAMERA_AIA_NOT_IMPLEMENTED = 0x8001, //设备不支持的命令
        CAMERA_AIA_INVALID_PARAMETER = 0x8002, //命令参数非法
        CAMERA_AIA_INVALID_ADDRESS = 0x8003, //不可访问的地址
        CAMERA_AIA_WRITE_PROTECT = 0x8004, //访问的对象不可写
        CAMERA_AIA_BAD_ALIGNMENT = 0x8005, //访问的地址没有按照要求对齐
        CAMERA_AIA_ACCESS_DENIED = 0x8006, //没有访问权限
        CAMERA_AIA_BUSY = 0x8007, //命令正在处理中
        CAMERA_AIA_DEPRECATED = 0x8008, //0x8008-0x0800B  0x800F  该指令已经废弃
        CAMERA_AIA_PACKET_UNAVAILABLE = 0x800C, //包无效
        CAMERA_AIA_DATA_OVERRUN = 0x800D, //数据溢出，通常是收到的数据比需要的多
        CAMERA_AIA_INVALID_HEADER = 0x800E, //数据包头部中某些区域与协议不匹配
        CAMERA_AIA_PACKET_NOT_YET_AVAILABLE = 0x8010, //图像分包数据还未准备好，多用于触发模式，应用程序访问超时
        CAMERA_AIA_PACKET_AND_PREV_REMOVED_FROM_MEMORY = 0x8011, //需要访问的分包已经不存在。多用于重传时数据已经不在缓冲区中
        CAMERA_AIA_PACKET_REMOVED_FROM_MEMORY = 0x8012, //CAMERA_AIA_PACKET_AND_PREV_REMOVED_FROM_MEMORY
        CAMERA_AIA_NO_REF_TIME = 0x0813, //没有参考时钟源。多用于时间同步的命令执行时
        CAMERA_AIA_PACKET_TEMPORARILY_UNAVAILABLE = 0x0814, //由于信道带宽问题，当前分包暂时不可用，需稍后进行访问
        CAMERA_AIA_OVERFLOW = 0x0815, //设备端数据溢出，通常是队列已满
        CAMERA_AIA_ACTION_LATE = 0x0816, //命令执行已经超过有效的指定时间
        CAMERA_AIA_ERROR = 0x8FFF   //错误
    }

    /*
       
    //tSdkResolutionRange结构体中SKIP、 BIN、RESAMPLE模式的掩码值
    MASK_2X2_HD   (1<<0)    //硬件SKIP、BIN、重采样 2X2
    MASK_3X3_HD   (1<<1)
    MASK_4X4_HD   (1<<2)
    MASK_5X5_HD   (1<<3)
    MASK_6X6_HD   (1<<4)
    MASK_7X7_HD   (1<<5)
    MASK_8X8_HD   (1<<6)
    MASK_9X9_HD   (1<<7)      
    MASK_10X10_HD   (1<<8)
    MASK_11X11_HD   (1<<9)
    MASK_12X12_HD   (1<<10)
    MASK_13X13_HD   (1<<11)
    MASK_14X14_HD   (1<<12)
    MASK_15X15_HD   (1<<13)
    MASK_16X16_HD   (1<<14)
    MASK_17X17_HD   (1<<15)
    MASK_2X2_SW   (1<<16) //硬件SKIP、BIN、重采样 2X2
    MASK_3X3_SW   (1<<17)
    MASK_4X4_SW   (1<<18)
    MASK_5X5_SW   (1<<19)
    MASK_6X6_SW   (1<<20)
    MASK_7X7_SW   (1<<21)
    MASK_8X8_SW   (1<<22)
    MASK_9X9_SW   (1<<23)     
    MASK_10X10_SW   (1<<24)
    MASK_11X11_SW   (1<<25)
    MASK_12X12_SW   (1<<26)
    MASK_13X13_SW   (1<<27)
    MASK_14X14_SW   (1<<28)
    MASK_15X15_SW   (1<<29)
    MASK_16X16_SW   (1<<30)
    MASK_17X17_SW   (1<<31) 
     */



    //图像查表变换的方式
    public enum emSdkLutMode
    {
        LUTMODE_PARAM_GEN = 0,//通过调节参数动态生成LUT表
        LUTMODE_PRESET,     //使用预设的LUT表
        LUTMODE_USER_DEF    //使用用户自定义的LUT表
    }

    //相机的视频流控制
    public enum emSdkRunMode
    {
        RUNMODE_PLAY = 0,    //正常预览，捕获到图像就显示。（如果相机处于触发模式，则会等待触发帧的到来）
        RUNMODE_PAUSE,     //暂停，会暂停相机的图像输出，同时也不会去捕获图像
        RUNMODE_STOP       //停止相机工作。反初始化后，相机就处于停止模式
    }

    //SDK内部显示接口的显示方式
    public enum emSdkDisplayMode
    {
        DISPLAYMODE_SCALE = 0,  //缩放显示模式，缩放到显示控件的尺寸
        DISPLAYMODE_REAL      //1:1显示模式，当图像尺寸大于显示控件的尺寸时，只显示局部  
    }

    //录像状态
    public enum emSdkRecordMode
    {
        RECORD_STOP = 0,  //停止
        RECORD_START,     //录像中
        RECORD_PAUSE      //暂停
    }

    //图像的镜像操作
    public enum emSdkMirrorDirection
    {
        MIRROR_DIRECTION_HORIZONTAL = 0,//水平镜像
        MIRROR_DIRECTION_VERTICAL       //垂直镜像
    }

    //相机视频的帧率
    public enum emSdkFrameSpeed
    {
        FRAME_SPEED_LOW = 0,  //低速模式
        FRAME_SPEED_NORMAL,   //普通模式
        FRAME_SPEED_HIGH,     //高速模式(需要较高的传输带宽,多设备共享传输带宽时会对帧率的稳定性有影响)
        FRAME_SPEED_SUPER     //超高速模式(需要较高的传输带宽,多设备共享传输带宽时会对帧率的稳定性有影响)
    }

    //保存文件的格式类型
    public enum emSdkFileType
    {
        FILE_JPG = 1,//JPG
        FILE_BMP = 2,//BMP
        FILE_RAW = 4,//相机输出的bayer格式文件,对于不支持bayer格式输出相机，无法保存为该格式
        FILE_PNG = 8 //PNG
    }

    //相机中的图像传感器的工作模式
    public enum emSdkSnapMode
    {
        CONTINUATION = 0,//连续采集模式
        SOFT_TRIGGER,    //软件触发模式，由软件发送指令后，传感器开始采集指定帧数的图像，采集完成后，停止输出
        EXTERNAL_TRIGGER //硬件触发模式，当接收到外部信号，传感器开始采集指定帧数的图像，采集完成后，停止输出
    }

    //自动曝光时抗频闪的频闪
    public enum emSdkLightFrequency
    {
        LIGHT_FREQUENCY_50HZ = 0,//50HZ,一般的灯光都是50HZ
        LIGHT_FREQUENCY_60HZ     //60HZ,主要是指显示器的
    }

    //相机的配置参数，分为A,B,C,D 4组进行保存。
    public enum emSdkParameterTeam
    {
        PARAMETER_TEAM_DEFAULT = 0xff,
        PARAMETER_TEAM_A = 0,
        PARAMETER_TEAM_B = 1,
        PARAMETER_TEAM_C = 2,
        PARAMETER_TEAM_D = 3
    }

    //相机参数加载模式，参数加载分为从文件和从设备加载两种方式
    public enum emSdkParameterMode
    {
        PARAM_MODE_BY_MODEL = 0,  //根据相机型号名从文件中加载参数，例如MV-U300
        PARAM_MODE_BY_NAME,       //根据设备昵称(tSdkCameraDevInfo.acFriendlyName)从文件中加载参数，例如MV-U300,该昵称可自定义
        PARAM_MODE_BY_SN,         //根据设备的唯一序列号从文件中加载参数，序列号在出厂时已经写入设备，每台相机拥有不同的序列号。
        PARAM_MODE_IN_DEVICE      //从设备的固态存储器中加载参数。不是所有的型号都支持从相机中读写参数组，由tSdkCameraCapbility.bParamInDevice决定
    }

    //SDK生成的相机配置页面掩码值
    public enum emSdkPropSheetMask
    {
        PROP_SHEET_INDEX_EXPOSURE = 0,
        PROP_SHEET_INDEX_ISP_COLOR,
        PROP_SHEET_INDEX_ISP_LUT,
        PROP_SHEET_INDEX_ISP_SHAPE,
        PROP_SHEET_INDEX_VIDEO_FORMAT,
        PROP_SHEET_INDEX_RESOLUTION,
        PROP_SHEET_INDEX_IO_CTRL,
        PROP_SHEET_INDEX_TRIGGER_SET,
        PROP_SHEET_INDEX_OVERLAY
    }

    //SDK生成的相机配置页面的回调消息类型
    public enum emSdkPropSheetMsg
    {
        SHEET_MSG_LOAD_PARAM_DEFAULT = 0, //参数被恢复成默认后，触发该消息
        SHEET_MSG_LOAD_PARAM_GROUP,       //加载指定参数组，触发该消息
        SHEET_MSG_LOAD_PARAM_FROMFILE,    //从指定文件加载参数后，触发该消息
        SHEET_MSG_SAVE_PARAM_GROUP        //当前参数组被保存时，触发该消息
    }

    //可视化选择参考窗口的类型
    public enum emSdkRefWintype
    {
        REF_WIN_AUTO_EXPOSURE = 0,
        REF_WIN_WHITE_BALANCE,
    }

    //闪光灯信号控制方式
    public enum emStrobeControl
    {
        STROBE_SYNC_WITH_TRIG_AUTO = 0,    //和触发信号同步，触发后，相机进行曝光时，自动生成STROBE信号。此时，有效极性可设置(CameraSetStrobePolarity)。
        STROBE_SYNC_WITH_TRIG_MANUAL,      //和触发信号同步，触发后，STROBE延时指定的时间后(CameraSetStrobeDelayTime)，再持续指定时间的脉冲(CameraSetStrobePulseWidth)，有效极性可设置(CameraSetStrobePolarity)。
        STROBE_ALWAYS_HIGH,                //始终为高，忽略STROBE信号的其他设置
        STROBE_ALWAYS_LOW                  //始终为低，忽略STROBE信号的其他设置
    }

    //硬件外触发的信号种类
    public enum emExtTrigSignal
    {
        EXT_TRIG_LEADING_EDGE = 0,     //上升沿触发，默认为该方式
        EXT_TRIG_TRAILING_EDGE,        //下降沿触发  
        EXT_TRIG_HIGH_LEVEL,           //高电平触发,电平宽度决定曝光时间，仅部分型号的相机支持电平触发方式。
        EXT_TRIG_LOW_LEVEL             //低电平触发,
    }

    //硬件外触发时的快门方式
    public enum emExtTrigShutterMode
    {
        EXT_TRIG_EXP_STANDARD = 0,     //标准方式，默认为该方式。
        EXT_TRIG_EXP_GRR,              //全局复位方式，部分滚动快门的CMOS型号的相机支持该方式，配合外部机械快门，可以达到全局快门的效果，适合拍高速运动的物体
    }

    public enum emImageFormat
    {
        CAMERA_MEDIA_TYPE_MONO = 0x01000000,
        CAMERA_MEDIA_TYPE_RGB = 0x02000000,
        CAMERA_MEDIA_TYPE_COLOR = 0x02000000,
        CAMERA_MEDIA_TYPE_OCCUPY1BIT = 0x00010000,
        CAMERA_MEDIA_TYPE_OCCUPY2BIT = 0x00020000,
        CAMERA_MEDIA_TYPE_OCCUPY4BIT = 0x00040000,
        CAMERA_MEDIA_TYPE_OCCUPY8BIT = 0x00080000,
        CAMERA_MEDIA_TYPE_OCCUPY10BIT = 0x000A0000,
        CAMERA_MEDIA_TYPE_OCCUPY12BIT = 0x000C0000,
        CAMERA_MEDIA_TYPE_OCCUPY16BIT = 0x00100000,
        CAMERA_MEDIA_TYPE_OCCUPY24BIT = 0x00180000,
        CAMERA_MEDIA_TYPE_OCCUPY32BIT = 0x00200000,
        CAMERA_MEDIA_TYPE_OCCUPY36BIT = 0x00240000,
        CAMERA_MEDIA_TYPE_OCCUPY48BIT = 0x00300000,
        CAMERA_MEDIA_TYPE_EFFECTIVE_PIXEL_SIZE_MASK = 0x00FF0000,
        CAMERA_MEDIA_TYPE_EFFECTIVE_PIXEL_SIZE_SHIFT = 16,


        CAMERA_MEDIA_TYPE_ID_MASK = 0x0000FFFF,
        CAMERA_MEDIA_TYPE_COUNT = 0x46,

        /*mono*/
        CAMERA_MEDIA_TYPE_MONO1P = (CAMERA_MEDIA_TYPE_MONO | CAMERA_MEDIA_TYPE_OCCUPY1BIT | 0x0037),
        CAMERA_MEDIA_TYPE_MONO2P = (CAMERA_MEDIA_TYPE_MONO | CAMERA_MEDIA_TYPE_OCCUPY2BIT | 0x0038),
        CAMERA_MEDIA_TYPE_MONO4P = (CAMERA_MEDIA_TYPE_MONO | CAMERA_MEDIA_TYPE_OCCUPY4BIT | 0x0039),
        CAMERA_MEDIA_TYPE_MONO8 = (CAMERA_MEDIA_TYPE_MONO | CAMERA_MEDIA_TYPE_OCCUPY8BIT | 0x0001),
        CAMERA_MEDIA_TYPE_MONO8S = (CAMERA_MEDIA_TYPE_MONO | CAMERA_MEDIA_TYPE_OCCUPY8BIT | 0x0002),
        CAMERA_MEDIA_TYPE_MONO10 = (CAMERA_MEDIA_TYPE_MONO | CAMERA_MEDIA_TYPE_OCCUPY16BIT | 0x0003),
        CAMERA_MEDIA_TYPE_MONO10_PACKED = (CAMERA_MEDIA_TYPE_MONO | CAMERA_MEDIA_TYPE_OCCUPY12BIT | 0x0004),
        CAMERA_MEDIA_TYPE_MONO12 = (CAMERA_MEDIA_TYPE_MONO | CAMERA_MEDIA_TYPE_OCCUPY16BIT | 0x0005),
        CAMERA_MEDIA_TYPE_MONO12_PACKED = (CAMERA_MEDIA_TYPE_MONO | CAMERA_MEDIA_TYPE_OCCUPY12BIT | 0x0006),
        CAMERA_MEDIA_TYPE_MONO14 = (CAMERA_MEDIA_TYPE_MONO | CAMERA_MEDIA_TYPE_OCCUPY16BIT | 0x0025),
        CAMERA_MEDIA_TYPE_MONO16 = (CAMERA_MEDIA_TYPE_MONO | CAMERA_MEDIA_TYPE_OCCUPY16BIT | 0x0007),

        /*Bayer */
        CAMERA_MEDIA_TYPE_BAYGR8 = (CAMERA_MEDIA_TYPE_MONO | CAMERA_MEDIA_TYPE_OCCUPY8BIT | 0x0008),
        CAMERA_MEDIA_TYPE_BAYRG8 = (CAMERA_MEDIA_TYPE_MONO | CAMERA_MEDIA_TYPE_OCCUPY8BIT | 0x0009),
        CAMERA_MEDIA_TYPE_BAYGB8 = (CAMERA_MEDIA_TYPE_MONO | CAMERA_MEDIA_TYPE_OCCUPY8BIT | 0x000A),
        CAMERA_MEDIA_TYPE_BAYBG8 = (CAMERA_MEDIA_TYPE_MONO | CAMERA_MEDIA_TYPE_OCCUPY8BIT | 0x000B),

        CAMERA_MEDIA_TYPE_BAYGR10_MIPI = (CAMERA_MEDIA_TYPE_MONO | CAMERA_MEDIA_TYPE_OCCUPY10BIT | 0x0026),
        CAMERA_MEDIA_TYPE_BAYRG10_MIPI = (CAMERA_MEDIA_TYPE_MONO | CAMERA_MEDIA_TYPE_OCCUPY10BIT | 0x0027),
        CAMERA_MEDIA_TYPE_BAYGB10_MIPI = (CAMERA_MEDIA_TYPE_MONO | CAMERA_MEDIA_TYPE_OCCUPY10BIT | 0x0028),
        CAMERA_MEDIA_TYPE_BAYBG10_MIPI = (CAMERA_MEDIA_TYPE_MONO | CAMERA_MEDIA_TYPE_OCCUPY10BIT | 0x0029),


        CAMERA_MEDIA_TYPE_BAYGR10 = (CAMERA_MEDIA_TYPE_MONO | CAMERA_MEDIA_TYPE_OCCUPY16BIT | 0x000C),
        CAMERA_MEDIA_TYPE_BAYRG10 = (CAMERA_MEDIA_TYPE_MONO | CAMERA_MEDIA_TYPE_OCCUPY16BIT | 0x000D),
        CAMERA_MEDIA_TYPE_BAYGB10 = (CAMERA_MEDIA_TYPE_MONO | CAMERA_MEDIA_TYPE_OCCUPY16BIT | 0x000E),
        CAMERA_MEDIA_TYPE_BAYBG10 = (CAMERA_MEDIA_TYPE_MONO | CAMERA_MEDIA_TYPE_OCCUPY16BIT | 0x000F),

        CAMERA_MEDIA_TYPE_BAYGR12 = (CAMERA_MEDIA_TYPE_MONO | CAMERA_MEDIA_TYPE_OCCUPY16BIT | 0x0010),
        CAMERA_MEDIA_TYPE_BAYRG12 = (CAMERA_MEDIA_TYPE_MONO | CAMERA_MEDIA_TYPE_OCCUPY16BIT | 0x0011),
        CAMERA_MEDIA_TYPE_BAYGB12 = (CAMERA_MEDIA_TYPE_MONO | CAMERA_MEDIA_TYPE_OCCUPY16BIT | 0x0012),
        CAMERA_MEDIA_TYPE_BAYBG12 = (CAMERA_MEDIA_TYPE_MONO | CAMERA_MEDIA_TYPE_OCCUPY16BIT | 0x0013),


        CAMERA_MEDIA_TYPE_BAYGR10_PACKED = (CAMERA_MEDIA_TYPE_MONO | CAMERA_MEDIA_TYPE_OCCUPY12BIT | 0x0026),
        CAMERA_MEDIA_TYPE_BAYRG10_PACKED = (CAMERA_MEDIA_TYPE_MONO | CAMERA_MEDIA_TYPE_OCCUPY12BIT | 0x0027),
        CAMERA_MEDIA_TYPE_BAYGB10_PACKED = (CAMERA_MEDIA_TYPE_MONO | CAMERA_MEDIA_TYPE_OCCUPY12BIT | 0x0028),
        CAMERA_MEDIA_TYPE_BAYBG10_PACKED = (CAMERA_MEDIA_TYPE_MONO | CAMERA_MEDIA_TYPE_OCCUPY12BIT | 0x0029),

        CAMERA_MEDIA_TYPE_BAYGR12_PACKED = (CAMERA_MEDIA_TYPE_MONO | CAMERA_MEDIA_TYPE_OCCUPY12BIT | 0x002A),
        CAMERA_MEDIA_TYPE_BAYRG12_PACKED = (CAMERA_MEDIA_TYPE_MONO | CAMERA_MEDIA_TYPE_OCCUPY12BIT | 0x002B),
        CAMERA_MEDIA_TYPE_BAYGB12_PACKED = (CAMERA_MEDIA_TYPE_MONO | CAMERA_MEDIA_TYPE_OCCUPY12BIT | 0x002C),
        CAMERA_MEDIA_TYPE_BAYBG12_PACKED = (CAMERA_MEDIA_TYPE_MONO | CAMERA_MEDIA_TYPE_OCCUPY12BIT | 0x002D),

        CAMERA_MEDIA_TYPE_BAYGR16 = (CAMERA_MEDIA_TYPE_MONO | CAMERA_MEDIA_TYPE_OCCUPY16BIT | 0x002E),
        CAMERA_MEDIA_TYPE_BAYRG16 = (CAMERA_MEDIA_TYPE_MONO | CAMERA_MEDIA_TYPE_OCCUPY16BIT | 0x002F),
        CAMERA_MEDIA_TYPE_BAYGB16 = (CAMERA_MEDIA_TYPE_MONO | CAMERA_MEDIA_TYPE_OCCUPY16BIT | 0x0030),
        CAMERA_MEDIA_TYPE_BAYBG16 = (CAMERA_MEDIA_TYPE_MONO | CAMERA_MEDIA_TYPE_OCCUPY16BIT | 0x0031),

        /*RGB */
        CAMERA_MEDIA_TYPE_RGB8 = (CAMERA_MEDIA_TYPE_COLOR | CAMERA_MEDIA_TYPE_OCCUPY24BIT | 0x0014),
        CAMERA_MEDIA_TYPE_BGR8 = (CAMERA_MEDIA_TYPE_COLOR | CAMERA_MEDIA_TYPE_OCCUPY24BIT | 0x0015),
        CAMERA_MEDIA_TYPE_RGBA8 = (CAMERA_MEDIA_TYPE_COLOR | CAMERA_MEDIA_TYPE_OCCUPY32BIT | 0x0016),
        CAMERA_MEDIA_TYPE_BGRA8 = (CAMERA_MEDIA_TYPE_COLOR | CAMERA_MEDIA_TYPE_OCCUPY32BIT | 0x0017),
        CAMERA_MEDIA_TYPE_RGB10 = (CAMERA_MEDIA_TYPE_COLOR | CAMERA_MEDIA_TYPE_OCCUPY48BIT | 0x0018),
        CAMERA_MEDIA_TYPE_BGR10 = (CAMERA_MEDIA_TYPE_COLOR | CAMERA_MEDIA_TYPE_OCCUPY48BIT | 0x0019),
        CAMERA_MEDIA_TYPE_RGB12 = (CAMERA_MEDIA_TYPE_COLOR | CAMERA_MEDIA_TYPE_OCCUPY48BIT | 0x001A),
        CAMERA_MEDIA_TYPE_BGR12 = (CAMERA_MEDIA_TYPE_COLOR | CAMERA_MEDIA_TYPE_OCCUPY48BIT | 0x001B),
        CAMERA_MEDIA_TYPE_RGB16 = (CAMERA_MEDIA_TYPE_COLOR | CAMERA_MEDIA_TYPE_OCCUPY48BIT | 0x0033),
        CAMERA_MEDIA_TYPE_RGB10V1_PACKED = (CAMERA_MEDIA_TYPE_COLOR | CAMERA_MEDIA_TYPE_OCCUPY32BIT | 0x001C),
        CAMERA_MEDIA_TYPE_RGB10P32 = (CAMERA_MEDIA_TYPE_COLOR | CAMERA_MEDIA_TYPE_OCCUPY32BIT | 0x001D),
        CAMERA_MEDIA_TYPE_RGB12V1_PACKED = (CAMERA_MEDIA_TYPE_COLOR | CAMERA_MEDIA_TYPE_OCCUPY36BIT | 0X0034),
        CAMERA_MEDIA_TYPE_RGB565P = (CAMERA_MEDIA_TYPE_COLOR | CAMERA_MEDIA_TYPE_OCCUPY16BIT | 0x0035),
        CAMERA_MEDIA_TYPE_BGR565P = (CAMERA_MEDIA_TYPE_COLOR | CAMERA_MEDIA_TYPE_OCCUPY16BIT | 0X0036),

        /*YUV and YCbCr*/
        CAMERA_MEDIA_TYPE_YUV411_8_UYYVYY = (CAMERA_MEDIA_TYPE_COLOR | CAMERA_MEDIA_TYPE_OCCUPY12BIT | 0x001E),
        CAMERA_MEDIA_TYPE_YUV422_8_UYVY = (CAMERA_MEDIA_TYPE_COLOR | CAMERA_MEDIA_TYPE_OCCUPY16BIT | 0x001F),
        CAMERA_MEDIA_TYPE_YUV422_8 = (CAMERA_MEDIA_TYPE_COLOR | CAMERA_MEDIA_TYPE_OCCUPY16BIT | 0x0032),
        CAMERA_MEDIA_TYPE_YUV8_UYV = (CAMERA_MEDIA_TYPE_COLOR | CAMERA_MEDIA_TYPE_OCCUPY24BIT | 0x0020),
        CAMERA_MEDIA_TYPE_YCBCR8_CBYCR = (CAMERA_MEDIA_TYPE_COLOR | CAMERA_MEDIA_TYPE_OCCUPY24BIT | 0x003A),
        //CAMERA_MEDIA_TYPE_YCBCR422_8 : YYYYCbCrCbCr
        CAMERA_MEDIA_TYPE_YCBCR422_8 = (CAMERA_MEDIA_TYPE_COLOR | CAMERA_MEDIA_TYPE_OCCUPY16BIT | 0x003B),
        CAMERA_MEDIA_TYPE_YCBCR422_8_CBYCRY = (CAMERA_MEDIA_TYPE_COLOR | CAMERA_MEDIA_TYPE_OCCUPY16BIT | 0x0043),
        CAMERA_MEDIA_TYPE_YCBCR411_8_CBYYCRYY = (CAMERA_MEDIA_TYPE_COLOR | CAMERA_MEDIA_TYPE_OCCUPY12BIT | 0x003C),
        CAMERA_MEDIA_TYPE_YCBCR601_8_CBYCR = (CAMERA_MEDIA_TYPE_COLOR | CAMERA_MEDIA_TYPE_OCCUPY24BIT | 0x003D),
        CAMERA_MEDIA_TYPE_YCBCR601_422_8 = (CAMERA_MEDIA_TYPE_COLOR | CAMERA_MEDIA_TYPE_OCCUPY16BIT | 0x003E),
        CAMERA_MEDIA_TYPE_YCBCR601_422_8_CBYCRY = (CAMERA_MEDIA_TYPE_COLOR | CAMERA_MEDIA_TYPE_OCCUPY16BIT | 0x0044),
        CAMERA_MEDIA_TYPE_YCBCR601_411_8_CBYYCRYY = (CAMERA_MEDIA_TYPE_COLOR | CAMERA_MEDIA_TYPE_OCCUPY12BIT | 0x003F),
        CAMERA_MEDIA_TYPE_YCBCR709_8_CBYCR = (CAMERA_MEDIA_TYPE_COLOR | CAMERA_MEDIA_TYPE_OCCUPY24BIT | 0x0040),
        CAMERA_MEDIA_TYPE_YCBCR709_422_8 = (CAMERA_MEDIA_TYPE_COLOR | CAMERA_MEDIA_TYPE_OCCUPY16BIT | 0x0041),
        CAMERA_MEDIA_TYPE_YCBCR709_422_8_CBYCRY = (CAMERA_MEDIA_TYPE_COLOR | CAMERA_MEDIA_TYPE_OCCUPY16BIT | 0x0045),
        CAMERA_MEDIA_TYPE_YCBCR709_411_8_CBYYCRYY = (CAMERA_MEDIA_TYPE_COLOR | CAMERA_MEDIA_TYPE_OCCUPY12BIT | 0x0042),

        /*RGB Planar */
        CAMERA_MEDIA_TYPE_RGB8_PLANAR = (CAMERA_MEDIA_TYPE_COLOR | CAMERA_MEDIA_TYPE_OCCUPY24BIT | 0x0021),
        CAMERA_MEDIA_TYPE_RGB10_PLANAR = (CAMERA_MEDIA_TYPE_COLOR | CAMERA_MEDIA_TYPE_OCCUPY48BIT | 0x0022),
        CAMERA_MEDIA_TYPE_RGB12_PLANAR = (CAMERA_MEDIA_TYPE_COLOR | CAMERA_MEDIA_TYPE_OCCUPY48BIT | 0x0023),
        CAMERA_MEDIA_TYPE_RGB16_PLANAR = (CAMERA_MEDIA_TYPE_COLOR | CAMERA_MEDIA_TYPE_OCCUPY48BIT | 0x0024),
    }




    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct BITMAPFILEHEADER
    {
        public ushort bfType;
        public uint bfSize;
        public ushort bfReserved1;
        public ushort bfReserved2;
        public uint bfOffBits;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct BITMAPINFOHEADER
    {
        public uint biSize;
        public int biWidth;
        public int biHeight;
        public ushort biPlanes;
        public ushort biBitCount;
        public uint biCompression;
        public uint biSizeImage;
        public int biXPelsPerMeter;
        public int biYPelsPerMeter;
        public uint biClrUsed;
        public uint biClrImportant;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct RGBQUAD
    {
        public byte rgbBlue;
        public byte rgbGreen;
        public byte rgbRed;
        public byte rgbReserved;
    }

    //相机的设备信息，只读信息，请勿修改
    public struct tSdkCameraDevInfo
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] acProductSeries; // 产品系列
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] acProductName;    // 产品名称
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] acFriendlyName;   // 昵称，后加#和索引号来区分
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] acLinkName;       // 设备符号连接名，内部使用
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] acDriverVersion;  // 驱动版本
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] acSensorType;     // sensor类型
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] acPortType;       // 接口类型  
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] acSn;             // 产品唯一序列号
        public uint uInstance;        // 该型号相机在该电脑上的实例索引号，用于区分同型号多相机

    }

    //相机的分辨率设定范围
    public struct tSdkResolutionRange
    {
        public int iHeightMax;            //图像最大高度
        public int iHeightMin;            //图像最小高度
        public int iWidthMax;             //图像最大宽度
        public int iWidthMin;             //图像最小宽度
        public int uSkipModeMask;         //SKIP模式掩码，为0，表示不支持SKIP 。bit0为1,表示支持SKIP 2x2 ;bit1为1，表示支持SKIP 3x3....
        public int uBinSumModeMask;       //BIN(求和)模式掩码，为0，表示不支持BIN 。bit0为1,表示支持BIN 2x2 ;bit1为1，表示支持BIN 3x3....
        public int uBinAverageModeMask;   //BIN(求均值)模式掩码，为0，表示不支持BIN 。bit0为1,表示支持BIN 2x2 ;bit1为1，表示支持BIN 3x3....
        public int uResampleMask;         //硬件重采样的掩码
    }

    //相机的分辨率描述
    public struct tSdkImageResolution
    {
        public int iIndex;               // 索引号，[0,N]表示预设的分辨率(N 为预设分辨率的最大个数，一般不超过20),OXFF 表示自定义分辨率(ROI)
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] acDescription;        // 该分辨率的描述信息。仅预设分辨率时该信息有效。自定义分辨率可忽略该信息
        public uint uBinSumMode;          // BIN(求和)的模式,范围不能超过tSdkResolutionRange中uBinSumModeMask
        public uint uBinAverageMode;      // BIN(求均值)的模式,范围不能超过tSdkResolutionRange中uBinAverageModeMask
        public uint uSkipMode;            // 是否SKIP的尺寸，为0表示禁止SKIP模式，范围不能超过tSdkResolutionRange中uSkipModeMask
        public uint uResampleMask;        // 硬件重采样的掩码
        public int iHOffsetFOV;        // 采集视场相对于Sensor最大视场左上角的垂直偏移
        public int iVOffsetFOV;        // 采集视场相对于Sensor最大视场左上角的水平偏移
        public int iWidthFOV;          // 采集视场的宽度 
        public int iHeightFOV;         // 采集视场的高度
        public int iWidth;             // 相机最终输出的图像的宽度
        public int iHeight;            // 相机最终输出的图像的高度
        public int iWidthZoomHd;       // 硬件缩放的宽度,不需要进行此操作的分辨率，此变量设置为0.
        public int iHeightZoomHd;      // 硬件缩放的高度,不需要进行此操作的分辨率，此变量设置为0.
        public int iWidthZoomSw;       // 软件缩放的宽度,不需要进行此操作的分辨率，此变量设置为0.
        public int iHeightZoomSw;      // 软件缩放的高度,不需要进行此操作的分辨率，此变量设置为0.
    }

    //相机白平衡模式描述信息
    public struct tSdkColorTemperatureDes
    {
        public int iIndex;              // 模式索引号
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] acDescription;    // 描述信息
    }

    //相机帧率描述信息
    public struct tSdkFrameSpeed
    {
        public int iIndex;            // 帧率索引号，一般0对应于低速模式，1对应于普通模式，2对应于高速模式      
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] acDescription;  // 描述信息      
    }

    //相机曝光功能范围定义
    public struct tSdkExpose
    {
        public uint uiTargetMin;       //自动曝光亮度目标最小值     
        public uint uiTargetMax;       //自动曝光亮度目标最大值         
        public uint uiAnalogGainMin;   //模拟增益的最小值，单位为fAnalogGainStep中定义      
        public uint uiAnalogGainMax;   //模拟增益的最大值，单位为fAnalogGainStep中定义        
        public float fAnalogGainStep;   //模拟增益每增加1，对应的增加的放大倍数。例如，uiAnalogGainMin一般为16，fAnalogGainStep一般为0.125，那么最小放大倍数就是16*0.125 = 2倍
        public uint uiExposeTimeMin;   //手动模式下，曝光时间的最小值，单位:行。根据CameraGetExposureLineTime可以获得一行对应的时间(微秒),从而得到整帧的曝光时间    
        public uint uiExposeTimeMax;   //手动模式下，曝光时间的最大值，单位:行        
    }

    //触发模式描述
    public struct tSdkTrigger
    {
        public int iIndex;             //模式索引号      
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] acDescription;      //该模式的描述信息    
    }

    //传输分包大小描述(主要是针对网络相机有效)
    public struct tSdkPackLength
    {
        public int iIndex;        //分包大小索引号      
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] acDescription; //对应的描述信息     
        public uint iPackSize;
    }

    //预设的LUT表描述
    public struct tSdkPresetLut
    {
        public int iIndex;             //编号     
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] acDescription;    //描述信息
    }

    //AE算法描述
    public struct tSdkAeAlgorithm
    {
        public int iIndex;                 //编号   
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] acDescription;        //描述信息
    }

    //RAW转RGB算法描述
    public struct tSdkBayerDecodeAlgorithm
    {
        public int iIndex;                 //编号  
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] acDescription;        //描述信息
    }

    //帧率统计信息
    public struct tSdkFrameStatistic
    {
        public int iTotal;        //当前采集的总帧数（包括错误帧）
        public int iCapture;      //当前采集的有效帧的数量    
        public int iLost;         //当前丢帧的数量    
    }

    //相机输出的图像数据格式
    public struct tSdkMediaType
    {
        public int iIndex;                 //格式种类编号
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] acDescription;          //描述信息
        public uint iMediaType;             //对应的图像格式编码，如CAMERA_MEDIA_TYPE_BAYGR8，在本文件中有定义。
    }

    //伽马的设定范围
    public struct tGammaRange
    {
        public int iMin;       //最小值
        public int iMax;       //最大值
    }

    //对比度的设定范围
    public struct tContrastRange
    {
        public int iMin;    //最小值
        public int iMax;    //最大值
    }

    //RGB三通道数字增益的设定范围
    public struct tRgbGainRange
    {
        public int iRGainMin;   //红色增益的最小值
        public int iRGainMax;   //红色增益的最大值
        public int iGGainMin;   //绿色增益的最小值
        public int iGGainMax;   //绿色增益的最大值
        public int iBGainMin;   //蓝色增益的最小值
        public int iBGainMax;   //蓝色增益的最大值
    }

    //饱和度设定的范围
    public struct tSaturationRange
    {
        public int iMin;    //最小值
        public int iMax;    //最大值
    }

    //锐化的设定范围
    public struct tSharpnessRange
    {
        public int iMin;    //最小值
        public int iMax;    //最大值
    }

    //ISP模块的使能信息
    public struct tSdkIspCapacity
    {
        public uint bMonoSensor;        //表示该型号相机是否为黑白相机,如果是黑白相机，则颜色相关的功能都无法调节
        public uint bWbOnce;            //表示该型号相机是否支持手动白平衡功能  
        public uint bAutoWb;            //表示该型号相机是否支持自动白平衡功能
        public uint bAutoExposure;      //表示该型号相机是否支持自动曝光功能
        public uint bManualExposure;    //表示该型号相机是否支持手动曝光功能
        public uint bAntiFlick;         //表示该型号相机是否支持抗频闪功能
        public uint bDeviceIsp;         //表示该型号相机是否支持硬件ISP功能
        public uint bForceUseDeviceIsp; //bDeviceIsp和bForceUseDeviceIsp同时为TRUE时，表示强制只用硬件ISP，不可取消。
        public uint bZoomHD;            //相机硬件是否支持图像缩放输出(只能是缩小)。
    }

    /* 定义整合的设备描述信息，这些信息可以用于动态构建UI */
    public struct tSdkCameraCapbility
    {
        public IntPtr pTriggerDesc;
        public int iTriggerDesc;           //触发模式的个数，即pTriggerDesc数组的大小

        public IntPtr pImageSizeDesc;         //预设分辨率选择
        public int iImageSizeDesc;         //预设分辨率的个数，即pImageSizeDesc数组的大小 

        public IntPtr pClrTempDesc;           //预设色温模式，用于白平衡
        public int iClrTempDesc;

        public IntPtr pMediaTypeDesc;         //相机输出图像格式
        public int iMediaTypdeDesc;        //相机输出图像格式的种类个数，即pMediaTypeDesc数组的大小。

        public IntPtr pFrameSpeedDesc;        //可调节帧速类型，对应界面上普通 高速 和超级三种速度设置
        public int iFrameSpeedDesc;        //可调节帧速类型的个数，即pFrameSpeedDesc数组的大小。

        public IntPtr pPackLenDesc;           //传输包长度，一般用于网络设备
        public int iPackLenDesc;           //可供选择的传输分包长度的个数，即pPackLenDesc数组的大小。 

        public int iOutputIoCounts;        //可编程输出IO的个数
        public int iInputIoCounts;         //可编程输入IO的个数

        public IntPtr pPresetLutDesc;         //相机预设的LUT表
        public int iPresetLut;             //相机预设的LUT表的个数，即pPresetLutDesc数组的大小

        public int iUserDataMaxLen;        //指示该相机中用于保存用户数据区的最大长度。为0表示无。
        public uint bParamInDevice;         //指示该设备是否支持从设备中读写参数组。1为支持，0不支持。

        public IntPtr pAeAlmSwDesc;           //软件自动曝光算法描述
        public int iAeAlmSwDesc;           //软件自动曝光算法个数

        public IntPtr pAeAlmHdDesc;           //硬件自动曝光算法描述，为NULL表示不支持硬件自动曝光
        public int iAeAlmHdDesc;           //硬件自动曝光算法个数，为0表示不支持硬件自动曝光

        public IntPtr pBayerDecAlmSwDesc;     //软件Bayer转换为RGB数据的算法描述
        public int iBayerDecAlmSwDesc;     //软件Bayer转换为RGB数据的算法个数

        public IntPtr pBayerDecAlmHdDesc;     //硬件Bayer转换为RGB数据的算法描述，为NULL表示不支持
        public int iBayerDecAlmHdDesc;     //硬件Bayer转换为RGB数据的算法个数，为0表示不支持

        /* 图像参数的调节范围定义,用于动态构建UI*/
        public tSdkExpose sExposeDesc;      //曝光的范围值
        public tSdkResolutionRange sResolutionRange; //分辨率范围描述  
        public tRgbGainRange sRgbGainRange;    //图像数字增益范围描述  
        public tSaturationRange sSaturationRange; //饱和度范围描述  
        public tGammaRange sGammaRange;      //伽马范围描述  
        public tContrastRange sContrastRange;   //对比度范围描述  
        public tSharpnessRange sSharpnessRange;  //锐化范围描述  
        public tSdkIspCapacity sIspCapacity;     //ISP能力描述

    }

    //图像帧头信息
    public struct tSdkFrameHead
    {
        public uint uiMediaType;       // 图像格式,Image Format
        public uint uBytes;            // 图像数据字节数,Total bytes
        public int iWidth;            // 宽度 Image height
        public int iHeight;           // 高度 Image width
        public int iWidthZoomSw;      // 软件缩放的宽度,不需要进行软件裁剪的图像，此变量设置为0.
        public int iHeightZoomSw;     // 软件缩放的高度,不需要进行软件裁剪的图像，此变量设置为0.
        public uint bIsTrigger;        // 指示是否为触发帧 is trigger 
        public uint uiTimeStamp;       // 该帧的采集时间，单位0.1毫秒 
        public uint uiExpTime;         // 当前图像的曝光值，单位为微秒us
        public float fAnalogGain;       // 当前图像的模拟增益倍数
        public int iGamma;            // 该帧图像的伽马设定值，仅当LUT模式为动态参数生成时有效，其余模式下为-1
        public int iContrast;         // 该帧图像的对比度设定值，仅当LUT模式为动态参数生成时有效，其余模式下为-1
        public int iSaturation;       // 该帧图像的饱和度设定值，对于黑白相机无意义，为0
        public float fRgain;            // 该帧图像处理的红色数字增益倍数，对于黑白相机无意义，为1
        public float fGgain;            // 该帧图像处理的绿色数字增益倍数，对于黑白相机无意义，为1
        public float fBgain;            // 该帧图像处理的蓝色数字增益倍数，对于黑白相机无意义，为1
    }

    //图像帧描述
    public struct tSdkFrame
    {
        public tSdkFrameHead head;        //帧头
        public IntPtr pBuffer;     //数据区
    }

    //图像捕获的回调函数定义
    public delegate void CAMERA_SNAP_PROC(CameraHandle hCamera, IntPtr pFrameBuffer, ref tSdkFrameHead pFrameHead, IntPtr pContext);

    //SDK生成的相机配置页面的消息回调函数定义
    public delegate void CAMERA_PAGE_MSG_PROC(CameraHandle hCamera, uint MSG, uint uParam, IntPtr pContext);


    // Grabber统计信息
    public struct tSdkGrabberStat
    {
        public int Width, Height;	// 帧图像大小
        public int Disp;			// 显示帧数量
        public int Capture;		    // 采集的有效帧的数量
        public int Lost;			// 丢帧的数量
        public int Error;			// 错帧的数量
        public float DispFps;		// 显示帧率
        public float CapFps;		// 捕获帧率
    }

    // Grabber图像捕获的回调函数定义
    public delegate int pfnCameraGrabberFrameListener(
        IntPtr Grabber,
        int Phase,
        IntPtr pFrameBuffer,
        ref tSdkFrameHead pFrameHead,
        IntPtr Context);

    // Grabber异步抓图的回调函数定义
    public delegate void pfnCameraGrabberSaveImageComplete(
        IntPtr Grabber,
        IntPtr Image,	// 需要调用CameraImage_Destroy释放
        CameraSdkStatus Status,
        IntPtr Context);


    static public class MvApi
    {

        /******************************************************/
        // 函数名   : CameraSdkInit
        // 功能描述 : 相机SDK初始化，在调用任何SDK其他接口前，必须
        //        先调用该接口进行初始化。该函数在整个进程运行
        //        期间只需要调用一次。   
        // 参数     : iLanguageSel 用于选择SDK内部提示信息和界面的语种,
        //               0:表示英文,1:表示中文。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraSdkInit(
            int iLanguageSel
        );

        /******************************************************/
        // 函数名   : CameraEnumerateDevice
        // 功能描述 : 枚举设备，并建立设备列表。在调用CameraInit
        //        之前，必须调用该函数来获得设备的信息。    
        // 参数     : pCameraList 设备列表数组指针。
        //             piNums        设备的个数指针，调用时传入pCameraList
        //                            数组的元素个数，函数返回时，保存实际找到的设备个数。
        //              注意，piNums指向的值必须初始化，且不超过pCameraList数组元素个数，
        //              否则有可能造成内存溢出。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraEnumerateDevice(
            IntPtr pCameraList,
        ref int piNums
        );

        public static CameraSdkStatus CameraEnumerateDevice(out tSdkCameraDevInfo[] CameraList)
        {
            int CameraCount = 256;
            CameraSdkStatus status;
            IntPtr ptr;

            CameraList = null;

            ptr = Marshal.AllocHGlobal(Marshal.SizeOf(new tSdkCameraDevInfo()) * CameraCount);
            status = CameraEnumerateDevice(ptr, ref CameraCount);
            if (status == CameraSdkStatus.CAMERA_STATUS_SUCCESS && CameraCount > 0)
            {
                CameraList = new tSdkCameraDevInfo[CameraCount];
                for (int i = 0; i < CameraCount; ++i)
                {
                    CameraList[i] = (tSdkCameraDevInfo)Marshal.PtrToStructure((IntPtr)((Int64)ptr + i * Marshal.SizeOf(new tSdkCameraDevInfo())), typeof(tSdkCameraDevInfo));
                }
            }

            Marshal.FreeHGlobal(ptr);
            return status;
        }

        /******************************************************/
        // 函数名   : CameraInit
        // 功能描述 : 相机初始化。初始化成功后，才能调用任何其他
        //        相机相关的操作接口。    
        // 参数     : pCameraInfo    该相机的设备描述信息，由CameraEnumerateDevice
        //               函数获得。 
        //            iParamLoadMode  相机初始化时使用的参数加载方式。-1表示使用上次退出时的参数加载方式。
        //            emTeam         初始化时使用的参数组。-1表示加载上次退出时的参数组。
        //            pCameraHandle  相机的句柄指针，初始化成功后，该指针
        //               返回该相机的有效句柄，在调用其他相机
        //               相关的操作接口时，都需要传入该句柄，主要
        //               用于多相机之间的区分。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraStatus.h
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraInit(
        ref tSdkCameraDevInfo pCameraInfo,
            int emParamLoadMode,
            int emTeam,
        ref CameraHandle pCameraHandle
        );

        /******************************************************/
        // 函数名   : CameraSetCallbackFunction
        // 功能描述 : 设置图像捕获的回调函数。当捕获到新的图像数据帧时，
        //        pCallBack所指向的回调函数就会被调用。 
        // 参数     : hCamera 相机的句柄，由CameraInit函数获得。
        //            pCallBack 回调函数指针。
        //            pContext  回调函数的附加参数，在回调函数被调用时
        //            该附加参数会被传入，可以为NULL。多用于
        //            多个相机时携带附加信息。
        //            pCallbackOld  用于保存当前的回调函数。可以为NULL。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraSetCallbackFunction(
            CameraHandle hCamera,
            CAMERA_SNAP_PROC pCallBack,
            IntPtr pContext,
        ref CAMERA_SNAP_PROC pCallbackOld
        );

        /******************************************************/
        // 函数名   : CameraUnInit
        // 功能描述 : 相机反初始化。释放资源。
        // 参数     : hCamera 相机的句柄，由CameraInit函数获得。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraUnInit(
            CameraHandle hCamera
        );

        /******************************************************/
        // 函数名   : CameraGetInformation
        // 功能描述 : 获得相机的描述信息
        // 参数     : hCamera 相机的句柄，由CameraInit函数获得。
        //            pbuffer 指向相机描述信息指针的指针。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraGetInformation(
            CameraHandle hCamera,
            ref IntPtr pbuffer
        );

        /******************************************************/
        // 函数名   : CameraImageProcess
        // 功能描述 : 将获得的相机原始输出图像数据进行处理，叠加饱和度、
        //        颜色增益和校正、降噪等处理效果，最后得到RGB888
        //        格式的图像数据。  
        // 参数     : hCamera  相机的句柄，由CameraInit函数获得。
        //            pbyIn    输入图像数据的缓冲区地址，不能为NULL。 
        //            pbyOut   处理后图像输出的缓冲区地址，不能为NULL。
        //            pFrInfo  输入图像的帧头信息，处理完成后，帧头信息
        //             中的图像格式uiMediaType会随之改变。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraImageProcess(
            CameraHandle hCamera,
            IntPtr pbyIn,
            IntPtr pbyOut,
        ref tSdkFrameHead pFrInfo
        );

        /******************************************************/
        // 函数名   : CameraDisplayInit
        // 功能描述 : 初始化SDK内部的显示模块。在调用CameraDisplayRGB24
        //        前必须先调用该函数初始化。如果您在二次开发中，
        //        使用自己的方式进行图像显示(不调用CameraDisplayRGB24)，
        //        则不需要调用本函数。  
        // 参数     : hCamera   相机的句柄，由CameraInit函数获得。
        //            IntPtrDisplay 显示窗口的句柄，一般为窗口的m_IntPtr成员。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraDisplayInit(
            CameraHandle hCamera,
            IntPtr IntPtrDisplay
        );

        /******************************************************/
        // 函数名   : CameraDisplayRGB24
        // 功能描述 : 显示图像。必须调用过CameraDisplayInit进行
        //        初始化才能调用本函数。  
        // 参数     : hCamera  相机的句柄，由CameraInit函数获得。
        //            pbyRGB24 图像的数据缓冲区，RGB888格式。
        //            pFrInfo  图像的帧头信息。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraDisplayRGB24(
            CameraHandle hCamera,
            IntPtr pbyRGB24,
        ref tSdkFrameHead pFrInfo
        );

        /******************************************************/
        // 函数名   : CameraSetDisplayMode
        // 功能描述 : 设置显示的模式。必须调用过CameraDisplayInit
        //        进行初始化才能调用本函数。
        // 参数     : hCamera  相机的句柄，由CameraInit函数获得。
        //            iMode    显示模式，DISPLAYMODE_SCALE或者
        //             DISPLAYMODE_REAL,具体参见CameraDefine.h
        //             中emSdkDisplayMode的定义。    
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraSetDisplayMode(
            CameraHandle hCamera,
            int iMode
        );

        /******************************************************/
        // 函数名   : CameraSetDisplayOffset
        // 功能描述 : 设置显示的起始偏移值。仅当显示模式为DISPLAYMODE_REAL
        //        时有效。例如显示控件的大小为320X240，而图像的
        //        的尺寸为640X480，那么当iOffsetX = 160,iOffsetY = 120时
        //        显示的区域就是图像的居中320X240的位置。必须调用过
        //        CameraDisplayInit进行初始化才能调用本函数。
        // 参数     : hCamera   相机的句柄，由CameraInit函数获得。
        //            iOffsetX  偏移的X坐标。
        //            iOffsetY  偏移的Y坐标。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraSetDisplayOffset(
            CameraHandle hCamera,
            int iOffsetX,
            int iOffsetY
        );

        /******************************************************/
        // 函数名   : CameraSetDisplaySize
        // 功能描述 : 设置显示控件的尺寸。必须调用过
        //        CameraDisplayInit进行初始化才能调用本函数。
        // 参数     : hCamera   相机的句柄，由CameraInit函数获得。
        //            iWidth    宽度
        //            iHeight   高度
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraSetDisplaySize(
            CameraHandle hCamera,
            int iWidth,
            int iHeight
        );

        /******************************************************/
        // 函数名   : CameraGetImageBuffer
        // 功能描述 : 获得一帧图像数据。为了提高效率，SDK在图像抓取时采用了零拷贝机制，
        //        CameraGetImageBuffer实际获得是内核中的一个缓冲区地址，
        //        该函数成功调用后，必须调用CameraReleaseImageBuffer释放由
        //        CameraGetImageBuffer得到的缓冲区,以便让内核继续使用
        //        该缓冲区。  
        // 参数     : hCamera   相机的句柄，由CameraInit函数获得。
        //            pFrameInfo  图像的帧头信息指针。
        //            pbyBuffer   指向图像的数据的缓冲区指针。由于
        //              采用了零拷贝机制来提高效率，因此
        //              这里使用了一个指向指针的指针。
        //            uint wTimes 抓取图像的超时时间。单位毫秒。在
        //              wTimes时间内还未获得图像，则该函数
        //              会返回超时信息。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraGetImageBuffer(
            CameraHandle hCamera,
        out tSdkFrameHead pFrameInfo,
        out IntPtr pbyBuffer,
            uint wTimes
        );

        /******************************************************/
        // 函数名   : CameraSnapToBuffer
        // 功能描述 : 抓拍一张图像到缓冲区中。相机会进入抓拍模式，并且
        //        自动切换到抓拍模式的分辨率进行图像捕获。然后将
        //        捕获到的数据保存到缓冲区中。
        //        该函数成功调用后，必须调用CameraReleaseImageBuffer
        //        释放由CameraSnapToBuffer得到的缓冲区。具体请参考
        //        CameraGetImageBuffer函数的功能描述部分。  
        // 参数     : hCamera   相机的句柄，由CameraInit函数获得。
        //            pFrameInfo  指针，返回图像的帧头信息。
        //            pbyBuffer   指向指针的指针，用来返回图像缓冲区的地址。
        //            uWaitTimeMs 超时时间，单位毫秒。在该时间内，如果仍然没有
        //              成功捕获的数据，则返回超时信息。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraSnapToBuffer(
            CameraHandle hCamera,
        out tSdkFrameHead pFrameInfo,
        out IntPtr pbyBuffer,
            uint uWaitTimeMs
        );

        /******************************************************/
        // 函数名   : CameraReleaseImageBuffer
        // 功能描述 : 释放由CameraGetImageBuffer获得的缓冲区。
        // 参数     : hCamera   相机的句柄，由CameraInit函数获得。
        //            pbyBuffer   由CameraGetImageBuffer获得的缓冲区地址。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraReleaseImageBuffer(
            CameraHandle hCamera,
            IntPtr pbyBuffer
        );

        /******************************************************/
        // 函数名   : CameraPlay
        // 功能描述 : 让SDK进入工作模式，开始接收来自相机发送的图像
        //        数据。如果当前相机是触发模式，则需要接收到
        //        触发帧以后才会更新图像。
        // 参数     : hCamera   相机的句柄，由CameraInit函数获得。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraPlay(
            CameraHandle hCamera
        );

        /******************************************************/
        // 函数名   : CameraPause
        // 功能描述 : 让SDK进入暂停模式，不接收来自相机的图像数据，
        //        同时也会发送命令让相机暂停输出，释放传输带宽。
        //        暂停模式下，可以对相机的参数进行配置，并立即生效。  
        // 参数     : hCamera   相机的句柄，由CameraInit函数获得。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraPause(
            CameraHandle hCamera
        );

        /******************************************************/
        // 函数名   : CameraStop
        // 功能描述 : 让SDK进入停止状态，一般是反初始化时调用该函数，
        //        该函数被调用，不能再对相机的参数进行配置。
        // 参数     : hCamera   相机的句柄，由CameraInit函数获得。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraStop(
            CameraHandle hCamera
        );

        /******************************************************/
        // 函数名   : CameraInitRecord
        // 功能描述 : 初始化一次录像。
        // 参数     : hCamera   相机的句柄，由CameraInit函数获得。
        //            iFormat   录像的格式，当前只支持不压缩和MSCV两种方式。  
        //              0:不压缩；1:MSCV方式压缩。
        //            pcSavePath  录像文件保存的路径。
        //            b2GLimit    如果为TRUE,则文件大于2G时自动分割。
        //            dwQuality   录像的质量因子，越大，则质量越好。范围1到100.
        //            iFrameRate  录像的帧率。建议设定的比实际采集帧率大，
        //              这样就不会漏帧。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraInitRecord(
            CameraHandle hCamera,
            int iFormat,
            byte[] pcSavePath,
            uint b2GLimit,
            uint dwQuality,
            int iFrameRate
        );

        /******************************************************/
        // 函数名   : CameraStopRecord
        // 功能描述 : 结束本次录像。当CameraInitRecord后，可以通过该函数
        //        来结束一次录像，并完成文件保存操作。
        // 参数     : hCamera   相机的句柄，由CameraInit函数获得。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraStopRecord(
            CameraHandle hCamera
        );

        /******************************************************/
        // 函数名   : CameraPushFrame
        // 功能描述 : 将一帧数据存入录像流中。必须调用CameraInitRecord
        //        才能调用该函数。CameraStopRecord调用后，不能再调用
        //        该函数。由于我们的帧头信息中携带了图像采集的时间戳
        //        信息，因此录像可以精准的时间同步，而不受帧率不稳定
        //        的影响。
        // 参数     : hCamera     相机的句柄，由CameraInit函数获得。
        //            pbyImageBuffer    图像的数据缓冲区，必须是RGB格式。
        //            pFrInfo           图像的帧头信息。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraPushFrame(
            CameraHandle hCamera,
            IntPtr pbyImageBuffer,
            ref tSdkFrameHead pFrInfo
        );

        /******************************************************/
        // 函数名   : CameraSaveImage
        // 功能描述 : 将图像缓冲区的数据保存成图片文件。
        // 参数     : hCamera    相机的句柄，由CameraInit函数获得。
        //            lpszFileName   图片保存文件完整路径。
        //            pbyImageBuffer 图像的数据缓冲区。
        //            pFrInfo        图像的帧头信息。
        //            byFileType     图像保存的格式。取值范围参见CameraDefine.h
        //               中emSdkFileType的类型定义。目前支持  
        //               BMP、JPG、PNG、RAW四种格式。其中RAW表示
        //               相机输出的原始数据，保存RAW格式文件要求
        //               pbyImageBuffer和pFrInfo是由CameraGetImageBuffer
        //               获得的数据，而且未经CameraImageProcess转换
        //               成BMP格式；反之，如果要保存成BMP、JPG或者
        //               PNG格式，则pbyImageBuffer和pFrInfo是由
        //               CameraImageProcess处理后的RGB格式数据。
        //                 具体用法可以参考Advanced的例程。   
        //            byQuality      图像保存的质量因子，仅当保存为JPG格式
        //                 时该参数有效，范围1到100。其余格式
        //                           可以写成0。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraSaveImage(
            CameraHandle hCamera,
            byte[] lpszFileName,
            IntPtr pbyImageBuffer,
        ref tSdkFrameHead pFrInfo,
            emSdkFileType byFileType,
            Byte byQuality
        );

        // 把帧图像转换成C# Image格式
        public static System.Drawing.Image CSharpImageFromFrame(IntPtr pFrameBuffer, ref tSdkFrameHead tFrameHead)
        {
            BITMAPINFOHEADER bmi;
            BITMAPFILEHEADER bmfi;
            RGBQUAD[] bmiColors = new RGBQUAD[256];
            Boolean bMono8 = (tFrameHead.uiMediaType == (uint)emImageFormat.CAMERA_MEDIA_TYPE_MONO8);
            uint HeadTotalSize = (uint)(bMono8 ? 54 + 1024 : 54);

            bmfi.bfType = ((int)'M' << 8) | ((int)'B');
            bmfi.bfOffBits = HeadTotalSize;
            bmfi.bfSize = (uint)(HeadTotalSize + tFrameHead.uBytes);
            bmfi.bfReserved1 = 0;
            bmfi.bfReserved2 = 0;

            bmi.biBitCount = (ushort)(bMono8 ? 8 : 24);
            bmi.biClrImportant = 0;
            bmi.biClrUsed = 0;
            bmi.biCompression = 0;
            bmi.biPlanes = 1;
            bmi.biSize = 40;
            bmi.biHeight = tFrameHead.iHeight;
            bmi.biWidth = tFrameHead.iWidth;
            bmi.biXPelsPerMeter = 0;
            bmi.biYPelsPerMeter = 0;
            bmi.biSizeImage = 0;

            if (bMono8)
            {
                for (int i = 0; i < 256; ++i)
                {
                    bmiColors[i].rgbReserved = 0;
                    bmiColors[i].rgbBlue =
                        bmiColors[i].rgbGreen =
                        bmiColors[i].rgbRed = (byte)i;
                }
            }

            MemoryStream stream = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(stream);
            byte[] data = new byte[14];
            IntPtr ptr = Marshal.AllocHGlobal((int)HeadTotalSize);
            Marshal.StructureToPtr((object)bmfi, ptr, false);
            Marshal.Copy(ptr, data, 0, data.Length);
            bw.Write(data);
            data = new byte[40];
            Marshal.StructureToPtr((object)bmi, ptr, false);
            Marshal.Copy(ptr, data, 0, data.Length);
            bw.Write(data);
            if (bMono8)
            {
                data = new byte[1024];
                for (int i = 0; i < 256; ++i)
                    Marshal.StructureToPtr(bmiColors[i], (IntPtr)((Int64)ptr + i * 4), false);
                Marshal.Copy(ptr, data, 0, data.Length);
                bw.Write(data);
            }
            data = new byte[tFrameHead.uBytes];
            Marshal.Copy(pFrameBuffer, data, 0, data.Length);
            bw.Write(data);
            Marshal.FreeHGlobal(ptr);

            return System.Drawing.Image.FromStream(stream);
        }

        /******************************************************/
        // 函数名   : CameraGetImageResolution
        // 功能描述 : 获得当前预览的分辨率。
        // 参数     : hCamera    相机的句柄，由CameraInit函数获得。
        //            psCurVideoSize 结构体指针，用于返回当前的分辨率。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraGetImageResolution(
            CameraHandle hCamera,
        ref tSdkImageResolution psCurVideoSize
        );

        /******************************************************/
        // 函数名   : CameraSetImageResolution
        // 功能描述 : 设置预览的分辨率。
        // 参数     : hCamera      相机的句柄，由CameraInit函数获得。
        //            pImageResolution 结构体指针，用于返回当前的分辨率。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraSetImageResolution(
            CameraHandle hCamera,
        ref tSdkImageResolution pImageResolution
        );

        /******************************************************/
        // 函数名   : CameraGetMediaType
        // 功能描述 : 获得相机当前输出原始数据的格式索引号。
        // 参数     : hCamera   相机的句柄，由CameraInit函数获得。
        //            piMediaType   指针，用于返回当前格式类型的索引号。
        //              由CameraGetCapability获得相机的属性，
        //              在tSdkCameraCapbility结构体中的pMediaTypeDesc
        //              成员中，以数组的形式保存了相机支持的格式，
        //              piMediaType所指向的索引号，就是该数组的索引号。
        //              pMediaTypeDesc[*piMediaType].iMediaType则表示当前格式的 
        //              编码。该编码请参见CameraDefine.h中[图像格式定义]部分。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraGetMediaType(
            CameraHandle hCamera,
        ref int piMediaType
        );

        /******************************************************/
        // 函数名   : CameraSetMediaType
        // 功能描述 : 设置相机的输出原始数据格式。
        // 参数     : hCamera   相机的句柄，由CameraInit函数获得。
        //            iMediaType  由CameraGetCapability获得相机的属性，
        //              在tSdkCameraCapbility结构体中的pMediaTypeDesc
        //              成员中，以数组的形式保存了相机支持的格式，
        //              iMediaType就是该数组的索引号。
        //              pMediaTypeDesc[iMediaType].iMediaType则表示当前格式的 
        //              编码。该编码请参见CameraDefine.h中[图像格式定义]部分。   
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraSetMediaType(
            CameraHandle hCamera,
            int iMediaType
        );

        /******************************************************/
        // 函数名   : CameraSetAeState
        // 功能描述 : 设置相机曝光的模式。自动或者手动。
        // 参数     : hCamera   相机的句柄，由CameraInit函数获得。
        //            bAeState    TRUE，使能自动曝光；FALSE，停止自动曝光。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraSetAeState(
            CameraHandle hCamera,
            uint bAeState
        );

        /******************************************************/
        // 函数名   : CameraGetAeState
        // 功能描述 : 获得相机当前的曝光模式。
        // 参数     : hCamera  相机的句柄，由CameraInit函数获得。
        //            pAeState   指针，用于返回自动曝光的使能状态。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraGetAeState(
            CameraHandle hCamera,
        ref uint pAeState
        );

        /******************************************************/
        // 函数名   : CameraSetSharpness
        // 功能描述 : 设置图像的处理的锐化参数。
        // 参数     : hCamera  相机的句柄，由CameraInit函数获得。
        //            iSharpness 锐化参数。范围由CameraGetCapability
        //               获得，一般是[0,100]，0表示关闭锐化处理。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraSetSharpness(
            CameraHandle hCamera,
            int iSharpness
        );

        /******************************************************/
        // 函数名   : CameraGetSharpness
        // 功能描述 : 获取当前锐化设定值。
        // 参数     : hCamera   相机的句柄，由CameraInit函数获得。
        //            piSharpness 指针，返回当前设定的锐化的设定值。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraGetSharpness(
            CameraHandle hCamera,
        ref int piSharpness
        );

        /******************************************************/
        // 函数名   : CameraSetLutMode
        // 功能描述 : 设置相机的查表变换模式LUT模式。
        // 参数     : hCamera  相机的句柄，由CameraInit函数获得。
        //            emLutMode  LUTMODE_PARAM_GEN 表示由伽马和对比度参数动态生成LUT表。
        //             LUTMODE_PRESET    表示使用预设的LUT表。
        //             LUTMODE_USER_DEF  表示使用用户自定的LUT表。
        //             LUTMODE_PARAM_GEN的定义参考CameraDefine.h中emSdkLutMode类型。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraSetLutMode(
            CameraHandle hCamera,
            int emLutMode
        );

        /******************************************************/
        // 函数名   : CameraGetLutMode
        // 功能描述 : 获得相机的查表变换模式LUT模式。
        // 参数     : hCamera  相机的句柄，由CameraInit函数获得。
        //            pemLutMode 指针，返回当前LUT模式。意义与CameraSetLutMode
        //             中emLutMode参数相同。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraGetLutMode(
            CameraHandle hCamera,
        ref int pemLutMode
        );

        /******************************************************/
        // 函数名   : CameraSelectLutPreset
        // 功能描述 : 选择预设LUT模式下的LUT表。必须先使用CameraSetLutMode
        //        将LUT模式设置为预设模式。
        // 参数     : hCamera  相机的句柄，由CameraInit函数获得。
        //            iSel     表的索引号。表的个数由CameraGetCapability
        //             获得。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraSelectLutPreset(
            CameraHandle hCamera,
            int iSel
        );

        /******************************************************/
        // 函数名   : CameraGetLutPresetSel
        // 功能描述 : 获得预设LUT模式下的LUT表索引号。
        // 参数     : hCamera  相机的句柄，由CameraInit函数获得。
        //            piSel      指针，返回表的索引号。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraGetLutPresetSel(
            CameraHandle hCamera,
        ref int piSel
        );

        /******************************************************/
        // 函数名   : CameraSetCustomLut
        // 功能描述 : 设置自定义的LUT表。必须先使用CameraSetLutMode
        //        将LUT模式设置为自定义模式。
        // 参数     : hCamera  相机的句柄，由CameraInit函数获得。
        //             iChannel 指定要设定的LUT颜色通道，当为LUT_CHANNEL_ALL时，
        //                      三个通道的LUT将被同时替换。
        //                      参考CameraDefine.h中emSdkLutChannel定义。
        //            pLut     指针，指向LUT表的地址。LUT表为无符号短整形数组，数组大小为
        //           4096，分别代码颜色通道从0到4096(12bit颜色精度)对应的映射值。 
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraSetCustomLut(
            CameraHandle hCamera,
            int iChannel,
            ushort[] pLut
        );

        /******************************************************/
        // 函数名   : CameraGetCustomLut
        // 功能描述 : 获得当前使用的自定义LUT表。
        // 参数     : hCamera  相机的句柄，由CameraInit函数获得。
        //             iChannel 指定要获得的LUT颜色通道。当为LUT_CHANNEL_ALL时，
        //                      返回红色通道的LUT表。
        //                      参考CameraDefine.h中emSdkLutChannel定义。
        //            pLut     指针，指向LUT表的地址。LUT表为无符号短整形数组，数组大小为
        //           4096，分别代码颜色通道从0到4096(12bit颜色精度)对应的映射值。 
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraGetCustomLut(
            CameraHandle hCamera,
            int iChannel,
        ref ushort[] pLut
        );

        /******************************************************/
        // 函数名   : CameraGetCurrentLut
        // 功能描述 : 获得相机当前的LUT表，在任何LUT模式下都可以调用,
        //        用来直观的观察LUT曲线的变化。
        // 参数     : hCamera  相机的句柄，由CameraInit函数获得。
        //             iChannel 指定要获得的LUT颜色通道。当为LUT_CHANNEL_ALL时，
        //                      返回红色通道的LUT表。
        //                      参考CameraDefine.h中emSdkLutChannel定义。
        //            pLut     指针，指向LUT表的地址。LUT表为无符号短整形数组，数组大小为
        //           4096，分别代码颜色通道从0到4096(12bit颜色精度)对应的映射值。 
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraGetCurrentLut(
            CameraHandle hCamera,
            int iChannel,
        ref ushort[] pLut
        );

        /******************************************************/
        // 函数名   : CameraSetWbMode
        // 功能描述 : 设置相机白平衡模式。分为手动和自动两种方式。
        // 参数     : hCamera  相机的句柄，由CameraInit函数获得。
        //            bAuto      TRUE，则表示使能自动模式。
        //             FALSE，则表示使用手动模式，通过调用
        //                 CameraSetOnceWB来进行一次白平衡。        
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraSetWbMode(
            CameraHandle hCamera,
            uint bAuto
        );

        /******************************************************/
        // 函数名   : CameraGetWbMode
        // 功能描述 : 获得当前的白平衡模式。
        // 参数     : hCamera  相机的句柄，由CameraInit函数获得。
        //            pbAuto   指针，返回TRUE表示自动模式，FALSE
        //             为手动模式。 
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraGetWbMode(
            CameraHandle hCamera,
        ref uint pbAuto
        );

        /******************************************************/
        // 函数名   : CameraSetPresetClrTemp
        // 功能描述 : 设置色温模式
        // 参数     : hCamera  相机的句柄，由CameraInit函数获得。
        //             iSel     索引号。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraSetPresetClrTemp(
            CameraHandle hCamera,
            int iSel
        );

        /******************************************************/
        // 函数名   : CameraGetPresetClrTemp
        // 功能描述 : 
        // 参数     : hCamera  相机的句柄，由CameraInit函数获得。
        //            int* piSel
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraGetPresetClrTemp(
            CameraHandle hCamera,
        ref int piSel
        );

        /******************************************************/
        // 函数名   : CameraSetOnceWB
        // 功能描述 : 在手动白平衡模式下，调用该函数会进行一次白平衡。
        //        生效的时间为接收到下一帧图像数据时。
        // 参数     : hCamera  相机的句柄，由CameraInit函数获得。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraSetOnceWB(
            CameraHandle hCamera
        );

        /******************************************************/
        // 函数名   : CameraSetOnceBB
        // 功能描述 : 执行一次黑平衡操作。
        // 参数     : hCamera  相机的句柄，由CameraInit函数获得。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraSetOnceBB(
            CameraHandle hCamera
        );


        /******************************************************/
        // 函数名   : CameraSetAeTarget
        // 功能描述 : 设定自动曝光的亮度目标值。设定范围由CameraGetCapability
        //        函数获得。
        // 参数     : hCamera  相机的句柄，由CameraInit函数获得。
        //            iAeTarget  亮度目标值。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraSetAeTarget(
            CameraHandle hCamera,
            int iAeTarget
        );

        /******************************************************/
        // 函数名   : CameraGetAeTarget
        // 功能描述 : 获得自动曝光的亮度目标值。
        // 参数     : hCamera   相机的句柄，由CameraInit函数获得。
        //            *piAeTarget 指针，返回目标值。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraGetAeTarget(
            CameraHandle hCamera,
        ref int piAeTarget
        );

        /******************************************************/
        // 函数名   : CameraSetAeExposureRange
        // 功能描述 : 设定自动曝光模式的曝光时间调节范围
        // 参数     : hCamera  相机的句柄，由CameraInit函数获得。
        //           fMinExposureTime 最小曝光时间（微秒）
        //			 fMaxExposureTime 最大曝光时间（微秒）
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraStatus.h
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraSetAeExposureRange(
            CameraHandle hCamera,
            double fMinExposureTime,
            double fMaxExposureTime
            );

        /******************************************************/
        // 函数名   : CameraGetAeExposureRange
        // 功能描述 : 获得自动曝光模式的曝光时间调节范围
        // 参数     : hCamera   相机的句柄，由CameraInit函数获得。
        //           fMinExposureTime 最小曝光时间（微秒）
        //			 fMaxExposureTime 最大曝光时间（微秒）
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraStatus.h
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraGetAeExposureRange(
            CameraHandle hCamera,
            out double fMinExposureTime,
            out double fMaxExposureTime
            );

        /******************************************************/
        // 函数名   : CameraSetAeAnalogGainRange
        // 功能描述 : 设定自动曝光模式的增益调节范围
        // 参数     : hCamera  相机的句柄，由CameraInit函数获得。
        //           iMinAnalogGain 最小增益
        //			 iMaxAnalogGain 最大增益
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraStatus.h
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraSetAeAnalogGainRange(
            CameraHandle hCamera,
            int iMinAnalogGain,
            int iMaxAnalogGain
            );

        /******************************************************/
        // 函数名   : CameraGetAeAnalogGainRange
        // 功能描述 : 获得自动曝光模式的增益调节范围
        // 参数     : hCamera   相机的句柄，由CameraInit函数获得。
        //           iMinAnalogGain 最小增益
        //			 iMaxAnalogGain 最大增益
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraStatus.h
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraGetAeAnalogGainRange(
            CameraHandle hCamera,
            out int iMinAnalogGain,
            out int iMaxAnalogGain
            );

        /******************************************************/
        // 函数名   : CameraSetExposureTime
        // 功能描述 : 设置曝光时间。单位为微秒。对于CMOS传感器，其曝光
        //        的单位是按照行来计算的，因此，曝光时间并不能在微秒
        //        级别连续可调。而是会按照整行来取舍。在调用
        //        本函数设定曝光时间后，建议再调用CameraGetExposureTime
        //        来获得实际设定的值。
        // 参数     : hCamera      相机的句柄，由CameraInit函数获得。
        //            fExposureTime 曝光时间，单位微秒。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraSetExposureTime(
            CameraHandle hCamera,
            double fExposureTime
        );

        /******************************************************/
        // 函数名   : CameraGetExposureLineTime
        // 功能描述 : 获得一行的曝光时间。对于CMOS传感器，其曝光
        //        的单位是按照行来计算的，因此，曝光时间并不能在微秒
        //        级别连续可调。而是会按照整行来取舍。这个函数的
        //          作用就是返回CMOS相机曝光一行对应的时间。
        // 参数     : hCamera  相机的句柄，由CameraInit函数获得。
        //            pfLineTime 指针，返回一行的曝光时间，单位为微秒。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraGetExposureLineTime(
            CameraHandle hCamera,
            ref double pfLineTime
        );

        /******************************************************/
        // 函数名   : CameraGetExposureTime
        // 功能描述 : 获得相机的曝光时间。请参见CameraSetExposureTime
        //        的功能描述。
        // 参数     : hCamera        相机的句柄，由CameraInit函数获得。
        //            pfExposureTime   指针，返回当前的曝光时间，单位微秒。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraGetExposureTime(
            CameraHandle hCamera,
            ref double pfExposureTime
        );

        /******************************************************/
        // 函数名   : CameraGetExposureTimeRange
        // 功能描述 : 获得相机的曝光时间范围
        // 参数     : hCamera        相机的句柄，由CameraInit函数获得。
        //            pfMin			指针，返回曝光时间的最小值，单位微秒。
        //            pfMax			指针，返回曝光时间的最大值，单位微秒。
        //            pfStep		指针，返回曝光时间的步进值，单位微秒。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraStatus.h
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraGetExposureTimeRange(
            CameraHandle hCamera,
            ref double pfMin,
            ref double pfMax,
            ref double pfStep
        );

        /******************************************************/
        // 函数名   : CameraSetAnalogGain
        // 功能描述 : 设置相机的图像模拟增益值。该值乘以CameraGetCapability获得
        //        的相机属性结构体中sExposeDesc.fAnalogGainStep，就
        //        得到实际的图像信号放大倍数。
        // 参数     : hCamera   相机的句柄，由CameraInit函数获得。
        //            iAnalogGain 设定的模拟增益值。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraSetAnalogGain(
            CameraHandle hCamera,
            int iAnalogGain
        );

        /******************************************************/
        // 函数名   : CameraGetAnalogGain
        // 功能描述 : 获得图像信号的模拟增益值。参见CameraSetAnalogGain
        //        详细说明。
        // 参数     : hCamera    相机的句柄，由CameraInit函数获得。
        //            piAnalogGain 指针，返回当前的模拟增益值。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraGetAnalogGain(
            CameraHandle hCamera,
        ref int piAnalogGain
        );

        /******************************************************/
        // 函数名   : CameraSetGain
        // 功能描述 : 设置图像的数字增益。设定范围由CameraGetCapability
        //        获得的相机属性结构体中sRgbGainRange成员表述。
        //        实际的放大倍数是设定值/100。
        // 参数     : hCamera  相机的句柄，由CameraInit函数获得。
        //            iRGain   红色通道的增益值。 
        //            iGGain   绿色通道的增益值。
        //            iBGain   蓝色通道的增益值。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraSetGain(
            CameraHandle hCamera,
            int iRGain,
            int iGGain,
            int iBGain
        );


        /******************************************************/
        // 函数名   : CameraGetGain
        // 功能描述 : 获得图像处理的数字增益。具体请参见CameraSetGain
        //        的功能描述部分。
        // 参数     : hCamera  相机的句柄，由CameraInit函数获得。
        //            piRGain  指针，返回红色通道的数字增益值。
        //            piGGain    指针，返回绿色通道的数字增益值。
        //            piBGain    指针，返回蓝色通道的数字增益值。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraGetGain(
            CameraHandle hCamera,
        ref int piRGain,
        ref int piGGain,
        ref int piBGain
        );


        /******************************************************/
        // 函数名   : CameraSetGamma
        // 功能描述 : 设定LUT动态生成模式下的Gamma值。设定的值会
        //        马上保存在SDK内部，但是只有当相机处于动态
        //        参数生成的LUT模式时，才会生效。请参考CameraSetLutMode
        //        的函数说明部分。
        // 参数     : hCamera  相机的句柄，由CameraInit函数获得。
        //            iGamma     要设定的Gamma值。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraSetGamma(
            CameraHandle hCamera,
            int iGamma
        );

        /******************************************************/
        // 函数名   : CameraGetGamma
        // 功能描述 : 获得LUT动态生成模式下的Gamma值。请参考CameraSetGamma
        //        函数的功能描述。
        // 参数     : hCamera  相机的句柄，由CameraInit函数获得。
        //            piGamma    指针，返回当前的Gamma值。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraGetGamma(
            CameraHandle hCamera,
        ref int piGamma
        );

        /******************************************************/
        // 函数名   : CameraSetContrast
        // 功能描述 : 设定LUT动态生成模式下的对比度值。设定的值会
        //        马上保存在SDK内部，但是只有当相机处于动态
        //        参数生成的LUT模式时，才会生效。请参考CameraSetLutMode
        //        的函数说明部分。
        // 参数     : hCamera  相机的句柄，由CameraInit函数获得。
        //            iContrast  设定的对比度值。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraSetContrast(
            CameraHandle hCamera,
            int iContrast
        );

        /******************************************************/
        // 函数名   : CameraGetContrast
        // 功能描述 : 获得LUT动态生成模式下的对比度值。请参考
        //        CameraSetContrast函数的功能描述。
        // 参数     : hCamera  相机的句柄，由CameraInit函数获得。
        //            piContrast 指针，返回当前的对比度值。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraGetContrast(
            CameraHandle hCamera,
        ref int piContrast
        );

        /******************************************************/
        // 函数名   : CameraSetSaturation
        // 功能描述 : 设定图像处理的饱和度。对黑白相机无效。
        //        设定范围由CameraGetCapability获得。100表示
        //        表示原始色度，不增强。
        // 参数     : hCamera    相机的句柄，由CameraInit函数获得。
        //            iSaturation  设定的饱和度值。 
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraSetSaturation(
            CameraHandle hCamera,
            int iSaturation
        );

        /******************************************************/
        // 函数名   : CameraGetSaturation
        // 功能描述 : 获得图像处理的饱和度。
        // 参数     : hCamera    相机的句柄，由CameraInit函数获得。
        //            piSaturation 指针，返回当前图像处理的饱和度值。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraGetSaturation(
            CameraHandle hCamera,
        ref int piSaturation
        );

        /******************************************************/
        // 函数名   : CameraSetMonochrome
        // 功能描述 : 设置彩色转为黑白功能的使能。
        // 参数     : hCamera 相机的句柄，由CameraInit函数获得。
        //            bEnable   TRUE，表示将彩色图像转为黑白。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraSetMonochrome(
            CameraHandle hCamera,
            uint bEnable
        );

        /******************************************************/
        // 函数名   : CameraGetMonochrome
        // 功能描述 : 获得彩色转换黑白功能的使能状况。
        // 参数     : hCamera  相机的句柄，由CameraInit函数获得。
        //            pbEnable   指针。返回TRUE表示开启了彩色图像
        //             转换为黑白图像的功能。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraGetMonochrome(
            CameraHandle hCamera,
        ref uint pbEnable
        );

        /******************************************************/
        // 函数名   : CameraSetInverse
        // 功能描述 : 设置彩图像颜色翻转功能的使能。
        // 参数     : hCamera  相机的句柄，由CameraInit函数获得。
        //            bEnable    TRUE，表示开启图像颜色翻转功能，
        //             可以获得类似胶卷底片的效果。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraSetInverse(
            CameraHandle hCamera,
            uint bEnable
        );

        /******************************************************/
        // 函数名   : CameraGetInverse
        // 功能描述 : 获得图像颜色反转功能的使能状态。
        // 参数     : hCamera  相机的句柄，由CameraInit函数获得。
        //            pbEnable   指针，返回该功能使能状态。 
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraGetInverse(
            CameraHandle hCamera,
        ref uint pbEnable
        );

        /******************************************************/
        // 函数名   : CameraSetAntiFlick
        // 功能描述 : 设置自动曝光时抗频闪功能的使能状态。对于手动
        //        曝光模式下无效。
        // 参数     : hCamera  相机的句柄，由CameraInit函数获得。
        //            bEnable    TRUE，开启抗频闪功能;FALSE，关闭该功能。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraSetAntiFlick(
            CameraHandle hCamera,
            uint bEnable
        );

        /******************************************************/
        // 函数名   : CameraGetAntiFlick
        // 功能描述 : 获得自动曝光时抗频闪功能的使能状态。
        // 参数     : hCamera  相机的句柄，由CameraInit函数获得。
        //            pbEnable   指针，返回该功能的使能状态。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraGetAntiFlick(
            CameraHandle hCamera,
        ref uint pbEnable
        );

        /******************************************************/
        // 函数名   : CameraGetLightFrequency
        // 功能描述 : 获得自动曝光时，消频闪的频率选择。
        // 参数     : hCamera      相机的句柄，由CameraInit函数获得。
        //            piFrequencySel 指针，返回选择的索引号。0:50HZ 1:60HZ
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraGetLightFrequency(
            CameraHandle hCamera,
        ref int piFrequencySel
        );

        /******************************************************/
        // 函数名   : CameraSetLightFrequency
        // 功能描述 : 设置自动曝光时消频闪的频率。
        // 参数     : hCamera     相机的句柄，由CameraInit函数获得。
        //            iFrequencySel 0:50HZ , 1:60HZ 
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraSetLightFrequency(
            CameraHandle hCamera,
            int iFrequencySel
        );

        /******************************************************/
        // 函数名   : CameraSetFrameSpeed
        // 功能描述 : 设定相机输出图像的帧率。相机可供选择的帧率模式由
        //        CameraGetCapability获得的信息结构体中iFrameSpeedDesc
        //        表示最大帧率选择模式个数。
        // 参数     : hCamera   相机的句柄，由CameraInit函数获得。
        //            iFrameSpeed 选择的帧率模式索引号，范围从0到
        //              CameraGetCapability获得的信息结构体中iFrameSpeedDesc - 1   
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraSetFrameSpeed(
            CameraHandle hCamera,
            int iFrameSpeed
        );

        /******************************************************/
        // 函数名   : CameraGetFrameSpeed
        // 功能描述 : 获得相机输出图像的帧率选择索引号。具体用法参考
        //        CameraSetFrameSpeed函数的功能描述部分。
        // 参数     : hCamera    相机的句柄，由CameraInit函数获得。
        //            piFrameSpeed 指针，返回选择的帧率模式索引号。 
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraGetFrameSpeed(
            CameraHandle hCamera,
        ref int piFrameSpeed
        );


        /******************************************************/
        // 函数名   : CameraSetParameterMode
        // 功能描述 : 设定参数存取的目标对象。
        // 参数     : hCamera  相机的句柄，由CameraInit函数获得。
        //            iMode  参数存取的对象。参考
        //          emSdkParameterMode的类型定义。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraSetParameterMode(
            CameraHandle hCamera,
            int iTarget
        );

        /******************************************************/
        // 函数名   : CameraGetParameterMode
        // 功能描述 : 
        // 参数     : hCamera  相机的句柄，由CameraInit函数获得。
        //            int* piTarget
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraGetParameterMode(
            CameraHandle hCamera,
        ref int piTarget
        );

        /******************************************************/
        // 函数名   : CameraSetParameterMask
        // 功能描述 : 设置参数存取的掩码。参数加载和保存时会根据该
        //        掩码来决定各个模块参数的是否加载或者保存。
        // 参数     : hCamera 相机的句柄，由CameraInit函数获得。
        //            uMask     掩码。参考CameraDefine.h中PROP_SHEET_INDEX
        //            类型定义。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraSetParameterMask(
            CameraHandle hCamera,
            uint uMask
        );

        /******************************************************/
        // 函数名   : CameraSaveParameter
        // 功能描述 : 保存当前相机参数到指定的参数组中。相机提供了A,B,C,D
        //        A,B,C,D四组空间来进行参数的保存。 
        // 参数     : hCamera  相机的句柄，由CameraInit函数获得。
        //            iTeam      PARAMETER_TEAM_A 保存到A组中,
        //             PARAMETER_TEAM_B 保存到B组中,
        //             PARAMETER_TEAM_C 保存到C组中,
        //             PARAMETER_TEAM_D 保存到D组中
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraSaveParameter(
            CameraHandle hCamera,
            int iTeam
        );

        /******************************************************/
        // 函数名   : CameraReadParameterFromFile
        // 功能描述 : 从PC上指定的参数文件中加载参数。我公司相机参数
        //        保存在PC上为.config后缀的文件，位于安装下的
        //        Camera\Configs文件夹中。
        // 参数     : hCamera  相机的句柄，由CameraInit函数获得。
        //            *sFileName 参数文件的完整路径。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraReadParameterFromFile(
            CameraHandle hCamera,
            byte[] sFileName
        );

        /******************************************************/
        // 函数名   : CameraLoadParameter
        // 功能描述 : 加载指定组的参数到相机中。
        // 参数     : hCamera  相机的句柄，由CameraInit函数获得。
        //            iTeam    PARAMETER_TEAM_A 加载A组参数,
        //             PARAMETER_TEAM_B 加载B组参数,
        //             PARAMETER_TEAM_C 加载C组参数,
        //             PARAMETER_TEAM_D 加载D组参数,
        //             PARAMETER_TEAM_DEFAULT 加载默认参数。    
        //             类型定义参考CameraDefine.h中emSdkParameterTeam类型
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraLoadParameter(
            CameraHandle hCamera,
            int iTeam
        );

        /******************************************************/
        // 函数名   : CameraGetCurrentParameterGroup
        // 功能描述 : 获得当前选择的参数组。
        // 参数     : hCamera  相机的句柄，由CameraInit函数获得。
        //            piTeam     指针，返回当前选择的参数组。返回值
        //             参考CameraLoadParameter中iTeam参数。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraGetCurrentParameterGroup(
            CameraHandle hCamera,
        ref int piTeam
        );

        /******************************************************/
        // 函数名   : CameraSetTransPackLen
        // 功能描述 : 设置相机传输图像数据的分包大小。
        //        目前的SDK版本中，该接口仅对GIGE接口相机有效，
        //        用来控制网络传输的分包大小。对于支持巨帧的网卡，
        //        我们建议选择8K的分包大小，可以有效的降低传输
        //        所占用的CPU处理时间。
        // 参数     : hCamera  相机的句柄，由CameraInit函数获得。
        //            iPackSel   分包长度选择的索引号。分包长度可由
        //             获得相机属性结构体中pPackLenDesc成员表述，
        //             iPackLenDesc成员则表示最大可选的分包模式个数。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraSetTransPackLen(
            CameraHandle hCamera,
            int iPackSel
        );

        /******************************************************/
        // 函数名   : CameraGetTransPackLen
        // 功能描述 : 获得相机当前传输分包大小的选择索引号。
        // 参数     : hCamera  相机的句柄，由CameraInit函数获得。
        //            piPackSel  指针，返回当前选择的分包大小索引号。
        //             参见CameraSetTransPackLen中iPackSel的
        //             说明。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraGetTransPackLen(
            CameraHandle hCamera,
        ref int piPackSel
        );

        /******************************************************/
        // 函数名   : CameraIsAeWinVisible
        // 功能描述 : 获得自动曝光参考窗口的显示状态。
        // 参数     : hCamera    相机的句柄，由CameraInit函数获得。
        //            pbIsVisible  指针，返回TRUE，则表示当前窗口会
        //               被叠加在图像内容上。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraIsAeWinVisible(
            CameraHandle hCamera,
        ref uint pbIsVisible
        );

        /******************************************************/
        // 函数名   : CameraSetAeWinVisible
        // 功能描述 : 设置自动曝光参考窗口的显示状态。当设置窗口状态
        //        为显示，调用CameraImageOverlay后，能够将窗口位置
        //        以矩形的方式叠加在图像上。
        // 参数     : hCamera   相机的句柄，由CameraInit函数获得。
        //            bIsVisible  TRUE，设置为显示；FALSE，不显示。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraSetAeWinVisible(
            CameraHandle hCamera,
            uint bIsVisible
        );

        /******************************************************/
        // 函数名   : CameraGetAeWindow
        // 功能描述 : 获得自动曝光参考窗口的位置。
        // 参数     : hCamera  相机的句柄，由CameraInit函数获得。
        //            piHOff     指针，返回窗口位置左上角横坐标值。
        //            piVOff     指针，返回窗口位置左上角纵坐标值。
        //            piWidth    指针，返回窗口的宽度。
        //            piHeight   指针，返回窗口的高度。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraGetAeWindow(
            CameraHandle hCamera,
        ref int piHOff,
        ref int piVOff,
        ref int piWidth,
        ref int piHeight
        );

        /******************************************************/
        // 函数名   : CameraSetAeWindow
        // 功能描述 : 设置自动曝光的参考窗口。
        // 参数     : hCamera  相机的句柄，由CameraInit函数获得。
        //            iHOff    窗口左上角的横坐标
        //            iVOff      窗口左上角的纵坐标
        //            iWidth     窗口的宽度 
        //            iHeight    窗口的高度
        //        如果iHOff、iVOff、iWidth、iHeight全部为0，则
        //        窗口设置为每个分辨率下的居中1/2大小。可以随着
        //        分辨率的变化而跟随变化；如果iHOff、iVOff、iWidth、iHeight
        //        所决定的窗口位置范围超出了当前分辨率范围内， 
        //          则自动使用居中1/2大小窗口。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraSetAeWindow(
            CameraHandle hCamera,
            int iHOff,
            int iVOff,
            int iWidth,
            int iHeight
        );

        /******************************************************/
        // 函数名   : CameraSetMirror
        // 功能描述 : 设置图像镜像操作。镜像操作分为水平和垂直两个方向。
        // 参数     : hCamera  相机的句柄，由CameraInit函数获得。
        //            iDir     表示镜像的方向。0，表示水平方向；1，表示垂直方向。
        //            bEnable  TRUE，使能镜像;FALSE，禁止镜像
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraSetMirror(
            CameraHandle hCamera,
            int iDir,
            uint bEnable
        );

        /******************************************************/
        // 函数名   : CameraGetMirror
        // 功能描述 : 获得图像的镜像状态。
        // 参数     : hCamera  相机的句柄，由CameraInit函数获得。
        //            iDir     表示要获得的镜像方向。
        //             0，表示水平方向；1，表示垂直方向。
        //            pbEnable   指针，返回TRUE，则表示iDir所指的方向
        //             镜像被使能。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraGetMirror(
            CameraHandle hCamera,
            int iDir,
        ref uint pbEnable
        );

        /******************************************************/
        // 函数名   : CameraSetRotate
        // 功能描述 : 设置图像旋转操作
        // 参数     : hCamera  相机的句柄，由CameraInit函数获得。
        //            iRot     表示旋转的角度（逆时针方向）（0：不旋转 1:90度 2:180度 3:270度）
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraStatus.h
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraSetRotate(
            CameraHandle hCamera,
            int iRot
        );

        /******************************************************/
        // 函数名   : CameraGetRotate
        // 功能描述 : 获得图像的旋转状态。
        // 参数     : hCamera  相机的句柄，由CameraInit函数获得。
        //            iRot     表示要获得的旋转方向。
        //               （逆时针方向）（0：不旋转 1:90度 2:180度 3:270度）
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraStatus.h
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraGetRotate(
            CameraHandle hCamera,
            out int iRot
        );

        /******************************************************/
        // 函数名   : CameraGetWbWindow
        // 功能描述 : 获得白平衡参考窗口的位置。
        // 参数     : hCamera  相机的句柄，由CameraInit函数获得。
        //            PiHOff   指针，返回参考窗口的左上角横坐标 。
        //            PiVOff     指针，返回参考窗口的左上角纵坐标 。
        //            PiWidth    指针，返回参考窗口的宽度。
        //            PiHeight   指针，返回参考窗口的高度。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraGetWbWindow(
            CameraHandle hCamera,
        ref int PiHOff,
        ref int PiVOff,
        ref int PiWidth,
        ref int PiHeight
        );

        /******************************************************/
        // 函数名   : CameraSetWbWindow
        // 功能描述 : 设置白平衡参考窗口的位置。
        // 参数     : hCamera 相机的句柄，由CameraInit函数获得。
        //            iHOff   参考窗口的左上角横坐标。
        //            iVOff     参考窗口的左上角纵坐标。
        //            iWidth    参考窗口的宽度。
        //            iHeight   参考窗口的高度。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraSetWbWindow(
            CameraHandle hCamera,
            int iHOff,
            int iVOff,
            int iWidth,
            int iHeight
        );

        /******************************************************/
        // 函数名   : CameraIsWbWinVisible
        // 功能描述 : 获得白平衡窗口的显示状态。
        // 参数     : hCamera  相机的句柄，由CameraInit函数获得。
        //            pbShow   指针，返回TRUE，则表示窗口是可见的。 
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraIsWbWinVisible(
            CameraHandle hCamera,
        ref uint pbShow
        );

        /******************************************************/
        // 函数名   : CameraSetWbWinVisible
        // 功能描述 : 设置白平衡窗口的显示状态。
        // 参数     : hCamera  相机的句柄，由CameraInit函数获得。
        //            bShow      TRUE，则表示设置为可见。在调用
        //             CameraImageOverlay后，图像内容上将以矩形
        //             的方式叠加白平衡参考窗口的位置。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraSetWbWinVisible(
            CameraHandle hCamera,
            uint bShow
        );

        /******************************************************/
        // 函数名   : CameraImageOverlay
        // 功能描述 : 将输入的图像数据上叠加十字线、白平衡参考窗口、
        //        自动曝光参考窗口等图形。只有设置为可见状态的
        //        十字线和参考窗口才能被叠加上。
        //        注意，该函数的输入图像必须是RGB格式。
        // 参数     : hCamera  相机的句柄，由CameraInit函数获得。
        //            pRgbBuffer 图像数据缓冲区。
        //            pFrInfo    图像的帧头信息。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraImageOverlay(
            CameraHandle hCamera,
            IntPtr pRgbBuffer,
        ref tSdkFrameHead pFrInfo
        );

        /******************************************************/
        // 函数名   : CameraSetCrossLine
        // 功能描述 : 设置指定十字线的参数。
        // 参数     : hCamera  相机的句柄，由CameraInit函数获得。
        //            iLine    表示要设置第几条十字线的状态。范围为[0,8]，共9条。    
        //            x          十字线中心位置的横坐标值。
        //            y      十字线中心位置的纵坐标值。
        //            uColor     十字线的颜色，格式为(R|(G<<8)|(B<<16))
        //            bVisible   十字线的显示状态。TRUE，表示显示。
        //             只有设置为显示状态的十字线，在调用
        //             CameraImageOverlay后才会被叠加到图像上。     
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraSetCrossLine(
            CameraHandle hCamera,
            int iLine,
            int x,
            int y,
            uint uColor,
            uint bVisible
        );

        /******************************************************/
        // 函数名   : CameraGetCrossLine
        // 功能描述 : 获得指定十字线的状态。
        // 参数     : hCamera  相机的句柄，由CameraInit函数获得。
        //            iLine    表示要获取的第几条十字线的状态。范围为[0,8]，共9条。  
        //            px     指针，返回该十字线中心位置的横坐标。
        //            py     指针，返回该十字线中心位置的横坐标。
        //            pcolor     指针，返回该十字线的颜色，格式为(R|(G<<8)|(B<<16))。
        //            pbVisible  指针，返回TRUE，则表示该十字线可见。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraGetCrossLine(
            CameraHandle hCamera,
            int iLine,
        ref int px,
        ref int py,
        ref uint pcolor,
        ref uint pbVisible
        );

        /******************************************************/
        // 函数名   : CameraGetCapability
        // 功能描述 : 获得相机的特性描述结构体。该结构体中包含了相机
        //        可设置的各种参数的范围信息。决定了相关函数的参数
        //        返回，也可用于动态创建相机的配置界面。
        // 参数     : hCamera   相机的句柄，由CameraInit函数获得。
        //            pCameraInfo 指针，返回该相机特性描述的结构体。
        //                        tSdkCameraCapbility在CameraDefine.h中定义。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraGetCapability(
            CameraHandle hCamera,
            IntPtr pCameraInfo
        );

        public static CameraSdkStatus CameraGetCapability(CameraHandle hCamera, out tSdkCameraCapbility cap)
        {
            CameraSdkStatus status;
            IntPtr ptr;

            ptr = Marshal.AllocHGlobal(Marshal.SizeOf(new tSdkCameraCapbility()));
            status = MvApi.CameraGetCapability(hCamera, ptr);
            cap = (tSdkCameraCapbility)Marshal.PtrToStructure(ptr, typeof(tSdkCameraCapbility));
            Marshal.FreeHGlobal(ptr);

            return status;
        }

        /******************************************************/
        // 函数名   : CameraWriteSN
        // 功能描述 : 设置相机的序列号。我公司相机序列号分为3级。
        //        0级的是我公司自定义的相机序列号，出厂时已经
        //        设定好，1级和2级留给二次开发使用。每级序列
        //        号长度都是32个字节。
        // 参数     : hCamera  相机的句柄，由CameraInit函数获得。
        //            pbySN    序列号的缓冲区。 
        //            iLevel   要设定的序列号级别，只能是1或者2。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraWriteSN(
            CameraHandle hCamera,
            Byte[] pbySN,
            int iLevel
        );

        /******************************************************/
        // 函数名   : CameraReadSN
        // 功能描述 : 读取相机指定级别的序列号。序列号的定义请参考
        //          CameraWriteSN函数的功能描述部分。
        // 参数     : hCamera  相机的句柄，由CameraInit函数获得。
        //            pbySN    序列号的缓冲区。
        //            iLevel     要读取的序列号级别。只能是1和2。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraReadSN(
            CameraHandle hCamera,
            Byte[] pbySN,
            int iLevel
        );
        /******************************************************/
        // 函数名   : CameraSetTriggerDelayTime
        // 功能描述 : 设置硬件触发模式下的触发延时时间，单位微秒。
        //        当硬触发信号来临后，经过指定的延时，再开始采集
        //        图像。仅部分型号的相机支持该功能。具体请查看
        //        产品说明书。
        // 参数     : hCamera    相机的句柄，由CameraInit函数获得。
        //            uDelayTimeUs 硬触发延时。单位微秒。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraSetTriggerDelayTime(
            CameraHandle hCamera,
            uint uDelayTimeUs
        );

        /******************************************************/
        // 函数名   : CameraGetTriggerDelayTime
        // 功能描述 : 获得当前设定的硬触发延时时间。
        // 参数     : hCamera     相机的句柄，由CameraInit函数获得。
        //            puDelayTimeUs 指针，返回延时时间，单位微秒。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraGetTriggerDelayTime(
            CameraHandle hCamera,
        ref uint puDelayTimeUs
        );

        /******************************************************/
        // 函数名   : CameraSetTriggerCount
        // 功能描述 : 设置触发模式下的触发帧数。对软件触发和硬件触发
        //        模式都有效。默认为1帧，即一次触发信号采集一帧图像。
        // 参数     : hCamera 相机的句柄，由CameraInit函数获得。
        //            iCount    一次触发采集的帧数。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraSetTriggerCount(
            CameraHandle hCamera,
            int iCount
        );

        /******************************************************/
        // 函数名   : CameraGetTriggerCount
        // 功能描述 : 获得一次触发的帧数。
        // 参数     : hCamera  相机的句柄，由CameraInit函数获得。
        //            int* piCount
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraGetTriggerCount(
            CameraHandle hCamera,
        ref int piCount
        );

        /******************************************************/
        // 函数名   : CameraSoftTrigger
        // 功能描述 : 执行一次软触发。执行后，会触发由CameraSetTriggerCount
        //          指定的帧数。
        // 参数     : hCamera  相机的句柄，由CameraInit函数获得。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraSoftTrigger(
            CameraHandle hCamera
        );

        /******************************************************/
        // 函数名   : CameraSetTriggerMode
        // 功能描述 : 设置相机的触发模式。
        // 参数     : hCamera  相机的句柄，由CameraInit函数获得。
        //            iModeSel   模式选择索引号。可设定的模式由
        //             CameraGetCapability函数获取。请参考
        //               CameraDefine.h中tSdkCameraCapbility的定义。
        //             一般情况，0表示连续采集模式；1表示
        //             软件触发模式；2表示硬件触发模式。  
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraSetTriggerMode(
            CameraHandle hCamera,
            int iModeSel
        );

        /******************************************************/
        // 函数名   : CameraGetTriggerMode
        // 功能描述 : 获得相机的触发模式。
        // 参数     : hCamera  相机的句柄，由CameraInit函数获得。
        //            piModeSel  指针，返回当前选择的相机触发模式的索引号。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraGetTriggerMode(
            CameraHandle hCamera,
        ref int piModeSel
        );


        /******************************************************/
        // 函数名 	: CameraSetStrobeMode
        // 功能描述	: 设置IO引脚端子上的STROBE信号。该信号可以做闪光灯控制，也可以做外部机械快门控制。
        // 参数	    : hCamera 相机的句柄，由CameraInit函数获得。
        //             iMode   当为STROBE_SYNC_WITH_TRIG_AUTO      和触发信号同步，触发后，相机进行曝光时，自动生成STROBE信号。
        //                                                         此时，有效极性可设置(CameraSetStrobePolarity)。
        //                     当为STROBE_SYNC_WITH_TRIG_MANUAL时，和触发信号同步，触发后，STROBE延时指定的时间后(CameraSetStrobeDelayTime)，
        //                                                         再持续指定时间的脉冲(CameraSetStrobePulseWidth)，
        //                                                         有效极性可设置(CameraSetStrobePolarity)。
        //                     当为STROBE_ALWAYS_HIGH时，STROBE信号恒为高,忽略其他设置
        //                     当为STROBE_ALWAYS_LOW时，STROBE信号恒为低,忽略其他设置
        //
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraSetStrobeMode(
            CameraHandle hCamera,
            int iMode
        );

        /******************************************************/
        // 函数名 	: CameraGetStrobeMode
        // 功能描述	: 或者当前STROBE信号设置的模式。
        // 参数	    : hCamera 相机的句柄，由CameraInit函数获得。
        //             piMode  指针，返回STROBE_SYNC_WITH_TRIG_AUTO,STROBE_SYNC_WITH_TRIG_MANUAL、STROBE_ALWAYS_HIGH或者STROBE_ALWAYS_LOW。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraGetStrobeMode(
            CameraHandle hCamera,
        ref int piMode
        );

        /******************************************************/
        // 函数名 	: CameraSetStrobeDelayTime
        // 功能描述	: 当STROBE信号处于STROBE_SYNC_WITH_TRIG时，通过该函数设置其相对触发信号延时时间。
        // 参数	    : hCamera       相机的句柄，由CameraInit函数获得。
        //             uDelayTimeUs  相对触发信号的延时时间，单位为us。可以为0，但不能为负数。 
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraSetStrobeDelayTime(
            CameraHandle hCamera,
            uint uDelayTimeUs
        );

        /******************************************************/
        // 函数名 	: CameraGetStrobeDelayTime
        // 功能描述	: 当STROBE信号处于STROBE_SYNC_WITH_TRIG时，通过该函数获得其相对触发信号延时时间。
        // 参数	    : hCamera           相机的句柄，由CameraInit函数获得。
        //             upDelayTimeUs     指针，返回延时时间，单位us。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraGetStrobeDelayTime(
            CameraHandle hCamera,
        ref uint upDelayTimeUs
        );

        /******************************************************/
        // 函数名 	: CameraSetStrobePulseWidth
        // 功能描述	: 当STROBE信号处于STROBE_SYNC_WITH_TRIG时，通过该函数设置其脉冲宽度。
        // 参数	    : hCamera       相机的句柄，由CameraInit函数获得。
        //             uTimeUs       脉冲的宽度，单位为时间us。  
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraSetStrobePulseWidth(
            CameraHandle hCamera,
            uint uTimeUs
        );

        /******************************************************/
        // 函数名 	: CameraGetStrobePulseWidth
        // 功能描述	: 当STROBE信号处于STROBE_SYNC_WITH_TRIG时，通过该函数获得其脉冲宽度。
        // 参数	    : hCamera   相机的句柄，由CameraInit函数获得。
        //             upTimeUs  指针，返回脉冲宽度。单位为时间us。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraGetStrobePulseWidth(
            CameraHandle hCamera,
        ref uint upTimeUs
        );

        /******************************************************/
        // 函数名 	: CameraSetStrobePolarity
        // 功能描述	: 当STROBE信号处于STROBE_SYNC_WITH_TRIG时，通过该函数设置其有效电平的极性。默认为高有效，当触发信号到来时，STROBE信号被拉高。
        // 参数	    : hCamera   相机的句柄，由CameraInit函数获得。
        //             iPolarity STROBE信号的极性，0为低电平有效，1为高电平有效。默认为高电平有效。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraSetStrobePolarity(
            CameraHandle hCamera,
            int uPolarity
        );

        /******************************************************/
        // 函数名 	: CameraGetStrobePolarity
        // 功能描述	: 获得相机当前STROBE信号的有效极性。默认为高电平有效。
        // 参数	    : hCamera       相机的句柄，由CameraInit函数获得。
        //             ipPolarity    指针，返回STROBE信号当前的有效极性。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraGetStrobePolarity(
            CameraHandle hCamera,
        ref int upPolarity
        );

        /******************************************************/
        // 函数名 	: CameraSetExtTrigSignalType
        // 功能描述	: 设置相机外触发信号的种类。上边沿、下边沿、或者高、低电平方式。
        // 参数	    : hCamera   相机的句柄，由CameraInit函数获得。
        //             iType     外触发信号种类，返回值参考CameraDefine.h中
        //                       emExtTrigSignal类型定义。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraSetExtTrigSignalType(
            CameraHandle hCamera,
            int iType
        );

        /******************************************************/
        // 函数名 	: CameraGetExtTrigSignalType
        // 功能描述	: 获得相机当前外触发信号的种类。
        // 参数	    : hCamera   相机的句柄，由CameraInit函数获得。
        //             ipType    指针，返回外触发信号种类，返回值参考CameraDefine.h中
        //                       emExtTrigSignal类型定义。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraGetExtTrigSignalType(
            CameraHandle hCamera,
        ref int ipType
        );

        /******************************************************/
        // 函数名 	: CameraSetExtTrigShutterType
        // 功能描述	: 设置外触发模式下，相机快门的方式，默认为标准快门方式。
        //              部分滚动快门的CMOS相机支持GRR方式。
        // 参数	    : hCamera   相机的句柄，由CameraInit函数获得。
        //             iType     外触发快门方式。参考CameraDefine.h中emExtTrigShutterMode类型。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraSetExtTrigShutterType(
            CameraHandle hCamera,
            int iType
        );

        /******************************************************/
        // 函数名 	: CameraSetExtTrigShutterType
        // 功能描述	: 获得外触发模式下，相机快门的方式，默认为标准快门方式。
        //              部分滚动快门的CMOS相机支持GRR方式。
        // 参数	    : hCamera   相机的句柄，由CameraInit函数获得。
        //             ipType    指针，返回当前设定的外触发快门方式。返回值参考
        //                       CameraDefine.h中emExtTrigShutterMode类型。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraGetExtTrigShutterType(
            CameraHandle hCamera,
        ref int ipType
        );

        /******************************************************/
        // 函数名 	: CameraSetExtTrigDelayTime
        // 功能描述	: 设置外触发信号延时时间，默认为0，单位为微秒。 
        //              当设置的值uDelayTimeUs不为0时，相机接收到外触发信号后，将延时uDelayTimeUs个微秒后再进行图像捕获。
        // 参数	    : hCamera       相机的句柄，由CameraInit函数获得。
        //             uDelayTimeUs  延时时间，单位为微秒，默认为0.
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraSetExtTrigDelayTime(
            CameraHandle hCamera,
            uint uDelayTimeUs
        );

        /******************************************************/
        // 函数名 	: CameraGetExtTrigDelayTime
        // 功能描述	: 获得设置的外触发信号延时时间，默认为0，单位为微秒。 
        // 参数	    : hCamera   相机的句柄，由CameraInit函数获得。
        //            ref uint  upDelayTimeUs
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraGetExtTrigDelayTime(
            CameraHandle hCamera,
        ref uint upDelayTimeUs
        );

        /******************************************************/
        // 函数名 	: CameraSetExtTrigJitterTime
        // 功能描述	: 设置相机外触发信号的消抖时间。默认为0，单位为微秒。
        // 参数	    : hCamera   相机的句柄，由CameraInit函数获得。
        //            uint uTimeUs
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraSetExtTrigJitterTime(
            CameraHandle hCamera,
            uint uTimeUs
        );

        /******************************************************/
        // 函数名 	: CameraGetExtTrigJitterTime
        // 功能描述	: 获得设置的相机外触发消抖时间，默认为0.单位为微妙
        // 参数	    : hCamera   相机的句柄，由CameraInit函数获得。
        //            ref uint      upTimeUs
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraGetExtTrigJitterTime(
            CameraHandle hCamera,
        ref uint upTimeUs
        );

        /******************************************************/
        // 函数名 	: CameraGetExtTrigCapability
        // 功能描述	: 获得相机外触发的属性掩码
        // 参数	    : hCamera           相机的句柄，由CameraInit函数获得。
        //             puCapabilityMask  指针，返回该相机外触发特性掩码，掩码参考CameraDefine.h中
        //                               EXT_TRIG_MASK_ 开头的宏定义。   
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraGetExtTrigCapability(
            CameraHandle hCamera,
        ref uint puCapabilityMask
        );


        /******************************************************/
        // 函数名   : CameraGetResolutionForSnap
        // 功能描述 : 获得抓拍模式下的分辨率选择索引号。
        // 参数     : hCamera        相机的句柄，由CameraInit函数获得。
        //            pImageResolution 指针，返回抓拍模式的分辨率。 
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraGetResolutionForSnap(
            CameraHandle hCamera,
        ref tSdkImageResolution pImageResolution
        );

        /******************************************************/
        // 函数名   : CameraSetResolutionForSnap
        // 功能描述 : 设置抓拍模式下相机输出图像的分辨率。
        // 参数     : hCamera       相机的句柄，由CameraInit函数获得。
        //            pImageResolution 如果pImageResolution->iWidth 
        //                 和 pImageResolution->iHeight都为0，
        //                         则表示设定为跟随当前预览分辨率。抓
        //                         怕到的图像的分辨率会和当前设定的 
        //                 预览分辨率一样。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraSetResolutionForSnap(
            CameraHandle hCamera,
        ref tSdkImageResolution pImageResolution
        );

        /******************************************************/
        // 函数名   : CameraCustomizeResolution
        // 功能描述 : 打开分辨率自定义面板，并通过可视化的方式
        //        来配置一个自定义分辨率。
        // 参数     : hCamera    相机的句柄，由CameraInit函数获得。
        //            pImageCustom 指针，返回自定义的分辨率。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraCustomizeResolution(
            CameraHandle hCamera,
        ref tSdkImageResolution pImageCustom
        );

        /******************************************************/
        // 函数名   : CameraCustomizeReferWin
        // 功能描述 : 打开参考窗口自定义面板。并通过可视化的方式来
        //        获得一个自定义窗口的位置。一般是用自定义白平衡
        //        和自动曝光的参考窗口。
        // 参数     : hCamera  相机的句柄，由CameraInit函数获得。
        //            iWintype   要生成的参考窗口的用途。0,自动曝光参考窗口；
        //             1,白平衡参考窗口。
        //            hParent    调用该函数的窗口的句柄。可以为NULL。
        //            piHOff     指针，返回自定义窗口的左上角横坐标。
        //            piVOff     指针，返回自定义窗口的左上角纵坐标。
        //            piWidth    指针，返回自定义窗口的宽度。 
        //            piHeight   指针，返回自定义窗口的高度。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraCustomizeReferWin(
            CameraHandle hCamera,
            int iWintype,
            IntPtr hParent,
        ref int piHOff,
        ref int piVOff,
        ref int piWidth,
        ref int piHeight
        );

        /******************************************************/
        // 函数名   : CameraShowSettingPage
        // 功能描述 : 设置相机属性配置窗口显示状态。必须先调用CameraCreateSettingPage
        //        成功创建相机属性配置窗口后，才能调用本函数进行
        //        显示。
        // 参数     : hCamera  相机的句柄，由CameraInit函数获得。
        //            bShow    TRUE，显示;FALSE，隐藏。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraShowSettingPage(
            CameraHandle hCamera,
            uint bShow
        );

        /******************************************************/
        // 函数名   : CameraCreateSettingPage
        // 功能描述 : 创建该相机的属性配置窗口。调用该函数，SDK内部会
        //        帮您创建好相机的配置窗口，省去了您重新开发相机
        //        配置界面的时间。强烈建议使用您使用该函数让
        //        SDK为您创建好配置窗口。
        // 参数     : hCamera     相机的句柄，由CameraInit函数获得。
        //            hParent       应用程序主窗口的句柄。可以为NULL。
        //            pWintext      字符串指针，窗口显示的标题栏。
        //            pCallbackFunc 窗口消息的回调函数，当相应的事件发生时，
        //              pCallbackFunc所指向的函数会被调用，
        //              例如切换了参数之类的操作时，pCallbackFunc
        //              被回调时，在入口参数处指明了消息类型。
        //              这样可以方便您自己开发的界面和我们生成的UI
        //              之间进行同步。该参数可以为NULL。    
        //            pCallbackCtx  回调函数的附加参数。可以为NULL。pCallbackCtx
        //              会在pCallbackFunc被回调时，做为参数之一传入。
        //              您可以使用该参数来做一些灵活的判断。
        //            uReserved     预留。必须设置为0。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraCreateSettingPage(
            CameraHandle hCamera,
            IntPtr hParent,
            byte[] pWintext,
            CAMERA_PAGE_MSG_PROC pCallbackFunc,
            IntPtr pCallbackCtx,
            uint uReserved
        );

        /******************************************************/
        // 函数名   : CameraSetActiveSettingSubPage
        // 功能描述 : 设置相机配置窗口的激活页面。相机配置窗口有多个
        //        子页面构成，该函数可以设定当前哪一个子页面
        //        为激活状态，显示在最前端。
        // 参数     : hCamera  相机的句柄，由CameraInit函数获得。
        //            index      子页面的索引号。参考CameraDefine.h中
        //             PROP_SHEET_INDEX的定义。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraSetActiveSettingSubPage(
            CameraHandle hCamera,
            int index
        );

        /******************************************************/
        // 函数名   : CameraSpecialControl
        // 功能描述 : 相机一些特殊配置所调用的接口，二次开发时一般不需要
        //        调用。
        // 参数     : hCamera  相机的句柄，由CameraInit函数获得。
        //            dwCtrlCode 控制码。
        //            dwParam    控制子码，不同的dwCtrlCode时，意义不同。
        //            lpData     附加参数。不同的dwCtrlCode时，意义不同。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraSpecialControl(
            CameraHandle hCamera,
            uint dwCtrlCode,
            uint dwParam,
            IntPtr lpData
        );

        /******************************************************/
        // 函数名   : CameraGetFrameStatistic
        // 功能描述 : 获得相机接收帧率的统计信息，包括错误帧和丢帧的情况。
        // 参数     : hCamera        相机的句柄，由CameraInit函数获得。
        //            psFrameStatistic 指针，返回统计信息。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraGetFrameStatistic(
            CameraHandle hCamera,
        out tSdkFrameStatistic psFrameStatistic
        );

        /******************************************************/
        // 函数名   : CameraSetNoiseFilter
        // 功能描述 : 设置图像降噪模块的使能状态。
        // 参数     : hCamera 相机的句柄，由CameraInit函数获得。
        //            bEnable   TRUE，使能；FALSE，禁止。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraSetNoiseFilter(
            CameraHandle hCamera,
            uint bEnable
        );

        /******************************************************/
        // 函数名   : CameraGetNoiseFilterState
        // 功能描述 : 获得图像降噪模块的使能状态。
        // 参数     : hCamera  相机的句柄，由CameraInit函数获得。
        //            *pEnable   指针，返回状态。TRUE，为使能。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraGetNoiseFilterState(
            CameraHandle hCamera,
        ref uint pEnable
        );

        /******************************************************/
        // 函数名   : CameraRstTimeStamp
        // 功能描述 : 复位图像采集的时间戳，从0开始。
        // 参数     : CameraHandle hCamera
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraRstTimeStamp(
            CameraHandle hCamera
        );

        /******************************************************/
        // 函数名   : CameraSaveUserData
        // 功能描述 : 将用户自定义的数据保存到相机的非易性存储器中。
        //              每个型号的相机可能支持的用户数据区最大长度不一样。
        //              可以从设备的特性描述中获取该长度信息。
        // 参数     : hCamera    相机的句柄，由CameraInit函数获得。
        //            uStartAddr  起始地址，从0开始。
        //            pbData      数据缓冲区指针
        //            ilen        写入数据的长度，ilen + uStartAddr必须
        //                        小于用户区最大长度
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraSaveUserData(
            CameraHandle hCamera,
            uint uStartAddr,
            Byte[] pbData,
            int ilen
        );

        /******************************************************/
        // 函数名   : CameraLoadUserData
        // 功能描述 : 从相机的非易性存储器中读取用户自定义的数据。
        //              每个型号的相机可能支持的用户数据区最大长度不一样。
        //              可以从设备的特性描述中获取该长度信息。
        // 参数     : hCamera    相机的句柄，由CameraInit函数获得。
        //            uStartAddr  起始地址，从0开始。
        //            pbData      数据缓冲区指针，返回读到的数据。
        //            ilen        读取数据的长度，ilen + uStartAddr必须
        //                        小于用户区最大长度
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraLoadUserData(
            CameraHandle hCamera,
            uint uStartAddr,
            Byte[] pbData,
            int ilen
        );

        /******************************************************/
        // 函数名 : CameraGetFriendlyName
        // 功能描述 : 读取用户自定义的设备昵称。
        // 参数   : hCamera  相机的句柄，由CameraInit函数获得。
        //        pName    指针，返回指向0结尾的字符串，
        //             设备昵称不超过32个字节，因此该指针
        //             指向的缓冲区必须大于等于32个字节空间。
        // 返回值 : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //        否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //        中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraGetFriendlyName(
          CameraHandle hCamera,
          Byte[] pName
        );

        /******************************************************/
        // 函数名 : CameraSetFriendlyName
        // 功能描述 : 设置用户自定义的设备昵称。
        // 参数   : hCamera  相机的句柄，由CameraInit函数获得。
        //        pName    指针，指向0结尾的字符串，
        //             设备昵称不超过32个字节，因此该指针
        //             指向字符串必须小于等于32个字节空间。
        // 返回值 : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //        否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //        中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraSetFriendlyName(
          CameraHandle hCamera,
          Byte[] pName
        );

        /******************************************************/
        // 函数名 : CameraSdkGetVersionString
        // 功能描述 : 
        // 参数   : pVersionString 指针，返回SDK版本字符串。
        //                该指针指向的缓冲区大小必须大于
        //                32个字节
        // 返回值 : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //        否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //        中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraSdkGetVersionString(
            Byte[] pVersionString
        );

        /******************************************************/
        // 函数名 : CameraCheckFwUpdate
        // 功能描述 : 检测固件版本，是否需要升级。
        // 参数   : hCamera 相机的句柄，由CameraInit函数获得。
        //        pNeedUpdate 指针，返回固件检测状态，TRUE表示需要更新
        // 返回值 : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //        否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //        中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraCheckFwUpdate(
          CameraHandle hCamera,
        ref uint pNeedUpdate
        );

        /******************************************************/
        // 函数名 : CameraGetFirmwareVision
        // 功能描述 : 获得固件版本的字符串
        // 参数   : hCamera 相机的句柄，由CameraInit函数获得。
        //        pVersion 必须指向一个大于32字节的缓冲区，
        //            返回固件的版本字符串。
        // 返回值 : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //        否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //        中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraGetFirmwareVision(
          CameraHandle hCamera,
          Byte[] pVersion
        );

        /******************************************************/
        // 函数名 : CameraGetEnumInfo
        // 功能描述 : 获得指定设备的枚举信息
        // 参数   : hCamera 相机的句柄，由CameraInit函数获得。
        //        pCameraInfo 指针，返回设备的枚举信息。
        // 返回值 : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //        否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //        中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraGetEnumInfo(
          CameraHandle hCamera,
        ref tSdkCameraDevInfo pCameraInfo
        );

        /******************************************************/
        // 函数名 : CameraGetInerfaceVersion
        // 功能描述 : 获得指定设备接口的版本
        // 参数   : hCamera 相机的句柄，由CameraInit函数获得。
        //        pVersion 指向一个大于32字节的缓冲区，返回接口版本字符串。
        // 返回值 : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //        否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //        中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraGetInerfaceVersion(
          CameraHandle hCamera,
          Byte[] pVersion
        );

        /******************************************************/
        // 函数名 : CameraSetIOState
        // 功能描述 : 设置指定IO的电平状态，IO为输出型IO，相机
        //        预留可编程输出IO的个数由tSdkCameraCapbility中
        //        iOutputIoCounts决定。
        // 参数   : hCamera 相机的句柄，由CameraInit函数获得。
        //        iOutputIOIndex IO的索引号，从0开始。
        //        uState 要设定的状态，1为高，0为低
        // 返回值 : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //        否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //        中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraSetIOState(
          CameraHandle hCamera,
          int iOutputIOIndex,
          uint uState
        );

        /******************************************************/
        // 函数名 : CameraGetIOState
        // 功能描述 : 设置指定IO的电平状态，IO为输入型IO，相机
        //        预留可编程输出IO的个数由tSdkCameraCapbility中
        //        iInputIoCounts决定。
        // 参数   : hCamera 相机的句柄，由CameraInit函数获得。      
        //        iInputIOIndex IO的索引号，从0开始。
        //        puState 指针，返回IO状态,1为高，0为低
        // 返回值 : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //        否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //        中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraGetIOState(
          CameraHandle hCamera,
          int iInputIOIndex,
        ref uint puState
        );

        /******************************************************/
        // 函数名   : CameraSetInPutIOMode
        // 功能描述 : 设置输入IO的模式，相机
        //              预留可编程输出IO的个数由tSdkCameraCapbility中
        //              iInputIoCounts决定。
        // 参数     : hCamera 相机的句柄，由CameraInit函数获得。          
        //            iInputIOIndex IO的索引号，从0开始。
        //            iMode IO模式,参考CameraDefine.h中emCameraGPIOMode
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraStatus.h
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraSetInPutIOMode(
            CameraHandle hCamera,
            int iInputIOIndex,
            int iMode
            );

        /******************************************************/
        // 函数名   : CameraSetOutPutIOMode
        // 功能描述 : 设置输出IO的模式，相机
        //              预留可编程输出IO的个数由tSdkCameraCapbility中
        //              iOutputIoCounts决定。
        // 参数     : hCamera 相机的句柄，由CameraInit函数获得。          
        //            iOutputIOIndex IO的索引号，从0开始。
        //            iMode IO模式,参考CameraDefine.h中emCameraGPIOMode
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraStatus.h
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraSetOutPutIOMode(
            CameraHandle hCamera,
            int iOutputIOIndex,
            int iMode
            );

        /******************************************************/
        // 函数名   : CameraSetOutPutPWM
        // 功能描述 : 设置PWM型输出的参数，相机
        //              预留可编程输出IO的个数由tSdkCameraCapbility中
        //              iOutputIoCounts决定。
        // 参数     : hCamera 相机的句柄，由CameraInit函数获得。          
        //            iOutputIOIndex IO的索引号，从0开始。
        //            iCycle PWM的周期，单位(us)
        //			  uDuty  占用比，取值1%~99%
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraStatus.h
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraSetOutPutPWM(
            CameraHandle hCamera,
            int iOutputIOIndex,
            uint iCycle,
            uint uDuty
            );

        /******************************************************/
        // 函数名 : CameraSetAeAlgorithm
        // 功能描述 : 设置自动曝光时选择的算法，不同的算法适用于
        //        不同的场景。
        // 参数   : hCamera     相机的句柄，由CameraInit函数获得。 
        //        iIspProcessor   选择执行该算法的对象，参考CameraDefine.h
        //                emSdkIspProcessor的定义
        //        iAeAlgorithmSel 要选择的算法编号。从0开始，最大值由tSdkCameraCapbility
        //                中iAeAlmSwDesc和iAeAlmHdDesc决定。  
        // 返回值 : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //        否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //        中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraSetAeAlgorithm(
          CameraHandle hCamera,
          int iIspProcessor,
          int iAeAlgorithmSel
        );

        /******************************************************/
        // 函数名 : CameraGetAeAlgorithm
        // 功能描述 : 获得当前自动曝光所选择的算法
        // 参数   : hCamera     相机的句柄，由CameraInit函数获得。 
        //        iIspProcessor   选择执行该算法的对象，参考CameraDefine.h
        //                emSdkIspProcessor的定义
        //        piAeAlgorithmSel  返回当前选择的算法编号。从0开始，最大值由tSdkCameraCapbility
        //                中iAeAlmSwDesc和iAeAlmHdDesc决定。  
        // 返回值 : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //        否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //        中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraGetAeAlgorithm(
          CameraHandle hCamera,
          int iIspProcessor,
        ref int piAlgorithmSel
        );

        /******************************************************/
        // 函数名 : CameraSetBayerDecAlgorithm
        // 功能描述 : 设置Bayer数据转彩色的算法。
        // 参数   : hCamera     相机的句柄，由CameraInit函数获得。 
        //        iIspProcessor   选择执行该算法的对象，参考CameraDefine.h
        //                emSdkIspProcessor的定义
        //        iAlgorithmSel   要选择的算法编号。从0开始，最大值由tSdkCameraCapbility
        //                中iBayerDecAlmSwDesc和iBayerDecAlmHdDesc决定。    
        // 返回值 : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //        否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //        中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraSetBayerDecAlgorithm(
          CameraHandle hCamera,
          int iIspProcessor,
          int iAlgorithmSel
        );

        /******************************************************/
        // 函数名 : CameraGetBayerDecAlgorithm
        // 功能描述 : 获得Bayer数据转彩色所选择的算法。
        // 参数   : hCamera     相机的句柄，由CameraInit函数获得。 
        //        iIspProcessor   选择执行该算法的对象，参考CameraDefine.h
        //                emSdkIspProcessor的定义
        //        piAlgorithmSel  返回当前选择的算法编号。从0开始，最大值由tSdkCameraCapbility
        //                中iBayerDecAlmSwDesc和iBayerDecAlmHdDesc决定。  
        // 返回值 : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //        否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //        中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraGetBayerDecAlgorithm(
          CameraHandle hCamera,
          int iIspProcessor,
        ref int piAlgorithmSel
        );

        /******************************************************/
        // 函数名 : CameraSetIspProcessor
        // 功能描述 : 设置图像处理单元的算法执行对象，由PC端或者相机端
        //        来执行算法，当由相机端执行时，会降低PC端的CPU占用率。
        // 参数   : hCamera   相机的句柄，由CameraInit函数获得。 
        //        iIspProcessor 参考CameraDefine.h中
        //              emSdkIspProcessor的定义。
        // 返回值 : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //        否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //        中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraSetIspProcessor(
          CameraHandle hCamera,
          int iIspProcessor
        );

        /******************************************************/
        // 函数名 : CameraGetIspProcessor
        // 功能描述 : 获得图像处理单元的算法执行对象。
        // 参数   : hCamera    相机的句柄，由CameraInit函数获得。 
        //        piIspProcessor 返回选择的对象，返回值参考CameraDefine.h中
        //               emSdkIspProcessor的定义。
        // 返回值 : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //        否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //        中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraGetIspProcessor(
          CameraHandle hCamera,
        ref int piIspProcessor
        );

        /******************************************************/
        // 函数名 : CameraSetBlackLevel
        // 功能描述 : 设置图像的黑电平基准，默认值为0
        // 参数   : hCamera   相机的句柄，由CameraInit函数获得。 
        //        iBlackLevel 要设定的电平值。范围为0到255。  
        // 返回值 : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //        否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //        中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraSetBlackLevel(
          CameraHandle hCamera,
          int iBlackLevel
        );

        /******************************************************/
        // 函数名 : CameraGetBlackLevel
        // 功能描述 : 获得图像的黑电平基准，默认值为0
        // 参数   : hCamera    相机的句柄，由CameraInit函数获得。 
        //        piBlackLevel 返回当前的黑电平值。范围为0到255。
        // 返回值 : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //        否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //        中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraGetBlackLevel(
          CameraHandle hCamera,
        ref int piBlackLevel
        );

        /******************************************************/
        // 函数名 : CameraSetWhiteLevel
        // 功能描述 : 设置图像的白电平基准，默认值为255
        // 参数   : hCamera   相机的句柄，由CameraInit函数获得。 
        //        iWhiteLevel 要设定的电平值。范围为0到255。  
        // 返回值 : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //        否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //        中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraSetWhiteLevel(
          CameraHandle hCamera,
          int iWhiteLevel
        );

        /******************************************************/
        // 函数名 : CameraGetWhiteLevel
        // 功能描述 : 获得图像的白电平基准，默认值为255
        // 参数   : hCamera    相机的句柄，由CameraInit函数获得。 
        //        piWhiteLevel 返回当前的白电平值。范围为0到255。
        // 返回值 : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //        否则返回非0值的错误码,请参考CameraSdkStatus的类型定义
        //        中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraGetWhiteLevel(
          CameraHandle hCamera,
        ref int piWhiteLevel
        );



        /******************************************************/
        // 函数名   : CameraEnumerateDeviceEx
        // 功能描述 : 枚举设备，并建立设备列表。在调用CameraInitEx
        //            之前，必须调用该函数枚举设备。
        // 参数      : 
        // 返回值     : 返回设备个数，0表示无。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern int CameraEnumerateDeviceEx(
        );


        /******************************************************/
        // 函数名 	: CameraInitEx
        // 功能描述	: 相机初始化。初始化成功后，才能调用任何其他
        //			  相机相关的操作接口。		
        // 参数	    : iDeviceIndex    相机的索引号，CameraEnumerateDeviceEx返回相机个数。	
        //            iParamLoadMode  相机初始化时使用的参数加载方式。-1表示使用上次退出时的参数加载方式。
        //            emTeam         初始化时使用的参数组。-1表示加载上次退出时的参数组。
        //            pCameraHandle  相机的句柄指针，初始化成功后，该指针
        //							 返回该相机的有效句柄，在调用其他相机
        //							 相关的操作接口时，都需要传入该句柄，主要
        //							 用于多相机之间的区分。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraStatus.h
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraInitEx(
            int iDeviceIndex,
            int iParamLoadMode,
            int emTeam,
        ref CameraHandle pCameraHandle
        );


        /******************************************************/
        // 函数名 	: CameraInitEx2
        // 功能描述	: 相机初始化。初始化成功后，才能调用任何其他
        //			  相机相关的操作接口。		
        // 参数	    : CameraName     相机名称
        //            pCameraHandle  相机的句柄指针，初始化成功后，该指针
        //							 返回该相机的有效句柄，在调用其他相机
        //							 相关的操作接口时，都需要传入该句柄，主要
        //							 用于多相机之间的区分。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraStatus.h
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraInitEx2(
            byte[] CameraName,
            ref CameraHandle pCameraHandle
        );


        /******************************************************/
        // 函数名   : CameraGetImageBufferEx
        // 功能描述 : 获得一帧图像数据。该接口获得的图像是经过处理后的RGB格式。该函数调用后，
        //            不需要调用 CameraReleaseImageBuffer 释放，也不要调用free之类的函数释放
        //              来释放该函数返回的图像数据缓冲区。
        // 参数     : hCamera     相机的句柄，由CameraInit函数获得。
        //            piWidth    整形指针，返回图像的宽度
        //            piHeight   整形指针，返回图像的高度
        //            uint wTimes 抓取图像的超时时间。单位毫秒。在
        //                        wTimes时间内还未获得图像，则该函数
        //                        会返回超时信息。
        // 返回值   : 成功时，返回RGB数据缓冲区的首地址;
        //            否则返回0。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern IntPtr CameraGetImageBufferEx(
            CameraHandle hCamera,
        ref int piWidth,
        ref int piHeight,
            uint wTimes
        );

        /******************************************************/
        // 函数名   : CameraImageProcessEx
        // 功能描述 : 将获得的相机原始输出图像数据进行处理，叠加饱和度、
        //            颜色增益和校正、降噪等处理效果，最后得到RGB888
        //            格式的图像数据。  
        // 参数     : hCamera      相机的句柄，由CameraInit函数获得。
        //            pbyIn      输入图像数据的缓冲区地址，不能为NULL。 
        //            pbyOut        处理后图像输出的缓冲区地址，不能为NULL。
        //            pFrInfo       输入图像的帧头信息，处理完成后，帧头信息
        //            uOutFormat    处理完后图像的输出格式可以是CAMERA_MEDIA_TYPE_MONO8 CAMERA_MEDIA_TYPE_RGB CAMERA_MEDIA_TYPE_RGBA8的其中一种。
        //                          pbyIn对应的缓冲区大小，必须和uOutFormat指定的格式相匹配。
        //            uReserved     预留参数，必须设置为0     
        //                     中的图像格式uiMediaType会随之改变。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraStatus.h
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraImageProcessEx(
            CameraHandle hCamera,
            IntPtr pbyIn,
            IntPtr pbyOut,
        ref tSdkFrameHead pFrInfo,
            uint uOutFormat,
            uint uReserved
        );

        /******************************************************/
        // 函数名 	: CameraSetIspOutFormat
        // 功能描述	: 设置CameraGetImageBuffer函数的图像处理的输出格式，支持
        //              CAMERA_MEDIA_TYPE_MONO8和CAMERA_MEDIA_TYPE_RGB8和CAMERA_MEDIA_TYPE_RGBA8
        //              以及CAMERA_MEDIA_TYPE_BGR8、CAMERA_MEDIA_TYPE_BGRA8
        //              (在CameraDefine.h中定义)5种，分别对应8位灰度图像和24RGB、32位RGB、24位BGR、32位BGR彩色图像。
        // 参数	    : hCamera		相机的句柄，由CameraInit函数获得。 
        //             uFormat	要设定格式。CAMERA_MEDIA_TYPE_MONO8或者CAMERA_MEDIA_TYPE_RGB8、CAMERA_MEDIA_TYPE_RGBA8	
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraStatus.h
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraSetIspOutFormat(
            CameraHandle hCamera,
            uint uFormat
        );

        /******************************************************/
        // 函数名 	: CameraGetIspOutFormat
        // 功能描述	: 获得CameraGetImageBuffer函数图像处理的输出格式，支持
        //              CAMERA_MEDIA_TYPE_MONO8和CAMERA_MEDIA_TYPE_RGB8和CAMERA_MEDIA_TYPE_RGBA8
        //              以及CAMERA_MEDIA_TYPE_BGR8、CAMERA_MEDIA_TYPE_BGRA8
        //              (在CameraDefine.h中定义)三种，分别对应8位灰度图像和24RGB、32位RGB、24位BGR、32位BGR彩色图像。
        // 参数	    : hCamera		相机的句柄，由CameraInit函数获得。 
        //             puFormat	返回当前设定的格式。CAMERA_MEDIA_TYPE_MONO8或者CAMERA_MEDIA_TYPE_RGB8、CAMERA_MEDIA_TYPE_RGBA8	
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraStatus.h
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraGetIspOutFormat(
            CameraHandle hCamera,
        ref uint puFormat
        );

        /******************************************************/
        // 函数名 	: CameraReConnect
        // 功能描述	: 重新连接设备，用于USB设备意外掉线后重连
        // 参数	    : hCamera	   相机的句柄，由CameraInit函数获得。 
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraStatus.h
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraReConnect(
            CameraHandle hCamera
        );

        /******************************************************/
        // 函数名 	: CameraConnectTest
        // 功能描述	: 测试相机的连接状态，用于检测相机是否掉线
        // 参数	    : hCamera	   相机的句柄，由CameraInit函数获得。 
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0)，表示相机连接状态正常;
        //            否则返回非0值的错误码,请参考CameraStatus.h
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraConnectTest(
            CameraHandle hCamera
        );

        /******************************************************/
        // 函数名 	: CameraSetLedEnable
        // 功能描述	: 设置相机的LED使能状态，不带LED的型号，此函数返回错误代码，表示不支持。
        // 参数	    : hCamera	   相机的句柄，由CameraInit函数获得。 
        //             index       LED灯的索引号，从0开始。如果只有一个可控制亮度的LED，则该参数为0 。
        //             enable      使能状态
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0)，表示相机连接状态正常;
        //            否则返回 非0值，参考CameraStatus.h中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraSetLedEnable(
            CameraHandle hCamera,
            int index,
            int enable
            );

        /******************************************************/
        // 函数名 	: CameraGetLedEnable
        // 功能描述	: 获得相机的LED使能状态，不带LED的型号，此函数返回错误代码，表示不支持。
        // 参数	    : hCamera	   相机的句柄，由CameraInit函数获得。 
        //             index       LED灯的索引号，从0开始。如果只有一个可控制亮度的LED，则该参数为0 。
        //             enable      指针，返回LED使能状态
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0)，表示相机连接状态正常;
        //            否则返回 非0值，参考CameraStatus.h中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraGetLedEnable(
            CameraHandle hCamera,
            int index,
            out int enable
            );

        /******************************************************/
        // 函数名 	: CameraSetLedOnOff
        // 功能描述	: 设置相机的LED开关状态，不带LED的型号，此函数返回错误代码，表示不支持。
        // 参数	    : hCamera	   相机的句柄，由CameraInit函数获得。 
        //             index       LED灯的索引号，从0开始。如果只有一个可控制亮度的LED，则该参数为0 。
        //             onoff	   LED开关状态
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0)，表示相机连接状态正常;
        //            否则返回 非0值，参考CameraStatus.h中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraSetLedOnOff(
            CameraHandle hCamera,
            int index,
            int onoff
            );

        /******************************************************/
        // 函数名 	: CameraGetLedOnOff
        // 功能描述	: 获得相机的LED开关状态，不带LED的型号，此函数返回错误代码，表示不支持。
        // 参数	    : hCamera	   相机的句柄，由CameraInit函数获得。 
        //             index       LED灯的索引号，从0开始。如果只有一个可控制亮度的LED，则该参数为0 。
        //             onoff	   指针，返回LED开关状态
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0)，表示相机连接状态正常;
        //            否则返回 非0值，参考CameraStatus.h中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraGetLedOnOff(
            CameraHandle hCamera,
            int index,
            out int onoff
            );

        /******************************************************/
        // 函数名 	: CameraSetLedDuration
        // 功能描述	: 设置相机的LED持续时间，不带LED的型号，此函数返回错误代码，表示不支持。
        // 参数	    : hCamera	    相机的句柄，由CameraInit函数获得。 
        //             index        LED灯的索引号，从0开始。如果只有一个可控制亮度的LED，则该参数为0 。
        //             duration		LED持续时间，单位毫秒
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0)，表示相机连接状态正常;
        //            否则返回 非0值，参考CameraStatus.h中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraSetLedDuration(
            CameraHandle hCamera,
            int index,
            uint duration
            );

        /******************************************************/
        // 函数名 	: CameraGetLedDuration
        // 功能描述	: 获得相机的LED持续时间，不带LED的型号，此函数返回错误代码，表示不支持。
        // 参数	    : hCamera	    相机的句柄，由CameraInit函数获得。 
        //             index        LED灯的索引号，从0开始。如果只有一个可控制亮度的LED，则该参数为0 。
        //             duration		指针，返回LED持续时间，单位毫秒
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0)，表示相机连接状态正常;
        //            否则返回 非0值，参考CameraStatus.h中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraGetLedDuration(
            CameraHandle hCamera,
            int index,
            out uint duration
            );

        /******************************************************/
        // 函数名 	: CameraSetLedBrightness
        // 功能描述	: 设置相机的LED亮度，不带LED的型号，此函数返回错误代码，表示不支持。
        // 参数	    : hCamera	   相机的句柄，由CameraInit函数获得。 
        //             index      LED灯的索引号，从0开始。如果只有一个可控制亮度的LED，则该参数为0 。
        //             uBrightness LED亮度值，范围0到255. 0表示关闭，255最亮。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0)，表示相机连接状态正常;
        //            否则返回 非0值，参考CameraStatus.h中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraSetLedBrightness(
            CameraHandle hCamera,
            int index,
            uint uBrightness
        );

        /******************************************************/
        // 函数名 	: CameraGetLedBrightness
        // 功能描述	: 获得相机的LED亮度，不带LED的型号，此函数返回错误代码，表示不支持。
        // 参数	    : hCamera	   相机的句柄，由CameraInit函数获得。 
        //             index      LED灯的索引号，从0开始。如果只有一个可控制亮度的LED，则该参数为0 。
        //             uBrightness 指针，返回LED亮度值，范围0到255. 0表示关闭，255最亮。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0)，表示相机连接状态正常;
        //            否则返回 非0值，参考CameraStatus.h中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraGetLedBrightness(
            CameraHandle hCamera,
            int index,
            ref uint uBrightness
        );

        /******************************************************/
        // 函数名 	: CameraEnableTransferRoi
        // 功能描述	: 使能或者禁止相机的多区域传输功能，不带该功能的型号，此函数返回错误代码，表示不支持。
        //              该功能主要用于在相机端将采集的整幅画面切分，只传输指定的多个区域，以提高传输帧率。
        //              多个区域传输到PC上后，会自动拼接成整幅画面，没有被传输的部分，会用黑色填充。
        // 参数	    : hCamera	    相机的句柄，由CameraInit函数获得。 
        //             index       ROI区域的索引号，从0开始。
        //             uEnableMask 区域使能状态掩码，对应的比特位为1表示使能。0为禁止。目前SDK支持4个可编辑区域，index范围为0到3，即bit0 ，bit1，bit2，bit3控制4个区域的使能状态。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0)，表示相机连接状态正常;
        //            对于不支持多区域ROI传输的型号，该函数会返回 CAMERA_STATUS_NOT_SUPPORTED(-4) 表示不支持   
        //            其它非0值，参考CameraStatus.h中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraEnableTransferRoi(
            CameraHandle hCamera,
            uint uEnableMask
        );

        /******************************************************/
        // 函数名 	: CameraSetTransferRoi
        // 功能描述	: 设置相机传输的裁剪区域。在相机端，图像从传感器上被采集后，将会被裁剪成指定的区域来传送，此函数返回错误代码，表示不支持。
        // 参数	    : hCamera	   相机的句柄，由CameraInit函数获得。 
        //             index      ROI区域的索引号，从0开始。
        //             X1,Y1      ROI区域的左上角坐标
        //             X2,Y2      ROI区域的右上角坐标
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0)，表示相机连接状态正常;
        //            对于不支持多区域ROI传输的型号，该函数会返回 CAMERA_STATUS_NOT_SUPPORTED(-4) 表示不支持   
        //            其它非0值，参考CameraStatus.h中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraSetTransferRoi(
            CameraHandle hCamera,
            int index,
            uint X1,
            uint Y1,
            uint X2,
            uint Y2
        );

        /******************************************************/
        // 函数名 	: CameraGetTransferRoi
        // 功能描述	: 设置相机传输的裁剪区域。在相机端，图像从传感器上被采集后，将会被裁剪成指定的区域来传送，此函数返回错误代码，表示不支持。
        // 参数	    : hCamera	   相机的句柄，由CameraInit函数获得。 
        //             index      ROI区域的索引号，从0开始。
        //             pX1,pY1      ROI区域的左上角坐标
        //             pX2,pY2      ROI区域的右上角坐标
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0)，表示相机连接状态正常;
        //            对于不支持多区域ROI传输的型号，该函数会返回 CAMERA_STATUS_NOT_SUPPORTED(-4) 表示不支持   
        //            其它非0值，参考CameraStatus.h中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraGetTransferRoi(
            CameraHandle hCamera,
            int index,
            ref uint pX1,
            ref uint pY1,
            ref uint pX2,
            ref uint pY2
        );

        /******************************************************/
        // 函数名 	: CameraAlignMalloc
        // 功能描述	: 申请一段对齐的内存空间。功能和malloc类似，但
        //			  是返回的内存是以align指定的字节数对齐的。
        // 参数	    : size	   空间的大小。 
        //            align    地址对齐的字节数。
        // 返回值   : 成功时，返回非0值，表示内存首地址。失败返回NULL。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern IntPtr CameraAlignMalloc(
            int size,
            int align
        );

        /******************************************************/
        // 函数名 	: CameraAlignFree
        // 功能描述	: 释放由CameraAlignMalloc函数申请的内存空间。
        // 参数	    : membuffer	   由CameraAlignMalloc返回的内存首地址。 
        // 返回值   : 无。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern void CameraAlignFree(
            IntPtr membuffer
        );

        /******************************************************/
        // 函数名 	: CameraSetAutoConnect
        // 功能描述	: 设置自动使能重连
        // 参数	    : hCamera	   相机的句柄，由CameraInit函数获得。 
        //			  bEnable	   使能相机重连，当位TRUE时，SDK内部自动检测相机是否掉线，掉线后自己重连。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0)，表示相机连接状态正常;
        //            对于不支持的型号，该函数会返回 CAMERA_STATUS_NOT_SUPPORTED(-4) 表示不支持   
        //            其它非0值，参考CameraStatus.h中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraSetAutoConnect(CameraHandle hCamera, int bEnable);

        /******************************************************/
        // 函数名 	: CameraGetReConnectCounts
        // 功能描述	: 获得相机自动重连的次数，前提是CameraSetAutoConnect 使能相机自动重连功能。默认是使能的。
        // 参数	    : hCamera	   相机的句柄，由CameraInit函数获得。 
        //			 puCounts	   返回掉线自动重连的次数
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0)，表示相机连接状态正常;
        //            对于不支持的型号，该函数会返回 CAMERA_STATUS_NOT_SUPPORTED(-4) 表示不支持   
        //            其它非0值，参考CameraStatus.h中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraGetReConnectCounts(CameraHandle hCamera, ref uint puCounts);

        /******************************************************/
        // 函数名   : CameraEvaluateImageDefinition
        // 功能描述 : 图片清晰度评估
        // 参数     : hCamera  相机的句柄，由CameraInit函数获得。
        //			  iAlgorithSel 使用的评估算法,详见emEvaluateDefinitionAlgorith中的定义
        //            pbyIn    输入图像数据的缓冲区地址，不能为NULL。 
        //            pFrInfo  输入图像的帧头信息
        //			  DefinitionValue 返回的清晰度估值（越大越清晰）
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraStatus.h
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraEvaluateImageDefinition(
            CameraHandle hCamera,
            int iAlgorithSel,
            IntPtr pbyIn,
            ref tSdkFrameHead pFrInfo,
            out double DefinitionValue
        );

        /******************************************************/
        // 函数名   : CameraDrawText
        // 功能描述 : 在输入的图像数据中绘制文字
        // 参数     : pRgbBuffer 图像数据缓冲区
        //			  pFrInfo 图像的帧头信息
        //			  pFontFileName 字体文件名
        //			  FontWidth 字体宽度
        //			  FontHeight 字体高度
        //			  pText 要输出的文字
        //			  (Left, Top, Width, Height) 文字的输出矩形
        //			  TextColor 文字颜色RGB
        //			  uFlags 输出标志,详见emCameraDrawTextFlags中的定义
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraStatus.h
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraDrawText(
            IntPtr pRgbBuffer,
            ref tSdkFrameHead pFrInfo,
            byte[] pFontFileName,
            uint FontWidth,
            uint FontHeight,
            byte[] pText,
            int Left,
            int Top,
            uint Width,
            uint Height,
            uint TextColor,
            uint uFlags
        );

        /******************************************************/
        // 函数名   : CameraGigeGetIp
        // 功能描述 : 获取GIGE相机的IP地址
        // 参数     : pCameraInfo 相机的设备描述信息，可由CameraEnumerateDevice函数获得。 
        //			  CamIp 相机IP(注意：必须保证传入的缓冲区大于等于16字节)
        //			  CamMask 相机子网掩码(注意：必须保证传入的缓冲区大于等于16字节)
        //			  CamGateWay 相机网关(注意：必须保证传入的缓冲区大于等于16字节)
        //			  EtIp 网卡IP(注意：必须保证传入的缓冲区大于等于16字节)
        //			  EtMask 网卡子网掩码(注意：必须保证传入的缓冲区大于等于16字节)
        //			  EtGateWay 网卡网关(注意：必须保证传入的缓冲区大于等于16字节)
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraStatus.h
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraGigeGetIp(
            ref tSdkCameraDevInfo pCameraInfo,
            byte[] CamIp,
            byte[] CamMask,
            byte[] CamGateWay,
            byte[] EtIp,
            byte[] EtMask,
            byte[] EtGateWay
        );

        /******************************************************/
        // 函数名   : CameraGigeSetIp
        // 功能描述 : 设置GIGE相机的IP地址
        // 参数     : pCameraInfo 相机的设备描述信息，可由CameraEnumerateDevice函数获得。 
        //			  Ip 相机IP(如：192.168.1.100)
        //			  SubMask 相机子网掩码(如：255.255.255.0)
        //			  GateWay 相机网关(如：192.168.1.1)
        //			  bPersistent TRUE: 设置相机为固定IP，FALSE：设置相机自动分配IP（忽略参数Ip, SubMask, GateWay）
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraStatus.h
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraGigeSetIp(
            ref tSdkCameraDevInfo pCameraInfo,
            byte[] Ip,
            byte[] SubMask,
            byte[] GateWay,
            int bPersistent
        );

        /******************************************************/
        // 函数名   : CameraGigeGetMac
        // 功能描述 : 获取GIGE相机的MAC地址
        // 参数     : pCameraInfo 相机的设备描述信息，可由CameraEnumerateDevice函数获得。 
        //			  CamMac 相机MAC(注意：必须保证传入的缓冲区大于等于18字节)
        //			  EtMac 网卡MAC(注意：必须保证传入的缓冲区大于等于18字节)
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraStatus.h
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraGigeGetMac(
            ref tSdkCameraDevInfo pCameraInfo,
            byte[] CamMac,
            byte[] EtMac
        );

        /******************************************************/
        // 函数名   : CameraEnableFastResponse
        // 功能描述 : 使能快速响应
        // 参数     : hCamera  相机的句柄，由CameraInit函数获得。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraStatus.h
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraEnableFastResponse(
            CameraHandle hCamera
        );

        /******************************************************/
        // 函数名   : CameraSetCorrectDeadPixel
        // 功能描述 : 使能坏点修正
        // 参数     : hCamera  相机的句柄，由CameraInit函数获得。
        //				bEnable     TRUE: 使能坏点修正   FALSE: 关闭坏点修正
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraStatus.h
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraSetCorrectDeadPixel(
            CameraHandle hCamera,
            int bEnable
        );

        /******************************************************/
        // 函数名   : CameraGetCorrectDeadPixel
        // 功能描述 : 获取坏点修正使能状态
        // 参数     : hCamera  相机的句柄，由CameraInit函数获得。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraStatus.h
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraGetCorrectDeadPixel(
            CameraHandle hCamera,
            out int pbEnable
        );

        /******************************************************/
        // 函数名   : CameraFlatFieldingCorrectSetEnable
        // 功能描述 : 使能平场校正
        // 参数     : hCamera  相机的句柄，由CameraInit函数获得。
        //				bEnable     TRUE: 使能平场校正   FALSE: 关闭平场校正
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraStatus.h
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraFlatFieldingCorrectSetEnable(
            CameraHandle hCamera,
            int bEnable
        );

        /******************************************************/
        // 函数名   : CameraFlatFieldingCorrectGetEnable
        // 功能描述 : 获取平场校正使能状态
        // 参数     : hCamera  相机的句柄，由CameraInit函数获得。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraStatus.h
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraFlatFieldingCorrectGetEnable(
            CameraHandle hCamera,
            out int pbEnable
        );

        /******************************************************/
        // 函数名   : CameraFlatFieldingCorrectSetParameter
        // 功能描述 : 设置平场校正参数
        // 参数     :	hCamera  相机的句柄，由CameraInit函数获得。
        //				pDarkFieldingImage 暗场图片
        //				pDarkFieldingFrInfo 暗场图片信息
        //				pLightFieldingImage 明场图片
        //				pLightFieldingFrInfo 明场图片信息
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraStatus.h
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraFlatFieldingCorrectSetParameter(
            CameraHandle hCamera,
            IntPtr pDarkFieldingImage,
            ref tSdkFrameHead pDarkFieldingFrInfo,
            IntPtr pLightFieldingImage,
            ref tSdkFrameHead pLightFieldingFrInfo
        );

        /******************************************************/
        // 函数名   : CameraFlatFieldingCorrectSaveParameterToFile
        // 功能描述 : 保存平场校正参数到文件
        // 参数     :	hCamera  相机的句柄，由CameraInit函数获得。
        //				pszFileName 文件路径
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraStatus.h
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraFlatFieldingCorrectSaveParameterToFile(
            CameraHandle hCamera,
            byte[] pszFileName
        );

        /******************************************************/
        // 函数名   : CameraFlatFieldingCorrectLoadParameterFromFile
        // 功能描述 : 从文件中加载平场校正参数
        // 参数     :	hCamera  相机的句柄，由CameraInit函数获得。
        //				pszFileName 文件路径
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraStatus.h
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraFlatFieldingCorrectLoadParameterFromFile(
            CameraHandle hCamera,
            byte[] pszFileName
        );

        /******************************************************/
        // 函数名   : CameraCommonCall
        // 功能描述 : 相机的一些特殊功能调用，二次开发时一般不需要调用。
        // 参数     : hCamera   相机的句柄，由CameraInit函数获得。
        //            pszCall   功能及参数
        //            pszResult 调用结果，不同的pszCall时，意义不同。
        //            uResultBufSize pszResult指向的缓冲区的字节大小
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraStatus.h
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraCommonCall(
            CameraHandle hCamera,
            byte[] pszCall,
            byte[] pszResult,
            UInt32 uResultBufSize
        );

        /******************************************************/
        // 函数名   : CameraSetDenoise3DParams
        // 功能描述 : 设置3D降噪参数
        // 参数     : hCamera   相机的句柄，由CameraInit函数获得。
        //            bEnable  启用或禁用
        //            nCount   使用几张图片进行降噪(2-8张)
        //            Weights  降噪权重
        //					   如当使用3张图片进行降噪则这个参数可以传入3个浮点(0.3,0.3,0.4)，最后一张图片的权重大于前2张
        //					   如果不需要使用权重，则把这个参数传入0，表示所有图片的权重相同(0.33,0.33,0.33)
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraStatus.h
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraSetDenoise3DParams(
            CameraHandle hCamera,
            int bEnable,
            int nCount,
            float[] Weights
        );

        /******************************************************/
        // 函数名   : CameraGetDenoise3DParams
        // 功能描述 : 获取当前的3D降噪参数
        // 参数     : hCamera   相机的句柄，由CameraInit函数获得。
        //            bEnable  启用或禁用
        //            nCount   使用了几张图片进行降噪
        //			  bUseWeight 是否使用了降噪权重
        //            Weights  降噪权重
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraStatus.h
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraGetDenoise3DParams(
            CameraHandle hCamera,
            out int bEnable,
            out int nCount,
            out int bUseWeight,
            float[] Weights
        );

        /******************************************************/
        // 函数名   : CameraManualDenoise3D
        // 功能描述 : 对一组帧进行一次降噪处理
        // 参数     : InFramesHead  输入帧头
        //			  InFramesData  输入帧数据
        //            nCount   输入帧的数量
        //            Weights  降噪权重
        //					   如当使用3张图片进行降噪则这个参数可以传入3个浮点(0.3,0.3,0.4)，最后一张图片的权重大于前2张
        //					   如果不需要使用权重，则把这个参数传入0，表示所有图片的权重相同(0.33,0.33,0.33)
        //			  OutFrameHead 输出帧头
        //			  OutFrameData 输出帧数据
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraStatus.h
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraManualDenoise3D(
            tSdkFrameHead[] InFramesHead,
            IntPtr[] InFramesData,
            int nCount,
            float[] Weights,
            ref tSdkFrameHead OutFrameHead,
            IntPtr OutFrameData
        );

        /******************************************************/
        // 函数名   : CameraCustomizeDeadPixels
        // 功能描述 : 打开坏点编辑面板
        // 参数     : hCamera    相机的句柄，由CameraInit函数获得。
        //            hParent    调用该函数的窗口的句柄。可以为NULL。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraStatus.h
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraCustomizeDeadPixels(
            CameraHandle hCamera,
            IntPtr hParent
            );

        /******************************************************/
        // 函数名   : CameraReadDeadPixels
        // 功能描述 : 读取相机坏点
        // 参数     : hCamera   相机的句柄，由CameraInit函数获得。
        //			  pRows 坏点y坐标
        //			  pCols 坏点x坐标
        //			  pNumPixel 输入时表示行列缓冲区的大小，返回时表示行列缓冲区中返回的坏点数量。
        //			  当pRows或者pCols为NULL时函数会把相机当前的坏点个数通过pNumPixel返回
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraStatus.h
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraReadDeadPixels(
            CameraHandle hCamera,
            ushort[] pRows,
            ushort[] pCols,
            ref uint pNumPixel
            );

        /******************************************************/
        // 函数名   : CameraAddDeadPixels
        // 功能描述 : 添加相机坏点
        // 参数     : hCamera   相机的句柄，由CameraInit函数获得。
        //			  pRows 坏点y坐标
        //			  pCols 坏点x坐标
        //			  NumPixel 行列缓冲区中的坏点个数
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraStatus.h
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraAddDeadPixels(
            CameraHandle hCamera,
            ushort[] pRows,
            ushort[] pCols,
            uint NumPixel
            );

        /******************************************************/
        // 函数名   : CameraRemoveDeadPixels
        // 功能描述 : 删除相机指定坏点
        // 参数     : hCamera   相机的句柄，由CameraInit函数获得。
        //			  pRows 坏点y坐标
        //			  pCols 坏点x坐标
        //			  NumPixel 行列缓冲区中的坏点个数
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraStatus.h
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraRemoveDeadPixels(
            CameraHandle hCamera,
            ushort[] pRows,
            ushort[] pCols,
            uint NumPixel
            );

        /******************************************************/
        // 函数名   : CameraRemoveAllDeadPixels
        // 功能描述 : 删除相机的所有坏点
        // 参数     : hCamera   相机的句柄，由CameraInit函数获得。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraStatus.h
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraRemoveAllDeadPixels(
            CameraHandle hCamera
            );

        /******************************************************/
        // 函数名   : CameraSaveDeadPixels
        // 功能描述 : 保存相机坏点到相机存储中
        // 参数     : hCamera   相机的句柄，由CameraInit函数获得。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraStatus.h
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraSaveDeadPixels(
            CameraHandle hCamera
            );

        /******************************************************/
        // 函数名   : CameraSaveDeadPixelsToFile
        // 功能描述 : 保存相机坏点到文件中
        // 参数     : hCamera   相机的句柄，由CameraInit函数获得。
        //			  sFileName  坏点文件的完整路径。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraStatus.h
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraSaveDeadPixelsToFile(
            CameraHandle hCamera,
            byte[] sFileName
            );

        /******************************************************/
        // 函数名   : CameraLoadDeadPixelsFromFile
        // 功能描述 : 从文件加载相机坏点
        // 参数     : hCamera   相机的句柄，由CameraInit函数获得。
        //			  sFileName  坏点文件的完整路径。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraStatus.h
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraLoadDeadPixelsFromFile(
            CameraHandle hCamera,
            byte[] sFileName
            );

        /******************************************************/
        // 函数名   : CameraGetImageBufferPriority
        // 功能描述 : 获得一帧图像数据。为了提高效率，SDK在图像抓取时采用了零拷贝机制，
        //        CameraGetImageBuffer实际获得是内核中的一个缓冲区地址，
        //        该函数成功调用后，必须调用CameraReleaseImageBuffer释放由
        //        CameraGetImageBuffer得到的缓冲区,以便让内核继续使用
        //        该缓冲区。  
        // 参数     : hCamera   相机的句柄，由CameraInit函数获得。
        //            pFrameInfo  图像的帧头信息指针。
        //            pbyBuffer   指向图像的数据的缓冲区指针。由于
        //              采用了零拷贝机制来提高效率，因此
        //              这里使用了一个指向指针的指针。
        //            wTimes 抓取图像的超时时间。单位毫秒。在
        //              wTimes时间内还未获得图像，则该函数
        //              会返回超时信息。
        //			  Priority 取图优先级 详见：emCameraGetImagePriority
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraStatus.h
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraGetImageBufferPriority(
            CameraHandle hCamera,
            out tSdkFrameHead pFrameInfo,
            out IntPtr pbyBuffer,
            uint wTimes,
            uint Priority
            );

        /******************************************************/
        // 函数名 	: CameraGetImageBufferPriorityEx
        // 功能描述	: 获得一帧图像数据。该接口获得的图像是经过处理后的RGB格式。该函数调用后，
        //			  不需要调用 CameraReleaseImageBuffer 释放，也不要调用free之类的函数释放
        //              来释放该函数返回的图像数据缓冲区。
        // 参数	    : hCamera	  相机的句柄，由CameraInit函数获得。
        //            piWidth    整形指针，返回图像的宽度
        //            piHeight   整形指针，返回图像的高度
        //            UINT wTimes 抓取图像的超时时间。单位毫秒。在
        //						  wTimes时间内还未获得图像，则该函数
        //						  会返回超时信息。
        //			  Priority   取图优先级 详见：emCameraGetImagePriority
        // 返回值   : 成功时，返回RGB数据缓冲区的首地址;
        //            否则返回0。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern IntPtr CameraGetImageBufferPriorityEx(
            CameraHandle hCamera,
            out int piWidth,
            out int piHeight,
            uint wTimes,
            uint Priority
            );

        /******************************************************/
        // 函数名 	: CameraGetImageBufferPriorityEx2
        // 功能描述	: 获得一帧图像数据。该接口获得的图像是经过处理后的RGB格式。该函数调用后，
        //			  不需要调用 CameraReleaseImageBuffer 释放，也不要调用free之类的函数释放
        //              来释放该函数返回的图像数据缓冲区。
        // 参数	    : hCamera	    相机的句柄，由CameraInit函数获得。
        //             pImageData  接收图像数据的缓冲区，大小必须和uOutFormat指定的格式相匹配，否则数据会溢出
        //             piWidth     整形指针，返回图像的宽度
        //             piHeight    整形指针，返回图像的高度
        //             wTimes      抓取图像的超时时间。单位毫秒。在
        //						wTimes时间内还未获得图像，则该函数
        //						会返回超时信息。
        //			  Priority	   取图优先级 详见：emCameraGetImagePriority
        // 返回值   : 成功时，返回RGB数据缓冲区的首地址;
        //            否则返回0。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraGetImageBufferPriorityEx2(
            CameraHandle hCamera,
            IntPtr pImageData,
            uint uOutFormat,
            out int piWidth,
            out int piHeight,
            uint wTimes,
            uint Priority
            );

        /******************************************************/
        // 函数名 	: CameraGetImageBufferPriorityEx3
        // 功能描述	: 获得一帧图像数据。该接口获得的图像是经过处理后的RGB格式。该函数调用后，
        //			  不需要调用 CameraReleaseImageBuffer 释放.
        //              uOutFormat 0 : 8 BIT gray 1:rgb24 2:rgba32 3:bgr24 4:bgra32
        // 参数	    : hCamera	    相机的句柄，由CameraInit函数获得。
        //             pImageData  接收图像数据的缓冲区，大小必须和uOutFormat指定的格式相匹配，否则数据会溢出
        //            piWidth      整形指针，返回图像的宽度
        //            piHeight     整形指针，返回图像的高度
        //            puTimeStamp  无符号整形，返回图像时间戳 
        //            UINT wTimes  抓取图像的超时时间。单位毫秒。在
        //			  wTimes       时间内还未获得图像，则该函数会返回超时信息。
        //			  Priority	   取图优先级 详见：emCameraGetImagePriority
        // 返回值   : 成功时，返回RGB数据缓冲区的首地址;
        //            否则返回0。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraGetImageBufferPriorityEx3(
            CameraHandle hCamera,
            IntPtr pImageData,
            uint uOutFormat,
            out int piWidth,
            out int piHeight,
            out uint puTimeStamp,
            uint wTimes,
            uint Priority
            );

        /******************************************************/
        // 函数名   : CameraClearBuffer
        // 功能描述 : 清空相机内已缓存的所有帧
        // 参数     : hCamera  相机的句柄，由CameraInit函数获得。
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraStatus.h
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraClearBuffer(
            CameraHandle hCamera
            );

        /******************************************************/
        // 函数名   : CameraSoftTriggerEx
        // 功能描述 : 执行一次软触发。执行后，会触发由CameraSetTriggerCount
        //          指定的帧数。
        // 参数     : hCamera  相机的句柄，由CameraInit函数获得。
        //			  uFlags 功能标志,详见emCameraSoftTriggerExFlags中的定义
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraStatus.h
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraSoftTriggerEx(
            CameraHandle hCamera,
            uint uFlags
            );

        /******************************************************/
        // 函数名   : CameraGrabber_CreateFromDevicePage
        // 功能描述 : 弹出相机列表让用户选择要打开的相机
        // 参数     : 如果函数执行成功返回函数创建的Grabber
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraStatus.h
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraGrabber_CreateFromDevicePage(
            out IntPtr Grabber
        );

        /******************************************************/
        // 函数名   : CameraGrabber_Create
        // 功能描述 : 从设备描述信息创建Grabber
        // 参数     : Grabber    如果函数执行成功返回函数创建的Grabber对象
        //			  pDevInfo	该相机的设备描述信息，由CameraEnumerateDevice函数获得。 
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraStatus.h
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraGrabber_Create(
            out IntPtr Grabber,
            ref tSdkCameraDevInfo pDevInfo
        );

        /******************************************************/
        // 函数名   : CameraGrabber_Destroy
        // 功能描述 : 销毁Grabber
        // 参数     : Grabber
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraStatus.h
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraGrabber_Destroy(
            IntPtr Grabber
        );

        /******************************************************/
        // 函数名	: CameraGrabber_SetHWnd
        // 功能描述	: 设置预览视频的显示窗口
        // 参数		: Grabber
        //			  hWnd  窗口句柄
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraStatus.h
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraGrabber_SetHWnd(
            IntPtr Grabber,
            IntPtr hWnd
        );

        /******************************************************/
        // 函数名	: CameraGrabber_StartLive
        // 功能描述	: 启动预览
        // 参数		: Grabber
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraStatus.h
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraGrabber_StartLive(
            IntPtr Grabber
        );

        /******************************************************/
        // 函数名	: CameraGrabber_StopLive
        // 功能描述	: 停止预览
        // 参数		: Grabber
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraStatus.h
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraGrabber_StopLive(
            IntPtr Grabber
        );

        /******************************************************/
        // 函数名	: CameraGrabber_SaveImage
        // 功能描述	: 抓图
        // 参数		: Grabber
        //			  Image 返回抓取到的图像（需要调用CameraImage_Destroy释放）
        //			  TimeOut 超时时间（毫秒）
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraStatus.h
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraGrabber_SaveImage(
            IntPtr Grabber,
            out IntPtr Image,
            uint TimeOut
        );

        /******************************************************/
        // 函数名	: CameraGrabber_SaveImageAsync
        // 功能描述	: 提交一个异步的抓图请求，提交成功后待抓图完成会回调用户设置的完成函数
        // 参数		: Grabber
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraStatus.h
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraGrabber_SaveImageAsync(
            IntPtr Grabber
        );

        /******************************************************/
        // 函数名	: CameraGrabber_SaveImageAsyncEx
        // 功能描述	: 提交一个异步的抓图请求，提交成功后待抓图完成会回调用户设置的完成函数
        // 参数		: Grabber
        //			  UserData 可使用CameraImage_GetUserData从Image获取此值
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraStatus.h
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraGrabber_SaveImageAsyncEx(
            IntPtr Grabber,
            IntPtr UserData
        );

        /******************************************************/
        // 函数名	: CameraGrabber_SetSaveImageCompleteCallback
        // 功能描述	: 设置异步方式抓图的完成函数
        // 参数		: Grabber
        //			  Callback 当有抓图任务完成时被调用
        //			  Context 当Callback被调用时，作为参数传入Callback
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraStatus.h
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraGrabber_SetSaveImageCompleteCallback(
            IntPtr Grabber,
            pfnCameraGrabberSaveImageComplete Callback,
            IntPtr Context
        );

        /******************************************************/
        // 函数名	: CameraGrabber_SetFrameListener
        // 功能描述	: 设置帧监听函数
        // 参数		: Grabber
        //			  Listener 监听函数，此函数返回0表示丢弃当前帧
        //			  Context 当Listener被调用时，作为参数传入Listener
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraStatus.h
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraGrabber_SetFrameListener(
            IntPtr Grabber,
            pfnCameraGrabberFrameListener Listener,
            IntPtr Context
        );

        /******************************************************/
        // 函数名	: CameraGrabber_GetCameraHandle
        // 功能描述	: 获取相机句柄
        // 参数		: Grabber
        //			  hCamera 返回的相机句柄
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraStatus.h
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraGrabber_GetCameraHandle(
            IntPtr Grabber,
            out CameraHandle hCamera
        );

        /******************************************************/
        // 函数名	: CameraGrabber_GetStat
        // 功能描述	: 获取帧统计信息
        // 参数		: Grabber
        //			  stat 返回的统计信息
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraStatus.h
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraGrabber_GetStat(
            IntPtr Grabber,
            out tSdkGrabberStat stat
        );

        /******************************************************/
        // 函数名	: CameraGrabber_GetCameraDevInfo
        // 功能描述	: 获取相机DevInfo
        // 参数		: Grabber
        //			  DevInfo 返回的相机DevInfo
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraStatus.h
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraGrabber_GetCameraDevInfo(
            IntPtr Grabber,
            out tSdkCameraDevInfo DevInfo
        );

        /******************************************************/
        // 函数名	: CameraImage_Create
        // 功能描述	: 创建一个新的Image
        // 参数		: Image
        //			  pFrameBuffer 帧数据缓冲区
        //			  pFrameHead 帧头
        //			  bCopy TRUE: 复制出一份新的帧数据  FALSE: 不复制，直接使用pFrameBuffer指向的缓冲区
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraStatus.h
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraImage_Create(
            out IntPtr Image,
            IntPtr pFrameBuffer,
            ref tSdkFrameHead pFrameHead,
            int bCopy
        );

        /******************************************************/
        // 函数名	: CameraImage_CreateEmpty
        // 功能描述	: 创建一个空的Image
        // 参数		: Image
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraStatus.h
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraImage_CreateEmpty(
            out IntPtr Image
        );

        /******************************************************/
        // 函数名	: CameraImage_Destroy
        // 功能描述	: 销毁Image
        // 参数		: Image
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraStatus.h
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraImage_Destroy(
            IntPtr Image
        );

        /******************************************************/
        // 函数名	: CameraImage_GetData
        // 功能描述	: 获取Image数据
        // 参数		: Image
        //			  DataBuffer 图像数据
        //			  Head 图像信息
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraStatus.h
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraImage_GetData(
            IntPtr Image,
            out IntPtr DataBuffer,
            out IntPtr Head
        );

        /******************************************************/
        // 函数名	: CameraImage_GetUserData
        // 功能描述	: 获取Image的用户自定义数据
        // 参数		: Image
        //			  UserData 返回用户自定义数据
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraStatus.h
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraImage_GetUserData(
            IntPtr Image,
            out IntPtr UserData
        );

        /******************************************************/
        // 函数名	: CameraImage_SetUserData
        // 功能描述	: 设置Image的用户自定义数据
        // 参数		: Image
        //			  UserData 用户自定义数据
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraStatus.h
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraImage_SetUserData(
            IntPtr Image,
            IntPtr UserData
        );

        /******************************************************/
        // 函数名	: CameraImage_IsEmpty
        // 功能描述	: 判断一个Image是否为空
        // 参数		: Image
        //			  IsEmpty 为空返回:TRUE(1)  否则返回:FALSE(0)
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraStatus.h
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraImage_IsEmpty(
            IntPtr Image,
            out int IsEmpty
        );

        /******************************************************/
        // 函数名	: CameraImage_Draw
        // 功能描述	: 绘制Image到指定窗口
        // 参数		: Image
        //			  hWnd 目的窗口
        //			  Algorithm 缩放算法  0：快速但质量稍差  1：速度慢但质量好
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraStatus.h
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraImage_Draw(
            IntPtr Image,
            IntPtr hWnd,
            int Algorithm
        );

        /******************************************************/
        // 函数名	: CameraImage_BitBlt
        // 功能描述	: 绘制Image到指定窗口（不缩放）
        // 参数		: Image
        //			  hWnd 目的窗口
        //			  xDst,yDst: 目标矩形的左上角坐标
        //			  cxDst,cyDst: 目标矩形的宽高
        //			  xSrc,ySrc: 图像矩形的左上角坐标
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraStatus.h
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraImage_BitBlt(
            IntPtr Image,
            IntPtr hWnd,
            int xDst,
            int yDst,
            int cxDst,
            int cyDst,
            int xSrc,
            int ySrc
        );

        /******************************************************/
        // 函数名	: CameraImage_SaveAsBmp
        // 功能描述	: 以bmp格式保存Image
        // 参数		: Image
        //			  FileName 文件名
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraStatus.h
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraImage_SaveAsBmp(
            IntPtr Image,
            byte[] FileName
        );

        /******************************************************/
        // 函数名	: CameraImage_SaveAsJpeg
        // 功能描述	: 以jpg格式保存Image
        // 参数		: Image
        //			  FileName 文件名
        //			  Quality 保存质量(1-100)，100为质量最佳但文件也最大
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraStatus.h
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraImage_SaveAsJpeg(
            IntPtr Image,
            byte[] FileName,
            byte Quality
        );

        /******************************************************/
        // 函数名	: CameraImage_SaveAsPng
        // 功能描述	: 以png格式保存Image
        // 参数		: Image
        //			  FileName 文件名
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraStatus.h
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraImage_SaveAsPng(
            IntPtr Image,
            byte[] FileName
        );

        /******************************************************/
        // 函数名	: CameraImage_SaveAsRaw
        // 功能描述	: 保存raw Image
        // 参数		: Image
        //			  FileName 文件名
        //			  Format 0: 8Bit Raw     1: 16Bit Raw
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraStatus.h
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraImage_SaveAsRaw(
            IntPtr Image,
            byte[] FileName,
            int Format
        );

        /******************************************************/
        // 函数名	: CameraImage_IPicture
        // 功能描述	: 从Image创建一个IPicture
        // 参数		: Image
        //			  Picture 新创建的IPicture
        // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
        //            否则返回非0值的错误码,请参考CameraStatus.h
        //            中错误码的定义。
        /******************************************************/
        [DllImport("MVCAMSDK.dll")]
        public static extern CameraSdkStatus CameraImage_IPicture(
            IntPtr Image,
            out Object NewPic
        );

    }
}
