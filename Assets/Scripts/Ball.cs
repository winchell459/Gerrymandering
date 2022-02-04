﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private float speed = 1;
    [SerializeField] private Vector2 destination;
    private bool moving;
    public bool Stopped{get{return !moving;}}

    [SerializeField] SpriteRenderer sprite;
    [SerializeField] Animator anim;
    [SerializeField] float jumpOffset = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        setDestination(transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, destination) < speed * Time.deltaTime)
        {
            moving = false;
            anim.SetFloat("Speed", 0);
        }
        else
        {
            anim.SetFloat("Speed", 1);
            moving = true;
            Vector2 direction = jumpOffset * (destination - (Vector2)transform.position).normalized;
            if(FindObjectOfType<GolfMoveFinder>().GetTile((Vector2)transform.position + direction).tileType == GolfASP.tile_types.air)
            {
                anim.SetBool("Jumping", true);
            }
            else
            {
                anim.SetBool("Jumping", false);
            }
            
        }
        transform.position = Vector2.MoveTowards( transform.position, destination, speed * Time.deltaTime);
    }

    private void setDestination(Vector2 destination)
    {
        this.destination = destination;
    }

    public void SetDestination(Vector2 destination)
    {
        if (destination.x < transform.position.x) sprite.flipX = true;
        else if (destination.x > transform.position.x) sprite.flipX = false;
        setDestination(destination);
    }
}
