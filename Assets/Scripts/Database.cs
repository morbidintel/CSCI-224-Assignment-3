using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using System.IO;

public class Database
{
	public static SqliteConnection _db = null;
	public static SqliteConnection DB
	{
		get
		{
			if (_db == null)
			{
				if (!File.Exists("db.sqlite")) Debug.LogError("DB not found!");
				_db = new SqliteConnection("Data Source=db.sqlite; Version=3");
				_db.Open();
			}
			return _db;
		}
	}
}
