﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb2D;
    public float moveSpeed = 5f;
    public float pullForce = 100f;
    public float rotateSpeed = 360f;
    private GameObject closestTower;
    private GameObject hookedTower;
    private bool isPulled = false;
    private UIController uiControl;
    private AudioSource myAudio;
    private bool isCrashed = false;
    public Vector3 startPosition;

    // Start is called before the first frame update
    void Start()
    {
        rb2D = this.gameObject.GetComponent<Rigidbody2D>();
        uiControl = GameObject.Find("Canvas").GetComponent<UIController>();
        myAudio = this.gameObject.GetComponent<AudioSource>();
        startPosition = new Vector3(-8f, 2.8f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        // Move Object
        rb2D.velocity = -transform.up * moveSpeed;

        if(Input.GetKeyDown(KeyCode.Mouse0) && !isPulled)
        {

            if (isCrashed)
            {
                if (!myAudio.isPlaying)
                {
                    //Restart scene
                    restartPosition();
                }
            }
            else
            {
                //Move the object
                rb2D.velocity = -transform.up * moveSpeed;
                rb2D.angularVelocity = 0f;
            }

            if (closestTower != null && hookedTower == null)
            {
                hookedTower = closestTower;
            }

            if (hookedTower)
            {
                float distance = Vector2.Distance(transform.position, hookedTower.transform.position);

                //Gravitation toward tower
                Vector3 pullDirection = (hookedTower.transform.position - transform.position).normalized;
                float newPullForce = Mathf.Clamp(pullForce / distance, 20, 50);
                rb2D.AddForce(pullDirection * newPullForce);
                isPulled = true;

                //Angular Velocity
                rb2D.angularVelocity = -rotateSpeed / distance;
                isPulled = true;
            }

    
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            rb2D.angularVelocity = 0;
            isPulled = false;
            hookedTower = null;
        }

    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Wall")
        {
            if (!isCrashed)
            {
                //Play SFX
                myAudio.Play();
                rb2D.velocity = new Vector3(0f, 0f, 0f);
                rb2D.angularVelocity = 0f;
                isCrashed = true;
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Goal")
        {
            Debug.Log("LevelClear!");
            uiControl.endGame();
        }
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Tower")
        {
            closestTower = collision.gameObject;

            // Change tower color to green as indicator
            collision.gameObject.GetComponent<SpriteRenderer>().color = Color.green;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (isPulled) return;

        if (collision.gameObject.tag == "Tower")
        {
            closestTower = null;
            hookedTower = null;

            // Change tower color back to white
            collision.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    public void restartPosition()
    {
        this.transform.position = startPosition;
        this.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
        isCrashed = false;

        if(closestTower)
        {
            closestTower.GetComponent<SpriteRenderer>().color = Color.white;
            closestTower = null;
        }

    }
}
