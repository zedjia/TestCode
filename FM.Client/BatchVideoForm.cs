using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraPrinting.Native;
using Color = System.Windows.Media.Color;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;
using Z.Lib.Model;

namespace FMShell
{
    public partial class BatchVideoForm : Form
    {
        public MainForm MainForm;
        /// <summary>
        /// 视频信息DTO列表
        /// </summary>
        private List<EquipmentTbDto> Videolist;
        /// <summary>
        /// 列数
        /// </summary>
        private int colunm = 3;
        /// <summary>
        /// 行数
        /// </summary>
        private int row = 3;
        /// <summary>
        /// 轮询的组数
        /// </summary>
        private int circlenum { get; set; }
        /// <summary>
        /// 当前播放的组数
        /// </summary>
        private int nowcirclenum = 0;
        /// <summary>
        /// 一个屏幕最多显示的视频个数
        /// </summary>
        private int all_video = 9;
        /// <summary>
        /// 所有视频播放控件
        /// </summary>
        private List<PictureEdit> pictureEdits;
        /// <summary>
        /// 存储每个视频对应的对象
        /// </summary>
        private Hashtable paramHashtable = new Hashtable();
        //获取的IP地址
        private string ipaddress = "";
        private int _count = 0;
        ImageList imagelist;
        public BatchVideoForm()
        {
            InitializeComponent();
        }
        public BatchVideoForm(MainForm mainForm, string jsondata)
        {
            InitializeComponent();
            //var dto = Util.Json.ToObject<VideoFrameDto>(jsondata);//视频参数数据
            //Videolist = new List<VideoFrameDto>() { dto, dto, dto, dto, dto, dto, dto, dto, dto };
            //jsondata = "[" + jsondata + "]";
            Videolist = Util.Json.ToObject<List<EquipmentTbDto>>(jsondata);
            MainForm = mainForm;
        }

        private void BatchVideoForm_Load(object sender, EventArgs e)
        {
            //this.Location=new Point(280,130);
            if (Videolist == null || Videolist.Count == 0)
            {
                XtraMessageBox.Show("获取视频列表出错无法加载视频");
                //this.Close();
                return;
            }
            foreach (var equipmentTbDto in Videolist)
            {
                //equipmentTbDto.Image = global::FMShell.Properties.Resources.shexiangji;
            }
            Font font = new System.Drawing.Font("", 30, FontStyle.Regular);
            this.treeList1.Font = font;
            TreeList();
            nowcirclenum = 0;
            setrowcloum(Videolist);
            CreatVedioPanel();
            getcirclenum();
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            _count++;

            if (_count == 1)
            {
                if (Videolist == null || Videolist.Count == 0)
                {
                    timer1.Stop();
                    return;
                }
                this.MainForm.MainformGetVideoIPByStationID(Videolist[0].StationID.ToString());
            }
            if (_count % 3 == 0)
            {
                //if (!string.IsNullOrEmpty(this.MainForm.GettedUserIP) && Videolist != null)
                //{
                //    if (string.IsNullOrEmpty(ipaddress))
                //    {
                //        string[] ip_port = this.MainForm.GettedUserIP.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                //        ipaddress = ip_port[0];
                //        if (Videolist.Any())
                //        {
                //            var node = treeList1.Nodes.FirstNode;
                //            string deptname = null;
                //            if (Convert.ToInt16(node["Opreate"]) != 1)
                //            {
                //                deptname = node["DisPlayName"].ToString();
                //            }
                            
                //            nowshow(Videolist[0].ESubTypeName, deptname, false);
                //        }
                //        //nowshow_camare();
                //        timer1.Stop();
                //    }
                //}
            }
            if (_count == 180)
            {
                foreach (var value in paramHashtable.Values)
                {
                    var paramvideo = value as Param;
                    Label infoLabel = new Label();
                    paramvideo.InfoLabel = infoLabel;
                    paramvideo.videoCtrl.Controls.Add(infoLabel);
                    setinfolabel(paramvideo, "连接超时...");
                }
            }
        }

