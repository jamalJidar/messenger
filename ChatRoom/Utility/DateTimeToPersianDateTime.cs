using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
namespace Utility
{

    public enum DayOfWeekPersian
    {
        [Display(Name = "یکشنبه")]
        Sunday = 0,
        [Display(Name = "دوشنبه")]
        Monday = 1,
        [Display(Name = "سه شنبه")]
        Tuesday = 2,
        [Display(Name = "چهارشنبه")]
        Wednesday = 3,
        [Display(Name = "پنج شنبه")]
        Thursday = 4,
        [Display(Name = "جمعه")]
        Friday = 5,
        [Display(Name = "شنبه")]
        Saturday = 6
    }
    /*
     ژانویه (January) دارای 31 روز
فوریه (February) دارای 28 یا 29 روز
مارس یا مارچ (March) دارای 31 روز
آوریل یا آپریل (April) دارای 30 روز
مه یا می (May) دارای 31 روز
ژوئن یا جون (June) دارای 31 روز
ژوئیه یا جولای (July) دارای 31 روز
اوت یا آگوست (August) دارای 31 روز
سپتامبر(September) دارای 30 روز
اکتبر (October) دارای 31 روز
نوامبر (November) دارای 30 روز
دسامبر (December) دارای 31 روز
     
     */

    public enum MonthOfYearPersian
    {
        [Display(Name = "فروردین")]
        January = 1,
        [Display(Name = "اردیبهشت")]
        February = 2,
        [Display(Name = "خرداد")]
        March = 3,
        [Display(Name = "تیر")]
        April = 4,
        [Display(Name = "مرداد")]
        May = 5,
        [Display(Name = "شهریور")]
        June = 6,
        [Display(Name = "مهر")]
        July = 7,
        [Display(Name = "آبان")]
        August = 8,
        [Display(Name = "آذر")]
        September = 9,
        [Display(Name = "دی")]
        October = 10,
        [Display(Name = "بهمن")]
        November = 11,
        [Display(Name = "اسفند")]
        December = 12,
    }


    public static class DateTimeToPersianDateTime
    {
        public static PersianCalendar pc = new PersianCalendar();
        public static string ConvertToPersianDate_Full(this DateTime dt) =>
              $"{pc.GetYear(dt)}/{ pc.GetMonth(dt)}/" +
            $"{pc.GetDayOfMonth(dt)}  , ({Environment.NewLine}" +
            $"{pc.GetHour(dt)}:{pc.GetMinute(dt)}:" +
            $"{pc.GetSecond(dt)})";

        public static string ConvertToPersianDate_Full_space(this DateTime dt) =>
             $"{pc.GetYear(dt)}/{ pc.GetMonth(dt)}/" +
           $"{pc.GetDayOfMonth(dt)}      " +
           $"{pc.GetHour(dt)}:{pc.GetMinute(dt)}:" +
           $"{pc.GetSecond(dt)}";




        public static string ConvertToPersianDate_OnlyDate(this DateTime dt) =>
                $"{pc.GetYear(dt)}/{ pc.GetMonth(dt)}/{pc.GetDayOfMonth(dt)}";
        public static string ConvertToPersianDate_OnlyDateFormat(this DateTime dt) =>
                $"{ GetDayNamePersian((int)pc.GetDayOfWeek(dt))} ," +
            $" {pc.GetDayOfMonth(dt)} { GetMonthNamePersian((int)pc.GetMonth(dt))} {pc.GetYear(dt)} ";
        public static string GetDayNamePersian(int x)
        {
            string dayName = string.Empty;

            switch (x)
            {

                case 0:
                    dayName = EnumExtensions.GetDisplayName(DayOfWeekPersian.Sunday);
                    break;

                case 1:
                    dayName = EnumExtensions.GetDisplayName(DayOfWeekPersian.Monday);
                    break;

                case 2:
                    dayName = EnumExtensions.GetDisplayName(DayOfWeekPersian.Tuesday);
                    break;

                case 3:
                    dayName = EnumExtensions.GetDisplayName(DayOfWeekPersian.Wednesday);
                    break;

                case 4:
                    dayName = EnumExtensions.GetDisplayName(DayOfWeekPersian.Thursday);
                    break;

                case 5:
                    dayName = EnumExtensions.GetDisplayName(DayOfWeekPersian.Friday);
                    break;

                case 6:
                    dayName = EnumExtensions.GetDisplayName(DayOfWeekPersian.Saturday);
                    break;


            }

            return dayName;
        }
        public static string GetMonthNamePersian(int x)
        {
            string monthName = string.Empty;

 
       //foreach (MonthOfYearPersian suit in     (MonthOfYearPersian[])Enum.GetValues(typeof(MonthOfYearPersian)))  {  }

            switch (x)
            {
                  case 1:
                    monthName = EnumExtensions.GetDisplayName(MonthOfYearPersian.January);
                    break;

                case 2:
                    monthName = EnumExtensions.GetDisplayName(MonthOfYearPersian.February);
                    break;

                case 3:
                    monthName = EnumExtensions.GetDisplayName(MonthOfYearPersian.March);
                    break;

                case 4:
                    monthName = EnumExtensions.GetDisplayName(MonthOfYearPersian.April);
                    break;

                case 5:
                    monthName = EnumExtensions.GetDisplayName(MonthOfYearPersian.May);
                    break;

                case 6:
                    monthName = EnumExtensions.GetDisplayName(MonthOfYearPersian.June);
                    break;


                case 7:
                    monthName = EnumExtensions.GetDisplayName(MonthOfYearPersian.July);
                    break;

                case 8:
                    monthName = EnumExtensions.GetDisplayName(MonthOfYearPersian.August);
                    break;

                case 9:
                    monthName = EnumExtensions.GetDisplayName(MonthOfYearPersian.September);
                    break;

                case 10:
                    monthName = EnumExtensions.GetDisplayName(MonthOfYearPersian.October);
                    break;

                case 11:
                    monthName = EnumExtensions.GetDisplayName(MonthOfYearPersian.November);
                    break;

                case 12:
                    monthName = EnumExtensions.GetDisplayName(MonthOfYearPersian.December);
                    break;


            }

            return monthName;
        }

    }
}
