using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Mono.Data.Sqlite;

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

	void Start()
	{
		DontDestroyOnLoad(gameObject);

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
			using (SqliteCommand command = new SqliteCommand(Database.db))
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
			SceneManager.LoadSceneAsync("main");
			SceneManager.sceneLoaded += OnSceneLoaded;
		}
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		User.Instance.SetUser(Username, Role);
		Destroy(gameObject);
	}
}
