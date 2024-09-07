using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour
{
    public GameObject linePrefabs;
    private List<LineRenderer> listLine = new List<LineRenderer>();
    private List<LineRenderer> lineActive = new List<LineRenderer>();

    private bool connectedAll = false;


    private void LateUpdate()
    {
        if (lineActive.Count > 0 && !connectedAll)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            lineActive[lineActive.Count - 1].SetPosition(1, new Vector3(mousePos.x, mousePos.y, 1f));
        }
    }

    public void InitLine(int numLines)
    {
        while (this.transform.childCount < numLines)
        {
            var line = Instantiate(linePrefabs, this.transform);
            listLine.Add(line.GetComponent<LineRenderer>());
        }
    }

    public void AddNewLine(Vector3 position)
    {
        if (lineActive.Count > 0)
        {
            var line = lineActive[lineActive.Count - 1];
            line.SetPosition(1, position);
        }

        if(listLine.Count > 0)
        {
            var newLine = listLine[0];
            newLine.positionCount = 2;
            newLine.SetPosition(0, position);

            lineActive.Add(newLine);
            listLine.Remove(newLine);

            connectedAll = false;
        } else
        {
            connectedAll = true;
        }
    }

    public void RemoveLine()
    {
        if (connectedAll)
        {
            connectedAll = false;
        } 
        else
        {
            var line = lineActive[lineActive.Count - 1];
            line.positionCount = 0;
            listLine.Add(line);
            lineActive.Remove(line);
        }

    }

    public void ClearLine()
    {
        while (lineActive.Count > 0)
        {
            RemoveLine();
        }
    }

}
