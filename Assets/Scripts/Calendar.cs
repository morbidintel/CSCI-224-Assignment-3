using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Mono.Data.Sqlite;

public class Calendar : MonoBehaviour
{
	public class HoursEntry
	{
		public DateTime date, time_in, time_out;
		public string rawDate, rawTimeIn, rawTimeOut;

		public HoursEntry() { }
		public HoursEntry(string rawDate, string rawTimeIn, string rawTimeOut)
		{
			this.rawDate = rawDate; this.rawTimeIn = rawTimeIn; this.rawTimeOut = rawTimeOut;
			date = DateTime.ParseExact(rawDate, "dd-MM-yyyy", null);
			time_in = DateTime.ParseExact(rawDate + " " + rawTimeIn, "dd-MM-yyyy HHmm", null);
			time_out = DateTime.ParseExact(rawDate + " " + rawTimeOut, "dd-MM-yyyy HHmm", null);
		}

		public HoursEntry(DateTime date, DateTime time_in, DateTime time_out)
		{
			this.date = date; this.time_in = time_in; this.time_out = time_out;
			rawDate = date.ToString("dd-MM-yyyy");
			rawTimeIn = time_in.ToString("HHMM");
			rawTimeOut = time_out.ToString("HHMM");
		}
	}

	[SerializeField]
	GameObject dayPrefab = null;
	[SerializeField]
	FlexStatus flexStatusScript = null;
	[SerializeField]
	AddHours addHoursScript = null;

	[SerializeField, ReadOnly]
	static int currentYear, currentMonth;
	public static List<Day> daysShown = new List<Day>();
	static List<HoursEntry> hours = new List<HoursEntry>();

	// Use this for initialization
	void Start()
	{
		if (hours.Count == 0) PopulateHoursFromDB();
		if (daysShown.Count == 0) ShowMonth(DateTime.Now.Year, DateTime.Now.Month);
		else ShowMonth(daysShown);

		flexStatusScript.totalFlexHours = (float)(hours.Sum(he => (he.time_out - he.time_in).TotalHours) - (CountWeekdaysInMonth(currentYear, currentMonth) * 8));
	}

	// Update is called once per frame
	void Update()
	{

	}

	void PopulateHoursFromDB()
	{
		using (SqliteCommand command = new SqliteCommand(Database.db))
		{
			command.CommandText =
				"SELECT date, time_in AS 'in', time_out AS 'out' FROM hours " +
				"WHERE hours.username = \"" + User.username + "\"";

			using (SqliteDataReader reader = command.ExecuteReader())
			{
				while (reader.Read())
				{
					hours.Add(new HoursEntry(reader["date"].ToString(), reader["time_in"].ToString(), reader["time_out"].ToString()));
				}
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
			day.MakeTransparent(true);
			day.AddButtonListener(() => addHoursScript.ShowHours(day));
			day.AddButtonListener(() => ShowMonth(lastMonthsYear, lastMonth));
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
			day.MakeTransparent(false);
			day.AddButtonListener(() => addHoursScript.ShowHours(day));
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
				day.MakeTransparent(true);
				day.AddButtonListener(() => addHoursScript.ShowHours(day));
				day.AddButtonListener(() => ShowMonth(nextMonthsYear, nextMonth));
				daysShown.Add(day);
			}
		}
	}

	public void ShowMonth(List<Day> daysToShow)
	{
		foreach (Day d in daysToShow)
			Instantiate(d.gameObject, transform);
	}

	int CountWeekdaysInMonth(int year, int month)
	{
		return Enumerable.Range(1, DateTime.DaysInMonth(currentYear, currentMonth))
					 .Select(day => new DateTime(currentYear, currentMonth, day))
					 .Count(d => d.DayOfWeek != DayOfWeek.Saturday && d.DayOfWeek != DayOfWeek.Friday);
	}
}
