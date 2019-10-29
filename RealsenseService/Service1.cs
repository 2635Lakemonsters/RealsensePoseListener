using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace RealsenseService
{
    public partial class Service1 : ServiceBase
    {
        System.Diagnostics.Process proc;
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            proc = new Process();
            System.Diagnostics.ProcessStartInfo startInfo = new ProcessStartInfo(@"C:\dev\cpp\RealsensePoseListener\x64\Release\RealsensePoseListener.exe");
            proc.StartInfo = startInfo;
            proc.Start();

        }

        protected override void OnStop()
        {
            proc.Kill();
        }
    }
}
