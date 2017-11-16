using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Mono.Data.Sqlite;

public class UserList : MonoBehaviour
{
	[SerializeField]
	GameObject entryPrefab = null;

	List<UserListEntry> entries = new List<UserListEntry>();

	// Use this for initialization
	void Start()
	{
		using (SqliteCommand command = new SqliteCommand(Database.DB))
		{
			command.CommandText = "SELECT * FROM users";

			using (SqliteDataReader reader = command.ExecuteReader())
			{
				while (reader.Read())
				{
					UserListEntry entry = Instantiate(entryPrefab, transform).GetComponent<UserListEntry>();
					entry.gameObject.SetActive(true);
					entry.Init(reader["username"].ToString(), reader["role"].ToString(), reader["full_name"].ToString());

					entries.Add(entry);
				}
			}
		}
	}

	// Update is called once per frame
	void Update()
	{

	}

	public void AddUser()
	{
		UserListEntry entry = Instantiate(entryPrefab, transform).GetComponent<UserListEntry>();
		entry.gameObject.SetActive(true);
		entry.Init("username" + entries.Count(e => e.username.Contains("username")), "Employee", "John Appleseed");
		entries.Add(entry);

		using (SqliteCommand command = new SqliteCommand(Database.DB))
		{
			command.CommandText = string.Format("INSERT INTO users (username) VALUES (\"{0}\")", entry.username);
			command.ExecuteNonQuery();
		}
	}
}