        /// <summary>
        /// 获取设置当前的视频列表的最大分组数
        /// </summary>
        private void getcirclenum()
        {
            this.TopMost = true;
            this.BringToFront();
            if (Videolist.Count % 9 == 0)
            {
                circlenum = Convert.ToInt16(Videolist.Count / 9.0);
            }
            else
            {
                circlenum = Convert.ToInt16(Videolist.Count / 9.0 + 0.5);
            }
        }

        private void setrowcloum(IList<EquipmentTbDto> list)
        {

            int count = list.Count;
            if (count <= all_video - 3)
            {
                if (count % 2 == 0)
                {
                    row = 2;
                    colunm = count / 2;
                }
                else
                {
                    row = 2;
                    colunm = (count + 1) / 2;
                }
                if (count <= 2)
                {

                    row = colunm = 1;
                }
            }
            else
            {
                row = colunm = 3;
            }
        }
        //private void getallpictureeditors()
        //{
        //    if (pictureEdits == null)
        //    {
        //        pictureEdits = new List<PictureEdit>();
        //    }
        //    for (int i = panelControl1.Controls.Count-1; i >=0; i--)
        //    {
        //        pictureEdits.Add(panelControl1.Controls[i] as PictureEdit);
        //    }
        //    //foreach (var control in panelControl1.Controls)
        //    //{
        //    //    pictureEdits.Add(control as PictureEdit);
        //    //}
        //}
        private void nowshow_camare()
        {
            nowcirclenum++;
            //Console.WriteLine("当前是第" + nowcirclenum + "组摄像头");
            int nowshowmaxnum = nowcirclenum * 9;
            List<EquipmentTbDto> c = null;
            if (nowshowmaxnum > Videolist.Count)
            {
                nowshowmaxnum = Videolist.Count - (nowcirclenum - 1) * all_video;//当最后显示的视频不足3个的时候

                c = Videolist.GetRange((nowcirclenum - 1) * all_video, nowshowmaxnum);
            }
            else
            {
                c = Videolist.GetRange((nowcirclenum - 1) * all_video, all_video);
            }
            Initvideo(c);
            //AppPara.flytocameraview(c[0]);
            //this.videolistgride.FocusedRowHandle = (nowcirclenum - 1) * 9;
        }

