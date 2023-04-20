using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using System;

[System.Serializable]
public class GameTimestamp 
{
    public int year;

    public enum Season
    {
        // Spring,
        // Summer,
        // Fall,
        // Winter
       Mùa_Xuân,
       Mùa_Hạ,
       Mùa_Thu,
       Mùa_Đông
    }

    public Season season;

    public enum DayOfTheWeek
    {
        // Sunday,
        // Monday,
        // Tuesday,
        // Wednesday,
        // Thursday,
        // Friday,
        // Saturday,
        Chủ_Nhật,
        Thứ_Hai,
        Thứ_Ba,
        Thứ_Tư,
        Thứ_Năm,
        Thứ_Sáu,
        Thứ_Bảy,
    }

    public int day;
    public int hour;
    public int minute;

    public GameTimestamp(int year, Season season, int day, int hour, int minute)
    {
        this.year = year;
        this.season = season;
        this.day = day;
        this.hour = hour;
        this.minute = minute;
    }

    public GameTimestamp(GameTimestamp timestamp)
    {
        this.year = timestamp.year;
        this.season = timestamp.season;
        this.day = timestamp.day;
        this.hour = timestamp.hour;
        this.minute = timestamp.minute;
    }

    public void UpdateClock()
    {
        minute++;

        if(minute >= 60)
        {
            minute = 0;
            hour++;
        }

        if(hour >= 24)
        {
            hour = 0;
            day++;
        }

        if(day > 30)
        {
            day = 1;
            
            if(season == Season.Mùa_Đông)
            {
                season = Season.Mùa_Xuân;

                year++;
            }  else
            {
                season++;
            }
        }
    }

    public DayOfTheWeek GetDayOfTheWeek()
    {
        int dayPassed = YearsToDays(year) + SeasonsToDays(season) + day;

        int dayIndex = dayPassed % 7;

        return (DayOfTheWeek)dayIndex;
    }

    public static int MinutesToSeconds(int minute)
    {
        return minute * 60;
    }

    public static int HoursToMinutes(int hour)
    {
        return hour * 60;
    }

    public static int DaysToHours(int days)
    {
        return days * 24;
    }

    public static int SeasonsToDays(Season season)
    {
        int seasonIndex = (int)season;
        return seasonIndex * 30;
    }

    public static int YearsToDays(int years)
    {
        return years * 4 * 30;
    }

    public static int TimestampInMinutes(GameTimestamp timestamp)
    {
        return HoursToMinutes(DaysToHours(YearsToDays(timestamp.year))
                              + DaysToHours(SeasonsToDays(timestamp.season))
                              + DaysToHours(timestamp.day)
                              + timestamp.hour) + timestamp.minute;
    }

    /// Calculate the difference between 2 timestamps
    public static int CompareTimestamps(GameTimestamp timestamp1, GameTimestamp timestamp2)
    {
        int timestamp1Hours = DaysToHours(YearsToDays(timestamp1.year))
                              + DaysToHours(SeasonsToDays(timestamp1.season))
                              + DaysToHours(timestamp1.day)
                              + timestamp1.hour; 

        int timestamp2Hours = DaysToHours(YearsToDays(timestamp2.year))
                              + DaysToHours(SeasonsToDays(timestamp2.season))
                              + DaysToHours(timestamp2.day)
                              + timestamp2.hour;
        
        int difference = timestamp2Hours - timestamp1Hours; 
        return Mathf.Abs(difference);    
    }
}
