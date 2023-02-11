using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMovement : MonoBehaviour
{
    private Dragging draggingComponent;
    private float timer = 0;
    private List<GameObject> currentWays = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        draggingComponent = GetComponent<Dragging>();
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("SIIUU");
        if (other.gameObject.tag == "Way")
        {
            currentWays.Add(other.gameObject);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Way")
        {
            currentWays.Remove(other.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (draggingComponent.dragging)
        {
            Vector2 direction = Vector2.zero;
            Vector3 targetPosOffset = draggingComponent.targetPos - transform.position;
            float latePos = 0;
            Collider currentPathCollider = null;

            Debug.Log("offset dir = " + targetPosOffset);
            foreach (GameObject way in currentWays)
            {
                //Debug.Log(way.transform.localScale);
                if (way.transform.localScale.z > .5) {
                    Debug.Log("testing vertical with " + Mathf.Abs(targetPosOffset.z) + " > " + Mathf.Abs(direction.x));
                    if (Mathf.Abs(targetPosOffset.z) > Mathf.Abs(direction.x))
                    {
                        direction = new Vector2(0, targetPosOffset.z);
                        latePos = way.transform.position.x;
                        currentPathCollider = way.GetComponent<Collider>();
                    }
                }
                else if (way.transform.localScale.x > .5) {
                    Debug.Log("testing horizontal with " + Mathf.Abs(targetPosOffset.x) + " > " + Mathf.Abs(direction.y));
                    if (Mathf.Abs(targetPosOffset.x) > Mathf.Abs(direction.y))
                    {
                        direction = new Vector2(targetPosOffset.x, 0);
                        latePos = way.transform.position.z;
                        currentPathCollider = way.GetComponent<Collider>();
                    }
                }
            }
            Debug.Log("final direction: " + direction);
            //direction = (direction.x == 0) ? new Vector2(latePos, direction.y) : new Vector2(direction.x, latePos);
            Vector3 target = new Vector3(direction.x, 0, direction.y) + transform.position;

            if (direction.x == 0 && direction != Vector2.zero)
                target.x = latePos;
            else if (direction != Vector2.zero)
                target.z = latePos;

            //Vector3 target = new Vector3(direction.x, 0, direction.y) + transform.position;
            Debug.Log("latePos: " + latePos);
            //transform.position = target;
            target = Vector3.Lerp(transform.position, target, timer / 5);
            if (currentPathCollider && currentPathCollider.bounds.Contains(target))
                transform.position = target;
            timer += Time.deltaTime;
        }
    }
}
