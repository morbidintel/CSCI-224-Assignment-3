using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class FlexStatus : Gamelogic.Extensions.Singleton<FlexStatus>
{
	[SerializeField]
	Text titleText = null, flexHoursText = null;

	public float totalFlexHours
	{
		get { return _totalFlexHours; }
		set
		{
			_totalFlexHours = value;
			flexHoursText.text = _totalFlexHours.ToString("#.## hours");
			flexHoursText.color = value < 0 ? Color.red : Color.black;
		}
	}
	float _totalFlexHours = 0;

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	public void CalculateFlexHours()
	{
		totalFlexHours = (float)(Calendar.hours.Sum(he => (he.time_out - he.time_in).TotalHours) - (CountWeekdaysInMonth(Calendar.currentYear, Calendar.currentMonth) * 8));
	}

	int CountWeekdaysInMonth(int year, int month)
	{
		return Enumerable.Range(1, DateTime.DaysInMonth(year, month))
					 .Select(day => new DateTime(year, month, day))
					 .Count(d => d.DayOfWeek != DayOfWeek.Saturday && d.DayOfWeek != DayOfWeek.Friday);
	}
}
