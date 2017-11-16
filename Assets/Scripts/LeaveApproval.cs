using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mono.Data.Sqlite;

public class LeaveApproval : MonoBehaviour
{
	[SerializeField]
	LeaveRequests leaveRequestsScript = null;

	[SerializeField]
	GameObject buttonsParent = null, detailsParent = null;

	[SerializeField]
	Text title = null;

	[SerializeField]
	Text nameText = null, submitText = null, statusText = null, datesText = null;

	LeaveRequestEntry selectedEntry = null;

	// Use this for initialization
	void Start()
	{
		buttonsParent.SetActive(false);
		detailsParent.SetActive(false);
	}

	// Update is called once per frame
	void Update()
	{

	}

	public void OnClick(LeaveRequestEntry entry)
	{
		foreach (var l in leaveRequestsScript.entries)
		{
			l.SetSelected(false);
		}

		if (selectedEntry != entry)
		{
			selectedEntry = entry;
			entry.SetSelected(true);
			title.gameObject.SetActive(false);
			detailsParent.SetActive(true);
			buttonsParent.SetActive(true);
			nameText.text = entry.nameText.text;
			submitText.text = entry.submitText.text;
			statusText.text = entry.statusText.text;
			datesText.text = entry.datesText.text;
		}
		else
		{
			selectedEntry = null;
			entry.SetSelected(false);
			title.gameObject.SetActive(true);
			detailsParent.SetActive(false);
			buttonsParent.SetActive(false);
			nameText.text = "";
			submitText.text = "";
			statusText.text = "";
			datesText.text = "";
		}
	}

	public void OnClickApprove()
	{
		using (SqliteCommand command = new SqliteCommand(Database.db))
		{
			command.CommandText = string.Format(
				"UPDATE leaves SET status = \"Approved\" " +
				"WHERE username = (SELECT username FROM users WHERE full_name = \"{0}\") " +
				"AND approving = \"{1}\" " +
				"AND timestamp = \"{2}\" " +
				"AND status = \"{3}\"",
				nameText.text, User.username, submitText.text.Replace("\n", ""), statusText.text);

			command.ExecuteNonQuery();
		}

		selectedEntry.statusText.text = statusText.text = "Approved";
	}

	public void OnClickReject()
	{
		using (SqliteCommand command = new SqliteCommand(Database.db))
		{
			command.CommandText = string.Format(
				"UPDATE leaves SET status = \"Rejected\" " +
				"WHERE username = (SELECT username FROM users WHERE full_name = \"{0}\") " +
				"AND approving = \"{1}\" " +
				"AND timestamp = \"{2}\" " +
				"AND status = \"{3}\"",
				nameText.text, User.username, submitText.text.Replace("\n", ""), statusText.text);

			command.ExecuteNonQuery();
		}

		selectedEntry.statusText.text = statusText.text = "Rejected";
	}
}
