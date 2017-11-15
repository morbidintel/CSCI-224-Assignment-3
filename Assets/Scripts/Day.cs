using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using Gamelogic.Extensions;

public class Day : MonoBehaviour
{
	public HoursEntry hours = new HoursEntry();
	public DateTime date
	{
		get { return hours.date; }
		set { hours.date = value; }
	}
	public Text dateText = null, hoursText = null;
	Image background = null;
	Button button = null;

	public Color origColor, selectedColor;

	// Use this for initialization
	void Awake()
	{
		background = GetComponent<Image>();
		button = GetComponent<Button>();
		origColor = background.color;
	}

	// Update is called once per frame
	void Update()
	{

	}

	void OnDestroy()
	{
		Destroy(gameObject);
	}

	public void MakeTransparent(bool value)
	{
		float newAlpha = value ? 64 / 255f : 1f;
		background.color = background.color.WithAlpha(newAlpha);
		dateText.color = dateText.color.WithAlpha(newAlpha);
		hoursText.color = hoursText.color.WithAlpha(newAlpha);
	}

	public void AddButtonListener(UnityEngine.Events.UnityAction action)
	{
		button.onClick.AddListener(action);
	}

	public void CalculateHours()
	{
		hoursText.text = (hours.time_out - hours.time_in).TotalHours.ToString("0.## hrs");
	}

	public void SetSelected(bool value)
	{
		background.color = value ? selectedColor : origColor;
	}

	public void SetLeaveStatus(ApplyLeave.LeaveStatus? leaveStatus)
	{
		switch (leaveStatus)
		{
			case ApplyLeave.LeaveStatus.Pending:
				origColor = background.color = Color.HSVToRGB(30 / 360f, 32 / 255f, 1);
				break;
			case ApplyLeave.LeaveStatus.Approved:
				origColor = background.color = Color.HSVToRGB(120 / 360f, 32 / 255f, 1);
				break;
			case ApplyLeave.LeaveStatus.Rejected:
				origColor = background.color = Color.HSVToRGB(0, 32 / 255f, 1);
				break;
			case null:
				origColor = background.color = Color.white;
				break;
			default:
				break;
		}
	}
}
