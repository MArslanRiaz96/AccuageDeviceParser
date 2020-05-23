using System;
using System.Collections.Generic;
using System.Text;

namespace AccuageDeviceParser.Model
{
    public enum PacketType
    {
        SD = 0,
        D = 1,
        B = 2
    }
    public class DataPacket
    {
        public DateTime Date { get; set; }
        public string Lat { get; set; }
        public string Lan { get; set; }
        public string Lat1 { get; set; }
        public string Lan1 { get; set; }
        public string Speed { get; set; }
        public string Course { get; set; }
        public string Height { get; set; }
        public string Stats { get; set; }
        public string Crc16 { get; set; }
        public string Hdop { get; set; }
        public string Input { get; set; }
        public string Output { get; set; }
        public string Adc { get; set; }
        public string IButton { get; set; }
        public string Params { get; set; }
        public string ThreadId { get; set; }
        public PacketType PacketType { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
