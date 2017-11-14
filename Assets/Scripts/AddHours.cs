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

	public void OnClickSave()
	{
		if (newTimeIn == "" || newTimeOut == "") // empty string error
		{
			if (newTimeIn == "")
				timeInField.GetComponent<DOTweenAnimation>()?.DOPlay();
			if (newTimeOut == "")
				timeOutField.GetComponent<DOTweenAnimation>()?.DOPlay();
			return;
		}
		else
		{
			DateTime testDT;
			if (!DateTime.TryParseExact( // wrong format error (e.g. 2561, 9999...)
				newTimeIn, "HHmm", null, System.Globalization.DateTimeStyles.None, out testDT))
				timeInField.GetComponent<DOTweenAnimation>()?.DOPlay();
			if (!DateTime.TryParseExact(
				newTimeOut, "HHmm", null, System.Globalization.DateTimeStyles.None, out testDT))
				timeOutField.GetComponent<DOTweenAnimation>()?.DOPlay();
		}

		using (SqliteCommand command = new SqliteCommand(Database.db))
		{
			// UPDATE hours SET time_out = "1730" WHERE hours.username = "george" AND hours.date = "02-11-2017"
			command.CommandText =
				"UPDATE hours SET time_in = \"" + newTimeIn + "\", time_out = \"" + newTimeOut + "\" " +
				"WHERE hours.username = \"" + User.username + "\" AND hours.date = \"" + currentDay.dateText;

			using (SqliteDataReader reader = command.ExecuteReader())
			{

			}
		}
	}

	public void OnClickCancel()
	{
		gameObject.SetActive(false);
	}

	public void ShowHours(Day day)
	{
		currentDay = day;
		gameObject.SetActive(true);
		timeIn.SetActive(true);
		timeOut.SetActive(true);
		buttons.SetActive(true);

		title.text = day.date.ToString("dddd dd MMM yyyy");
		if (day.hours.time_in != null) timeInField.text = day.hours.time_in.ToString("HHmm");
		if (day.hours.time_out != null) timeOutField.text = day.hours.time_out.ToString("HHmm");
	}
}
