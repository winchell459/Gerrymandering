using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private float speed = 1;
    [SerializeField] private Vector2 destination;
    private bool moving;
    public bool Stopped { get { return !moving; } }

    // Start is called before the first frame update
    void Start()
    {
        setDestination(transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, destination) < speed * Time.deltaTime) moving = false;
        else moving = true;
        transform.position = Vector2.MoveTowards( transform.position, destination, speed * Time.deltaTime);
    }

    private void setDestination(Vector2 destination)
    {
        this.destination = destination;
    }

    public void SetDestination(Vector2 destination)
    {
        setDestination(destination);
    }
}
