using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class GUIDebugger : MonoBehaviour
{
    static List<string> msg = new List<string>();
    static List<LineRenderer> lineList = new List<LineRenderer>();
    static LineRenderer lineConfig;
    static int lineCount = 0;
    static public GUIDebugger instance;
    public bool enabled;
    static bool _enabled;

    private void Awake()
    {
        instance = this;
        lineConfig = GetComponent<LineRenderer>();
        _enabled = enabled;
    }

    static public void Add(string message)
    {
        msg.Add(message);
    }

    static public void DrawLine(Vector2 origin, Vector2 sizeVector, Color color)
    {
        if (!_enabled) return;
        lineCount++;
        if (lineList.Count < lineCount)
        {

            var newObj = new GameObject();
            newObj.transform.parent = instance.transform;
            newObj.AddComponent<LineRenderer>();
            LineRenderer newLine = newObj.GetComponent<LineRenderer>();
            newLine.startWidth = newLine.endWidth = lineConfig.startWidth;
            newLine.material = lineConfig.material;
            newLine.sortingOrder = lineConfig.sortingOrder;
            newLine.gameObject.SetActive(false);
            lineList.Add(newLine);
        }

        LineRenderer lineToDraw = null;
        foreach(LineRenderer line in lineList)
        {
            if (line.gameObject.activeSelf == false) lineToDraw = line;
        }

        lineToDraw.gameObject.SetActive(true);        
        lineToDraw.startColor = lineToDraw.endColor = color;
        lineToDraw.positionCount = 2;
        lineToDraw.SetPosition(0, origin);
        lineToDraw.SetPosition(1, origin + sizeVector);
    }

    private void Update()
    {
        _enabled = enabled;
        msg.Clear();
        lineCount = 0;
        foreach (LineRenderer line in lineList)
        {
            line.gameObject.SetActive(false);
        }
    }

    private void OnGUI()
    {
        if (!_enabled) return;
        StringBuilder str = new StringBuilder();
        foreach (string line in msg)
        {
            str.Append(line).Append("\n");
        }        
        GUILayout.TextArea(str.ToString());        
    }
}
