using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using System.IO;

public class Database : Gamelogic.Extensions.Singleton<Database>
{
	SqliteConnection _db = null;
	public static SqliteConnection db { get { return Instance._db; } }

	// Use this for initialization
	void Awake()
	{
		DontDestroyOnLoad(gameObject);

		if (!File.Exists("db.sqlite")) Debug.LogError("DB not found!");
		_db = new SqliteConnection("Data Source=db.sqlite; Version=3");
		_db.Open();
	}
}