        private void CreatVedioPanel()
        {
            for (int i = 0; i < row * colunm; i++)
            {
                Param param = new Param();
                Panel panel = new Panel();
                param.videoCtrl = setpanelsize_position(panel, i);
                param.index = i;
                if (i > Videolist.Count - 1)
                {
                    var msg = "无视频源...";
                    var infoLabel = new Label();
                    param.InfoLabel = infoLabel;
                    param.videoCtrl.Controls.Add(infoLabel);
                    setinfolabel(param, msg);
                }
                paramHashtable.Add(param.videoCtrl.Name, param);

            }
        }
        private void Initvideo(List<EquipmentTbDto> videosDtos)
        {
            for (int i = 0; i < row * colunm; i++)
            {
                if (i >= videosDtos.Count) continue;
                var t = new Thread(fillcamera) { IsBackground = true };
                var param = paramHashtable[i.ToString()] as Param;
                //Panel panel=new Panel();
                //param.videoCtrl = setpanelsize_position(panel, i);
                param.index = i;
                param.thread = t;
                if (videosDtos[i] != null)
                {
                    param.VideoFrameDto = videosDtos[i];
                }
                //paramHashtable.Add(param.videoCtrl.Name, param);

                t.Start(param);
                //Thread.Sleep(200);
            }
        }
        /// <summary>
        /// 动态生成panel给定位置绑定事件
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        private Panel setpanelsize_position(Panel panel, int i)
        {

            panel.BackColor = System.Drawing.Color.Black;
            panel.BorderStyle = BorderStyle.Fixed3D;
            panelcontrol.Controls.Add(panel);
            panel.Width = panelcontrol.Width / colunm;
            panel.Name = i.ToString();
            panel.Height = panelcontrol.Height / row;
            int thisrow = Convert.ToInt16(i / colunm);
            int thiscolunm = System.Math.Abs(thisrow * colunm - i);
            panel.Location = new Point(thiscolunm * panel.Width, thisrow * panel.Height);
            panel.Anchor = AnchorStyles.Top & AnchorStyles.Left;
            panel.MouseDoubleClick -= new MouseEventHandler(this.doubclick);
            panel.MouseDoubleClick += new MouseEventHandler(this.doubclick);
            panel.AllowDrop = true;
            panel.DragDrop -= new DragEventHandler(this.panel_DragDrop);
            panel.DragDrop += new DragEventHandler(this.panel_DragDrop);
            panel.DragEnter -= new DragEventHandler(this.panel_DragEnter);
            panel.DragEnter += new DragEventHandler(this.panel_DragEnter);
            return panel;
        }
        private bool ismax;
        private Size tempSize;
        private Point oldPoint;
        /// <summary>
        /// panel的双击事件
        /// 双击放大，再双击回到原来的地方
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void doubclick(object sender, MouseEventArgs e)
        {
            //var frm = sender as frmFlash;
            var panel = sender as Panel;
            Param param = paramHashtable[panel.Name] as Param;
            // var panel = frm.Parent;
            if (panel != null)
            {
                if (ismax)
                {

                    panel.Size = tempSize;
                    panel.Location = oldPoint;
                    panel.SendToBack();

                }
                else
                {
                    tempSize = panel.Size;
                    oldPoint = panel.Location;
                    panel.Size = panelcontrol.Size;
                    panel.Location = new Point(0, 0);
                    panel.BringToFront();
                }
                setinfolabel(param, param.InfoLabel != null ? param.InfoLabel.Text : "无视频源...");
                ismax = !ismax;
            }
        }

