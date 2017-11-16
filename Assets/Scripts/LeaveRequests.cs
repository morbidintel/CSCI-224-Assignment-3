using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mono.Data.Sqlite;

public class LeaveRequests : MonoBehaviour
{
	[SerializeField]
	Transform content = null;
	[SerializeField]
	GameObject entryPrefab = null;

	public struct RequestRow { public string name, timestamp, status, dates; }
	public List<LeaveRequestEntry> entries = new List<LeaveRequestEntry>();

	// Use this for initialization
	void Start()
	{
		entries.Clear();

		using (SqliteCommand command = new SqliteCommand(Database.db))
		{
			command.CommandText =
				string.Format("SELECT " +
					"(SELECT full_name FROM users WHERE users.username = leaves.username) AS name, " +
					"timestamp, status, dates " +
					"FROM leaves WHERE approving = \"{0}\"", User.username);

			using (SqliteDataReader reader = command.ExecuteReader())
			{
				while (reader.Read())
				{
					RequestRow row = new RequestRow();
					row.name = reader["name"].ToString();
					row.timestamp = reader["timestamp"].ToString();
					row.status = reader["status"].ToString();
					row.dates = reader["dates"].ToString();

					var entry = Instantiate(entryPrefab, content).GetComponent<LeaveRequestEntry>();
					entry.Init(row);
					entries.Add(entry);
				}
			}
		}
	}

	// Update is called once per frame
	void Update()
	{

	}

	public void RebuildList()
	{
		foreach (var e in entries) Destroy(e.gameObject);
		Start();
	}
}
