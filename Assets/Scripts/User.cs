using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Mono.Data.Sqlite;

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
	string _username;
	string _role;

	// Use this for initialization
	void Awake()
	{
		if (_role == null || _role == "") SetUser(_username);
	}

	// Update is called once per frame
	void Update()
	{

	}

	public void SetUser(string username)
	{
		_username = username;

		using (SqliteCommand command = new SqliteCommand(Database.DB))
		{
			command.CommandText = string.Format("SELECT role FROM users WHERE username = \"{0}\"", username);
			using (SqliteDataReader reader = command.ExecuteReader())
			{
				while (reader.Read()) _role = reader["role"].ToString();
			}
		}
	}
}
