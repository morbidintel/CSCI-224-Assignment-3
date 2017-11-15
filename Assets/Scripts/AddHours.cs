using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Mono.Data.Sqlite;

public class AddHours : MonoBehaviour
{
	[SerializeField]
	Calendar calendar = null;

	[SerializeField]
	Text title = null, totalHours = null;
	[SerializeField]
	InputField timeInField = null, timeOutField = null;

	[SerializeField]
	GameObject timeIn = null, timeOut = null, buttons = null;

	public string newTimeIn { get; set; }
	public string newTimeOut { get; set; }

	Day currentDay = null;

	// Use this for initialization
	void Awake()
	{
		timeIn.SetActive(false);
		timeOut.SetActive(false);
		buttons.SetActive(false);

		title.text = "Choose Date";
	}

	// Update is called once per frame
	void Update()
	{

	}

	void OnDisable()
	{
		ShowHours(null);
		gameObject.SetActive(false);
	}

	public void OnClickSave()
	{
		DOTweenAnimation timeInAnim = timeInField.GetComponent<DOTweenAnimation>(),
			timeOutAnim = timeOutField.GetComponent<DOTweenAnimation>();
		bool hasError = false;

		TimeSpan testIn, testOut;
		if (!TimeSpan.TryParseExact( // wrong format error (e.g. 2561, 9999...)
			newTimeIn, "hhmm", null, System.Globalization.TimeSpanStyles.None, out testIn) || // different format from DateTime
			currentDay.hours.rawTimeIn != newTimeIn && testIn > testOut ||
			newTimeIn == "")
		{
			timeInAnim?.DORestart();
			hasError = true;
		}

		if (!TimeSpan.TryParseExact( // wrong format error (e.g. 2561, 9999...)
			newTimeOut, "hhmm", null, System.Globalization.TimeSpanStyles.None, out testOut) || // different format from DateTime
			currentDay.hours.rawTimeOut != newTimeOut && testOut < testIn ||
			newTimeOut == "")
		{
			timeOutAnim?.DORestart();
			hasError = true;
		}

		if (hasError) return;

		currentDay.hours.rawTimeIn = newTimeIn;
		currentDay.hours.rawTimeOut = newTimeOut;
		currentDay.hours.time_in = currentDay.hours.date + testIn;
		currentDay.hours.time_out = currentDay.hours.date + testOut;
		currentDay.CalculateHours();
		FlexStatus.Instance.CalculateFlexHours();

		using (SqliteCommand command = new SqliteCommand(Database.db))
		{
			command.CommandText = string.Format(
				"UPDATE hours SET time_in = \"{2}\", time_out = \"{3}\" WHERE username = \"{0}\" AND date = \"{1}\";" +
				"INSERT INTO hours (username, date, time_in, time_out) SELECT \"{0}\", \"{1}\", \"{2}\", \"{3}\" WHERE (SELECT changes() = 0);",
				//"INSERT OR REPLACE INTO hours (username, date, time_in, time_out) VALUES (\"{0}\", \"{1}\", \"{2}\", \"{3}\")",
				User.username, currentDay.hours.date.ToString("yyyy-MM-dd"), newTimeIn, newTimeOut);

			command.ExecuteNonQuery();
		}

		foreach (var anim in totalHours.GetComponents<DOTweenAnimation>()) anim.DORestart();
	}

	public void OnClickCancel()
	{
		gameObject.SetActive(false);
	}

	public void ShowHours(Day day)
	{
		currentDay = day;
		gameObject.SetActive(day != null);
		timeIn.SetActive(day != null);
		timeOut.SetActive(day != null);
		buttons.SetActive(day != null);

		if (day != null)
		{
			title.text = day.date.ToString("dddd dd MMM yyyy");
			timeInField.text = day.hours.time_in != DateTime.MinValue ? day.hours.time_in.ToString("HHmm") : "";
			timeOutField.text = day.hours.time_out != DateTime.MinValue ? day.hours.time_out.ToString("HHmm") : "";
		}
	}
}
