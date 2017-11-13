using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class User : Gamelogic.Extensions.Singleton<User>
{
	public enum Role
	{
		Employee, HR, Manager, Admin
	}
	public static string[] Roles = { "Employee", "HR", "Manager", "Admin" };

	public static string username { get { return Instance._username; } }
	public static Role role { get { return (Role)Array.IndexOf(Roles, Instance._role); } }
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
