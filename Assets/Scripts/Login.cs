using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Mono.Data.Sqlite;
using System.IO;

public class Login : MonoBehaviour
{
	[SerializeField]
	Text loginErrorText = null, loadingText = null;
	string defaultError = "Incorrect username or password.";
	string noUsernameError = "Please enter username.";
	string noPasswordError = "Please enter password.";

	public string Username { get; set; }
	public string Password { get; set; }
	public string Role;

	SqliteConnection db = null;

	void Awake()
	{
		DontDestroyOnLoad(gameObject);

		if (!File.Exists("db.sqlite")) Debug.LogError("DB not found!");
		db = new SqliteConnection("Data Source=db.sqlite; Version=3");
		db.Open();

		loginErrorText.gameObject.SetActive(false);
		loadingText.gameObject.SetActive(false);
	}

	void Update()
	{
	}

	public void OnClickLogin()
	{
		loginErrorText.gameObject.SetActive(true);
		loginErrorText.text = defaultError;

		if (Username == null || Username == "")
		{
			loginErrorText.text = noUsernameError;
			return;
		}
		else if (Password == null || Password == "")
		{
			loginErrorText.text = noPasswordError;
			return;
		}

		try
		{
			using (SqliteCommand command = new SqliteCommand(db))
			{
				command.CommandText =
					"SELECT role FROM users " +
					"WHERE users.username = \"" + Username + "\" " +
					"AND users.password = \"" + Password + "\"";

				using (SqliteDataReader reader = command.ExecuteReader())
				{
					while (reader.Read())
						Role = reader["role"].ToString();
				}
			}
		}
		catch
		{
		}

		if (Role != "")
		{
			loginErrorText.gameObject.SetActive(false);
			loginErrorText.transform.parent.gameObject.SetActive(false);
			loadingText.gameObject.SetActive(true);
			UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("main");
		}
	}
}
