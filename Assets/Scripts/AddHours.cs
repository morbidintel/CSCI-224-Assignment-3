using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddHours : MonoBehaviour
{
	[SerializeField]
	Calendar calendar = null;

	[SerializeField]
	Text title = null, totalHours = null;

	[SerializeField]
	GameObject timeIn = null, timeOut = null, buttons = null;

	public string newTimeIn { get; set; }
	public string newTimeOut { get; set; }

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

	}

	public void OnClickCancel()
	{

	}

	public void ShowHours(Day day)
	{
		gameObject.SetActive(true);
		timeIn.SetActive(true);
		timeOut.SetActive(true);
		buttons.SetActive(true);

		title.text = day.date.ToString("dddd dd MMM yyyy");
	}
}
