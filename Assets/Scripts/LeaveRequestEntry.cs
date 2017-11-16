using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class LeaveRequestEntry : MonoBehaviour
{
	public Text nameText = null, submitText = null, statusText = null, datesText = null;

	Image background = null;

	public Color origColor = Color.white,
		selectedColor = Color.HSVToRGB(160 / 360f, 32 / 360f, 1f);

	// Use this for initialization
	void Start()
	{
		background = GetComponent<Image>();
	}

	// Update is called once per frame
	void Update()
	{

	}

	public void Init(LeaveRequests.RequestRow request)
	{
		gameObject.SetActive(true);
		nameText.text = request.name;
		submitText.text = Regex.Replace(request.timestamp, "\\d{4} ", "$0\n");
		statusText.text = request.status;
		datesText.text = Regex.Replace(request.dates, "\\d{4}-", "").Replace(" ", ", ");
	}

	public void SetSelected(bool value)
	{
		background.color = value ? selectedColor : origColor;
	}
}
