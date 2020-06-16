using AccuageDeviceParser.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using System.Data;


namespace AccuageDeviceParser.Service
{
   public class DataService
    {
        public static string ConnectionStringTCP { get; set; } = "Server=173.248.132.203,1533; Initial Catalog=accuguage_TCP_db; User Id = sa; Password=7gUSS@nKH;";
        public static string ConnectionStringAccuguage { get; set; } = "Server=173.248.132.203,1533; Initial Catalog=accuguage_db; User Id = sa; Password=7gUSS@nKH;";
        public static void InsertData(DataPacket dataPacket,int deviceId)
        {
            if(dataPacket!=null)
            { 
            // define INSERT query with parameters
            var query = "INSERT INTO DeviceReadings (Date, Adc, Course, Crc16, Hdop,Height,IButton,Input,Lan,Lan1,Lat,Lat1,Output,PacketType,Params,Speed,Stats,DeviceId) " +
                "VALUES (@Date,@Adc, @Course, @Crc16, @Hdop, @Height,@IButton,@Input,@Lan,@Lan1,@Lat,@Lat1,@Output,@PacketType,@Params,@Speed,@Stats,@DeviceId) ";
                // create connection and command
                using (var cn = new SqlConnection(ConnectionStringTCP))
                using (var cmd = new SqlCommand(query, cn))
                {
                    // define parameters and their values
                    cmd.Parameters.Add("@Date", SqlDbType.DateTime, 50).Value = dataPacket.Date;
                    cmd.Parameters.Add("@Adc", SqlDbType.VarChar, 50).Value = dataPacket.Adc;
                    cmd.Parameters.Add("@Course", SqlDbType.VarChar, 50).Value = dataPacket.Course;
                    cmd.Parameters.Add("@Crc16", SqlDbType.VarChar, 50).Value = dataPacket.Crc16 == null ? "" : dataPacket.Crc16;
                    cmd.Parameters.Add("@Hdop", SqlDbType.VarChar, 50).Value = dataPacket.Hdop;
                    cmd.Parameters.Add("@Height", SqlDbType.VarChar, 50).Value = dataPacket.Height;
                    cmd.Parameters.Add("@IButton", SqlDbType.VarChar, 50).Value = dataPacket.IButton;
                    cmd.Parameters.Add("@Input", SqlDbType.VarChar, 50).Value = dataPacket.Input;
                    cmd.Parameters.Add("@Lan", SqlDbType.VarChar, 50).Value = dataPacket.Lan;
                    cmd.Parameters.Add("@Lan1", SqlDbType.VarChar, 50).Value = dataPacket.Lan1;
                    cmd.Parameters.Add("@Lat", SqlDbType.VarChar, 50).Value = dataPacket.Lat;
                    cmd.Parameters.Add("@Lat1", SqlDbType.VarChar, 50).Value = dataPacket.Lat1;
                    cmd.Parameters.Add("@Output", SqlDbType.VarChar, 50).Value = dataPacket.Output;
                    cmd.Parameters.Add("@PacketType", SqlDbType.Int, 50).Value = dataPacket.PacketType;
                    cmd.Parameters.Add("@Params", SqlDbType.VarChar, 500).Value = dataPacket.Params;
                    cmd.Parameters.Add("@Speed", SqlDbType.VarChar, 50).Value = dataPacket.Speed;
                    cmd.Parameters.Add("@Stats", SqlDbType.VarChar, 50).Value = dataPacket.Stats;
                    cmd.Parameters.Add("@DeviceId", SqlDbType.Int, 50).Value = deviceId;
                    //
                    // open connection, execute INSERT, close connection
                    cn.Open();
                    var response = cmd.ExecuteNonQuery();
                    cn.Close();
                }
            }
        }
        public  List<Device> LoadDevice()
        {
            SqlCommand cmd = new SqlCommand("Select * from Devices", new SqlConnection(ConnectionStringAccuguage));
            return fetchDevice(cmd);
        }
        public  List<DeviceRawData> LoadRawData()
        {
            SqlCommand cmd = new SqlCommand("Select * from DeviceRawDatas where IsSync='false'", new SqlConnection(ConnectionStringTCP));
            return fetchRawData(cmd);
        }
        private List<Device> fetchDevice(SqlCommand cmd)
        {
            SqlConnection con = cmd.Connection;
            List<Device> adsList = null;
            con.Open();
            using (con)
            {
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    adsList = new List<Device>();
                    while (dr.Read())
                    {
                        Device vad = new Device();
                        vad.DeviceId = dr["IMEI"].ToString();
                        vad.Id =Convert.ToInt32(dr["Id"]);
                        adsList.Add(vad);
                    }
                    adsList.TrimExcess();
                }
            }
            return adsList;
        }
        private List<DeviceRawData> fetchRawData(SqlCommand cmd)
        {
            SqlConnection con = cmd.Connection;
            List<DeviceRawData> adsList = null;
            con.Open();
            using (con)
            {
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    adsList = new List<DeviceRawData>();
                    while (dr.Read())
                    {
                        DeviceRawData vad = new DeviceRawData();
                        vad.Id = Convert.ToInt32(dr["Id"]);
                        vad.RawData = dr["DeviceData"].ToString();
                        vad.DeviceLogin = dr["DeviceLogin"].ToString();
                        adsList.Add(vad);
                    }
                    adsList.TrimExcess();
                }
            }
            return adsList;
        }
        public static void UpdateRawData(int id, bool isSync)
        {
            var query = "Update DeviceRawDatas SET IsSync=@IsSync Where Id=@id";
            // create connection and command
            using (var cn = new SqlConnection(ConnectionStringTCP))
            using (var cmd = new SqlCommand(query, cn))
            {
                // define parameters and their values
                cmd.Parameters.Add("@IsSync", SqlDbType.Bit).Value = isSync;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                // open connection, execute INSERT, close connection
                cn.Open();
                var response = cmd.ExecuteNonQuery();
                cn.Close();
            }
        }
    }
}
