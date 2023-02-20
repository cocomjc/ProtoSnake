using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailBehavior : MonoBehaviour
{
    public Vector3[] extremities = new Vector3[2];

    public void SetExtremities(Vector3 start, Vector3 end)
    {
        extremities[0] = start;
        extremities[1] = end;
    }
}
