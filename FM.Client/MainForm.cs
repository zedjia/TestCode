using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Z.Lib.Controls;
using Z.Lib.Model;


namespace FMShell
{
    public partial class MainForm : ClientBaseForm
    {



        public MainForm()
        {
            try
            {
                InitializeComponent();
                //this.Opacity = 0;
                //this.ShowInTaskbar = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Console.WriteLine("MainForm_Load");
            timer1.Start();

        }

        
        private void timer1_Tick(object sender, EventArgs e)
        {
            //SendBeatCommand();
        }


        #region 

        


        #region   Biz

        ///// <summary>
        ///// 通过web登陆后，获取用户信息，并且send给socket server
        ///// </summary>
        ///// <param name="data"></param>
        //public void PrepareLoginData(string data)
        //{
        //    CurrentUser = Util.Json.ToObject<UserDto>(data);
        //    CurrentUser.IsLogin = true;
        //    SendLoginCommand(data);

        //}
        //#region 导出文件
        //public string fileUrl = String.Empty;
        //public string fileName = String.Empty;
        ///// <summary>
        ///// 获取巡查分析数据
        ///// </summary>
        ///// <param name="data"></param>
        //public void GetInspectionData(string data)
        //{
        //    fileName = "巡查分析.xls";
        //    Thread InvokeThread = new Thread(new ThreadStart(InvokeMethod));
        //    InvokeThread.SetApartmentState(ApartmentState.STA);
        //    InvokeThread.Start();
        //    InvokeThread.Join();
        //    var list = Util.Json.ToObject<List<InspectionDataTbDto>>(data);
        //    StringBuilder sb = new StringBuilder();
        //    sb.Append("站点名称\t设备名称\t巡查人员\t巡查时间\t巡查状态\t巡查描述\t\n");
        //    foreach (var item in list)
        //    {
        //        sb.Append(item.StationName + "\t");
        //        sb.Append(item.EName + "\t");
        //        sb.Append(item.ReportorName + "\t");
        //        sb.Append(item.CheckTime + "\t");
        //        string state = string.Empty;
        //        if (item.State == (int)InspectionState.正常)
        //            state = "正常";
        //        if (item.State == (int)InspectionState.异常)
        //            state = "异常";
        //        if (item.State == (int)InspectionState.未巡查)
        //            state = "未巡查";
        //        sb.Append(state + "\t");
        //        sb.Append(item.Desc + "\t\n");
        //    }
        //    ExportToExcel(sb.ToString(), fileUrl);
        //}
        ///// <summary>
        ///// 获取维修分析数据
        ///// </summary>
        ///// <param name="data"></param>
        //public void GetEquipmentMaintRecord(string data)
        //{
        //    fileName = "维修分析.xls";
        //    Thread InvokeThread = new Thread(new ThreadStart(InvokeMethod));
        //    InvokeThread.SetApartmentState(ApartmentState.STA);
        //    InvokeThread.Start();
        //    InvokeThread.Join();
        //    var list = Util.Json.ToObject<List<EquipmentMaintRecordTbDto>>(data);
        //    StringBuilder sb = new StringBuilder();
        //    sb.Append("站点名称\t设备名称\t申报维修时间\t设备维护状态\t维护备注\t\n");
        //    foreach (var item in list)
        //    {
        //        sb.Append(item.StationName + "\t");
        //        sb.Append(item.EName + "\t");
        //        sb.Append(item.ReportMaintTime + "\t");
        //        string state = string.Empty;
        //        if (item.EMaintState == (int)EquirmentState.设备正常)
        //            state = "设备正常";
        //        if (item.EMaintState == (int)EquirmentState.设备维修)
        //            state = "设备维修";
        //        sb.Append(state + "\t");
        //        sb.Append(item.MaintRemark + "\t\n");
        //    }
        //    ExportToExcel(sb.ToString(), fileUrl);
        //}
        ///// <summary>
        ///// 获取查岗分析数据
        ///// </summary>
        ///// <param name="data"></param>
        //public void GetChechTaskData(string data)
        //{
        //    fileName = "查岗分析.xls";
        //    Thread InvokeThread = new Thread(new ThreadStart(InvokeMethod));
        //    InvokeThread.SetApartmentState(ApartmentState.STA);
        //    InvokeThread.Start();
        //    InvokeThread.Join();
        //    var list = Util.Json.ToObject<List<CheckTaskTbDto>>(data);
        //    StringBuilder sb = new StringBuilder();
        //    sb.Append("站点名称\t消控室名称\t下发命令时间\t确认完成时间\t是否短信通知\t状态\t\n");
        //    foreach (var item in list)
        //    {
        //        sb.Append(item.StationName + "\t");
        //        sb.Append(item.FireControlRoomName + "\t");
        //        sb.Append(item.CommandTime + "\t");
        //        sb.Append(item.ConfirmTime + "\t");
        //        string hasSms = String.Empty;
        //        if (item.HasSms == true)
        //            hasSms = "是";
        //        if (item.HasSms == false)
        //            hasSms = "否";
        //        sb.Append(hasSms + "\t");
        //        string state = string.Empty;
        //        if (item.Status == (int)CheckTaskState.查岗中)
        //            state = "查岗中";
        //        if (item.Status == (int)CheckTaskState.已完成)
        //            state = "已完成";
        //        if (item.Status == (int)CheckTaskState.未完成)
        //            state = "未完成";
        //        sb.Append(state + "\t\n");
        //    }
        //    ExportToExcel(sb.ToString(), fileUrl);
        //}

        ///// <summary>
        ///// 获取考核分析数据
        ///// </summary>
        ///// <param name="data"></param>
        //public void GetMonitoringStationData(string data)
        //{
        //    fileName = "考核分析.xls";
        //    Thread InvokeThread = new Thread(new ThreadStart(InvokeMethod));
        //    InvokeThread.SetApartmentState(ApartmentState.STA);
        //    InvokeThread.Start();
        //    InvokeThread.Join();
        //    var list = Util.Json.ToObject<List<MonitoringStationTbDto>>(data);
        //    StringBuilder sb = new StringBuilder();
        //    sb.Append("站点名称\t地址\t主管单位\t消防负责人\t查岗总数量\t查岗完成数量\t查岗未完成数量\t\n");
        //    foreach (var item in list)
        //    {
        //        sb.Append(item.StationName + "\t");
        //        sb.Append(item.Address + "\t");
        //        sb.Append(item.DeptName + "\t");
        //        sb.Append(item.FireResponsiblePerson + "\t");
        //        sb.Append((item.TotalNum ?? 0) + "\t");
        //        sb.Append((item.ComNum ?? 0) + "\t");
        //        sb.Append((item.FailNum ?? 0) + "\t\n");
        //    }
        //    ExportToExcel(sb.ToString(), fileUrl);
        //}
        ///// <summary>
        ///// 选择保存地址
        ///// </summary>
        //private void InvokeMethod()
        //{
        //    SaveFileDialog InvokeDialog = new SaveFileDialog();
        //    InvokeDialog.FileName = fileName;//保存的文件名
        //    if (InvokeDialog.ShowDialog() == DialogResult.OK)
        //    {
        //        fileUrl = InvokeDialog.FileName;
        //    }
        //}
        ///// <summary>  
        ///// 导出文件，使用文件流。该方法使用的数据源为DataTable,导出的Excel文件没有具体的样式。  
        ///// </summary>  
        ///// <param name="data">数据</param>  
        ///// <param name="path">保存地址</param>
        //public static string ExportToExcel(string data, string path)
        //{
        //    KillSpecialExcel();
        //    string result = string.Empty;
        //    try
        //    {
        //        // 实例化流对象，以特定的编码向流中写入字符。  
        //        StreamWriter sw = new StreamWriter(path, false, Encoding.GetEncoding("gb2312"));
        //        sw.Write(data);
        //        sw.Flush();
        //        sw.Close();
        //        sw.Dispose();
        //        // 导出成功后打开  
        //        //System.Diagnostics.Process.Start(path);  
        //    }
        //    catch (Exception)
        //    {
        //        result = "请保存或关闭可能已打开的Excel文件";
        //    }
        //    finally
        //    {
        //        //dt.Dispose();
        //    }
        //    return result;
        //}
        ///// <summary>  
        ///// 结束进程  
        ///// </summary>  
        //private static void KillSpecialExcel()
        //{
        //    foreach (System.Diagnostics.Process theProc in System.Diagnostics.Process.GetProcessesByName("EXCEL"))
        //    {
        //        if (!theProc.HasExited)
        //        {
        //            bool b = theProc.CloseMainWindow();
        //            if (b == false)
        //            {
        //                theProc.Kill();
        //            }
        //            theProc.Close();
        //        }
        //    }
        //}
        #endregion
        //public void StartVoiceCommand(string json)
        //{
        //    var dto = Util.Json.ToObject<VideoFrameDto>(json);
        //    dto.RoomId = CurrentUser.UserName;
        //    json = Util.Json.ToJson(dto);
        //    this.SendVoiceCommand(json);
        //}

        //public void EndVoiceCommand(string json)
        //{
        //    var dto = Util.Json.ToObject<VideoFrameDto>(json);
        //    dto.RoomId = CurrentUser.UserName;
        //    json = Util.Json.ToJson(dto);
        //    this.SendVoiceEndCommand(json);
        //}


        ///// <summary>
        ///// 查岗
        ///// </summary>
        ///// <param name="data"></param>
        //internal void Inspect(string data)
        //{
        //    SendInspectCommand(data);
        //}

        //internal void SendTask(string data)
        //{
        //    SendTaskCommand(data);
        //}
        //public void GetOnlineUsers()
        //{
        //    CurrentUsersCommand();

        //}
        //public void MainformGetUsedtoByLoginID(string json)
        //{
        //    GetUsedtoByLoginID(json);

        //}
        //public void MainformGetVideoIPByStationID(string json)
        //{
        //    GetUsedtoByStationID(json);

        //}
#endregion


        #region test

        private void button2_Click(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }
        private void button3_Click(object sender, EventArgs e)
        {
            //VideoForm form = new VideoForm(this);
            //form.Show(this);
            //form.DisplayStatusMsg("远程连接失败.");

        }

        private void button4_Click(object sender, EventArgs e)
        {
            //GetOnlineUsers();
            //GetUsedtoByLoginID("A4BFA4C4-D97E-4372-82E2-69854768939B");
            var dto = GettedUserIP;
            MessageBox.Show(dto);
        }


        #endregion

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            string name = System.Diagnostics.Process.GetCurrentProcess().ProcessName;
            System.Diagnostics.Process[] process = System.Diagnostics.Process.GetProcessesByName(name);
            foreach (System.Diagnostics.Process p in process)
            {
                p.Kill();
            }
        }

        private void exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }











    }



}
