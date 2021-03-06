using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    
    private float SCALE = .6f; 
    private float RESET_SPEED = 100;
    
    
    [Range(0,3)]
    public int numRotations;

    private Vector2 mouseClickOffset; 
    private Vector2 spawnPosition;
    private bool isResettingPosition = false;
    private bool isHolding = false;

    void Awake()
    {
        spawnPosition = transform.position;

        GameManager.instance.GameOverEvent += OnGameOverEvent;

        ScaleDown();
    }

    void Update() {
        if (isResettingPosition)
        {
            MoveTowardsSpawnPosition();
        }
    }

    private void OnGameOverEvent(object sender, System.EventArgs e)
    {
        if (isHolding) {
            ResetPosition(translate: true);
        }
    }

    private void SetSortingLayer(bool up)
    {   
        int change = up ? 1 : -1;
        foreach (Transform child in transform)
        {
            SpriteRenderer sprite = child.GetComponent<SpriteRenderer>();
            sprite.sortingOrder += change;
        }
    }

    private void MoveTowardsSpawnPosition()
    {
        if (spawnPosition != null && transform.position != (Vector3)spawnPosition) 
        {
            transform.position = Vector2.MoveTowards(transform.position, spawnPosition, Time.deltaTime * RESET_SPEED);
        }
        else
        {
            isResettingPosition = false;
        }
    }

    // Mouse Functions
    void OnMouseDown()
    {
        isHolding = true;

        mouseClickOffset = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        ResetScale();
        isResettingPosition = false;

        SetSortingLayer(true);
    }

    void OnMouseDrag()
    {
        if (isHolding) {
            Vector2 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = newPosition - mouseClickOffset;
        }
    }

    void OnMouseUp()
    {
        isHolding = false;

        SetSortingLayer(false);
        GameManager.instance.DropPiece(this);
    }


    public void ResetScale()
    {
        transform.localScale = new Vector2(1, 1);
    }

    public void ScaleDown()
    {
        transform.localScale = new Vector2(SCALE,SCALE);
    }

    public void ResetPosition(bool translate)
    {
        isHolding = false;

        if (spawnPosition != null)
        {
            ScaleDown();
            
            if (translate)
            {
                isResettingPosition = true;
            }
            else
            {
                transform.position = spawnPosition;
            }
        }
        else
        {
            Debug.Log("Spawn position was not set.");
        }
    }
}
