using AccuageDeviceParser.Helper;
using AccuageDeviceParser.Service;
using ParserServerv2.Helper;
using ParserServerv2.Logger;
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
                        if (rawData != null)
                        {
                            foreach (var item in rawData)
                            {
                                FileLogger fileLogger = new FileLogger();
                                if (device != null)
                                {
                                    try
                                    {
                                        new ParserHeleper().DataPacketParser(item.RawData, device.FirstOrDefault(x => x.DeviceId.Equals(item.DeviceLogin)).Id);
                                        DataService.UpdateRawData(item.Id, true);
                                        fileLogger.Log("DeviceId : " + device.FirstOrDefault(x => x.DeviceId.Equals(item.DeviceLogin)).Id + "\r\n" + "DeviceLogin : " + item.DeviceLogin
                                            + "\r\n" + "Raw Data :" + item.RawData + "\r\n" + "Status : Successful" + "\r\n");
                                        fileLogger = null;
                                    }
                                    catch (Exception ex)
                                    {
                                        fileLogger.Log("DeviceId : " + device.FirstOrDefault(x => x.DeviceId.Equals(item.DeviceLogin)).Id + "\r\n" + "DeviceLogin : " + item.DeviceLogin
                                            + "\r\n" + "Raw Data :" + item.RawData + "\r\n" + "Status : UnSuccessful " + "\r\n" + "Error : "+ ex.ToString() + "\r\n") ;
                                        fileLogger = null;
                                        isItInprogress = false;
                                    }
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
                    isItInprogress = false;
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
            button1.BackColor = Color.FromArgb(0, 128, 0);
            button2.BackColor = Color.FromArgb(255, 255, 255);
           // button1.Hide();
            Stop = false;
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += Bw_DoWork;
            bw.RunWorkerCompleted += Bw_RunWorkerCompleted;
            bw.RunWorkerAsync();
           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button1.BackColor = Color.FromArgb(255, 255, 255);
            button2.BackColor = Color.FromArgb(255, 0, 0);
            Stop = true;
          //  button1.Show();
        }
    }
}
