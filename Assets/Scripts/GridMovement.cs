using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridMovement : MonoBehaviour
{
    private Dragging draggingComponent;
    private TrailManager trailManager;
    [SerializeField] private List<GameObject> currentWays = new List<GameObject>();
    private Directions currentDir;
    [SerializeField] private DirectionEvent onDirectionChange;
    [SerializeField] private float lerpSpeed = 5f;
    [SerializeField] private float tresholdValue = .3f;

    // Start is called before the first frame update
    void Start()
    {
        currentDir = Directions.Init;
        draggingComponent = GetComponent<Dragging>();
        trailManager = GetComponent<TrailManager>();
    }

    void OnTriggerEnter(Collider other)
    {
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

            //Quick exit if we are not far enough to the player
            if (Mathf.Abs(targetPosOffset.x) < tresholdValue && Mathf.Abs(targetPosOffset.z) < tresholdValue)
                return;

            //Computing direction wich is the biggest offset vertical or horizontal
            foreach (GameObject way in currentWays)
            {
                //Determine if the way is horizontal or vertical
                // MAGIC VALUE A CORRIGER !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                if (way.transform.localScale.z > .5) {
                    //Test if vertical offset is bigger than horizontal
                    if (Mathf.Abs(targetPosOffset.z) > Mathf.Abs(direction.x))
                    {
                        direction = new Vector2(0, targetPosOffset.z);
                        latePos = way.transform.position.x;
                        currentPathCollider = way.GetComponent<Collider>();
                    }
                }
                else if (way.transform.localScale.x > .5) {
                    if (Mathf.Abs(targetPosOffset.x) > Mathf.Abs(direction.y))
                    {
                        direction = new Vector2(targetPosOffset.x, 0);
                        latePos = way.transform.position.z;
                        currentPathCollider = way.GetComponent<Collider>();
                    }
                }
            }
            Vector3 target = new Vector3(direction.x, 0, direction.y) + transform.position;

            if (direction.x == 0 && direction != Vector2.zero)
            {
                target.x = latePos;
                transform.position = new Vector3(target.x, 0, transform.position.z);
                // We determined that the movement is vertical, if it was not the case before, we raise the event onDirectionChange
                if (currentDir != Directions.Vertical)
                {
                    currentDir = Directions.Vertical;
                    onDirectionChange.RaiseEvent(currentDir);
                }
            }
            else if (direction != Vector2.zero)
            {
                target.z = latePos;
                transform.position = new Vector3(transform.position.x, 0, target.z);
                if (currentDir != Directions.Horizontal)
                {
                    currentDir = Directions.Horizontal;
                    onDirectionChange.RaiseEvent(currentDir);
                }
            }

            //Check among trailList if direction is the bounds of a trail
            Vector3 forwardCheck = transform.position + (target - transform.position).normalized * .5f;
            bool collide = false;
            foreach (GameObject trail in trailManager.trailList.ToList())
            {
                if (trail.GetComponent<Collider>().bounds.Contains(forwardCheck))
                {
                    Debug.Log("Collision with trail");
                    if (trail == trailManager.trailList[trailManager.trailList.Count - 1])
                    {
                        Debug.DrawLine(transform.position + new Vector3(0, .51f, 0), forwardCheck + new Vector3(0, .51f, 0), Color.magenta, 3);
                        trailManager.PopTrail();
                        break;
                    }
                    else
                        return;
                }
                if (collide)
                    break;
            }

            target = Vector3.Lerp(transform.position, target, lerpSpeed);
            if (currentPathCollider && currentPathCollider.bounds.Contains(target))
                transform.position = target;
        }
    }
}
