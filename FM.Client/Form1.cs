using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Xilium.CefGlue;
using Xilium.CefGlue.Demo;

namespace FMShell
{
    public partial class Form1 : Form, IMainView
    {
        //private readonly DemoApp _application;
        //private readonly string _applicationTitle;
        //private readonly TabControl _tabs;

        //private readonly SynchronizationContext _pUIThread;
        //public Form1(DemoApp application, Xilium.CefGlue.Demo.MenuItem[] menuItems)
        //{
        //    InitializeComponent();
        //    _pUIThread = WindowsFormsSynchronizationContext.Current;

        //    _application = application;
        //    //CreateMenu(menuItems);

        //    _applicationTitle = _application.Name + " (Windows Forms)";
        //    Text = _applicationTitle;
        //    Size = new Size(_application.DefaultWidth, _application.DefaultHeight);

        //    Padding = new Padding(0, 0, 0, 0);

        //    _tabs = new TabControl();
        //    _tabs.Parent = this;
        //    _tabs.Margin = new Padding(10, 10, 10, 10);
        //    _tabs.Dock = DockStyle.Fill;

        //    _tabs.Appearance = TabAppearance.Normal;
        //    _tabs.Padding = new Point(6, 6);

        //    Visible = true;
        //}


        public Form1()
        {
            InitializeComponent();
            Init();

        }


        void Init()
        {
            //CefRuntime.Load();
            //var mainArgs = new CefMainArgs(new string[] { });
            //var exitCode = CefRuntime.ExecuteProcess(mainArgs, null);
            //if (exitCode != -1)
            //    return;
            //var settings = new CefSettings
            //{
            //    SingleProcess = false,
            //    MultiThreadedMessageLoop = true,
            //    LogSeverity = CefLogSeverity.Disable,
            //    Locale = "zh-CN"
            //};
            //CefRuntime.Initialize(mainArgs, settings, null);
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //if (!settings.MultiThreadedMessageLoop)
            //{
            //    Application.Idle += (sender, e) => { CefRuntime.DoMessageLoopWork(); };
            //}
            //Application.Run(new CefBrowser());
            //CefRuntime.Shutdown();
            cefWebBrowser1.StartUrl = "http://www.baidu.com";
            //cefWebBrowser1.WebBrowser
            var cwi = CefWindowInfo.Create();
            cwi.SetAsChild(this.Handle, new CefRectangle(0, 0, this.Width, this.Height));
            var bc = new WebClient();
            var bs = new CefBrowserSettings() { };
            //CefBrowserHost.CreateBrowser(cwi, bc, bs, "http://www.cnblogs.com/liulun");

        }


        #region  接口

        public CefBrowser CurrentBrowser
        {
            get { throw new NotImplementedException(); }
        }

        public void NavigateTo(string url)
        {
            throw new NotImplementedException();
        }

        public void NewTab(string url)
        {
            throw new NotImplementedException();
        }

        public void NewWebView(string url, bool transparent)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
