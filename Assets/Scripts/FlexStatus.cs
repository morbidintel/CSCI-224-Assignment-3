using System.Collections;
using System.Collections.Generic;
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
}
