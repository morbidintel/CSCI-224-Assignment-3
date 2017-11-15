using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Mono.Data.Sqlite;

public class ApplyLeave : MonoBehaviour
{
	public enum LeaveStatus
	{
		Pending, Approved, Rejected
	}

	[SerializeField]
	LeaveCalendar calendar = null;
	[SerializeField]
	Text days = null;
	[SerializeField]
	GameObject buttonsParent = null;
	[SerializeField]
	Dropdown managersDropdown = null;

	List<Tuple<List<DateTime>, LeaveStatus>> leaves = new List<Tuple<List<DateTime>, LeaveStatus>>();

	// Use this for initialization
	void Start()
	{
		days.gameObject.SetActive(false);
		buttonsParent.SetActive(false);
		managersDropdown.transform.parent.gameObject.SetActive(false);

		using (SqliteCommand command = new SqliteCommand(Database.db))
		{
			// populate managers dropdown
			command.CommandText = "SELECT full_name FROM users WHERE role = \"Manager\" ORDER BY full_name ASC";

			using (SqliteDataReader reader = command.ExecuteReader())
			{
				List<string> managers = new List<string>();
				while (reader.Read()) managers.Add(reader["full_name"].ToString());
				managersDropdown.AddOptions(managers);
			}

			command.CommandText = string.Format("SELECT dates, status FROM leaves WHERE username = \"{0}\"", User.username);

			using (SqliteDataReader reader = command.ExecuteReader())
			{
				while (reader.Read())
				{
					List<DateTime> dates = new List<DateTime>();
					foreach (string d in reader["dates"].ToString().Split(' '))
						dates.Add(DateTime.ParseExact(d, "yyyy-MM-dd", null));
					leaves.Add(new Tuple<List<DateTime>, LeaveStatus>(dates, (LeaveStatus)Enum.Parse(typeof(LeaveStatus), reader["status"].ToString())));
				}
			}
		}

		foreach (var leave in leaves)
		{
			foreach (var date in leave.Item1)
			{
				calendar.datesWithLeaves.Add(date, leave.Item2);
			}
		}
	}

	// Update is called once per frame
	void Update()
	{
		if (calendar.daysSelected.Count > 0)
		{
			days.gameObject.SetActive(true);
			buttonsParent.SetActive(true);
			managersDropdown.transform.parent.gameObject.SetActive(true);
			days.text = calendar.daysSelected.Count.ToString() + " days";
		}
		else
		{
			days.gameObject.SetActive(false);
			buttonsParent.SetActive(false);
			managersDropdown.transform.parent.gameObject.SetActive(false);
		}
	}

	public void OnClickSubmit()
	{
		string dates = "";

		foreach (Day day in calendar.daysSelected)
		{
			dates += day.date.ToString("yyyy-MM-dd ");
		}
		dates = dates.TrimEnd(' ');

		using (SqliteCommand command = new SqliteCommand(Database.db))
		{
			command.CommandText = string.Format(
				"UPDATE leaves " +
				"SET approving = (SELECT username FROM users WHERE full_name = \"{1}\"), timestamp = \"{2}\", status = \"{3}\", dates = \"{4}\" " +
				"WHERE username = \"{0}\" AND status = \"Pending\"; " +
				"INSERT INTO leaves (username, approving, timestamp, status, dates) " +
				"SELECT \"{0}\", (SELECT username FROM users WHERE full_name = \"{1}\"), \"{2}\", \"{3}\", \"{4}\" " +
				"WHERE (SELECT changes() = 0);",
				User.username, // 0
				managersDropdown.options[managersDropdown.value].text, // 1
				DateTime.Now, // 2
				"Pending", // 3
				dates); // 4

			command.ExecuteNonQuery();
		}
	}

	public void OnClickCancel()
	{
		foreach (Day d in calendar.daysSelected) d.SetSelected(false);
		calendar.daysSelected.Clear();
	}

	public void OnClickDay(Day day)
	{
		if (!calendar.daysSelected.Contains(day))
		{
			if (calendar.datesWithLeaves.ContainsKey(day.date))
			{
				var leave = leaves.Find(l => l.Item1.Contains(day.date));
				if (leave != null)
					foreach (var date in leave.Item1)
					{
						Day d = LeaveCalendar.daysShown.Find(dd => dd.date == date);
						if (d != null) calendar.daysSelected.Add(d);
						d?.SetSelected(true);
					}
			}
			else
			{
				calendar.daysSelected.Add(day);
				day.SetSelected(true);
			}
		}
		else
		{
			if (calendar.datesWithLeaves.ContainsKey(day.date))
			{
				var leave = leaves.Find(l => l.Item1.Contains(day.date));
				if (leave != null)
					foreach (var date in leave.Item1)
					{
						Day d = LeaveCalendar.daysShown.Find(dd => dd.date == date);
						if (d != null) calendar.daysSelected.Remove(d);
						d?.SetSelected(false);
					}
			}
			else
			{
				calendar.daysSelected.Remove(day);
				day.SetSelected(false);
			}
		}

		if (day.date < new DateTime(LeaveCalendar.currentYear, LeaveCalendar.currentMonth, 1))
		{
			DateTime prev = day.date.AddMonths(-1);
			calendar.ShowMonth(prev.Year, prev.Month);
		}
		if (day.date > new DateTime(LeaveCalendar.currentYear, LeaveCalendar.currentMonth, 1).AddMonths(1).AddDays(-1))
		{
			DateTime next = day.date.AddMonths(1);
			calendar.ShowMonth(next.Year, next.Month);
		}
	}
}
