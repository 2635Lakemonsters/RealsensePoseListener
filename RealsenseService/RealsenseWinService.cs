using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Configuration;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace RealsenseService
{
    public partial class RealsenseWinService : ServiceBase
    {
        Process proc;
        EventLog eventLog;

        public int MonitorIntervalInSeconds { get; private set; } = 30;


        public RealsenseWinService()
        {
            InitializeComponent();
            this.AutoLog = false;

            if (System.Diagnostics.EventLog.SourceExists("RealsenseWinService"))
            {
                System.Diagnostics.EventLog.DeleteEventSource("RealsenseWinService");
            }

            if (!System.Diagnostics.EventLog.SourceExists("RealsenseWinService"))
            {
                System.Diagnostics.EventLog.CreateEventSource("RealsenseWinService", "Application");
            }

            eventLog = new System.Diagnostics.EventLog("Application");
            // configure the event log instance to use this source name
            eventLog.Source = "RealsenseWinService";
            eventLog.Log = "Application";

        }

        protected override void OnStart(string[] args)
        {
            eventLog.WriteEntry("In OnStart.");
            string executablePath = ConfigurationManager.AppSettings["ExecutablePath"];

            if (!File.Exists(executablePath))
            {
                eventLog.WriteEntry("Executable '" + executablePath + "' not found.", EventLogEntryType.Error);
                Environment.Exit(1);
            }
            else
            {
                eventLog.WriteEntry("Starting executable '" + executablePath + "'.", EventLogEntryType.Information);
            }
            

            if (proc == null)
            {
                proc = new Process();
                ProcessStartInfo startInfo = new ProcessStartInfo(executablePath);
                //ProcessStartInfo processInfo = new ProcessStartInfo("cmd.exe", "/C " + command);
                startInfo.CreateNoWindow = true;
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                startInfo.UseShellExecute = false;
                startInfo.RedirectStandardOutput = true;
                //startInfo.LoadUserProfile = true;
                proc.StartInfo = startInfo;
               
            }

            proc.Start();
            proc.OutputDataReceived += Proc_OutputDataReceived;
            proc.BeginOutputReadLine();


        }

        private void Proc_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            eventLog.WriteEntry("OutputData Received:" + e.Data);
        }

        protected override void OnStop()
        {
            proc.Close();
        }




    }
}
