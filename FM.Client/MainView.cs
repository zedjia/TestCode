using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using FM.Lib.Controls;
using Xilium.CefGlue;
using Xilium.CefGlue.Demo;
using MenuItem = Xilium.CefGlue.Demo.MenuItem;
using MenuItemImpl = System.Windows.Forms.MenuItem;

namespace FMShell
{
    public partial class MainView : Form, IMainView
    {
        private readonly DemoApp _application;
        private readonly string _applicationTitle;

        private readonly SynchronizationContext _pUIThread;
        private Sodao.FastSocket.Client.AsyncBinarySocketClient socketClient;

        public static MainView MainForm {  get;private set; }


        public MainView(DemoApp application, MenuItem[] menuItems)
        {
            InitializeComponent();
            //MainForm = this;
            _pUIThread = WindowsFormsSynchronizationContext.Current;

            _application = application;
            
            _applicationTitle = string.Empty ;
            Text = _applicationTitle;
            Size = new Size(_application.DefaultWidth, _application.DefaultHeight);

            Padding = new Padding(0, 0, 0, 0);


            Visible = true;

            SocketClientInit();
            updateTimer.Start();
            //NavigateTo("http://localhost/",true);
            //cefWebBrowser1.chi
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            //foreach (TabPage page in _tabs.TabPages)
            //{
            var browser = cefWebBrowser1; //page.Tag as CefWebBrowser;
            if (browser != null)
            {
                browser.Dispose();
            }
            //}

            base.OnClosing(e);
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            _application.Quit();
        }

        #region CreateMenu

      
        

        #endregion

        public void NewTab(string url)
        {
            var state = new WebBrowserState();

            //var tabPage = new TabPage(url);

            //var navBox = new WebNavigationBox();
            //navBox.Parent = tabPage;
            //navBox.Dock = DockStyle.Top;
            //navBox.Visible = true;
            //navBox.HomeUrl = _application.HomeUrl;
            //this.cefWebBrowser1=new CefWebBrowser();
            var browserCtl = this.cefWebBrowser1; //new CefWebBrowser();
            //tabPage.Tag = browserCtl;
            //browserCtl.Parent = tabPage;
            //browserCtl.Dock = DockStyle.Fill;
            //browserCtl.BringToFront();

            var browser = browserCtl.WebBrowser;
            //browser.StartUrl = url;
            //browser.StartUrl = "http://www.baidu.com";

            //navBox.Attach(browser);

            
            

            browser.TitleChanged += (s, e) =>
                {
                    state.Title = e.Title;
                    _pUIThread.Post((_state) => { UpdateTitle(e.Title); }, null);
                };

            browser.AddressChanged += (s, e) =>
                {
                    state.Title = e.Address;
                    //_pUIThread.Post((_state) => { navBox.Address = e.Address; }, null);
                };

            browser.TargetUrlChanged += (s, e) =>
                {
                    state.TargetUrl = e.TargetUrl;
                    // TODO: show targeturl in status bar
                    // _pUIThread.Post((_state) => { UpdateTargetUrl(e.TargetUrl); }, null);
                };

            browser.LoadingStateChanged += (s, e) =>
                {
                    _pUIThread.Post((_state) =>
                        {
                            //navBox.CanGoBack = e.CanGoBack;
                            //navBox.CanGoForward = e.CanGoForward;
                            //navBox.Loading = e.Loading;
                        }, null);
                };
            //_tabs.TabPages.Add(tabPage);
            //_tabs.SelectedTab = tabPage;
        }

        private void UpdateTitle(string title)
        {
            Text = string.IsNullOrEmpty(title) ? _applicationTitle : title + " - " + _applicationTitle;
        }

        
        public void NavigateTo(string url)
        {
            CurrentBrowser.StopLoad();
            CurrentBrowser.GetMainFrame().LoadUrl(url);
        }

        // public async void NavigateTo(string url,bool isSync=true)
        //{
        //    await Task.Run(() =>
        //    {
        //        var result = true;
        //        while (result)
        //        {
        //            if (CurrentBrowser != null)
        //            {
        //                CurrentBrowser.StopLoad();
        //                CurrentBrowser.GetMainFrame().LoadUrl(url);
        //                result = false;
        //            }
        //        }
        //    });
        //}

         


        public CefBrowser CurrentBrowser
        {
            get
            {
                //var navBox = GetCurrentNavBox();
                //if (navBox == null) return null;
                //return navBox.GetBrowser();
                return this.cefWebBrowser1.WebBrowser.CefBrowser;
            }
        }

        public void NewWebView(string url, bool transparent)
        {
            var view = new CefWebView(url, transparent);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            NavigateTo("http://www.baidu.com");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            NewTab("http://www.baidu.com");
        }

        #region  socket client init

        void SocketClientInit()
        {
            string serverIp = ConfigurationManager.AppSettings["SocketServerIp"];
            int serverPort = Convert.ToInt32(ConfigurationManager.AppSettings["SocketServerPort"]);
            socketClient = new Sodao.FastSocket.Client.AsyncBinarySocketClient(8192, 8192, 3000, 3000);
            socketClient.RegisterServerNode(serverIp, new System.Net.IPEndPoint(System.Net.IPAddress.Parse(serverIp), serverPort));
           
            
        }

        void SendBeatCommand()
        {
            if (socketClient == null)
                return;
            byte[] data = System.Text.Encoding.Default.GetBytes("心跳包:" + DateTime.Now.ToString("yyyy MMMM dd hh:mm:ss"));
            socketClient.Send("Beat", data, res => System.Text.Encoding.Default.GetString(res.Buffer)).ContinueWith(q =>
            {
                if (q.IsFaulted)
                {
                    //MessageBox.Show(string.Format("心跳包异常:{0}", q.Exception.ToString()));
                    return;
                }
            });
        }

        #endregion 

        private void updateTimer_Tick(object sender, EventArgs e)
        {
            SendBeatCommand();
        }



        #region static func

        public void ExitApplication()
        {
            System.Environment.Exit(0);
        }

        public void TestApp()
        {
            MessageBox.Show("test");
        }
        
        #endregion
    }
}
