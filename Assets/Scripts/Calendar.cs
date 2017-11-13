using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Mono.Data.Sqlite;

public class Calendar : MonoBehaviour
{
	[SerializeField]
	GameObject dayPrefab = null;
	[SerializeField]
	FlexStatus flexStatusScript = null;

	[SerializeField, ReadOnly]
	int currentYear, currentMonth;

	public class HoursEntry
	{
		public DateTime date, time_in, time_out;
		public HoursEntry() { }
		public HoursEntry(DateTime date, DateTime time_in, DateTime time_out)
		{
			this.date = date;
			this.time_in = time_in;
			this.time_out = time_out;
		}
	}
	List<HoursEntry> hours = new List<HoursEntry>();

	// Use this for initialization
	void Start()
	{
		PopulateHoursFromDB();
		ShowMonth(DateTime.Now.Year, DateTime.Now.Month);

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
					DateTime date = DateTime.ParseExact(reader["date"].ToString(), "dd-MM-yyyy", null);
					DateTime time_in = DateTime.ParseExact(reader["date"] + " " + reader["in"], "dd-MM-yyyy HHmm", null);
					DateTime time_out = DateTime.ParseExact(reader["date"] + " " + reader["out"], "dd-MM-yyyy HHmm", null);
					hours.Add(new HoursEntry(date, time_in, time_out));
				}
			}
		}
	}

	public void ShowMonth(int year, int month)
	{
		currentYear = year;
		currentMonth = month;
		DateTime firstDay = new DateTime(year, month, 1);
		int daysInMonth = DateTime.DaysInMonth(year, month);
		DateTime lastDay = new DateTime(year, month, daysInMonth);
		int daysInLastMonth = DateTime.DaysInMonth(month > 1 ? year : year - 1, month > 1 ? month - 1 : 12);
		int daysInNextMonth = DateTime.DaysInMonth(month < 12 ? year : year + 1, month < 12 ? month + 1 : 1);

		DayOfWeek dayOfWeek = firstDay.DayOfWeek;

		// instantiate the days before 1st
		for (int date = daysInLastMonth - ((int)dayOfWeek - 1); date < daysInLastMonth + 1; ++date)
		{
			Day day = Instantiate(dayPrefab, transform).GetComponent<Day>();
			day.date.text = date.ToString();

			HoursEntry he = hours.FirstOrDefault(
				h => h.date.Year == (month > 1 ? year : year - 1) &&
				h.date.Month == (month > 1 ? month - 1 : 12) &&
				h.date.Day == date);
			if (he != null)
			{
				day.hours.text = (he.time_out - he.time_in).TotalHours.ToString("0.## hrs");
			}
			else day.hours.text = "";

			day.gameObject.SetActive(true);
			day.MakeTransparent(true);
		}

		// instantiate the month itself
		for (int date = 1; date < daysInMonth + 1; ++date)
		{
			Day day = Instantiate(dayPrefab, transform).GetComponent<Day>();
			day.date.text = date.ToString();

			HoursEntry he = hours.FirstOrDefault(
				h => h.date.Year == year &&
				h.date.Month == month &&
				h.date.Day == date);
			if (he != null)
			{
				day.hours.text = (he.time_out - he.time_in).TotalHours.ToString("0.## hrs");
			}
			else day.hours.text = "";

			day.gameObject.SetActive(true);
			day.MakeTransparent(false);
		}

		// instantiate a few days of next month to fill up calendar
		if (lastDay.DayOfWeek != DayOfWeek.Saturday) // last day of month is not saturday
		{
			for (int date = 1; date < 7 - (int)lastDay.DayOfWeek; ++date)
			{
				Day day = Instantiate(dayPrefab, transform).GetComponent<Day>();
				day.date.text = date.ToString();

				HoursEntry he = hours.FirstOrDefault(
					h => h.date.Year == (month < 12 ? year : year + 1) &&
					h.date.Month == (month < 12 ? month + 1 : 1) &&
					h.date.Day == date);
				if (he != null)
				{
					day.hours.text = (he.time_out - he.time_in).TotalHours.ToString("0.## hrs");
				}
				else day.hours.text = "";

				day.gameObject.SetActive(true);
				day.MakeTransparent(true);
			}
		}
	}

	int CountWeekdaysInMonth(int year, int month)
	{
		return Enumerable.Range(1, DateTime.DaysInMonth(currentYear, currentMonth))
					 .Select(day => new DateTime(currentYear, currentMonth, day))
					 .Count(d => d.DayOfWeek != DayOfWeek.Saturday && d.DayOfWeek != DayOfWeek.Friday);
	}
}
