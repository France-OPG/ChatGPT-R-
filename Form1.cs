using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;

namespace ChatGPT_R_
{
    public partial class Form1 : Form
    {
        ChromiumWebBrowser webBrowser;

        private void Form1_Load(object sender, EventArgs e)
        {
            // Exemple d'exécution de JavaScript après le chargement de la page
            webBrowser.FrameLoadEnd += (s, args) =>
            {
                if (args.Frame.IsMain)
                {
                    // Exécutez du JavaScript après le chargement de la page
                    webBrowser.ExecuteScriptAsync("");
                }
            };
            webBrowser.FrameLoadEnd += (s, args) =>
            {
                if (args.Frame.IsMain)
                {
                    // Mettre à jour l'URL dans le ToolStripMenuItem
                    BeginInvoke(new Action(() =>
                    {
                        uRLToolStripMenuItem.Text = webBrowser.Address;
                    }));
                }
            };
        }

        public Form1()
        {
            InitializeComponent();
            InitializeChromium();
            Activated += Form1_Activated;
            Deactivate += Form1_Deactivate;

            StartPosition = FormStartPosition.Manual;
            Location = new System.Drawing.Point(0, Screen.PrimaryScreen.WorkingArea.Height - Height);
        }

        private void Form1_Activated(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Normal;
        }

        private void Form1_Deactivate(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        public void InitializeChromium()
        {

            CefSettings settings = new CefSettings();
            settings.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/96.0.4664.110 Safari/537.36 /CefSharp Browser" + Cef.CefSharpVersion;
            settings.Locale = "fr";
            settings.CachePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\CGPT+R\History";
            settings.PersistSessionCookies = true;
            settings.CefCommandLineArgs.Add("enable-javascript", "1"); // Activer JavaScript

            if (!Cef.IsInitialized)
            {
                Cef.Initialize(settings);
            }
            webBrowser = new ChromiumWebBrowser("https://chat.openai.com"); // https://chat.openai.com https://google.fr
            panel1.Controls.Add(webBrowser);

        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // Assurez-vous de fermer correctement CefSharp lors de la fermeture du formulaire
            Cef.Shutdown();
            base.OnFormClosing(e);
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void uRLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(webBrowser.Address);
        }


    }
}
