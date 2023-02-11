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
            Debug.Log("ways:" + currentWays.Count);
            foreach(GameObject way in currentWays)
            {
                Debug.Log(way.transform.localScale);
                if (way.transform.localScale.x > .3)
                    direction.x = draggingComponent.targetPos.x;
                else if (way.transform.localScale.z > .3)
                    direction.y = draggingComponent.targetPos.z;
            }
            if (direction.x > direction.y)
                direction.y = 0;
            else
                direction.x = 0;
            Vector3 target = new Vector3(direction.x, 0, direction.y);
            transform.position = Vector3.Lerp(transform.position, target, timer / 5);
            timer += Time.deltaTime;
        }
    }
}
