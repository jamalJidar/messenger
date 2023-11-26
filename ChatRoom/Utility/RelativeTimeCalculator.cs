using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Utility
{
    public static class RelativeTimeCalculator
    {
         

        public static string Calculate(DateTime? dt)
        {
            if (dt == null)
                return string.Empty;
            var dateTime = SetKind( dt.Value , DateTimeKind.Local);

            var ts = (SetKind(DateTime.Now, DateTimeKind.Local) - dateTime);

            if (ts.Days>0 && ts.Days<6)
            {
                return  ts.Days   + " روز گذشته";
            }
            if (ts.Hours <= 23 && ts.Hours>1)
            {
                return ts.Hours + " ساعت قبل";
            }
            if (ts.Minutes <= 59 && ts.Minutes>0)
            {
                return $"{ts.Minutes} دقیقه قبل";
            }
            if (ts.TotalSeconds <=59)
            {
                return ts.Seconds < 30 ? "لحظه ای قبل" : ts.Seconds + " ثانیه قبل";
            }
            
            
             
            
            else  
               return DateTimeToPersianDateTime.ConvertToPersianDate_Full_space(dateTime);

            //if (delta < 30 * DAY)
            //{
            //    return ts.Days + " روز قبل";
            //}


            //if (delta < 12 * MONTH)
            //{
            //    int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
            //    return months <= 1 ? "یک ماه قبل" : months + " ماه قبل";
            //}
            //int years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
            //return years <= 1 ? "یک سال قبل" : years + " سال قبل";
        }
		public static DateTime SetKind(this DateTime DT, DateTimeKind DTKind)
		{
			return DateTime.SpecifyKind(DT, DTKind);
		}
		 
	}
}