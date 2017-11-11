using UnityEditor;
using UnityEngine;

public class uGUITools : MonoBehaviour
{
    [MenuItem("uGUI/Anchors to Corners %[")]
    static void AnchorsToCorners()
    {
        var objs = Selection.gameObjects;

        foreach (var obj in objs)
        {
            RectTransform t = obj.GetComponent<RectTransform>();
            RectTransform pt = obj.transform.parent.GetComponent<RectTransform>();

            if (t == null || pt == null) continue;

            Vector2 newAnchorsMin = new Vector2(t.anchorMin.x + t.offsetMin.x / pt.rect.width,
                                                t.anchorMin.y + t.offsetMin.y / pt.rect.height);
            Vector2 newAnchorsMax = new Vector2(t.anchorMax.x + t.offsetMax.x / pt.rect.width,
                                                t.anchorMax.y + t.offsetMax.y / pt.rect.height);

            t.anchorMin = newAnchorsMin;
            t.anchorMax = newAnchorsMax;
            t.offsetMin = t.offsetMax = new Vector2(0, 0);
        }
    }


    [MenuItem("uGUI/Corners to Anchors %]")]
    static void CornersToAnchors()
    {
        var objs = Selection.gameObjects;

        foreach (var obj in objs)
        {
            RectTransform t = obj.GetComponent<RectTransform>();
           
            if (t == null) continue;
            
            t.offsetMin = t.offsetMax = new Vector2(0, 0);
        }
    }
}