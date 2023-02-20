using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailManager : MonoBehaviour
{
    private Vector3 startPos = new Vector3(-200, -200, -200);
    [SerializeField] private DirectionEvent onDirectionChange;
    [SerializeField] private GameObject scaledLine;
    [SerializeField] private GameObject trailPrefab;
    public List<GameObject> trailList = new List<GameObject>();
    private Directions currentDirection;

    private void OnEnable()
    {
        onDirectionChange.OnEventRaised += AddTrail;
    }

    private void OnDisable()
    {
        onDirectionChange.OnEventRaised -= AddTrail;
    }

    void Start()
    {
        startPos = transform.position;
    }

    private void AddTrail(Directions newDirection)
    {
        currentDirection = newDirection;
        if (scaledLine.transform.localScale.x >= 1 && scaledLine.transform.localScale.z >= 1)
        {
            GameObject newLine = Instantiate(trailPrefab);
            newLine.GetComponent<TrailBehavior>().SetExtremities(startPos, transform.position);
            newLine.transform.SetParent(transform.parent);
            SetSegment(newLine, false);
            trailList.Add(newLine);
            startPos = new Vector3(Mathf.Round(transform.position.x), 0, Mathf.Round(transform.position.z));
        }
    }

    public void PopTrail()
    {
        GameObject popedTrail = trailList[trailList.Count - 1];
        TrailBehavior trailBehavior = popedTrail.GetComponent<TrailBehavior>();
        
        trailList.Remove(popedTrail);
        if (Vector3.Distance(trailBehavior.extremities[0], transform.position) > Vector3.Distance(trailBehavior.extremities[1], transform.position))
            startPos = popedTrail.GetComponent<TrailBehavior>().extremities[0];
        else
            startPos = popedTrail.GetComponent<TrailBehavior>().extremities[1];
        Destroy(popedTrail);
    }

    private void Update()
    {
        if (startPos != new Vector3(-200, -200, -200))
        {
            SetSegment(scaledLine, true);
        }
    }

    private void SetSegment(GameObject segment, bool tail)
    {
        int offsetSign;
        Vector3 offset;
        float distance = Vector3.Distance(startPos, transform.position);

        Debug.DrawLine(startPos, transform.position, Color.red);
        // Debug.Log("Draw line from " + startPos + " to " + transform.position);
        segment.transform.localScale = new Vector3(1, 1, distance);
        if ((transform.position - startPos).x < 0 || (transform.position - startPos).z < 0)
            offsetSign = 1;
        else
            offsetSign = -1;
        offset = ((currentDirection == Directions.Vertical && tail) ||
            (currentDirection == Directions.Horizontal && !tail)) ? new Vector3(0, 0, offsetSign * .5f) : new Vector3(offsetSign * .5f, 0, 0);
        segment.transform.position = (startPos + (transform.position - startPos) / 2) + offset;
        segment.transform.rotation = Quaternion.LookRotation(transform.position - startPos);
    }

}
