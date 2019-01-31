using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public enum NightShiftType
    {
        Day = 1,
        Night = 2,
    }
    public static class ConstVar
    {
        //CA001 - CA010     0：夜班 1：白班
        public static int[] NightShift = new int[12] {
                (int)NightShiftType.Night, // 1月份 -0
                (int)NightShiftType.Day, // 2月份 -1
                (int)NightShiftType.Night, // 3月份
                (int)NightShiftType.Day, // 4月份
                (int)NightShiftType.Night, // 5月份
                (int)NightShiftType.Day, // 6月份
                (int)NightShiftType.Night, // 7月份
                (int)NightShiftType.Day, // 8月份
                (int)NightShiftType.Night, // 9月份
                (int)NightShiftType.Day, // 10月份
                (int)NightShiftType.Night, // 11月份 
                (int)NightShiftType.Day, // 12月份
        };

        //CA011 - CA020
        public static int[] DayShift = new int[12] {
                (int)NightShiftType.Day, // 1月份
                (int)NightShiftType.Night, // 2月份
                (int)NightShiftType.Day, // 3月份
                (int)NightShiftType.Night, // 4月份
                (int)NightShiftType.Day, // 5月份
                (int)NightShiftType.Night, // 6月份
                (int)NightShiftType.Day, // 7月份
                (int)NightShiftType.Night, // 8月份
                (int)NightShiftType.Day, // 9月份
                (int)NightShiftType.Night, // 10月份
                (int)NightShiftType.Day, // 11月份 
                (int)NightShiftType.Night, // 12月份
        };

        //CA021
        public static int[] DayShift_21 = new int[12] {
                (int)NightShiftType.Day, // 1月份
                (int)NightShiftType.Night, // 2月份
                (int)NightShiftType.Day, // 3月份
                (int)NightShiftType.Night, // 4月份
                (int)NightShiftType.Day, // 5月份
                (int)NightShiftType.Night, // 6月份
                (int)NightShiftType.Day, // 7月份
                (int)NightShiftType.Night, // 8月份
                (int)NightShiftType.Day, // 9月份
                (int)NightShiftType.Night, // 10月份
                (int)NightShiftType.Day, // 11月份 
                (int)NightShiftType.Night, // 12月份
        };


        //CA024
        public static int[] DayShift_24 = new int[12] {
               (int)NightShiftType.Night, // 1月份
                (int)NightShiftType.Day, // 2月份
                (int)NightShiftType.Night, // 3月份
                (int)NightShiftType.Day, // 4月份
                (int)NightShiftType.Night, // 5月份
                (int)NightShiftType.Day, // 6月份
                (int)NightShiftType.Night, // 7月份
                (int)NightShiftType.Day, // 8月份
                (int)NightShiftType.Night, // 9月份
                (int)NightShiftType.Day, // 10月份
                (int)NightShiftType.Night, // 11月份 
                (int)NightShiftType.Day, // 12月份
        };

    }
}