        private void fillcamera(object obj)
        {
            try
            {
                //this.Invoke((MethodInvoker)delegate
                //{
                Param paramvideo = obj as Param;
                paramvideo.m_lRealHandle = -1;
                Console.WriteLine(paramvideo.videoCtrl.Name);
                var DeviceInfo = new CHCNetSDK.NET_DVR_DEVICEINFO_V30();
                var m_bInitSDK = CHCNetSDK.NET_DVR_Init();
                if (m_bInitSDK == false)
                {
                    //MessageBox.Show("视频初始化失败");
                    var msg = "视频初始化失败";
                    this.Invoke((MethodInvoker)delegate
                    {
                        Label infoLabel = new Label();
                        paramvideo.InfoLabel = infoLabel;
                        paramvideo.videoCtrl.Controls.Add(infoLabel);

                        setinfolabel(paramvideo, msg);
                        return;

                    });
                }
                //修改为获取摄像头IP   PORT UNAME UPASSEORD 信息  2015.06.12  LEO
                //var cameraInfo = AppPara.getCameraInfoBySiteId(site.SID);

                //AppLog.WriteLog("cameraIp:" + cameraIp + " chanel:" + site.SPPORT + " username" + site.SPNAME + " password:" + site.SPPASSWORD);

                string DVRIPAddress = ipaddress; ;////DevicePoint.SiteIp; //设备IP地址或者域名 Device IP
                Int16 DVRPortNumber = Convert.ToInt16(paramvideo.VideoFrameDto.VedioPort); //Convert.ToInt16(DevicePoint.SPPORT); //设备服务端口号 Device Port
                string DVRUserName = paramvideo.VideoFrameDto.SPUserName; //DevicePoint.SPNAME; //设备登录用户名 User name to login
                string DVRPassword = paramvideo.VideoFrameDto.SPPassword;// DevicePoint.SPPASSWORD; //设备登录密码 Password to login

                //string DVRIPAddress = "192.168.1.64"; ;////DevicePoint.SiteIp; //设备IP地址或者域名 Device IP
                //Int16 DVRPortNumber = 8000; //Convert.ToInt16(DevicePoint.SPPORT); //设备服务端口号 Device Port
                //string DVRUserName = "admin"; //DevicePoint.SPNAME; //设备登录用户名 User name to login
                //string DVRPassword = "admin123";// DevicePoint.SPPASSWORD; //设备登录密码 Password to login


                //登录设备 Login the device
                paramvideo.m_lUserID = CHCNetSDK.NET_DVR_Login_V30(DVRIPAddress, DVRPortNumber, DVRUserName, DVRPassword,
                    ref DeviceInfo);
                paramvideo.DeviceInfo = DeviceInfo;
                if (paramvideo.m_lUserID < 0)
                {
                    var iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                    var str = "登录失败, error code= " + iLastErr;
                    //登录失败，输出错误号 Failed to login and output the error code
                    //DebugInfo(str);
                    //MessageBox.Show(str);
                    //labelControl1.Text = "登录视频失败";
                    this.Invoke((MethodInvoker)delegate
                    {
                        Label infoLabel = new Label();
                        paramvideo.InfoLabel = infoLabel;
                        paramvideo.videoCtrl.Controls.Add(infoLabel);

                        setinfolabel(paramvideo, str);
                        Console.WriteLine(paramvideo.videoCtrl.Name + "\t" + str);
                        return;
                    });
                }
                else
                {
                    //登录成功
                    //DebugInfo("NET_DVR_Login_V30 succ!");
                    //btnLogin.Text = "Logout";
                    var msg = "登录成功";
                    var dwAChanTotalNum = (int)DeviceInfo.byChanNum;
                    this.Invoke((MethodInvoker)delegate
                    {
                        Label infoLabel = new Label();
                        paramvideo.InfoLabel = infoLabel;
                        paramvideo.videoCtrl.Controls.Add(infoLabel);

                        setinfolabel(paramvideo, msg);
                    });
                    //label1.Text = "\t登录视频成功";
                    //if (!IsAudio)
                    //    btnRecord.Visible = true;

                }


                if (paramvideo.m_lRealHandle < 0)
                {
                    CHCNetSDK.NET_DVR_PREVIEWINFO lpPreviewInfo = new CHCNetSDK.NET_DVR_PREVIEWINFO();
                    this.Invoke((MethodInvoker)delegate
                    {
                        lpPreviewInfo.hPlayWnd = paramvideo.videoCtrl.Handle;//预览窗口 live view window
                    });
                    lpPreviewInfo.lChannel = Convert.ToInt32(paramvideo.VideoFrameDto.SPAlisle); //iChannelNum[(int)iSelIndex];//预览的设备通道 the device channel number
                    lpPreviewInfo.dwStreamType = 0;//码流类型：0-主码流，1-子码流，2-码流3，3-码流4，以此类推
                    lpPreviewInfo.dwLinkMode = 0;//连接方式：0- TCP方式，1- UDP方式，2- 多播方式，3- RTP方式，4-RTP/RTSP，5-RSTP/HTTP 
                    lpPreviewInfo.bBlocked = true; //0- 非阻塞取流，1- 阻塞取流

                    IntPtr pUser = IntPtr.Zero;//用户数据 user data 

                    //打开预览 Start live view 
                    paramvideo.m_lRealHandle = CHCNetSDK.NET_DVR_RealPlay_V40(paramvideo.m_lUserID, ref lpPreviewInfo, null/*RealData*/, pUser);


                    if (paramvideo.m_lRealHandle < 0)
                    {
                        var iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                        var str = "视频预览失败 " + iLastErr; //预览失败，输出错误号 failed to start live view, and output the error code.
                        //DebugInfo(str);
                        //label1.Text = "\t预览视频失败";
                        this.Invoke((MethodInvoker)delegate
                        {
                            if (paramvideo.InfoLabel == null)
                            {
                                Label infoLabel = new Label();
                                paramvideo.InfoLabel = infoLabel;
                                paramvideo.videoCtrl.Controls.Add(infoLabel);
                            }
                            setinfolabel(paramvideo, str);
                            //Console.WriteLine(paramvideo.videoCtrl.Name + "\t" + str);
                            return;
                        });
                    }
                    //btnOpenVideo.Text = "关闭视频";


                }

                //});

            }
            catch (Exception ex)
            {
                //AppLog.WriteLog(ex.StackTrace);
            }
        }
        /// <summary>
        /// 给label赋值
        /// </summary>
        /// <param name="paramvideo"></param>
        /// <param name="msg"></param>
        private void setinfolabel(Param paramvideo, string msg)
        {
            var infoLabel = paramvideo.InfoLabel;
            if (infoLabel == null) return;
            infoLabel.AutoSize = true;

            infoLabel.MaximumSize = new Size(paramvideo.videoCtrl.Width, paramvideo.videoCtrl.Height);
            infoLabel.Text = null;
            infoLabel.Text = msg;
            infoLabel.Location = new Point(0, paramvideo.videoCtrl.Height - 20);
            infoLabel.Anchor = AnchorStyles.Bottom;
            infoLabel.ForeColor = System.Drawing.Color.White;
            infoLabel.BringToFront();
        }
        /// <summary>
        /// 关闭所有视频
        /// </summary>
        private void closeanowcircleallvideos()
        {
            if (paramHashtable == null) return;
            foreach (var key in paramHashtable.Keys)
            {
                Param param = paramHashtable[key] as Param;
                Panel panel = param.videoCtrl;
                closesingelvideo(param.m_lRealHandle, param.m_lUserID);
                Thread thisthread = param.thread;
                abortthread(thisthread);
            }
            CHCNetSDK.NET_DVR_Cleanup();
        }
        /// <summary>
        /// 关闭单个视频
        /// </summary>
        /// <param name="m_lRealHandle"></param>
        /// <param name="m_lUserID"></param>
        /// 

