using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using FM.Lib.Controls;
using FM.Lib.Model;


namespace FMShell
{
    public partial class VideoForm : Form
    {
        public MainForm MainForm;
        private int _count = 0;
        public  VideoFrameDto FireControlRoomVideoFrameDto;//视频信息
        public CHCNetSDK.NET_DVR_DEVICEINFO_V30 DeviceInfo;
        private Int32 m_lUserID = -1;
        private bool m_bRecord = false;
        private Int32 m_lRealHandle = -1;
        private string sVideoFileName = string.Empty;//录像文件名
        private string JSonstring = "";

        public VideoForm(MainForm form)
        {
            InitializeComponent();
            MainForm = form;
        }
        public VideoForm(MainForm form, string json)
        {
            InitializeComponent();
            MainForm = form;
            //FireControlRoomVideoFrameDto = firecontrolroomVideoFrameDto;
            JSonstring = json;
            timer1.Start();
        }
        public void DisplayStatusMsg(string msg)
        {
            if (this.label2.InvokeRequired)
            {
                this.label2.BeginInvoke(new Func<bool>(() =>
                {
                    label2.Text = msg;
                    //label2.Text = "语音服务出错";
                    return false;
                }));
            }
            else
            {
                label2.Text = msg;
                //label2.Text = "语音服务出错";
            }
        }

        private void VideoForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.MainForm.BeginInvoke(new Func<bool>(() =>
            {
                MainForm.EndAudio();
                stopvideo();
                return false;
            }));
        }

        public void StartAudio()
        {
            timer1.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.MainForm.EndVoiceCommand(JSonstring);
            this.Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            _count++;
            //this.label3.Text = string.Format("连接时间:{0} 秒", _count);
            if (_count == 1)
            {
                FireControlRoomVideoFrameDto = Util.Json.ToObject<VideoFrameDto>(JSonstring);
                this.Text = string.Format("正在与{0}进行视频语音对讲", FireControlRoomVideoFrameDto.StationName + FireControlRoomVideoFrameDto.FireControlRoomName);

                if (FireControlRoomVideoFrameDto == null)
                {
                   // this.Text += "\t获取视频参数出错";
                    timer1.Stop();

                }
                this.MainForm.MainformGetUsedtoByLoginID(FireControlRoomVideoFrameDto.InChargeUser.ToString());
                //GetUsedtoByLoginID("A4BFA4C4-D97E-4372-82E2-69854768939B");
                this.MainForm.StartVoiceCommand(JSonstring);//
                this.MainForm.StartAudio(DisplayStatusMsg);
            }
            if (_count%3 == 0)
            {
                if (!string.IsNullOrEmpty(this.MainForm.GettedUserIP) && FireControlRoomVideoFrameDto != null)
                {
                    string[] ip_port =this.MainForm. GettedUserIP.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                    if (FireControlRoomVideoFrameDto.VideoIP != ip_port[0])
                    {
                        FireControlRoomVideoFrameDto.VideoIP = ip_port[0];
                        new Thread(new ThreadStart(() =>
                        {
                            Thread.CurrentThread.IsBackground = true;
                            Initvedio();
                        })).Start();
                        timer1.Stop();
                    }
                }
            }
        }

        private void VideoForm_Load(object sender, EventArgs e)
        {

        }


