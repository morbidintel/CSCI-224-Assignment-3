using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TopMenu : MonoBehaviour
{
	[SerializeField]
	Button hoursButton = null, leaveReqestButton = null, employeeMgmtButton = null, leaveApprovalButton = null;

	// Use this for initialization
	void Start()
	{
		employeeMgmtButton.gameObject.SetActive(User.role == User.Role.HR);
		leaveApprovalButton.gameObject.SetActive(User.role == User.Role.Manager);
	}

	// Update is called once per frame
	void Update()
	{

	}
}
