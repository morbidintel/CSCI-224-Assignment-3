using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Mono.Data.Sqlite;

public class LeaveCalendar : MonoBehaviour
{
	[SerializeField]
	protected GameObject dayPrefab = null;

	[SerializeField, ReadOnly]
	public static int currentYear, currentMonth;
	public static List<Day> daysShown = new List<Day>();
	public static List<HoursEntry> hours = new List<HoursEntry>();

	public List<Day> daysSelected = new List<Day>();
	public Dictionary<DateTime, ApplyLeave.LeaveStatus> datesWithLeaves = new Dictionary<DateTime, ApplyLeave.LeaveStatus>();

	// Use this for initialization
	protected void Start()
	{
		if (hours.Count == 0) PopulateHoursFromDB();
		if (daysShown.Count == 0) ShowMonth(DateTime.Now.Year, DateTime.Now.Month);
		else ShowMonth(daysShown);
	}

	// Update is called once per frame
	void Update()
	{
	}

	void OnDestroy()
	{
		foreach (var d in daysShown) if (d != null) Destroy(d.gameObject);
		daysShown.Clear();
		hours.Clear();
	}

	void PopulateHoursFromDB()
	{
		using (SqliteCommand command = new SqliteCommand(Database.DB))
		{
			command.CommandText =
				string.Format("SELECT date, time_in, time_out FROM hours WHERE hours.username = \"{0}\"", User.username);

			using (SqliteDataReader reader = command.ExecuteReader())
			{
				while (reader.Read()) hours.Add(new HoursEntry(
					reader["date"].ToString(), reader["time_in"].ToString(), reader["time_out"].ToString()));
			}
		}
	}

	public void ShowMonth(int year, int month)
	{
		foreach (Day d in daysShown) Destroy(d.gameObject);
		daysShown.Clear();

		currentYear = year;
		currentMonth = month;
		int lastMonthsYear = month > 1 ? year : year - 1;
		int lastMonth = month > 1 ? month - 1 : 12;
		int nextMonthsYear = month < 12 ? year : year + 1;
		int nextMonth = month < 12 ? month + 1 : 1;
		DateTime firstDay = new DateTime(year, month, 1);
		int daysInMonth = DateTime.DaysInMonth(year, month);
		DateTime lastDay = new DateTime(year, month, daysInMonth);
		int daysInLastMonth = DateTime.DaysInMonth(lastMonthsYear, lastMonth);
		int daysInNextMonth = DateTime.DaysInMonth(nextMonthsYear, nextMonth);

		DayOfWeek dayOfWeek = firstDay.DayOfWeek;

		// instantiate the days before 1st
		for (int date = daysInLastMonth - ((int)dayOfWeek - 1); date < daysInLastMonth + 1; ++date)
		{
			Day day = Instantiate(dayPrefab, transform).GetComponent<Day>();
			day.dateText.text = date.ToString();

			HoursEntry he = hours.FirstOrDefault(
				h => h.date.Year == (lastMonthsYear) &&
				h.date.Month == (lastMonth) &&
				h.date.Day == date);
			if (he != null)
			{
				day.hours = he;
				day.hoursText.text = (he.time_out - he.time_in).TotalHours.ToString("0.## hrs");
			}
			else
			{
				day.date = new DateTime(lastMonthsYear, lastMonth, date);
				day.hoursText.text = "";
			}

			day.gameObject.SetActive(true);
			if (datesWithLeaves.ContainsKey(day.date)) day.SetLeaveStatus(datesWithLeaves[day.date]);
			else day.MakeTransparent(true);
			daysShown.Add(day);
		}

		// instantiate the month itself
		for (int date = 1; date < daysInMonth + 1; ++date)
		{
			Day day = Instantiate(dayPrefab, transform).GetComponent<Day>();
			day.dateText.text = date.ToString();

			HoursEntry he = hours.FirstOrDefault(
				h => h.date.Year == year &&
				h.date.Month == month &&
				h.date.Day == date);
			if (he != null)
			{
				day.hours = he;
				day.hoursText.text = (he.time_out - he.time_in).TotalHours.ToString("0.## hrs");
			}
			else
			{
				day.date = new DateTime(year, month, date);
				day.hoursText.text = "";
			}

			day.gameObject.SetActive(true);
			if (datesWithLeaves.ContainsKey(day.date)) day.SetLeaveStatus(datesWithLeaves[day.date]);
			else day.MakeTransparent(false);
			daysShown.Add(day);
		}

		// instantiate a few days of next month to fill up calendar
		if (lastDay.DayOfWeek != DayOfWeek.Saturday) // last day of month is not saturday
		{
			for (int date = 1; date < 7 - (int)lastDay.DayOfWeek; ++date)
			{
				Day day = Instantiate(dayPrefab, transform).GetComponent<Day>();
				day.dateText.text = date.ToString();

				HoursEntry he = hours.FirstOrDefault(
					h => h.date.Year == (nextMonthsYear) &&
					h.date.Month == (nextMonth) &&
					h.date.Day == date);
				if (he != null)
				{
					day.hours = he;
					day.hoursText.text = (he.time_out - he.time_in).TotalHours.ToString("0.## hrs");
				}
				else
				{
					day.date = new DateTime(nextMonthsYear, nextMonth, date);
					day.hoursText.text = "";
				}

				day.gameObject.SetActive(true);
				if (datesWithLeaves.ContainsKey(day.date)) day.SetLeaveStatus(datesWithLeaves[day.date]);
				else day.MakeTransparent(true);
				daysShown.Add(day);
			}
		}
	}

	public void ShowMonth(List<Day> daysToShow)
	{
		foreach (Day d in daysToShow)
			Instantiate(d.gameObject, transform);
	}
}
