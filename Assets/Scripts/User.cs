using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User : Gamelogic.Extensions.Singleton<User>
{
	public static string username { get { return Instance._username; } }
	public static string role { get { return Instance._role; } }
	[SerializeField]
	string _username, _role;

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	public void SetUser(string username, string role)
	{
		_username = username;
		_role = role;
	}
}
