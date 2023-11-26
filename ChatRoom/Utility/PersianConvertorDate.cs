using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;
namespace Utility
{
    public static class PersianConvertorDate
    {
        public static string ToShamsi(this DateTime value)
        {
            PersianCalendar pc = new PersianCalendar();
            return pc.GetYear(value) + "/" + pc.GetMonth(value).ToString("00") + "/" +
                   pc.GetDayOfMonth(value).ToString("00");
        }


        public static DateTime ToMiladi(this string value)
        {

            if (string.IsNullOrWhiteSpace(value) || value.Length < 10)
                return DateTime.Now;
            //۱۴۰۲/۰۶/۱۵
            PersianCalendar pc = new PersianCalendar();
            var date = value.Split("/");
            if (date.Length < 8)
                return DateTime.Now;

            if (int.TryParse(date[0], out int  year) && int.TryParse(date[0], out int month) && int.TryParse(date[0], out int day))
             
            return    pc.ToDateTime(year, month, day, 0, 0, 0, 0);
             
            return DateTime.Now;
        }


    }
}