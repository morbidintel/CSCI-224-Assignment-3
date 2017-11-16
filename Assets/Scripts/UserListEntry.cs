using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Mono.Data.Sqlite;

public class UserListEntry : MonoBehaviour
{
	[SerializeField]
	InputField usernameField = null, roleField = null, fullNameField = null;

	public string username { get; set; }
	string _role;
	public string role { get; set; }
	public string fullName { get; set; }

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	public void Init(string username, string role, string fullName)
	{
		this.username = usernameField.text = username;
		this.role = _role = roleField.text = role;
		this.fullName = fullNameField.text = fullName;
	}

	public void OnEndEdit(DG.Tweening.DOTweenAnimation anim)
	{
		InputField field = anim.GetComponent<InputField>();
		
		if (!User.Roles.Contains(role))
		{
			role = roleField.text = _role;
			anim.onComplete.AddListener(() =>
			{
				anim.endValueColor = Color.HSVToRGB(120 / 360f, 32 / 255f, 1f);
				anim.CreateTween();
				anim.onComplete.RemoveAllListeners();
			});
			anim.endValueColor = Color.HSVToRGB(0, 32 / 255f, 1f);
			anim.CreateTween();
			anim.DORestart();
			return;
		}

		using (SqliteCommand command = new SqliteCommand(Database.db))
		{
			if (field == usernameField)
			{
				command.CommandText = string.Format(
					"UPDATE users SET username = \"{0}\" " +
					"WHERE role = \"{1}\" " +
					"AND full_name = \"{2}\"",
					username, role, fullName);
			}
			else if (field == roleField)
			{
				command.CommandText = string.Format(
					"UPDATE users SET  role = \"{1}\" " +
					"WHERE username = \"{0}\"" +
					"AND full_name = \"{2}\"",
					username, role, fullName);
			}
			else if (field == fullNameField)
			{
				command.CommandText = string.Format(
					"UPDATE users SET full_name = \"{2}\" " +
					"WHERE role = \"{1}\" " +
					"AND username = \"{0}\"",
					username, role, fullName);
			}
		}

		anim.DORestart();
	}
}
