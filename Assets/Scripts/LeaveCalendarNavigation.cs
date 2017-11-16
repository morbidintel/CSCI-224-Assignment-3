using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class LeaveCalendarNavigation : MonoBehaviour
{
	[SerializeField]
	LeaveCalendar calendar = null;
	[SerializeField]
	Text prevMonth = null, nextMonth = null, currMonth = null;

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	public void UpdateNavigation()
	{
		DateTime current = new DateTime(LeaveCalendar.currentYear, LeaveCalendar.currentMonth, 1);
		currMonth.text = current.ToString("MMMM");
		prevMonth.text = current.AddMonths(-1).ToString("MMMM");
		nextMonth.text = current.AddMonths(1).ToString("MMMM");
	}

	public void OnClickPrevMonth()
	{
		DateTime previous = new DateTime(LeaveCalendar.currentYear, LeaveCalendar.currentMonth, 1).AddMonths(-1);
		calendar.ShowMonth(previous.Year, previous.Month);
		UpdateNavigation();
	}

	public void OnClickNextMonth()
	{
		DateTime next = new DateTime(LeaveCalendar.currentYear, LeaveCalendar.currentMonth, 1).AddMonths(1);
		calendar.ShowMonth(next.Year, next.Month);
		UpdateNavigation();
	}
}
