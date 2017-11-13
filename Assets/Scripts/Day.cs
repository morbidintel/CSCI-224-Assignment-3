using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Gamelogic.Extensions;

public class Day : MonoBehaviour
{
	public Text date = null, hours = null;
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

	public void MakeTransparent(bool value)
	{
		float newAlpha = value ? 64 / 255f : 1f;
		background.color = background.color.WithAlpha(newAlpha);
		date.color = date.color.WithAlpha(newAlpha);
		hours.color = hours.color.WithAlpha(newAlpha);
	}
}