        private void Initvedio()
        {
            //this.Invoke((MethodInvoker)delegate
            //{
                if (FireControlRoomVideoFrameDto == null)
                {
                    MessageBox.Show("获取视频连接信息失败，无法使用视频功能");
                    //this.Invoke((MethodInvoker) delegate
                    //{
                    //    this.Text += "\t视频连接失败";
                    //});
                }
                //停止预览 Stop live view 
                if (m_lRealHandle >= 0)
                {
                    CHCNetSDK.NET_DVR_StopRealPlay(m_lRealHandle);
                    //panel1.Invalidate();//刷新窗口 refresh the window
                    m_lRealHandle = -1;

                    //注销登录 Logout the device
                    if (m_lUserID >= 0)
                    {
                        CHCNetSDK.NET_DVR_Logout(m_lUserID);
                        m_lUserID = -1;
                    }
                    CHCNetSDK.NET_DVR_Cleanup();
                    //btnOpenVideo.Text = "打开视频";
                }
                else
                {
                    var m_bInitSDK = CHCNetSDK.NET_DVR_Init();
                    if (m_bInitSDK == false)
                    {
                        MessageBox.Show("视频初始化失败");
                        //this.Invoke((MethodInvoker) delegate
                        //{
                        //    this.Text += "\t视频初始化失败";
                        //});
                        return;
                    }
                    //修改为获取摄像头IP   PORT UNAME UPASSEORD 信息  2015.06.12  LEO
                    //var cameraInfo = AppPara.getCameraInfoBySiteId(site.SID);

                    string DVRIPAddress = string.IsNullOrEmpty(FireControlRoomVideoFrameDto.VideoIP) ? "" : FireControlRoomVideoFrameDto.VideoIP; ;////DevicePoint.SiteIp; //设备IP地址或者域名 Device IP
                    Int32 DVRPortNumber = Convert.ToInt32(FireControlRoomVideoFrameDto.VideoPort); //Convert.ToInt16(DevicePoint.SPPORT); //设备服务端口号 Device Port
                    string DVRUserName = FireControlRoomVideoFrameDto.VideoUserName; //DevicePoint.SPNAME; //设备登录用户名 User name to login
                    string DVRPassword = FireControlRoomVideoFrameDto.VideoPassword;// DevicePoint.SPPASSWORD; //设备登录密码 Password to login

                    //string DVRIPAddress = "192.168.1.64"; ;////DevicePoint.SiteIp; //设备IP地址或者域名 Device IP
                    //Int16 DVRPortNumber = 8000; //Convert.ToInt16(DevicePoint.SPPORT); //设备服务端口号 Device Port
                    //string DVRUserName = "admin"; //DevicePoint.SPNAME; //设备登录用户名 User name to login
                    //string DVRPassword = "admin123";// DevicePoint.SPPASSWORD; //设备登录密码 Password to login


                    //登录设备 Login the device
                    m_lUserID = CHCNetSDK.NET_DVR_Login_V30(DVRIPAddress, DVRPortNumber, DVRUserName, DVRPassword,
                        ref DeviceInfo);
                    if (m_lUserID < 0)
                    {
                        var iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                        var str = "NET_DVR_Login_V30 failed, error code= " + iLastErr;
                        //this.Invoke((MethodInvoker) delegate
                        //{
                        //    this.Text += "\t登录视频失败";
                        //});
                        //登录失败，输出错误号 Failed to login and output the error code
                        //DebugInfo(str);
                        //MessageBox.Show(str);
                        //labelControl1.Text = "登录视频失败";
                        return;
                    }
                    else
                    {
                        //登录成功
                        //DebugInfo("NET_DVR_Login_V30 succ!");
                        //btnLogin.Text = "Logout";

                        var dwAChanTotalNum = (int)DeviceInfo.byChanNum;
                        //this.Invoke((MethodInvoker) delegate
                        //{
                        //    this.Text += "\t登录视频成功";
                        //});
                        //if (!IsAudio)
                        //    btnRecord.Visible = true;

                    }


                    if (m_lRealHandle < 0)
                    {
                        CHCNetSDK.NET_DVR_PREVIEWINFO lpPreviewInfo = new CHCNetSDK.NET_DVR_PREVIEWINFO();
                        this.Invoke((MethodInvoker)delegate
                        {
                            lpPreviewInfo.hPlayWnd = panel1.Handle;//预览窗口 live view window
                        });
                        lpPreviewInfo.lChannel = Convert.ToInt32(FireControlRoomVideoFrameDto.VideoAisle); //iChannelNum[(int)iSelIndex];//预览的设备通道 the device channel number
                        //lpPreviewInfo.lChannel = 1; //iChannelNum[(int)iSelIndex];//预览的设备通道 the device channel number
                        lpPreviewInfo.dwStreamType = 0;//码流类型：0-主码流，1-子码流，2-码流3，3-码流4，以此类推
                        lpPreviewInfo.dwLinkMode = 0;//连接方式：0- TCP方式，1- UDP方式，2- 多播方式，3- RTP方式，4-RTP/RTSP，5-RSTP/HTTP 
                        lpPreviewInfo.bBlocked = true; //0- 非阻塞取流，1- 阻塞取流

                        IntPtr pUser = IntPtr.Zero;//用户数据 user data 

                        //打开预览 Start live view 
                        m_lRealHandle = CHCNetSDK.NET_DVR_RealPlay_V40(m_lUserID, ref lpPreviewInfo, null/*RealData*/, pUser);


                        if (m_lRealHandle < 0)
                        {
                            var iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                            var str = "NET_DVR_RealPlay_V40 failed, error code= " + iLastErr; //预览失败，输出错误号 failed to start live view, and output the error code.
                            //DebugInfo(str);
                            this.Invoke((MethodInvoker) delegate
                            {
                                //this.Text += "\t预览视频失败";
                            });
                            return;
                        }
                        //btnOpenVideo.Text = "关闭视频";


                    }

                }
            //});
        }

        private void stopvideo()
        {
            //停止预览 Stop live view 
            if (m_lRealHandle >= 0)
            {
                CHCNetSDK.NET_DVR_StopRealPlay(m_lRealHandle);
                m_lRealHandle = -1;
            }

            //注销登录 Logout the device
            if (m_lUserID >= 0)
            {
                CHCNetSDK.NET_DVR_Logout(m_lUserID);
                m_lUserID = -1;
            }

            CHCNetSDK.NET_DVR_Cleanup();
        }
    }
}
