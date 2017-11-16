using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	[SerializeField]
	List<GameObject> pages = new List<GameObject>();

	[SerializeField]
	GameObject TopMenu = null, MainPage = null;

	[SerializeField]
	Button backButton = null, logoutButton = null;

	// Use this for initialization
	void Start()
	{
		foreach (GameObject p in pages) p.SetActive(false);
		MainPage.SetActive(true);
		backButton.gameObject.SetActive(false);
	}

	// Update is called once per frame
	void Update()
	{

	}

	public void OpenPage(GameObject page)
	{
		foreach (GameObject p in pages) p.SetActive(p == page);
		MainPage.SetActive(page == MainPage);
		backButton.gameObject.SetActive(page != MainPage);
	}

	public void Logout()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene("login");
	}
}
