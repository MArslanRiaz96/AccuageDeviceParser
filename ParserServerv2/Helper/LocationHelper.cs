using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserServerv2.Helper
{
    public  class LocationHelper
    {
        public static string  LatParser(string lat)
        {
            if (lat != "NA" && lat != null)
            {
                return $"{(Convert.ToDouble(lat.Substring(2)) / 60) + Convert.ToInt32(lat.Substring(0, 2))}";
            }
            return "NA";

        }
        public static string  LanHelper(string lan)
        {
            if (lan != "NA" && lan !=null )
            {
            return $"{(Convert.ToDouble(lan.Substring(3)) / 60) + Convert.ToInt32(lan.Substring(0,3))}";

            }
            return "NA";
        }


    }
}
