using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class LeaveRequestEntry : MonoBehaviour
{
	[SerializeField]
	Text nameText = null, submitText = null, statusText = null, datesText = null;

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	public void Init(LeaveRequests.RequestRow request)
	{
		gameObject.SetActive(true);
		nameText.text = request.name;
		submitText.text = Regex.Replace(request.timestamp, "\\d{4} ", "$1\n");
		statusText.text = request.status;
		datesText.text = Regex.Replace(request.dates, "\\d{4}-", "").Replace(" ", ", ");
	}
}
