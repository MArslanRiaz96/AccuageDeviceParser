using System;
using System.Collections.Generic;
using System.Text;

namespace AccuageDeviceParser.Model
{
   public class DeviceRawData
    {
        public int Id { get; set; }
        public string RawData { get; set; }
        public string DeviceLogin { get; set; }
        public bool IsSync { get; set; } 
    }
}
