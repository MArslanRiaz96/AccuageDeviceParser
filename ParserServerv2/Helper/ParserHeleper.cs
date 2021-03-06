﻿using AccuageDeviceParser.Model;
using AccuageDeviceParser.Service;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using ParserServerv2.Helper;
using ParserServerv2.Logger;

namespace AccuageDeviceParser.Helper
{
    public static class DateTimeExtensions
    {
        public static DateTime ToDateTime(this string s,
            string format = "ddMMyy", string cultureString = "tr-TR")
        {
            try
            {
                var r = DateTime.ParseExact(
                    s: s,
                    format: format,
                    provider: CultureInfo.GetCultureInfo(cultureString));
                return r;
            }
            catch (FormatException)
            {
                throw;
            }
            catch (CultureNotFoundException)
            {
                throw; // Given Culture is not supported culture
            }
        }

        public static DateTime ToDateTime(this string s,
            string format, CultureInfo culture)
        {
            try
            {
                var r = DateTime.ParseExact(s: s, format: format,
                    provider: culture);
                return r;
            }
            catch (FormatException)
            {
                throw;
            }
            catch (CultureNotFoundException)
            {
                throw; // Given Culture is not supported culture
            }

        }

    }

    public class ParserHeleper
    {

        private static string AddChunkSeparator(string str, int chunk_len, char separator)
        {
            if (str == null || str.Length < chunk_len)
            {
                return str;
            }

            int count = 0;
            StringBuilder builder = new StringBuilder();
            for (var index = 0; index < str.Length; index += chunk_len)
            {
                builder.Append(str, index, chunk_len);
                if (count < 2)
                {
                    builder.Append(separator);
                }
                count++;
            }

            count = 0;
            return builder.ToString();
        }
        private static DataPacket DataParse(string data)
        {
            FileLogger fileLogger = new FileLogger();
            var response = data.Split(';');
            if (response.Length > 5)
            {
                var dataPacket = new DataPacket();
                dataPacket.CreatedAt = DateTime.Now;
                dataPacket.PacketType = PacketType.D;
                dataPacket.ThreadId = "1";
                dataPacket.Date = response[0].Trim().ToDateTime(format: "ddMMyy");
                dataPacket.Date =
                    dataPacket.Date.Date.Add(DateTime.ParseExact(AddChunkSeparator(response[1].Trim(), 2, ':'), "HH:mm:ss", CultureInfo.InvariantCulture)
                        .TimeOfDay);
                dataPacket.Lat = LocationHelper.LatParser(response[2]);
                dataPacket.Lat1 = response[3];
                dataPacket.Lan = LocationHelper.LanHelper(response[4]);
                dataPacket.Lan1 = response[5];
                dataPacket.Speed = response[6];
                dataPacket.Course = response[7];
                dataPacket.Height = response[8];
                dataPacket.Stats = response[9];
                dataPacket.Hdop = response[10];
                dataPacket.Input = response[11];
                dataPacket.Output = response[12];
                dataPacket.Adc = response[13];
                dataPacket.IButton = response[14];
                dataPacket.Params = response[15];
                fileLogger.Log("Parse Data for the below Device ID : " + "\r\n"
                    + "CreatedAt = " + dataPacket.CreatedAt + "\r\n"
                    + "PacketType = " + dataPacket.PacketType + "\r\n"
                    + "ThreadId = " + dataPacket.ThreadId + "\r\n"
                    + "Date = " + dataPacket.Date + "\r\n"
                    + "Lat = " + dataPacket.Lat + "\r\n"
                    + "Lat1 = " + dataPacket.Lat1 + "\r\n"
                    + "Lan = " + dataPacket.Lan + "\r\n"
                    + "Lan1 = " + dataPacket.Lan1 + "\r\n"
                    + "Speed = " + dataPacket.Speed + "\r\n"
                    + "Course = " + dataPacket.Course + "\r\n"
                    + "Height = " + dataPacket.Height + "\r\n"
                    + "Stats = " + dataPacket.Stats + "\r\n"
                    + "Hdop = " + dataPacket.Hdop + "\r\n"
                    + "Input = " + dataPacket.Input + "\r\n"
                    + "Output = " + dataPacket.Output + "\r\n"
                    + "Adc = " + dataPacket.Adc + "\r\n"
                    + "IButton = " + dataPacket.IButton + "\r\n"
                    + "Params = " + dataPacket.Params + "\r\n"
                    + "\r\n");
                fileLogger = null;
                return dataPacket;
            }
            return null;
        }

        public void DataPacketParser(string data, int id)
        {
            if (data.Contains("#D#"))
            {
                //handel null here
                // recode as error
                DataService.InsertData(DataParse(data.Split('#')[2]), id);
            }
            else if (data.Contains("#B#"))
            {
                var response = data.Split('#')[2].Split('|');
                foreach (var item in response)
                {
                    DataService.InsertData(DataParse(item), id);
                }
            }
        }
    }
}
