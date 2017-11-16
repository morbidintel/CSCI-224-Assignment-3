using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TabSelect : MonoBehaviour
{
	void Awake()
	{
		DontDestroyOnLoad(gameObject);
	}

	// Update is called once per frame
	void Update()
	{
		// When TAB is pressed, we should select the next selectable UI element
		if (Input.GetKeyDown(KeyCode.Tab))
		{
			Selectable next = null;
			Selectable current = null;

			// Figure out if we have a valid current selected gameobject
			if (EventSystem.current.currentSelectedGameObject != null)
			{
				// Unity doesn't seem to "deselect" an object that is made inactive
				if (EventSystem.current.currentSelectedGameObject.activeInHierarchy)
				{
					current = EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>();
				}
			}

			if (current != null)
			{
				// When SHIFT is held along with tab, go backwards instead of forwards
				if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
				{
					next = current.FindSelectableOnLeft();
					if (next == null)
					{
						next = current.FindSelectableOnUp();
					}
				}
				else
				{
					next = current.FindSelectableOnRight();
					if (next == null)
					{
						next = current.FindSelectableOnDown();
					}
				}
			}
			else
			{
				// If there is no current selected gameobject, select the first one
				if (Selectable.allSelectables.Count > 0)
				{
					next = Selectable.allSelectables[0];
				}
			}

			if (next != null)
			{
				next.Select();
			}
		}
	}
}