        private void closesingelvideo(int m_lRealHandle, int m_lUserID)
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
        }
        /// <summary>
        /// 关闭当前视频的线程
        /// </summary>
        /// <param name="thisthread"></param>
        private void abortthread(Thread thisthread)
        {
            if (thisthread != null)
            {
                if (thisthread.IsAlive)
                {
                    thisthread.Abort();
                }
            }
        }
        private class Param
        {
            /// <summary>
            /// 视频播放的控件句柄
            /// </summary>
            public int m_lRealHandle { get; set; }
            /// <summary>
            /// 视频用户登录后的ID
            /// </summary>
            public int m_lUserID { get; set; }
            /// <summary>
            /// 设备信息
            /// </summary>
            public CHCNetSDK.NET_DVR_DEVICEINFO_V30 DeviceInfo { get; set; }
            /// <summary>
            /// 当前序号
            /// </summary>
            public int index { get; set; }
            /// <summary>
            /// 当前视频对应的控件
            /// </summary>
            public Panel videoCtrl { get; set; }

            public Label InfoLabel { get; set; }
            /// <summary>
            /// 当前视频的线程
            /// </summary>
            public Thread thread { get; set; }
            public EquipmentTbDto VideoFrameDto { get; set; }

        }

        private void splitContainer1_SizeChanged(object sender, EventArgs e)
        {
            foreach (var panelname in paramHashtable.Keys)
            {
                Param param = (paramHashtable[panelname] as Param);
                Panel p = param.videoCtrl;
                setpanelsize_position(p, Convert.ToInt16(p.Name));
                setinfolabel(param, param.InfoLabel != null ? param.InfoLabel.Text : "无视频源...");
            }
        }

