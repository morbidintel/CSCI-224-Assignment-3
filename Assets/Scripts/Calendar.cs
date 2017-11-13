using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using Mono.Data.Sqlite;

public class Calendar : MonoBehaviour
{
	[SerializeField]
	GameObject dayPrefab;

	[SerializeField, ReadOnly]
	int currentYear, currentMonth;

	// Use this for initialization
	void Start()
	{
		ShowMonth(DateTime.Now.Year, DateTime.Now.Month);
	}

	// Update is called once per frame
	void Update()
	{

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

		using (SqliteCommand command = new SqliteCommand(Database.db))
		{
			command.CommandText =
				"SELECT date, time_in AS 'in', time_out AS 'out' FROM hours " +
				"WHERE hours.username = \"" + User.username + "\" " +
				"AND hours.date LIKE \"%-" + (month > 1 ? month - 1 : 12) + "-" + (month > 1 ? year : year - 1) + "\"";

			using (SqliteDataReader reader = command.ExecuteReader())
			{
				// instantiate the days before 1st
				for (int date = daysInLastMonth - ((int)dayOfWeek - 1); date < daysInLastMonth + 1; ++date)
				{
					Day day = Instantiate(dayPrefab, transform).GetComponent<Day>();
					day.date.text = date.ToString();

					if (reader.Read())
					{
						DateTime time_in = DateTime.ParseExact(reader["date"] + " " + reader["in"], "dd-MM-yyyy HHmm", null);
						DateTime time_out = DateTime.ParseExact(reader["date"] + " " + reader["out"], "dd-MM-yyyy HHmm", null);
						day.hours.text = (time_out - time_in).TotalHours.ToString();
					}

					day.gameObject.SetActive(true);
					day.MakeTransparent(true);
				}
			}
		}

		using (SqliteCommand command = new SqliteCommand(Database.db))
		{
			command.CommandText =
				"SELECT date, time_in AS 'in', time_out AS 'out' FROM hours " +
				"WHERE hours.username = \"" + User.username + "\" " +
				"AND hours.date LIKE \"%-" + month + "-" + year + "\"";

			using (SqliteDataReader reader = command.ExecuteReader())
			{
				// instantiate the month itself
				for (int date = 1; date < daysInMonth + 1; ++date)
				{
					Day day = Instantiate(dayPrefab, transform).GetComponent<Day>();
					day.date.text = date.ToString();

					if (reader.Read())
					{
						DateTime time_in = DateTime.ParseExact(reader["date"] + " " + reader["in"], "dd-MM-yyyy HHmm", null);
						DateTime time_out = DateTime.ParseExact(reader["date"] + " " + reader["out"], "dd-MM-yyyy HHmm", null);
						day.hours.text = (time_out - time_in).TotalHours.ToString(day.hours.text);
					}

					day.gameObject.SetActive(true);
					day.MakeTransparent(false);
				}
			}
		}


		// instantiate a few days of next month to fill up calendar
		if (lastDay.DayOfWeek != DayOfWeek.Saturday) // last day of month is not saturday
		{
			for (int date = 1; date < 7 - (int)lastDay.DayOfWeek; ++date)
			{
				Day day = Instantiate(dayPrefab, transform).GetComponent<Day>();
				day.date.text = date.ToString();
				day.gameObject.SetActive(true);
				day.MakeTransparent(true);
			}
		}
	}
}
