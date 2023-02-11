using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragging : MonoBehaviour
{
    private float dist;
    public bool dragging = false;
    public Vector3 targetPos;

    void Update()
    {
        Vector3 v3;

        if (Input.touchCount != 1)
        {
            dragging = false;
            return;
        }

        Touch touch = Input.touches[0];
        Vector3 touchPos = touch.position;

        if (touch.phase == TouchPhase.Began)
        {
            RaycastHit hit;

            Ray ray = Camera.main.ScreenPointToRay(touchPos);
            Debug.DrawRay(ray.origin, ray.direction * 30, Color.yellow, 3);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject.tag == "Player")
                {
                    dist = hit.transform.position.y - Camera.main.transform.position.y;
                    dragging = true;
                }
            }
        }

        if (dragging && touch.phase == TouchPhase.Moved)
        {
            v3 = new Vector3(Input.mousePosition.x, Input.mousePosition.y, dist);
            v3 = Camera.main.ScreenToWorldPoint(v3);
            targetPos = - new Vector3(v3.x, 0, v3.z);
            //Debug.Log("target: " + targetPos);
        }

        if (dragging && (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled))
        {
            dragging = false;
        }
    }
}
