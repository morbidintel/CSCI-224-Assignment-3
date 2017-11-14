using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using Gamelogic.Extensions;

public class Day : MonoBehaviour
{
	public DateTime date;
	public Text dateText = null, hoursText = null;
	Image background = null;
	Button button = null;

	// Use this for initialization
	void Awake()
	{
		background = GetComponent<Image>();
		button = GetComponent<Button>();
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
}