        private void BatchVideoForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            closeanowcircleallvideos();
            if (paramHashtable != null)
            {
                paramHashtable.Clear();
            }
            if (Videolist != null)
            {
                Videolist.Clear();
                Videolist = null;
            }
            MainForm.GettedUserIP = string.Empty;//此处必须将这个IP设置为空不然会影响下次的查看，特别是上级和超级管理员查看视频
        }
        TreeListHitInfo m_DownHitInfo = null;

        private void treeList1_MouseDown(object sender, MouseEventArgs e)
        {
            Point p = new Point(Cursor.Position.X, Cursor.Position.Y);
            var hi = treeList1.CalcHitInfo(e.Location);
            if (hi.Column != null && e.Button == MouseButtons.Left)
            {
                m_DownHitInfo = hi;
            }
        }
        private void treeList1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && m_DownHitInfo != null)
            {
                // DataRowView rov = treeList1.GetDataRecordByNode(treeList1.FocusedNode) as DataRowView;
                //var data = Videolist.Where(t => t.Id.ToString().IndexOf(this.treeList1.FocusedNode.GetValue("ID").ToString()) != -1);//as EquipmentTbDto;
                //var data = treeList1.GetDataRecordByNode(treeList1.FocusedNode) as EquipmentTbDto;
                treeList1.DoDragDrop(treeList1.FocusedNode, DragDropEffects.Move);
            }
        }
        private void panel_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(TreeListNode)))
            {
                //var node = ((TreeListNode)(e.Data.GetData(typeof(TreeListNode))));
                //if (node == null || node.Level == 0) return;
                var node = treeList1.FocusedNode;
                if(node==null) return;
                if (Convert.ToInt16(node["Opreate"]) != 2)
                return;
            }
            else
            {
                return;
            }
            var dropPanel = sender as Panel;
            var droppanelParam = paramHashtable[dropPanel.Name] as Param;
            if (droppanelParam != null)
            {
                closesingelvideo(droppanelParam.m_lRealHandle, droppanelParam.m_lUserID);
                Thread thisthread = droppanelParam.thread;
                abortthread(thisthread);
                droppanelParam.VideoFrameDto = null;
                droppanelParam.VideoFrameDto = Videolist.FirstOrDefault(t => t.Id.ToString() == treeList1.FocusedNode.GetValue("ID").ToString());
                if (droppanelParam.InfoLabel != null)
                {
                    droppanelParam.InfoLabel.Dispose();
                    droppanelParam.InfoLabel = null;
                    dropPanel.Controls.Clear();
                }
                dropPanel.Refresh();
                fillcamera(droppanelParam);
            }
        }
        private void panel_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(DevExpress.XtraTreeList.Nodes.TreeListNode)))
            {
                e.Effect = DragDropEffects.Move;
            }
            else
                e.Effect = DragDropEffects.None;
        }

        /// <summary>
        /// 给左边的treelist绑定数据
        /// </summary>
        public void TreeList()
        {
            List<TestTreeList> pList = new List<TestTreeList>();
            # region 没有分单位部门的设备数据
            var nodeptnamelist = Videolist.Where(t => string.IsNullOrEmpty(t.deptname)).ToArray();
            //子设备列表
            HashSet<string> hsnodeptnamenodeSet = new HashSet<string>();
            foreach (var item in nodeptnamelist)
            {
                hsnodeptnamenodeSet.Add(item.ESubTypeName);
            }
            foreach (string s in hsnodeptnamenodeSet)
            {
                //找出当前子设备类型下的设备
                var nodeptnameFirst = nodeptnamelist.Where(p => p.ESubTypeName == s).ToArray();
                TestTreeList nodeptnamefirstnode = new TestTreeList {DisPlayName = s, ID = System.Guid.NewGuid().ToString(),Opreate = 1};
                pList.Add(nodeptnamefirstnode);
                nodeptnameFirst.ForEach(t =>
                {
                    TestTreeList nodeptnamesecondnode = new TestTreeList
                    {
                        ParentID = nodeptnamefirstnode.ID,
                        ID = t.Id.ToString(),
                        DisPlayName = t.EName,
                        Opreate = 2
                    };
                    pList.Add(nodeptnamesecondnode);
                });
            }

            #endregion

            #region 有分单位设备数据
            //分单位列表
            HashSet<string> hsroot = new HashSet<string>();
            foreach (var item in Videolist)
            {
                if (!string.IsNullOrEmpty(item.deptname))
                    hsroot.Add(item.deptname);
            }
            foreach (var root in hsroot)
            {
                //添加一级节点
                TestTreeList proot = new TestTreeList {DisPlayName = root, ID = Guid.NewGuid().ToString()};
                pList.Add(proot);
                //查找二级节点的列表
                var secondlist = Videolist.Where(t => t.deptname == root).ToArray();
                //子设备类型列表
                HashSet<string> hs = new HashSet<string>();
                foreach (var item in secondlist)
                {
                    hs.Add(item.ESubTypeName);
                }
                //构造并添加子设备类型下的设备数据
                foreach (var item in hs)
                {
                    TestTreeList sencondroot = new TestTreeList { DisPlayName = item, ParentID = proot.ID, ID = System.Guid.NewGuid().ToString(),Opreate = 1};
                    pList.Add(sencondroot);
                    if (string.IsNullOrEmpty(item))
                    {
                        var list = secondlist.Where(t => t.ESubTypeName == item).ToList();
                        for (int i = 0; i < list.Count; i++)
                        {
                            TestTreeList q = new TestTreeList
                            {
                                DisPlayName = list[i].EName,
                                ParentID = sencondroot.ID,
                                ID = list[i].Id.ToString(),
                                Opreate = 2
                            };
                            pList.Add(q);
                        }
                    }
                    else
                    {
                        var list = secondlist.Where(t => t.ESubTypeName == item).ToList();
                        for (int i = 0; i < list.Count; i++)
                        {
                            TestTreeList q = new TestTreeList
                            {
                                DisPlayName = list[i].EName,
                                ParentID = sencondroot.ID,
                                ID = list[i].Id.ToString(),
                                Opreate = 2
                                
                            };
                            pList.Add(q);
                        }
                    }
                }
            }
            #endregion

            pList = pList.Where((x, i) => pList.FindIndex(z => z.ID == x.ID) == i).ToList();//去重
            this.treeList1.DataSource = pList;
            this.treeList1.RefreshDataSource();
            imagelist = new ImageList();


            imagelist.Images.Add(Image.FromFile(System.IO.Directory.GetCurrentDirectory() + @"\Resources\shexiangji.png"));
            imagelist.Images.Add(Image.FromFile(System.IO.Directory.GetCurrentDirectory() + @"\Resources\camera.ico"));
            treeList1.SelectImageList = imagelist;
            if (treeList1.Nodes.Count > 0)
            {
                treeList1.Nodes[0].ExpandAll();
                treeList1.Nodes[0].Expanded = true;
            }
        }


        private void treeList1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var node = treeList1.FocusedNode;
            if (Convert.ToInt16(node["Opreate"]) == 1)
            {
                string disPlayText = node.GetValue("DisPlayName").ToString();
                string ParentID = node.GetValue("ParentID").ToString();
                string deptname = null;
                if (!string.IsNullOrEmpty(ParentID)&&ParentID.Trim()!="-1")
                {
                    deptname = treeList1.FindNodeByKeyID(ParentID)["DisPlayName"].ToString();
                }

                //var list = Videolist.Where(t => t.ESubTypeName.ToString() == disPlayText).ToList();
                if (node.HasChildren)
                {
                    closeanowcircleallvideos();
                    if (paramHashtable != null)
                    {
                        paramHashtable.Clear();
                    }
                    panelcontrol.Controls.Clear();
                    panelcontrol.Refresh();
                    nowshow(disPlayText,deptname);
                }
            }
        }

        private void treeList1_DoubleClick(object sender, EventArgs e)
        {
            //TreeListNode clickedNode = this.treeList1.FocusedNode;
            string disPlayText = this.treeList1.FocusedNode.GetValue("ID").ToString();

            if (!string.IsNullOrEmpty(disPlayText))//由于我增加了Filter所以Filter那行必须要忽略掉。
            {
                var list = Videolist.Where(t => t.Id.ToString().IndexOf(disPlayText) != -1).ToList();
                if (list.Count > 0)
                {
                    if (paramHashtable == null) return;
                    Param param = paramHashtable["0"] as Param;
                    Panel panel = param.videoCtrl;
                    closesingelvideo(param.m_lRealHandle, param.m_lUserID);
                    Thread thisthread = param.thread;
                    abortthread(thisthread);
                    param.VideoFrameDto = null;
                    param.VideoFrameDto = Videolist.Find(delegate(EquipmentTbDto cc) { return cc.Id.ToString() == disPlayText; });
                    //param.InfoLabel.Dispose();
                    param.InfoLabel = null;
                    //panelcontrol.Controls.Clear();
                    fillcamera(param);
                }
                else
                {
                    if (paramHashtable == null)
                    {
                        //nowshow(disPlayText);
                    }
                    else
                    {
                        foreach (var key in paramHashtable.Keys)
                        {
                            Param param = paramHashtable[key] as Param;
                            Panel panel = param.videoCtrl;
                            closesingelvideo(param.m_lRealHandle, param.m_lUserID);
                            Thread thisthread = param.thread;
                            abortthread(thisthread);
                            param.VideoFrameDto = null;
                            param.VideoFrameDto = Videolist.Find(delegate(EquipmentTbDto cc) { return cc.Id.ToString() == disPlayText; });
                            param.InfoLabel.Dispose();
                            param.InfoLabel = null;
                            panel.Controls.Clear();
                            panel.Refresh();
                        }
                        if (paramHashtable != null)
                        {
                            paramHashtable.Clear();
                        }
                        // panel.controls.clear();
                        nowcirclenum = 0;
                        //nowshow(disPlayText);
                    }
                }
            }
        }
          /// <summary>
          /// 播放一类视频
          /// </summary>
          /// <param name="ESubTypeName">设备类型名称</param>
          /// <param name="refresh">true是刷新或者false是首次创建</param>
        private void nowshow(string ESubTypeName, string deptname, bool refresh = true)
        {
            var list = Videolist.Where(t => t.ESubTypeName == ESubTypeName && t.deptname == deptname).ToList();

            //nowcirclenum++;
            if (refresh)
            {
                setrowcloum(list);
                CreatVedioPanel();
                getcirclenum();
            }

            nowcirclenum = 1;
            int nowshowmaxnum = nowcirclenum * 9;
            List<EquipmentTbDto> c = null;
            if (nowshowmaxnum > list.Count)
            {
                nowshowmaxnum = list.Count - (nowcirclenum - 1) * all_video;//当最后显示的视频不足3个的时候

                c = list.GetRange((nowcirclenum - 1) * all_video, nowshowmaxnum);
            }
            else
            {
                c = list.GetRange((nowcirclenum - 1) * all_video, all_video);
            }
            Initvideo(c);
        }
        public class TestTreeList
        {
            public TestTreeList()
            {
            }
            //名称字段变量
            private string m_sName = string.Empty;
            //子Node节点ID变量
            private string m_iID = "-1";
            //父Node节点ID变量
            private string m_iParentID = " -1";

            public string ID
            {
                get
                {
                    return m_iID;
                }
                set
                {
                    m_iID = value;
                }
            }
            public string ParentID
            {
                get
                {
                    return m_iParentID;
                }
                set
                {
                    m_iParentID = value;
                }
            }
            public string DisPlayName
            {
                get
                {
                    return m_sName;
                }
                set
                {
                    m_sName = value;
                }
            }
            /// <summary>
            /// 操作标识
            /// 1为可双击播放子类下的视频
            /// 2为可拖拽视频项到panel上播放视频
            /// </summary>
            public int Opreate { get; set; }
        }
        /// <summary>
        /// treelist图片显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeList1_CustomDrawNodeImages(object sender, CustomDrawNodeImagesEventArgs e)
        {
            e.SelectImageIndex = (e.Node.Level - 1) + 1; // e.SelectImageIndex为图片在ImageList中的index
        }





    }

}
