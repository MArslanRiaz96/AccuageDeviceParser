using AccuageDeviceParser.Helper;
using AccuageDeviceParser.Service;
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

namespace ParserServerv2
{
    public partial class Form1 : Form
    {
        public bool Stop { get; set; } = false;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void Bw_DoWork(object sender, DoWorkEventArgs e)
        {
            bool isItInprogress = false;

            while (true)
            {
                try
                {
                    var device = new DataService().LoadDevice();
                    Console.WriteLine(isItInprogress ? "Server buzzy" : "Server free");
                    if (!isItInprogress)
                    {
                        isItInprogress = true;
                        var rawData = new DataService().LoadRawData();
                        foreach (var item in rawData)
                        {
                            if (device != null)
                            {
                                try
                                {
                                    new ParserHeleper().DataPacketParser(item.RawData, device.FirstOrDefault(x => x.DeviceId.Equals(item.DeviceLogin)).Id);
                                    DataService.UpdateRawData(item.Id, true);
                                }
                                catch (Exception)
                                {
                                }
                            }
                        }
                        isItInprogress = false;
                    }
                    if (Stop)
                    {
                        break;
                    }
                }
                catch (Exception ex)
                {

                }
            }
            //Set your Result of the long running task
        }

        private void Bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //Apply your Results to your GUI-Elements
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Stop = false;
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += Bw_DoWork;
            bw.RunWorkerCompleted += Bw_RunWorkerCompleted;
            bw.RunWorkerAsync();
           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Stop = true;
        }
    }
}
