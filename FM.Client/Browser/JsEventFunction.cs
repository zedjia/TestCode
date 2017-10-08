using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraExport;
using FM.Lib.Model;

namespace FMShell.Browser
{
    public class JsEventFunction
    {
        public MainForm MainForm;
        public JsEventFunction(MainForm form)
        {
            MainForm = form;
        }

        public void CloseForm()
        {
            Application.ExitThread();
            Application.Exit();
            Process.GetCurrentProcess().Kill();
            System.Environment.Exit(0);
        }

        public void ShowVideoForm(string data)
        {
            MainForm.BeginInvoke(new Func<bool>(() =>
            {
                VideoForm form = new VideoForm(MainForm, data);
                //form.StartAudio();
                form.ShowDialog(MainForm);
                //MainForm.StartAudio(form.DisplayStatusMsg);
                //form.DisplayStatusMsg("远程连接失败.");
                return false;
            }));
        }

        public string GetUsedtoByLoginID(string json)
        {
            MainForm.MainformGetUsedtoByLoginID(json);
            return MainForm.GettedUserIP;
        }
        public void ShowStationVideoForm(string data)
        {
            MainForm.BeginInvoke(new Func<bool>(() =>
            {
                BatchVideoForm form = new BatchVideoForm(MainForm, data);
                form.Size = new Size(800, 450);
                form.ShowDialog(MainForm);
                return false;
            }));
        }
        public void GetOnlineUsers()
        {
            MainForm.GetOnlineUsers();
        }


        public void Login(string data)
        {
            MainForm.PrepareLoginData(data);

        }

        #region 导出文件
        /// <summary>
        /// 获取巡查分析数据
        /// </summary>
        /// <param name="data"></param>
        public void GetInspectionData(string data)
        {
            MainForm.GetInspectionData(data);
        }
        /// <summary>
        /// 获取维修分析数据
        /// </summary>
        /// <param name="data"></param>
        public void GetEquipmentMaintRecord(string data)
        {
            MainForm.GetEquipmentMaintRecord(data);
        }
        /// <summary>
        /// 获取查岗分析数据
        /// </summary>
        /// <param name="data"></param>
        public void GetChechTaskData(string data)
        {
            MainForm.GetChechTaskData(data);
        }
        /// <summary>
        /// 获取考核分析数据
        /// </summary>
        /// <param name="data"></param>
        public void GetMonitoringStationData(string data)
        {
            MainForm.GetMonitoringStationData(data);
        }

        #endregion
        /// <summary>
        /// 查岗  // 
        /// </summary>
        /// <param name="data"></param>
        public void Inspect(string taskId, string loginId)//loginid,loginid,loginid,loginid,loginid
        {
            MainForm.Inspect(string.Format("{0},{1}", taskId, loginId));
        }

        /// <summary>
        /// 任务下发
        /// </summary>
        /// <param name="data"></param>
        public void SendTask(string taskId, string loginIds)
        {
            MainForm.SendTask(string.Format("{0},{1}", taskId, loginIds));
        }


        /// <summary>
        /// 消控室自动登陆
        /// </summary>
        public string getDefaultLogin()
        {
            string uname = ConfigurationManager.AppSettings["username"];
            string pwd = ConfigurationManager.AppSettings["password"];
            if (string.IsNullOrEmpty(uname) || string.IsNullOrEmpty(pwd))
            {
                return string.Empty;
            }
            else
            {
                return string.Format("{0},{1}", uname, pwd);
            }
        }

    }
}
