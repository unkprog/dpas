using System;
using System.Globalization;

namespace dpas.Core.Helpers
{
    public static class TryParse
    {
        public static readonly int MinYear = 1899;
        public static readonly int MaxYear = 2199;
        public static readonly int MinMonth = 1;
        public static readonly int MaxMonth = 12;
        public static readonly int MinDay = 1;
        public static readonly int MaxDay = 31;
        public static readonly DateTime MinDate = new DateTime(MinYear, MaxMonth, MaxDay, 0, 0, 0, 0);
        public static readonly DateTime MaxDate = new DateTime(MaxYear, MaxMonth, MaxDay, 23, 59, 59, 999);
        public static readonly DateTime NilDate = new DateTime(MinYear, MaxMonth, 30, 0, 0, 0, 0);
        public static readonly DateTime NullDate = new DateTime(1900, MaxMonth, MaxDay, 0, 0, 0, 0);

        public static string String(string aValue, string aDefault = "")
        {
            return (string.IsNullOrEmpty(aValue) ? aDefault : aValue);
        }

        public static string String(object aValue, string aDefault = "")
        {
            return String(aValue, aDefault);
        }

        public static double Double(string aValue, double aDefault = 0.0)
        {
            if (string.IsNullOrEmpty(aValue)) return aDefault;
            double result = aDefault;
            string vValue = aValue.Replace(" ", "").Replace("'", "");
            if (!double.TryParse(vValue, out result))
            {
                vValue = vValue.Replace(".", ",");
                if (!double.TryParse(vValue, out result))
                {
                    vValue = vValue.Replace(",", ".");
                    if (!double.TryParse(vValue, out result))
                        result = aDefault;
                }
            }
            return result;
        }

        public static double Double(object aValue, double aDefault = 0.0)
        {
            return Double(aValue, aDefault);
        }

        public static int Int32(string aValue, int aDefault = 0)
        {
            if (string.IsNullOrEmpty(aValue)) return aDefault;
            int result;
            if (!int.TryParse(aValue, out result))
                result = aDefault;
            return result;
        }

        public static int Int32(object aValue, int aDefault = 0)
        {
            return Int32(aValue, aDefault);
        }

        public static bool Bool(string aValue, bool aDefault = false)
        {
            if (string.IsNullOrEmpty(aValue)) return aDefault;
            bool result;
            if (!bool.TryParse(aValue, out result))
            {
                string vValue = aValue.ToLower();
                result = (vValue == "true" || vValue == "истина" || vValue == "1" || vValue == "-1");
            }
            return result;
        }

        public static bool Bool(object aValue, bool aDefault = false)
        {
            return (aValue == null ? aDefault : Bool(aValue.ToString()));
        }

        public static DateTime DateTime(string aValue)
        {
            return DateTime(aValue, MinDate);
        }

        public static DateTime DateTime(string aValue, DateTime aDefault)
        {
            DateTime result = aDefault;
            string vValue = string.IsNullOrEmpty(aValue) ? string.Empty : aValue;
            if (System.DateTime.TryParseExact(vValue, "dd.MM.yyyy HH:mm:ss.fff", null, DateTimeStyles.None, out result))
                return result;

            result = aDefault;
            string[] vTmp = vValue.Split("/.: ".ToCharArray());
            if (vTmp.Length < 3)
                return result;

            int vDay = MaxDay;
            if (!int.TryParse(vTmp[0], out vDay))
                return result;

            int vMonth = MaxMonth;
            if (!int.TryParse(vTmp[1], out vMonth))
                return result;
            int vYear = MinYear;
            if (!int.TryParse(vTmp[2], out vYear))
                return result;

            int vHour = 0;
            if (vTmp.Length > 3)
                int.TryParse(vTmp[3], out vHour);
            int vMinut = 0;
            if (vTmp.Length > 4)
                int.TryParse(vTmp[4], out vMinut);
            int vSecond = 0;
            if (vTmp.Length > 5)
                int.TryParse(vTmp[5], out vSecond);
            int vMilisecond = 0;
            if (vTmp.Length > 6)
                int.TryParse(vTmp[6], out vMilisecond);

            result = new DateTime(vYear, vMonth, vDay, vHour, vMinut, vSecond, vMilisecond);

            return result;
        }

        public static DateTime DateTime(object aValue)
        {
            return DateTime(aValue, MinDate);
        }

        public static DateTime DateTime(object aValue, DateTime aDefault)
        {
            DateTime result = aDefault;
            if (aValue != null)
            {
                // TODO: Восстановить функциональность парсинга значения даты из чисел
                //if (aValue is double) result = System.DateTime..FromOADate((double)aValue);
                //else if (aValue is int) result = System.DateTime.FromOADate((double)(int)aValue);
                //else 
                if (aValue is DateTime) result = (DateTime)aValue;
                else result = DateTime(aValue.ToString(), aDefault);
            }
            return result;
        }

    }
}
