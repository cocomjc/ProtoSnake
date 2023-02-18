using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailManager : MonoBehaviour
{
    private Vector3 startPos = new Vector3(-200, -200, -200);
    [SerializeField] private DirectionEvent onDirectionChange;

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
        Debug.Log("Change Trail to : " + newDirection);
    }


}
